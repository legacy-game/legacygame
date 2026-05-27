using Legacy.Editor;
using Legacy.UnityBridge;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Legacy.Tests.EditMode
{
    public sealed class ArtAudioPipelineTests
    {
        [Test]
        public void AuthoringFoldersAndDocs_ArePresent()
        {
            foreach (string folder in ArtAudioPipelineConventions.RequiredFolders) {
                Assert.That(AssetDatabase.IsValidFolder(folder), Is.True, folder);
            }

            foreach (string documentPath in ArtAudioPipelineConventions.RequiredDocumentation) {
                Assert.That(AssetDatabase.LoadAssetAtPath<Object>(documentPath), Is.Not.Null, documentPath);
            }
        }

        [Test]
        public void PipelineValidator_PassesForSeedConventions()
        {
            Assert.That(ArtAudioPipelineValidator.Validate(), Is.Empty);
        }
    }
}
