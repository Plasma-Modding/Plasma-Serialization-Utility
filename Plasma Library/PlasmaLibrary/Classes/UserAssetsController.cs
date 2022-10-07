using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Sirenix.Serialization;

namespace Plasma.Classes
{
    // Token: 0x02000049 RID: 73
    public class UserAssetsController : Controller
    {
        // Token: 0x06000250 RID: 592 RVA: 0x000108F8 File Offset: 0x0000EAF8
        public override void Init()
        {
            this._libraryPath = PathExtension.Combine(new string[]
            {
            Application.persistentDataPath,
            this.subfolder
            });
            this._libraryFilePath = PathExtension.Combine(new string[]
            {
            this._libraryPath,
            "_library_.dry"
            });
            this._externalUserAssetsData = new Dictionary<string, byte[]>();
            this._assetsByName = new Dictionary<string, List<UserAssetsController.AssetDescription>>();
            if (!Directory.Exists(this._libraryPath))
            {
                Directory.CreateDirectory(this._libraryPath);
            }
            if (!File.Exists(this._libraryFilePath))
            {
                this._library = new UserAssetsController.AssetLibrary();
                this._library.assetDescriptions = new Dictionary<string, UserAssetsController.AssetDescription>();
                this.SaveLibrary();
            }
            else
            {
                byte[] bytes = File.ReadAllBytes(this._libraryFilePath);
                this._library = SerializationUtility.DeserializeValue<UserAssetsController.AssetLibrary>(bytes, DataFormat.Binary, null);
                this.UpdateNameDescriptions();
            }
        }

        // Token: 0x06000253 RID: 595 RVA: 0x00010B8A File Offset: 0x0000ED8A
        public bool DoesAssetExist(string md5)
        {
            return this._library.assetDescriptions.ContainsKey(md5);
        }

        // Token: 0x06000254 RID: 596 RVA: 0x00010BA0 File Offset: 0x0000EDA0
        public UserAssetsController.AssetDescription GetAssetDescription(string md5)
        {
            UserAssetsController.AssetDescription result;
            this._library.assetDescriptions.TryGetValue(md5, out result);
            return result;
        }

        // Token: 0x06000256 RID: 598 RVA: 0x00010C8C File Offset: 0x0000EE8C
        public void UserAssetReachedZeroReferenceCount(string md5HashString)
        {
            UserAssetsController.AssetDescription assetDescription;
            if (this._library.assetDescriptions.TryGetValue(md5HashString, out assetDescription) && assetDescription.isExternalAsset)
            {
                this._library.assetDescriptions.Remove(md5HashString);
                this._externalUserAssetsData.Remove(md5HashString);
            }
        }

        // Token: 0x06000257 RID: 599 RVA: 0x00010CD8 File Offset: 0x0000EED8
        public void PurgeAssets()
        {
            this._externalUserAssetsData.Clear();
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, UserAssetsController.AssetDescription> keyValuePair in this._library.assetDescriptions)
            {
                if (keyValuePair.Value.isExternalAsset)
                {
                    list.Add(keyValuePair.Key);
                }
            }
            foreach (string key in list)
            {
                this._library.assetDescriptions.Remove(key);
            }
            this.UpdateNameDescriptions();
        }

        // Token: 0x06000258 RID: 600 RVA: 0x00010DA8 File Offset: 0x0000EFA8
        private void SaveLibrary()
        {
            UserAssetsController.AssetLibrary assetLibrary = new UserAssetsController.AssetLibrary();
            assetLibrary.assetDescriptions = new Dictionary<string, UserAssetsController.AssetDescription>();
            foreach (KeyValuePair<string, UserAssetsController.AssetDescription> keyValuePair in this._library.assetDescriptions)
            {
                if (!keyValuePair.Value.isExternalAsset)
                {
                    assetLibrary.assetDescriptions.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
            byte[] bytes = SerializationUtility.SerializeValue<UserAssetsController.AssetLibrary>(assetLibrary, DataFormat.Binary, null);
            File.WriteAllBytes(this._libraryFilePath, bytes);
        }

        // Token: 0x06000259 RID: 601 RVA: 0x00010E48 File Offset: 0x0000F048
        private void UpdateNameDescriptions()
        {
            this._assetsByName.Clear();
            foreach (UserAssetsController.AssetDescription assetDescription in this._library.assetDescriptions.Values)
            {
                string key = assetDescription.name.ToUpperInvariant();
                List<UserAssetsController.AssetDescription> list;
                if (!this._assetsByName.TryGetValue(key, out list))
                {
                    list = new List<UserAssetsController.AssetDescription>();
                    this._assetsByName.Add(key, list);
                }
                list.Add(assetDescription);
            }
        }

        // Token: 0x0600025A RID: 602 RVA: 0x00010EE0 File Offset: 0x0000F0E0
        public void RenameAsset(UserAssetsController.AssetDescription assetDescription, string newName)
        {
            assetDescription.name = newName.ToUpperInvariant();
            this.UpdateNameDescriptions();
            this.SaveLibrary();
        }

        // Token: 0x0600025D RID: 605 RVA: 0x0001100C File Offset: 0x0000F20C
        private List<UserAssetsController.AssetDescription> GetAllAssets()
        {
            return this._library.assetDescriptions.Values.ToList<UserAssetsController.AssetDescription>();
        }

        // Token: 0x0600025E RID: 606 RVA: 0x00011024 File Offset: 0x0000F224
        private List<UserAssetsController.AssetDescription> GetAllImages()
        {
            List<UserAssetsController.AssetDescription> list = new List<UserAssetsController.AssetDescription>();
            foreach (UserAssetsController.AssetDescription assetDescription in this._library.assetDescriptions.Values)
            {
                if (assetDescription.type == UserAssetsController.AssetDescription.Types.Image)
                {
                    list.Add(assetDescription);
                }
            }
            return list;
        }

        // Token: 0x04000279 RID: 633
        public string subfolder;

        // Token: 0x0400027A RID: 634
        private string _libraryPath;

        // Token: 0x0400027B RID: 635
        private string _libraryFilePath;

        // Token: 0x0400027C RID: 636
        private UserAssetsController.AssetLibrary _library;

        // Token: 0x0400027D RID: 637
        private Dictionary<string, byte[]> _externalUserAssetsData;

        // Token: 0x0400027E RID: 638
        private Dictionary<string, List<UserAssetsController.AssetDescription>> _assetsByName;

        // Token: 0x020002EE RID: 750
        [Serializable]
        public class AssetDescription
        {
            // Token: 0x040019E2 RID: 6626
            public string name;

            // Token: 0x040019E3 RID: 6627
            public string filename;

            // Token: 0x040019E4 RID: 6628
            public string originalFilename;

            // Token: 0x040019E5 RID: 6629
            public string previewName;

            // Token: 0x040019E6 RID: 6630
            public string md5HashString;

            // Token: 0x040019E7 RID: 6631
            public UserAssetsController.AssetDescription.Types type;

            // Token: 0x040019E8 RID: 6632
            public bool isExternalAsset;

            // Token: 0x020004BA RID: 1210
            public enum Types
            {
                // Token: 0x040020D1 RID: 8401
                Image,
                // Token: 0x040020D2 RID: 8402
                All = 100
            }
        }

        // Token: 0x020002EF RID: 751
        [Serializable]
        private class AssetLibrary
        {
            // Token: 0x040019E9 RID: 6633
            public Dictionary<string, UserAssetsController.AssetDescription> assetDescriptions;
        }
    }
}
