using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;

namespace PlasmaFileReader.Plasma.Classes
{
    // Token: 0x02000185 RID: 389
    public class SerializedDeviceMetaData
    {
        // Token: 0x06000E92 RID: 3730 RVA: 0x0004AEB5 File Offset: 0x000490B5
        public SerializedDeviceMetaData()
        {
        }

        // Token: 0x06000E93 RID: 3731 RVA: 0x0004AEC0 File Offset: 0x000490C0
        public SerializedDeviceMetaData(SerializedDeviceMetaData metaData)
        {
            this.type = metaData.type;
            this.state = metaData.state;
            this.name = metaData.name;
            this.description = metaData.description;
            this.saveCounter = metaData.saveCounter;
            this.creationDate = metaData.creationDate;
            this.lastSaveDate = metaData.lastSaveDate;
            this.authorSteamId = metaData.authorSteamId;
            this.publishedFileId = metaData.publishedFileId;
            this.publishedDate = metaData.publishedDate;
            this.updateDate = metaData.updateDate;
            this.components = metaData.components;
            this.mass = metaData.mass;
            this.previewImage = metaData.previewImage;
            this.filename = metaData.filename;
            this.isDeviceFileMissing = metaData.isDeviceFileMissing;
            this.isModule = metaData.isModule;
        }

        // Token: 0x06000E94 RID: 3732 RVA: 0x0004AFA0 File Offset: 0x000491A0
        public static SerializedDeviceMetaData CreateMetaData(WorkshopController.WorkshopItemDetails workshopItemDetails, SerializedDeviceMetaData.Types type)
        {
            SerializedDeviceMetaData serializedDeviceMetaData = new SerializedDeviceMetaData();
            serializedDeviceMetaData.name = workshopItemDetails.title;
            serializedDeviceMetaData.description = workshopItemDetails.description;
            serializedDeviceMetaData.type = type;
            serializedDeviceMetaData.publishedFileId = workshopItemDetails.publishedFileId;
            serializedDeviceMetaData.publishedDate = workshopItemDetails.publishedDate;
            serializedDeviceMetaData.updateDate = workshopItemDetails.updateDate;
            serializedDeviceMetaData.authorSteamId = workshopItemDetails.authorSteamId;
            serializedDeviceMetaData.components = workshopItemDetails.numberOfComponents;
            serializedDeviceMetaData.mass = workshopItemDetails.mass;
            serializedDeviceMetaData.previewUrl = workshopItemDetails.previewUrl;
            serializedDeviceMetaData.isModule = workshopItemDetails.isModule;
            serializedDeviceMetaData.state = SerializedDeviceMetaData.States.None;
            if ((workshopItemDetails.itemState & EItemState.k_EItemStateSubscribed) != EItemState.k_EItemStateNone)
            {
                if ((workshopItemDetails.itemState & EItemState.k_EItemStateInstalled) != EItemState.k_EItemStateNone && (workshopItemDetails.itemState & EItemState.k_EItemStateDownloading) == EItemState.k_EItemStateNone && (workshopItemDetails.itemState & EItemState.k_EItemStateDownloadPending) == EItemState.k_EItemStateNone)
                {
                    serializedDeviceMetaData.state = SerializedDeviceMetaData.States.Subscribed;
                }
                else if ((workshopItemDetails.itemState & EItemState.k_EItemStateDownloading) != EItemState.k_EItemStateNone || (workshopItemDetails.itemState & EItemState.k_EItemStateDownloadPending) != EItemState.k_EItemStateNone)
                {
                    serializedDeviceMetaData.state = SerializedDeviceMetaData.States.Downloading;
                }
            }
            return serializedDeviceMetaData;
        }

        // Token: 0x04000C35 RID: 3125
        public SerializedDeviceMetaData.Types type;

        // Token: 0x04000C36 RID: 3126
        public SerializedDeviceMetaData.States state;

        // Token: 0x04000C37 RID: 3127
        public string name;

        // Token: 0x04000C38 RID: 3128
        public string description;

        // Token: 0x04000C39 RID: 3129
        public int saveCounter;

        // Token: 0x04000C3A RID: 3130
        public DateTime creationDate;

        // Token: 0x04000C3B RID: 3131
        public DateTime lastSaveDate;

        // Token: 0x04000C3C RID: 3132
        public ulong authorSteamId;

        // Token: 0x04000C3D RID: 3133
        public ulong publishedFileId;

        // Token: 0x04000C3E RID: 3134
        public DateTime publishedDate;

        // Token: 0x04000C3F RID: 3135
        public DateTime updateDate;

        // Token: 0x04000C40 RID: 3136
        public int components;

        // Token: 0x04000C41 RID: 3137
        public float mass;

        // Token: 0x04000C42 RID: 3138
        public byte[] previewImage;

        // Token: 0x04000C43 RID: 3139
        public string filename;

        // Token: 0x04000C44 RID: 3140
        public bool isDeviceFileMissing;

        // Token: 0x04000C45 RID: 3141
        public string previewUrl;

        // Token: 0x04000C46 RID: 3142
        public bool isModule;

        // Token: 0x04000C47 RID: 3143
        public static Dictionary<SerializedDeviceMetaData.States, string> statusLabels = new Dictionary<SerializedDeviceMetaData.States, string>
    {
        {
            SerializedDeviceMetaData.States.None,
            "NOT SHARED"
        },
        {
            SerializedDeviceMetaData.States.Subscribed,
            "SUBSCRIBED"
        },
        {
            SerializedDeviceMetaData.States.Published,
            "SHARED"
        },
        {
            SerializedDeviceMetaData.States.Downloading,
            "DOWNLOADING"
        },
        {
            SerializedDeviceMetaData.States.NeedsUpdate,
            "SHARED; CAN BE UPDATED"
        }
    };

        // Token: 0x020003C0 RID: 960
        public enum Types
        {
            // Token: 0x04001D35 RID: 7477
            Local,
            // Token: 0x04001D36 RID: 7478
            Discover,
            // Token: 0x04001D37 RID: 7479
            Subscribed
        }

        // Token: 0x020003C1 RID: 961
        public enum States
        {
            // Token: 0x04001D39 RID: 7481
            None,
            // Token: 0x04001D3A RID: 7482
            Published,
            // Token: 0x04001D3B RID: 7483
            NeedsUpdate,
            // Token: 0x04001D3C RID: 7484
            Subscribed,
            // Token: 0x04001D3D RID: 7485
            Downloading
        }
    }

}
