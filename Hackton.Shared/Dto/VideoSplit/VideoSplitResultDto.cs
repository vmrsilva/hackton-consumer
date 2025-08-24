namespace Hackton.Shared.Dto.VideoSplit
{
    public record VideoSplitResultDto
    {
        public string[] Files { get; init; }
        public string TempFilePath { get; init; }
        public string FramesFolder { get; init; }
    }
}
