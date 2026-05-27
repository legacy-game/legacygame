namespace Legacy.Editor
{
    public readonly struct ArtAudioPipelineIssue
    {
        public ArtAudioPipelineIssue(string assetPath, string message)
        {
            AssetPath = assetPath;
            Message = message;
        }

        public string AssetPath { get; }
        public string Message { get; }

        public override string ToString()
        {
            return $"{AssetPath}: {Message}";
        }
    }
}
