namespace Application.ViewModels
{
    public class TranscriptResponseViewModel
    {
        public string VideoTitle { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public bool HasError { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
