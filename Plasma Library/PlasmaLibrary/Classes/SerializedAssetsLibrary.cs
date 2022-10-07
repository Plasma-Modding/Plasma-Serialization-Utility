using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plasma.Classes
{
    // Token: 0x02000182 RID: 386
    public class SerializedAssetsLibrary
    {
        // Token: 0x06000E8B RID: 3723 RVA: 0x0004A8F8 File Offset: 0x00048AF8
        public SerializedAssetsLibrary()
        {
            this.assets = new Dictionary<string, SerializedAssetsLibrary.SerializedAsset>();
        }

        // Token: 0x06000E8C RID: 3724 RVA: 0x0004A90C File Offset: 0x00048B0C
        public bool AddAsset(UserAssetsController.AssetDescription assetDescription, byte[] data, string md5HashString, int resourceId, int referenceCount)
        {
            if (!this.assets.ContainsKey(md5HashString))
            {
                SerializedAssetsLibrary.SerializedAsset serializedAsset = new SerializedAssetsLibrary.SerializedAsset();
                serializedAsset.data = data;
                serializedAsset.assetDescription = assetDescription;
                serializedAsset.savedResourceId = resourceId;
                serializedAsset.referenceCount = referenceCount;
                this.assets.Add(md5HashString, serializedAsset);
                return true;
            }
            return false;
        }

        // Token: 0x04000C11 RID: 3089
        public Dictionary<string, SerializedAssetsLibrary.SerializedAsset> assets;

        // Token: 0x020003C0 RID: 960
        public class SerializedAsset
        {
            // Token: 0x04001D30 RID: 7472
            public byte[] data;

            // Token: 0x04001D31 RID: 7473
            public int savedResourceId;

            // Token: 0x04001D32 RID: 7474
            public int referenceCount;

            // Token: 0x04001D33 RID: 7475
            public UserAssetsController.AssetDescription assetDescription;
        }
    }
}
