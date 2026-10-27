namespace TaskManagerAPI.Services
{
    public class FileLogger : IDisposable
    {
        private StreamWriter _writer;
        private bool _disposed = false;
      

        public FileLogger(string path)
        {
            _writer = new StreamWriter(path, append: true);
        }

        public void Log(string message)
        {
            if (_disposed) {
            throw new ObjectDisposedException(nameof(FileLogger));
            }
            _writer.WriteLine(message);
            _writer.Flush();
        }

        public void Dispose() 
        {
            if (_disposed) 
            {
                return;
            }
            _writer?.Dispose();
            _disposed = true;
        }
    }
}
