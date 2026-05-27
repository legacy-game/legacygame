using System.Collections.Generic;
using System.IO;
using System.Text;
using Legacy.UnityBridge;
using UnityEditor;
using UnityEngine;

namespace Legacy.Editor
{
    public static class ArtAudioPipelineValidator
    {
        private static readonly HashSet<string> TextureExtensions = new() {
            ".png",
            ".psd",
            ".tga"
        };

        private static readonly HashSet<string> AudioExtensions = new() {
            ".wav",
            ".aiff",
            ".aif",
            ".mp3",
            ".ogg"
        };

        [MenuItem("Legacy/Art Audio/Validate Pipeline")]
        public static void ValidateFromMenu()
        {
            IReadOnlyList<ArtAudioPipelineIssue> issues = Validate();
            if (issues.Count == 0) {
                Debug.Log("Art/audio pipeline validation passed.");
                return;
            }

            var builder = new StringBuilder();
            builder.AppendLine("Art/audio pipeline validation failed:");
            for (int i = 0; i < issues.Count; i++) {
                builder.AppendLine(issues[i].ToString());
            }

            Debug.LogError(builder.ToString());
        }

        [MenuItem("Legacy/Art Audio/Apply Import Defaults")]
        public static void ApplyImportDefaultsFromMenu()
        {
            ApplyRecommendedImportSettings();
            ValidateFromMenu();
        }

        public static IReadOnlyList<ArtAudioPipelineIssue> Validate()
        {
            var issues = new List<ArtAudioPipelineIssue>();
            ValidateFolders(issues);
            ValidateDocumentation(issues);
            ValidateTextureImporters(issues);
            ValidateAudioImporters(issues);
            return issues;
        }

        public static void ApplyRecommendedImportSettings()
        {
            ApplyTextureDefaults();
            ApplyAudioDefaults();
            AssetDatabase.SaveAssets();
        }

        private static void ValidateFolders(List<ArtAudioPipelineIssue> issues)
        {
            foreach (string folder in ArtAudioPipelineConventions.RequiredFolders) {
                if (!AssetDatabase.IsValidFolder(folder)) {
                    issues.Add(new ArtAudioPipelineIssue(folder, "Required authoring folder is missing."));
                }
            }
        }

        private static void ValidateDocumentation(List<ArtAudioPipelineIssue> issues)
        {
            foreach (string documentPath in ArtAudioPipelineConventions.RequiredDocumentation) {
                if (AssetDatabase.LoadAssetAtPath<Object>(documentPath) == null) {
                    issues.Add(new ArtAudioPipelineIssue(documentPath, "Required authoring documentation is missing."));
                }
            }
        }

        private static void ValidateTextureImporters(List<ArtAudioPipelineIssue> issues)
        {
            foreach (string importRoot in ArtAudioPipelineConventions.ValidatedArtImportRoots) {
                if (!AssetDatabase.IsValidFolder(importRoot)) {
                    continue;
                }

                foreach (string assetPath in FindAssets(importRoot, TextureExtensions)) {
                    if (AssetImporter.GetAtPath(assetPath) is not TextureImporter importer) {
                        continue;
                    }

                    if (importer.textureType != TextureImporterType.Sprite) {
                        issues.Add(new ArtAudioPipelineIssue(assetPath, "Art textures must import as sprites."));
                    }

                    if (importer.spritePixelsPerUnit != ArtAudioPipelineConventions.DefaultPixelsPerUnit) {
                        issues.Add(new ArtAudioPipelineIssue(assetPath, $"Sprite pixels per unit must be {ArtAudioPipelineConventions.DefaultPixelsPerUnit}."));
                    }

                    if (importer.filterMode != FilterMode.Point) {
                        issues.Add(new ArtAudioPipelineIssue(assetPath, "Pixel art textures must use point filtering."));
                    }

                    if (importer.mipmapEnabled) {
                        issues.Add(new ArtAudioPipelineIssue(assetPath, "Pixel art textures must not generate mip maps."));
                    }

                    if (importer.textureCompression != TextureImporterCompression.Uncompressed) {
                        issues.Add(new ArtAudioPipelineIssue(assetPath, "Pixel art textures must be uncompressed."));
                    }
                }
            }
        }

