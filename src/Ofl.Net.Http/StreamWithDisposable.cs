using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Ofl.Net.Http
{
    internal class StreamWithDisposable : Stream
    {
        #region Constructor

        public StreamWithDisposable(
            Stream stream, 
            IDisposable disposable
        )
        {
            // Validate parameters.
            _stream = stream ?? throw new ArgumentNullException(nameof(stream));

            // Assign values.
            _disposable = disposable;
        }

        #endregion

        #region Instance, read-only state.

        private readonly Stream _stream;

        private readonly IDisposable _disposable;

        #endregion

        #region Overrides of Stream

        public override void Flush() => _stream.Flush();

        public override long Seek(long offset, SeekOrigin origin) => _stream.Seek(offset, origin);

        public override void SetLength(long value) => _stream.SetLength(value);

        public override int Read(byte[] buffer, int offset, int count) => _stream.Read(buffer, offset, count);

        public override void Write(byte[] buffer, int offset, int count) => _stream.Write(buffer, offset, count);

        public override bool CanRead => _stream.CanRead;

        public override bool CanSeek => _stream.CanSeek;

        public override bool CanWrite => _stream.CanWrite;

        public override long Length => _stream.Length;

        public override long Position { get => _stream.Position; set => _stream.Position = value; }

        public override bool CanTimeout => _stream.CanTimeout;

        public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken) => _stream.CopyToAsync(destination, bufferSize, cancellationToken);

        public override Task FlushAsync(CancellationToken cancellationToken) => _stream.FlushAsync(cancellationToken);

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) => _stream.ReadAsync(buffer, offset, count, cancellationToken);

        public override int ReadByte() => _stream.ReadByte();

        public override int ReadTimeout { get => _stream.ReadTimeout; set => _stream.ReadTimeout = value; }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) => _stream.WriteAsync(buffer, offset, count, cancellationToken);

        public override void WriteByte(byte value) => _stream.WriteByte(value);

        public override int WriteTimeout { get => _stream.WriteTimeout; set => _stream.WriteTimeout = value; }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
            => _stream.BeginRead(buffer, offset, count, callback, state);

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
            => _stream.BeginWrite(buffer, offset, count, callback, state);

        public override void Close() => _stream.Close();

        public override void CopyTo(Stream destination, int bufferSize) => _stream.CopyTo(destination, bufferSize);

        public override ValueTask DisposeAsync() => _stream.DisposeAsync();

        public override int EndRead(IAsyncResult asyncResult) => _stream.EndRead(asyncResult);

        public override void EndWrite(IAsyncResult asyncResult) => _stream.EndWrite(asyncResult);

        public override bool Equals(object obj) => _stream.Equals(obj);

        public override int GetHashCode() => _stream.GetHashCode();

        public override object InitializeLifetimeService() => _stream.InitializeLifetimeService();


        public override int Read(Span<byte> buffer) => _stream.Read(buffer);

        public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
            => _stream.ReadAsync(buffer, cancellationToken);

        public override string ToString() => _stream.ToString();

        public override void Write(ReadOnlySpan<byte> buffer) => _stream.Write(buffer);

        public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
            => _stream.WriteAsync(buffer, cancellationToken);

        protected override void Dispose(bool disposing)
        {
            // Call the base.
            base.Dispose(disposing);

            // Dispose of unmanaged resources.

            // If not disposing, get out.
            if (!disposing) return;

            // Dispose of the disposables.
            using var _ = _disposable;
        }

        #endregion
    }
}