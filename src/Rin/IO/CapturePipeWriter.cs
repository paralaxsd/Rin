using System;
using System.IO;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;

namespace Rin.IO
{
    public class CapturePipeWriter : PipeWriter
    {
        private readonly PipeWriter _pipeWriter;
        private readonly MemoryStream _capturedDataStream;

        public CapturePipeWriter(PipeWriter pipeWriter, MemoryStream capturedDataStream)
        {
            _pipeWriter = pipeWriter;
            _capturedDataStream = capturedDataStream;
        }

        // Delegate to inner writer; fall back to 0 if inner doesn't implement it.
        // System.Text.Json in .NET 10 uses this as a flush threshold — returning 0 means
        // "flush eagerly", which is safe. Without this override the base class throws and
        // STJ re-names the exception to this type regardless of who actually threw.
        public override long UnflushedBytes
        {
            get
            {
                try { return _pipeWriter.UnflushedBytes; }
                catch (NotImplementedException) { return 0; }
            }
        }

        public override bool CanGetUnflushedBytes => true;

        public override void Advance(int bytes)
        {
            _capturedDataStream.Write(_pipeWriter.GetSpan(bytes).Slice(0, bytes));

            _pipeWriter.Advance(bytes);
        }

        public override Memory<byte> GetMemory(int sizeHint = 0)
        {
            return _pipeWriter.GetMemory(sizeHint);
        }

        public override Span<byte> GetSpan(int sizeHint = 0)
        {
            return _pipeWriter.GetSpan(sizeHint);
        }

        public override void CancelPendingFlush()
        {
            _pipeWriter.CancelPendingFlush();
        }

        public override void Complete(Exception? exception = null)
        {
            _pipeWriter.Complete(exception);
        }

        public override ValueTask<FlushResult> FlushAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return _pipeWriter.FlushAsync(cancellationToken);
        }
    }
}