        private static void ValidateAudioImporters(List<ArtAudioPipelineIssue> issues)
        {
            foreach (string importRoot in ArtAudioPipelineConventions.ValidatedAudioImportRoots) {
                if (!AssetDatabase.IsValidFolder(importRoot)) {
                    continue;
                }

                foreach (string assetPath in FindAssets(importRoot, AudioExtensions)) {
                    if (AssetImporter.GetAtPath(assetPath) is not AudioImporter importer) {
                        continue;
                    }

                    AudioImporterSampleSettings settings = importer.defaultSampleSettings;
                    if (IsLongFormAudio(assetPath)) {
                        if (settings.loadType != AudioClipLoadType.Streaming) {
                            issues.Add(new ArtAudioPipelineIssue(assetPath, "Music and ambience clips must stream."));
                        }

                        if (!importer.loadInBackground) {
                            issues.Add(new ArtAudioPipelineIssue(assetPath, "Music and ambience clips must load in the background."));
                        }

                        continue;
                    }

                    if (settings.loadType != AudioClipLoadType.DecompressOnLoad) {
                        issues.Add(new ArtAudioPipelineIssue(assetPath, "Sfx and UI clips must decompress on load for low-latency playback."));
                    }
                }
            }
        }

        private static void ApplyTextureDefaults()
        {
            foreach (string importRoot in ArtAudioPipelineConventions.ValidatedArtImportRoots) {
                if (!AssetDatabase.IsValidFolder(importRoot)) {
                    continue;
                }

                foreach (string assetPath in FindAssets(importRoot, TextureExtensions)) {
                    if (AssetImporter.GetAtPath(assetPath) is not TextureImporter importer) {
                        continue;
                    }

                    importer.textureType = TextureImporterType.Sprite;
                    importer.spriteImportMode = SpriteImportMode.Single;
                    importer.spritePixelsPerUnit = ArtAudioPipelineConventions.DefaultPixelsPerUnit;
                    importer.filterMode = FilterMode.Point;
                    importer.mipmapEnabled = false;
                    importer.alphaIsTransparency = true;
                    importer.textureCompression = TextureImporterCompression.Uncompressed;
                    importer.SaveAndReimport();
                }
            }
        }

        private static void ApplyAudioDefaults()
        {
            foreach (string importRoot in ArtAudioPipelineConventions.ValidatedAudioImportRoots) {
                if (!AssetDatabase.IsValidFolder(importRoot)) {
                    continue;
                }

                foreach (string assetPath in FindAssets(importRoot, AudioExtensions)) {
                    if (AssetImporter.GetAtPath(assetPath) is not AudioImporter importer) {
                        continue;
                    }

                    AudioImporterSampleSettings settings = importer.defaultSampleSettings;
                    if (IsLongFormAudio(assetPath)) {
                        settings.loadType = AudioClipLoadType.Streaming;
                        settings.compressionFormat = AudioCompressionFormat.Vorbis;
                        settings.quality = 0.7f;
                        settings.preloadAudioData = false;
                        importer.loadInBackground = true;
                    } else {
                        settings.loadType = AudioClipLoadType.DecompressOnLoad;
                        settings.compressionFormat = AudioCompressionFormat.PCM;
                        settings.preloadAudioData = true;
                        importer.loadInBackground = false;
                    }

                    importer.defaultSampleSettings = settings;
                    importer.SaveAndReimport();
                }
            }
        }

        private static IEnumerable<string> FindAssets(string root, HashSet<string> extensions)
        {
            string[] guids = AssetDatabase.FindAssets(string.Empty, new[] { root });
            for (int i = 0; i < guids.Length; i++) {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                string extension = Path.GetExtension(assetPath).ToLowerInvariant();
                if (extensions.Contains(extension)) {
                    yield return assetPath;
                }
            }
        }

        private static bool IsLongFormAudio(string assetPath)
        {
            return assetPath.Contains("/Music/") || assetPath.Contains("/Ambience/");
        }
    }
}
