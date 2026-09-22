namespace Application.Exceptions
{
    public abstract class TubeTransException : Exception
    {
        protected TubeTransException(string message) : base(message)
        {
        }
    }
}
