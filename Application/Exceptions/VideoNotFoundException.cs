namespace Application.Exceptions
{
    public class VideoNotFoundException : TubeTransException
    {
        public VideoNotFoundException(string message) : base(message)
        {
        }
    }
}
