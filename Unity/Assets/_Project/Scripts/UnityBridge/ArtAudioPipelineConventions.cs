namespace Legacy.UnityBridge
{
    public static class ArtAudioPipelineConventions
    {
        public const string ArtRoot = "Assets/_Project/Art";
        public const string AudioRoot = "Assets/_Project/Audio";
        public const string ArtReadmePath = ArtRoot + "/README.md";
        public const string AudioReadmePath = AudioRoot + "/README.md";
        public const float DefaultPixelsPerUnit = 16f;

        public static readonly string[] RequiredFolders = {
            ArtRoot,
            ArtRoot + "/Sprites",
            ArtRoot + "/Sprites/Characters",
            ArtRoot + "/Sprites/Environment",
            ArtRoot + "/Sprites/UI",
            ArtRoot + "/Materials",
            ArtRoot + "/Palettes",
            ArtRoot + "/Placeholders",
            AudioRoot,
            AudioRoot + "/Music",
            AudioRoot + "/Ambience",
            AudioRoot + "/Sfx",
            AudioRoot + "/Sfx/UI",
            AudioRoot + "/Sfx/World",
            AudioRoot + "/Mixers",
            AudioRoot + "/Snapshots",
            AudioRoot + "/Placeholders"
        };

        public static readonly string[] RequiredDocumentation = {
            ArtReadmePath,
            AudioReadmePath
        };

        public static readonly string[] ValidatedArtImportRoots = {
            ArtRoot + "/Sprites",
            ArtRoot + "/Placeholders"
        };

        public static readonly string[] ValidatedAudioImportRoots = {
            AudioRoot + "/Music",
            AudioRoot + "/Ambience",
            AudioRoot + "/Sfx",
            AudioRoot + "/Placeholders"
        };

        public static bool IsArtAssetPath(string assetPath)
        {
            return assetPath != null && assetPath.StartsWith(ArtRoot + "/", System.StringComparison.Ordinal);
        }

        public static bool IsAudioAssetPath(string assetPath)
        {
            return assetPath != null && assetPath.StartsWith(AudioRoot + "/", System.StringComparison.Ordinal);
        }
    }
}
