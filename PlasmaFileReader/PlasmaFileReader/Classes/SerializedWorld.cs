using System;
using System.Runtime.Serialization;
using UnityEngine;
namespace PlasmaFileReader.Plasma.Classes
{
	// Token: 0x02000186 RID: 390
	public class SerializedWorld
	{
		// Token: 0x06000E96 RID: 3734 RVA: 0x0004AFC3 File Offset: 0x000491C3
		public SerializedWorld()
		{
			this.version = 2;
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x0004AFD4 File Offset: 0x000491D4
		[OnDeserialized]
		public void Defaults()
		{
			if (this.version < 1)
			{
				this.devicesMetaData = new string[this.devices.Length];
				this.devicesSaveCounter = new int[this.devices.Length];
			}
			if (this.version < 2)
			{
				this.globalPermissions = WorldController.GlobalPermissions.All;
			}
		}

		// Token: 0x04000C45 RID: 3141
		public SerializedDevice[] devices;

		// Token: 0x04000C46 RID: 3142
		public string[] devicesMetaData;

		// Token: 0x04000C47 RID: 3143
		public int[] devicesSaveCounter;

		// Token: 0x04000C48 RID: 3144
		public SerializedAssetsLibrary assetsLibrary;

		// Token: 0x04000C49 RID: 3145
		public float timeOfDay;

		// Token: 0x04000C4A RID: 3146
		public float timeSpeed;

		// Token: 0x04000C4B RID: 3147
		public int assetControllerUniqueId;

		// Token: 0x04000C4C RID: 3148
		public Vector3 playerPosition;

		// Token: 0x04000C4D RID: 3149
		public Vector3 playerOrientation;

		// Token: 0x04000C4E RID: 3150
		public SerializedPropsManager dynamicPropsManager;

		// Token: 0x04000C4F RID: 3151
		public WorldController.WorldPermissions permissions;

		// Token: 0x04000C50 RID: 3152
		public WorldController.GlobalPermissions globalPermissions;

		// Token: 0x04000C51 RID: 3153
		public bool hasMountedDevice;

		// Token: 0x04000C52 RID: 3154
		public int mountedDeviceId;

		// Token: 0x04000C53 RID: 3155
		public int mountedComponentId;

		// Token: 0x04000C54 RID: 3156
		public BuildReplayCanvas.SerializedData buildReplaySerializedData;

		// Token: 0x04000C55 RID: 3157
		public float stagedTimeOfDay;

		// Token: 0x04000C56 RID: 3158
		public float stagedTimeSpeed;

		// Token: 0x04000C57 RID: 3159
		public Vector3 stagedPlayerPosition;

		// Token: 0x04000C58 RID: 3160
		public Vector3 stagedPlayerOrientation;

		// Token: 0x04000C59 RID: 3161
		public int version;

		// Token: 0x04000C5A RID: 3162
		[NonSerialized]
		private const int currentVersion = 2;
	}
}
