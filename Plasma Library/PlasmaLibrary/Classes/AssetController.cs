using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plasma.Classes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEngine;

    // Token: 0x02000035 RID: 53
    public class AssetController : Controller
    {
        // Token: 0x14000001 RID: 1
        // (add) Token: 0x06000126 RID: 294 RVA: 0x00008550 File Offset: 0x00006750
        // (remove) Token: 0x06000127 RID: 295 RVA: 0x00008584 File Offset: 0x00006784
        public static event AssetController.AssetImageAddEvent OnImageAdded;

        // Token: 0x14000002 RID: 2
        // (add) Token: 0x06000128 RID: 296 RVA: 0x000085B8 File Offset: 0x000067B8
        // (remove) Token: 0x06000129 RID: 297 RVA: 0x000085EC File Offset: 0x000067EC
        public static event AssetController.AssetImageRemoveEvent OnImagesRemoved;

        // Token: 0x14000003 RID: 3
        // (add) Token: 0x0600012A RID: 298 RVA: 0x00008620 File Offset: 0x00006820
        // (remove) Token: 0x0600012B RID: 299 RVA: 0x00008654 File Offset: 0x00006854
        public static event AssetController.AssetImageAddEvent OnImageReferenceCountUpdated;

        // Token: 0x1700001C RID: 28
        // (get) Token: 0x0600012C RID: 300 RVA: 0x00008687 File Offset: 0x00006887
        // (set) Token: 0x0600012D RID: 301 RVA: 0x0000868F File Offset: 0x0000688F
        public int uniqueAssetIndex
        {
            get
            {
                return this._uniqueAssetIndex;
            }
            set
            {
                this._uniqueAssetIndex = value;
            }
        }

        // Token: 0x0600012E RID: 302 RVA: 0x00008698 File Offset: 0x00006898
        public override void Init()
        {
            this._textures = new Dictionary<int, AssetController.ImageAssetInfo>();
            this._zombieTextures = new Dictionary<int, AssetController.ImageAssetInfo>();
            this._uniqueAssetIndex = 3;
            Texture2D texture2D = this.CreateImageAsset(0, "<EMPTY>");
            Color[] array = new Color[this.textureWidth * this.textureHeight];
            for (int i = 0; i < array.Length; i++)
            {
                array[i].r = 0f;
                array[i].g = 0f;
                array[i].b = 0f;
                array[i].a = 0f;
            }
            texture2D.SetPixels(array);
            texture2D.Apply();
            this._textures[0].referenceCount = 100000;
            this.CreateDynamicTexture(1, "<CURRENT>", false);
            base.StartCoroutine(this.RunKillZombieTextures());
        }

        // Token: 0x0600012F RID: 303 RVA: 0x00008774 File Offset: 0x00006974
        private Dictionary<AssetController.ResourceTypes, List<int>> ReserveIds(Dictionary<AssetController.ResourceTypes, int> description)
        {
            Dictionary<AssetController.ResourceTypes, List<int>> dictionary = new Dictionary<AssetController.ResourceTypes, List<int>>();
            foreach (KeyValuePair<AssetController.ResourceTypes, int> keyValuePair in description)
            {
                for (int i = 0; i < keyValuePair.Value; i++)
                {
                    List<int> list;
                    if (!dictionary.TryGetValue(keyValuePair.Key, out list))
                    {
                        list = new List<int>();
                        dictionary.Add(keyValuePair.Key, list);
                    }
                    list.Add(this._uniqueAssetIndex);
                    this._uniqueAssetIndex++;
                }
            }
            return dictionary;
        }

        // Token: 0x06000130 RID: 304 RVA: 0x00008818 File Offset: 0x00006A18
        public int ReserveIdForDynamicProperty()
        {
            int uniqueAssetIndex = this._uniqueAssetIndex;
            this._uniqueAssetIndex = uniqueAssetIndex + 1;
            return uniqueAssetIndex;
        }

        // Token: 0x06000131 RID: 305 RVA: 0x00008838 File Offset: 0x00006A38
        public Dictionary<AssetController.ResourceTypes, List<int>> ReserveResourceIdsForGestalt(AgentGestalt agentGestalt)
        {
            MethodInfo method = agentGestalt.agent.GetMethod("GetResourcesDescription", BindingFlags.Static | BindingFlags.Public);
            if (method == null && agentGestalt.agent.BaseType != typeof(Agent))
            {
                method = agentGestalt.agent.BaseType.GetMethod("GetResourcesDescription", BindingFlags.Static | BindingFlags.Public);
                if (method == null && agentGestalt.agent.BaseType.BaseType != typeof(Agent))
                {
                    method = agentGestalt.agent.BaseType.BaseType.GetMethod("GetResourcesDescription", BindingFlags.Static | BindingFlags.Public);
                }
            }
            if (method != null)
            {
                Dictionary<AssetController.ResourceTypes, int> description = (Dictionary<AssetController.ResourceTypes, int>)method.Invoke(null, null);
                return this.ReserveIds(description);
            }
            string str = "Couldn't find static method GetResourcesDescription for class '";
            Type agent = agentGestalt.agent;
            //this.LogError(str + ((agent != null) ? agent.ToString() : null) + "'");
            return null;
        }

        // Token: 0x06000133 RID: 307 RVA: 0x00008994 File Offset: 0x00006B94
        private Texture2D CreateImageAsset(int assetIndex, string info)
        {
            Texture2D texture2D = new Texture2D(this.textureWidth, this.textureHeight, this.userTextureFormat, false);
            AssetController.ImageAssetInfo imageAssetInfo = new AssetController.ImageAssetInfo(texture2D, AssetController.ResourceTypes.UserTexture, info);
            imageAssetInfo.md5 = "";
            this._textures.Add(assetIndex, imageAssetInfo);
            if (AssetController.OnImageAdded != null)
            {
                AssetController.OnImageAdded(assetIndex);
            }
            return texture2D;
        }

        // Token: 0x06000134 RID: 308 RVA: 0x000089EC File Offset: 0x00006BEC
        public void LoadExternalUserAssetTexture(string md5, byte[] pixels, int existingResourceId)
        {
            //this.Log("Adding ExternalUserAssetTexture to AssetsController: " + md5);
            AssetController.ImageAssetInfo imageAssetInfo = new AssetController.ImageAssetInfo(this.LoadTexture(pixels), AssetController.ResourceTypes.UserTexture, null);
            imageAssetInfo.md5 = md5;
            imageAssetInfo.referenceCount = 0;
            if (existingResourceId >= 0)
            {
                this._textures.Add(existingResourceId, imageAssetInfo);
                if (AssetController.OnImageAdded != null)
                {
                    AssetController.OnImageAdded(existingResourceId);
                    return;
                }
            }
            else
            {
                this._textures.Add(this._uniqueAssetIndex, imageAssetInfo);
                if (AssetController.OnImageAdded != null)
                {
                    AssetController.OnImageAdded(this._uniqueAssetIndex);
                }
                this._uniqueAssetIndex++;
            }
        }

        // Token: 0x06000135 RID: 309 RVA: 0x00008A82 File Offset: 0x00006C82
        private Texture2D LoadTexture(byte[] rawData)
        {
            Texture2D texture2D = new Texture2D(this.textureWidth, this.textureHeight, this.userTextureFormat, false);
            texture2D.wrapMode = TextureWrapMode.Clamp;
            texture2D.LoadRawTextureData(rawData);
            texture2D.Apply();
            return texture2D;
        }

        // Token: 0x06000136 RID: 310 RVA: 0x00008AB0 File Offset: 0x00006CB0
        public string GetTextureInfo(int index)
        {
            if (this._textures.ContainsKey(index))
            {
                return this._textures[index].info;
            }
            //this.LogWarning("Couldn't find a texture with index: " + index.ToString());
            return null;
        }

        // Token: 0x06000137 RID: 311 RVA: 0x00008AEC File Offset: 0x00006CEC
        public int GetTextureReferenceCount(int index)
        {
            if (index < 3)
            {
                //this.LogWarning("Cannot get texture because it has an invalid index: " + index.ToString());
                return -1;
            }
            if (this._textures.ContainsKey(index))
            {
                return this._textures[index].referenceCount;
            }
            //this.LogWarning("Couldn't find a texture with index: " + index.ToString());
            return -1;
        }

        // Token: 0x06000138 RID: 312 RVA: 0x00008B4E File Offset: 0x00006D4E
        public Texture GetTexture(int index)
        {
            if (this._textures.ContainsKey(index))
            {
                return this._textures[index].texture;
            }
            //this.LogWarning("Couldn't find a texture with index: " + index.ToString());
            return null;
        }

        // Token: 0x06000139 RID: 313 RVA: 0x00008B88 File Offset: 0x00006D88
        public Texture2D GetUserTexture(int index)
        {
            Texture texture = this.GetTexture(index);
            if (!(texture is Texture2D))
            {
                //this.LogWarning("Couldn't find user texture with index: " + index.ToString());
            }
            return texture as Texture2D;
        }

        // Token: 0x0600013A RID: 314 RVA: 0x00008BB5 File Offset: 0x00006DB5
        public RenderTexture GetDynamicTexture(int index)
        {
            Texture texture = this.GetTexture(index);
            if (!(texture is RenderTexture))
            {
                //this.LogWarning("Couldn't find dynamic texture with index: " + index.ToString());
            }
            return texture as RenderTexture;
        }

        // Token: 0x0600013B RID: 315 RVA: 0x00008BE4 File Offset: 0x00006DE4
        public int GetTextureIndexForMD5(string md5)
        {
            foreach (KeyValuePair<int, AssetController.ImageAssetInfo> keyValuePair in this._textures)
            {
                if (keyValuePair.Value.md5 == md5)
                {
                    return keyValuePair.Key;
                }
            }
            return -1;
        }

        // Token: 0x0600013C RID: 316 RVA: 0x00008C68 File Offset: 0x00006E68
        public bool DoesTextureExist(int index)
        {
            return this._textures.ContainsKey(index);
        }

        // Token: 0x0600013D RID: 317 RVA: 0x00008C78 File Offset: 0x00006E78
        public bool DoesTextureExist(string md5)
        {
            foreach (KeyValuePair<int, AssetController.ImageAssetInfo> keyValuePair in this._textures)
            {
                if (keyValuePair.Value.md5 == md5)
                {
                    return true;
                }
            }
            return false;
        }

        // Token: 0x0600013E RID: 318 RVA: 0x00008CE0 File Offset: 0x00006EE0
        public bool IsUserTexture(int index)
        {
            AssetController.ImageAssetInfo imageAssetInfo;
            if (this._textures.TryGetValue(index, out imageAssetInfo))
            {
                return imageAssetInfo.type == AssetController.ResourceTypes.UserTexture;
            }
            //this.LogVerbose("Couldn't find a texture with index: " + index.ToString());
            return false;
        }

        // Token: 0x0600013F RID: 319 RVA: 0x00008D20 File Offset: 0x00006F20
        public bool IsDynamicTexture(int index)
        {
            AssetController.ImageAssetInfo imageAssetInfo;
            if (this._textures.TryGetValue(index, out imageAssetInfo))
            {
                return imageAssetInfo.type == AssetController.ResourceTypes.DynamicTexture;
            }
            //this.LogWarning("Couldn't find a texture with index: " + index.ToString());
            return false;
        }

        // Token: 0x06000140 RID: 320 RVA: 0x00008D60 File Offset: 0x00006F60
        public bool IsWebcamTexture(int index)
        {
            AssetController.ImageAssetInfo imageAssetInfo;
            if (this._textures.TryGetValue(index, out imageAssetInfo))
            {
                return imageAssetInfo.type == AssetController.ResourceTypes.WebcamTexture;
            }
            //this.LogWarning("Couldn't find a texture with index: " + index.ToString());
            return false;
        }

        // Token: 0x06000141 RID: 321 RVA: 0x00008D9F File Offset: 0x00006F9F
        public IEnumerable<int> GetAllImageIndices()
        {
            return new List<int>(this._textures.Keys);
        }

        // Token: 0x06000142 RID: 322 RVA: 0x00008DB4 File Offset: 0x00006FB4
        public RenderTexture CreateDynamicTexture(int index, int width, int height, string info, bool uiOnly = false)
        {
            RenderTexture renderTexture = RenderTexture.active = RenderTexture.GetTemporary(width, height, uiOnly ? 0 : this.renderTextureDepth, this.renderTextureFormat);
            GL.Clear(true, true, Color.black);
            renderTexture.wrapMode = TextureWrapMode.Clamp;
            AssetController.ImageAssetInfo value = new AssetController.ImageAssetInfo(renderTexture, AssetController.ResourceTypes.DynamicTexture, info);
            this._textures.Add(index, value);
            if (AssetController.OnImageAdded != null)
            {
                AssetController.OnImageAdded(index);
            }
            //this.Log("Created a dynamic texture with index: " + index.ToString());
            if (index > this._uniqueAssetIndex)
            {
                this._uniqueAssetIndex = index + 1;
            }
            return renderTexture;
        }

        // Token: 0x06000143 RID: 323 RVA: 0x00008E46 File Offset: 0x00007046
        public RenderTexture CreateDynamicTexture(int index, string info, bool uiOnly = false)
        {
            return this.CreateDynamicTexture(index, this.textureWidth, this.textureHeight, info, uiOnly);
        }

        // Token: 0x06000145 RID: 325 RVA: 0x00008ED4 File Offset: 0x000070D4
        public void DestroyTexture(int index)
        {
            if (index < 3)
            {
                //this.LogWarning("Cannot destroy texture because it has an invalid index: " + index.ToString());
                return;
            }
            if (this._textures.ContainsKey(index))
            {
                if (!this._zombieTextures.ContainsKey(index))
                {
                    this._zombieTextures.Add(index, this._textures[index]);
                }
                if (this.IsUserTexture(index))
                {
                    //GlobalControllers.userAssetsController.UserAssetReachedZeroReferenceCount(this._textures[index].md5);
                }
                this._textures.Remove(index);
                //this.Log("Destroyed a texture with index: " + index.ToString());
            }
        }

        // Token: 0x06000146 RID: 326 RVA: 0x00008F79 File Offset: 0x00007179
        public void PurgeAssets()
        {
            this.KillZombieTextures();
            this._uniqueAssetIndex = 3;
        }

        // Token: 0x06000147 RID: 327 RVA: 0x00008F88 File Offset: 0x00007188
        private void KillZombieTextures()
        {
            List<int> list = new List<int>();
            foreach (KeyValuePair<int, AssetController.ImageAssetInfo> keyValuePair in this._zombieTextures)
            {
                if (keyValuePair.Value.type == AssetController.ResourceTypes.UserTexture || keyValuePair.Value.type == AssetController.ResourceTypes.WebcamTexture)
                {
                    UnityEngine.Object.Destroy(keyValuePair.Value.texture);
                }
                else if (keyValuePair.Value.type == AssetController.ResourceTypes.DynamicTexture)
                {
                    RenderTexture.ReleaseTemporary((RenderTexture)keyValuePair.Value.texture);
                }
                list.Add(keyValuePair.Key);
            }
            if (AssetController.OnImagesRemoved != null)
            {
                AssetController.OnImagesRemoved(list);
            }
            this._zombieTextures.Clear();
        }

        // Token: 0x06000148 RID: 328 RVA: 0x0000905C File Offset: 0x0000725C
        private IEnumerator RunKillZombieTextures()
        {
            for (; ; )
            {
                if (this._zombieTextures.Count > 0)
                {
                    this.KillZombieTextures();
                }
                yield return new WaitForSeconds(1f);
            }
            yield break;
        }

        // Token: 0x06000149 RID: 329 RVA: 0x0000906C File Offset: 0x0000726C
        public void CopyRawDataToTexture(int index, byte[] rawData)
        {
            AssetController.ImageAssetInfo imageAssetInfo;
            if (this._textures.TryGetValue(index, out imageAssetInfo) && imageAssetInfo.type == AssetController.ResourceTypes.DynamicTexture)
            {
                Texture2D texture2D = new Texture2D(this.textureWidth, this.textureHeight, this.userTextureFormat, false);
                texture2D.LoadRawTextureData(rawData);
                texture2D.Apply();
                Graphics.Blit(texture2D, (RenderTexture)imageAssetInfo.texture);
                return;
            }
            //this.LogWarning("Cannot copy raw data to texture because it has an invalid index: " + index.ToString());
        }

        // Token: 0x0600014A RID: 330 RVA: 0x000090E0 File Offset: 0x000072E0
        public void IncrementReferenceCountForTexture(int index)
        {
            if (index == 0 || index == 1)
            {
                return;
            }
            AssetController.ImageAssetInfo imageAssetInfo;
            if (this._textures.TryGetValue(index, out imageAssetInfo) && imageAssetInfo.type == AssetController.ResourceTypes.UserTexture)
            {
                imageAssetInfo.referenceCount++;
                if (AssetController.OnImageReferenceCountUpdated != null)
                {
                    AssetController.OnImageReferenceCountUpdated(index);
                    return;
                }
            }
            else
            {
                //this.LogWarning("Cannot increment reference count because it has an invalid index: " + index.ToString());
            }
        }

        // Token: 0x0600014B RID: 331 RVA: 0x00009148 File Offset: 0x00007348
        public void DecrementReferenceCountForTexture(int index)
        {
            if (index == 0 || index == 1)
            {
                return;
            }
            AssetController.ImageAssetInfo imageAssetInfo;
            if (this._textures.TryGetValue(index, out imageAssetInfo) && imageAssetInfo.type == AssetController.ResourceTypes.UserTexture)
            {
                imageAssetInfo.referenceCount--;
                if (AssetController.OnImageReferenceCountUpdated != null)
                {
                    AssetController.OnImageReferenceCountUpdated(index);
                }
                if (imageAssetInfo.referenceCount == 0)
                {
                    this.DestroyTexture(index);
                    return;
                }
            }
            else
            {
                //this.LogWarning("Cannot decrement reference count because it has an invalid index: " + index.ToString());
            }
        }

        // Token: 0x0600014C RID: 332 RVA: 0x000091BC File Offset: 0x000073BC
        private void OnDestroy()
        {
            foreach (AssetController.ImageAssetInfo imageAssetInfo in this._textures.Values)
            {
                if (imageAssetInfo.type == AssetController.ResourceTypes.DynamicTexture)
                {
                    RenderTexture.ReleaseTemporary((RenderTexture)imageAssetInfo.texture);
                }
            }
            this._textures.Clear();
        }

        // Token: 0x0400015C RID: 348
        public int textureWidth;

        // Token: 0x0400015D RID: 349
        public int textureHeight;

        // Token: 0x0400015E RID: 350
        public int thumbnailWidth;

        // Token: 0x0400015F RID: 351
        public int thumbnailHeight;

        // Token: 0x04000160 RID: 352
        public int renderTextureDepth;

        // Token: 0x04000161 RID: 353
        public RenderTextureFormat renderTextureFormat;

        // Token: 0x04000162 RID: 354
        public TextureFormat userTextureFormat;

        // Token: 0x04000163 RID: 355
        public List<string> soundEvents;

        // Token: 0x04000164 RID: 356
        public const int emptyTextureIndex = 0;

        // Token: 0x04000165 RID: 357
        public const int runtimeTextureIndex = 1;

        // Token: 0x04000166 RID: 358
        public const int emptySoundIndex = 2;

        // Token: 0x04000167 RID: 359
        public const int firstAssetIndex = 3;

        // Token: 0x04000168 RID: 360
        public const string emptyLabel = "<EMPTY>";

        // Token: 0x04000169 RID: 361
        public const string currentLabel = "<CURRENT>";

        // Token: 0x0400016A RID: 362
        private Dictionary<int, AssetController.ImageAssetInfo> _textures;

        // Token: 0x0400016B RID: 363
        private Dictionary<int, AssetController.ImageAssetInfo> _zombieTextures;

        // Token: 0x0400016C RID: 364
        private int _uniqueAssetIndex;

        // Token: 0x020002D7 RID: 727
        public enum ResourceTypes
        {
            // Token: 0x0400194E RID: 6478
            UserTexture,
            // Token: 0x0400194F RID: 6479
            DynamicTexture,
            // Token: 0x04001950 RID: 6480
            WebcamTexture,
            // Token: 0x04001951 RID: 6481
            LibrarySound
        }

        // Token: 0x020002D8 RID: 728
        private class ImageAssetInfo
        {
            // Token: 0x06001E4F RID: 7759 RVA: 0x00095CFA File Offset: 0x00093EFA
            public ImageAssetInfo(Texture texture, AssetController.ResourceTypes type, string info)
            {
                this.texture = texture;
                this.type = type;
                this.info = info;
            }

            // Token: 0x04001952 RID: 6482
            public Texture texture;

            // Token: 0x04001953 RID: 6483
            public AssetController.ResourceTypes type;

            // Token: 0x04001954 RID: 6484
            public string info;

            // Token: 0x04001955 RID: 6485
            public string md5;

            // Token: 0x04001956 RID: 6486
            public int referenceCount;
        }

        // Token: 0x020002D9 RID: 729
        // (Invoke) Token: 0x06001E51 RID: 7761
        public delegate void AssetImageAddEvent(int index);

        // Token: 0x020002DA RID: 730
        // (Invoke) Token: 0x06001E55 RID: 7765
        public delegate void AssetImageRemoveEvent(List<int> indices);
    }

}
