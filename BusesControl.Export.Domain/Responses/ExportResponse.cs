namespace BusesControl.Export.Core.Responses
{
    public class ExportResponse
    {
        public FileResponse File { get; private set; }
        public string Message { get; private set; }
        public bool Success { get; private set; }

        public static ExportResponse Ok(FileResponse file)
            => new () { File = file, Success = true };

        public static ExportResponse Failed(string message)
            => new () { Message = message, Success = false };
    }
}
