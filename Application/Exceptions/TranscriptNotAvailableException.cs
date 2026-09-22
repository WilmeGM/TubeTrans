namespace Application.Exceptions
{
    public class TranscriptNotAvailableException : TubeTransException
    {
        public TranscriptNotAvailableException (string message) : base(message) { }
    }
}
