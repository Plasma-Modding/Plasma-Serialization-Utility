using System.Runtime.Serialization;

// Token: 0x02000181 RID: 385
public class SerializedAgent
{
	// Token: 0x06000E89 RID: 3721 RVA: 0x0004A814 File Offset: 0x00048A14
	public SerializedAgent()
	{
		this.version = 1;
	}

	// Token: 0x06000E8A RID: 3722 RVA: 0x0004A824 File Offset: 0x00048A24
	[OnDeserialized]
	public void Defaults()
	{
		if (this.version < 1)
		{
			if (this.agentId.agentGestaltId == AgentGestaltEnum.Screen || this.agentId.agentGestaltId == AgentGestaltEnum.Tablet || this.agentId.agentGestaltId == AgentGestaltEnum.Touchscreen)
			{
				Dictionary<AssetController.ResourceTypes, List<int>> dictionary = new Dictionary<AssetController.ResourceTypes, List<int>>
				{
					{
						AssetController.ResourceTypes.DynamicTexture,
						new List<int>
						{
							this.resourceIds[AssetController.ResourceTypes.DynamicTexture][0],
							this.resourceIds[AssetController.ResourceTypes.WebcamTexture][0]
						}
					}
				};
				this.resourceIds = dictionary;
			}
			if (this.agentId.agentGestaltId == AgentGestaltEnum.ImageGenerator)
			{
				Dictionary<AssetController.ResourceTypes, List<int>> dictionary2 = new Dictionary<AssetController.ResourceTypes, List<int>>
				{
					{
						AssetController.ResourceTypes.DynamicTexture,
						new List<int>
						{
							this.resourceIds[AssetController.ResourceTypes.WebcamTexture][0]
						}
					}
				};
				this.resourceIds = dictionary2;
			}
		}
	}

	// Token: 0x04000C09 RID: 3081
	public AgentId agentId;

	// Token: 0x04000C0A RID: 3082
	public Agent.ModuleInterface moduleInterface;

	// Token: 0x04000C0B RID: 3083
	public object persistentStorage;

	// Token: 0x04000C0C RID: 3084
	public object runtimeStorage;

	// Token: 0x04000C0D RID: 3085
	public Dictionary<AssetController.ResourceTypes, List<int>> resourceIds;

	// Token: 0x04000C0E RID: 3086
	public Dictionary<int, string> resourcesTextures;

	// Token: 0x04000C0F RID: 3087
	public int version;

	// Token: 0x04000C10 RID: 3088
	[NonSerialized]
	private const int currentVersion = 1;
}