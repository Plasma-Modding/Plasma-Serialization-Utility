using System;
using System.Collections.Generic;
using System.Linq;

namespace PlasmaFileReader.Plasma.Classes
{
	// Token: 0x02000187 RID: 391
	public class SerializedWorldMetaData
	{
		// Token: 0x06000E98 RID: 3736 RVA: 0x0004B024 File Offset: 0x00049224
		public SerializedWorldMetaData()
		{
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x0004B02C File Offset: 0x0004922C
		public SerializedWorldMetaData(SerializedWorldMetaData metaData)
		{
			this.type = metaData.type;
			this.state = metaData.state;
			this.name = metaData.name;
			this.description = metaData.description;
			this.biomeId = metaData.biomeId;
			this.isStaged = metaData.isStaged;
			this.devices = metaData.devices;
			this.creationDate = metaData.creationDate;
			this.lastSaveDate = metaData.lastSaveDate;
			this.authorSteamId = metaData.authorSteamId;
			this.publishedFileId = metaData.publishedFileId;
			this.publishedDate = metaData.publishedDate;
			this.publishedVersion = metaData.publishedVersion;
			byte[] array = metaData.previewImage;
			this.previewImage = ((array != null) ? array.ToArray<byte>() : null);
			this.userPreviewImage = metaData.userPreviewImage;
			this.updateDate = metaData.updateDate;
			this.filename = metaData.filename;
			this.isTemporary = metaData.isTemporary;
			this.isWorldFileMissing = metaData.isWorldFileMissing;
			this.previewUrl = metaData.previewUrl;
			this.publishedTag = metaData.publishedTag;
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x0004B148 File Offset: 0x00049348
		public static SerializedWorldMetaData CreateMetaData(WorkshopController.WorkshopItemDetails workshopItemDetails, SerializedWorldMetaData.Types type)
		{
			SerializedWorldMetaData serializedWorldMetaData = new SerializedWorldMetaData();
			serializedWorldMetaData.name = workshopItemDetails.title;
			serializedWorldMetaData.description = workshopItemDetails.description;
			serializedWorldMetaData.type = type;
			serializedWorldMetaData.publishedFileId = workshopItemDetails.publishedFileId;
			serializedWorldMetaData.publishedDate = workshopItemDetails.publishedDate;
			serializedWorldMetaData.updateDate = workshopItemDetails.updateDate;
			serializedWorldMetaData.authorSteamId = workshopItemDetails.authorSteamId;
			serializedWorldMetaData.devices = workshopItemDetails.numberOfDevices;
			serializedWorldMetaData.biomeId = (BiomeGestaltEnum)workshopItemDetails.biomeId;
			serializedWorldMetaData.isStaged = workshopItemDetails.isStaged;
			serializedWorldMetaData.publishedTag = workshopItemDetails.tag;
			serializedWorldMetaData.previewUrl = workshopItemDetails.previewUrl;
			serializedWorldMetaData.state = SerializedWorldMetaData.States.None;
			if ((workshopItemDetails.itemState & EItemState.k_EItemStateSubscribed) != EItemState.k_EItemStateNone)
			{
				if ((workshopItemDetails.itemState & EItemState.k_EItemStateInstalled) != EItemState.k_EItemStateNone && (workshopItemDetails.itemState & EItemState.k_EItemStateDownloading) == EItemState.k_EItemStateNone && (workshopItemDetails.itemState & EItemState.k_EItemStateDownloadPending) == EItemState.k_EItemStateNone)
				{
					serializedWorldMetaData.state = SerializedWorldMetaData.States.Subscribed;
				}
				else if ((workshopItemDetails.itemState & EItemState.k_EItemStateDownloading) != EItemState.k_EItemStateNone || (workshopItemDetails.itemState & EItemState.k_EItemStateDownloadPending) != EItemState.k_EItemStateNone)
				{
					serializedWorldMetaData.state = SerializedWorldMetaData.States.Downloading;
				}
			}
			return serializedWorldMetaData;
		}

		// Token: 0x04000C5B RID: 3163
		public SerializedWorldMetaData.Types type;

		// Token: 0x04000C5C RID: 3164
		public SerializedWorldMetaData.States state;

		// Token: 0x04000C5D RID: 3165
		public string name;

		// Token: 0x04000C5E RID: 3166
		public string description;

		// Token: 0x04000C5F RID: 3167
		public BiomeGestaltEnum biomeId;

		// Token: 0x04000C60 RID: 3168
		public bool isStaged;

		// Token: 0x04000C61 RID: 3169
		public int devices;

		// Token: 0x04000C62 RID: 3170
		public DateTime creationDate;

		// Token: 0x04000C63 RID: 3171
		public DateTime lastSaveDate;

		// Token: 0x04000C64 RID: 3172
		public ulong authorSteamId;

		// Token: 0x04000C65 RID: 3173
		public ulong publishedFileId;

		// Token: 0x04000C66 RID: 3174
		public DateTime publishedDate;

		// Token: 0x04000C67 RID: 3175
		public DateTime updateDate;

		// Token: 0x04000C68 RID: 3176
		public string publishedVersion;

		// Token: 0x04000C69 RID: 3177
		public byte[] previewImage;

		// Token: 0x04000C6A RID: 3178
		public bool userPreviewImage;

		// Token: 0x04000C6B RID: 3179
		public string filename;

		// Token: 0x04000C6C RID: 3180
		public bool isTemporary;

		// Token: 0x04000C6D RID: 3181
		public bool isWorldFileMissing;

		// Token: 0x04000C6E RID: 3182
		public string previewUrl;

		// Token: 0x04000C6F RID: 3183
		public string publishedTag;

		// Token: 0x04000C70 RID: 3184
		public const int screenshotWidth = 1014;

		// Token: 0x04000C71 RID: 3185
		public const int screenshotHeight = 624;

		// Token: 0x04000C72 RID: 3186
		public static Dictionary<SerializedWorldMetaData.States, string> statusLabels = new Dictionary<SerializedWorldMetaData.States, string>
	{
		{
			SerializedWorldMetaData.States.None,
			"NOT SHARED"
		},
		{
			SerializedWorldMetaData.States.Subscribed,
			"SUBSCRIBED"
		},
		{
			SerializedWorldMetaData.States.Published,
			"SHARED"
		},
		{
			SerializedWorldMetaData.States.Downloading,
			"DOWNLOADING"
		},
		{
			SerializedWorldMetaData.States.NeedsUpdate,
			"SHARED; CAN BE UPDATED"
		}
	};

		// Token: 0x020003C3 RID: 963
		public enum Types
		{
			// Token: 0x04001D3F RID: 7487
			Local,
			// Token: 0x04001D40 RID: 7488
			Discover,
			// Token: 0x04001D41 RID: 7489
			Subscribed
		}

		// Token: 0x020003C4 RID: 964
		public enum States
		{
			// Token: 0x04001D43 RID: 7491
			None,
			// Token: 0x04001D44 RID: 7492
			Published,
			// Token: 0x04001D45 RID: 7493
			NeedsUpdate,
			// Token: 0x04001D46 RID: 7494
			Subscribed,
			// Token: 0x04001D47 RID: 7495
			Downloading
		}
	}
}
