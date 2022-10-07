using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace PlasmaFileReader.Plasma.Classes
{
	// Token: 0x02000183 RID: 387
	public class SerializedComponent
	{
		// Token: 0x06000E8D RID: 3725 RVA: 0x0004A95B File Offset: 0x00048B5B
		public SerializedComponent()
		{
			this.version = 4;
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x0004A96C File Offset: 0x00048B6C
		public SerializedComponent(ComponentHandler componentHandler)
		{
			this.version = 4;
			this.agentId = new AgentId(componentHandler.agentId);
			this.position = componentHandler.wireframePosition;
			this.rotation = componentHandler.wireframeRotation;
			this.scale = componentHandler.scale;
			this.colorIndex = componentHandler.colorizer.colorId;
			this.altColorIndex = componentHandler.colorizer.altColorId;
			this.parentPositionReferenceFrame = componentHandler.parentReferenceFrame.position;
			this.childPositionReferenceFrame = componentHandler.childReferenceFrame.position;
			this.parentRotationReferenceFrame = componentHandler.parentReferenceFrame.rotation;
			this.childRotationReferenceFrame = componentHandler.childReferenceFrame.rotation;
			this.attachmentReferenceFrame = componentHandler.attachmentReferenceFrame;
			this.parentComponentId = ((componentHandler.parentSubComponent != null) ? componentHandler.parentSubComponent.component.guid : -1);
			this.parentSubComponentId = ((componentHandler.parentSubComponent != null) ? componentHandler.parentSubComponent.subComponentIndex : -1);
			this.childSubComponentId = ((componentHandler.parentSubComponent != null) ? componentHandler.childSubComponent.subComponentIndex : -1);
			this.pitchYawRoll = componentHandler.pitchYawRoll;
			this.extrusionAmount = componentHandler.attachmentPointOffset;
			this.baseComponentInternalIndex = componentHandler.baseSubComponent.internalSubComponentIndex;
			this.childSocketIndex = -1;
			this.parentSocketIndex = -1;
			this.massCategory = componentHandler.massCategory;
			this.massMultiplier = componentHandler.massMultiplier;
			this.bounciness = componentHandler.bounciness;
			this.staticFriction = componentHandler.staticFriction;
			this.dynamicFriction = componentHandler.dynamicFriction;
			this.audioVolume = componentHandler.audioVolume;
			this.parentSnappingObjectIndex = ((componentHandler.parentSnappingObject != null) ? componentHandler.parentSnappingObject.index : -1);
			if (componentHandler.parentSubComponent != null && componentHandler.childAttachmentSocket != null)
			{
				this.childSocketIndex = componentHandler.childAttachmentSocket.index;
			}
			if (componentHandler.parentAttachmentSocket != null)
			{
				this.parentSocketIndex = componentHandler.parentAttachmentSocket.index;
			}
			List<SerializedSocket> list = new List<SerializedSocket>();
			foreach (FemaleSocketPoint femaleSocketPoint in componentHandler.GetCustomSockets())
			{
				list.Add(new SerializedSocket
				{
					ownerSubComponentIndex = femaleSocketPoint.owner.subComponentIndex,
					index = femaleSocketPoint.index,
					localPosition = femaleSocketPoint.transform.localPosition,
					localRotation = femaleSocketPoint.transform.localRotation
				});
			}
			this.sockets = list.ToArray();
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x0004AC28 File Offset: 0x00048E28
		public List<SerializedComponent> GetChildrenRecursive()
		{
			List<SerializedComponent> list = new List<SerializedComponent>();
			list.Add(this);
			foreach (SerializedComponent serializedComponent in this.children)
			{
				list.AddRange(serializedComponent.GetChildrenRecursive());
			}
			return list;
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x0004AC68 File Offset: 0x00048E68
		[OnDeserialized]
		public void Defaults()
		{
			if (this.version < 1)
			{
				//this.massMultiplier = PhysicsUtilities.GetMassMultiplierForCategory(this.massCategory);
				this.dynamicFriction = 0.6f;
				this.staticFriction = 0.6f;
			}
			if (this.version < 2 && this.agentId.agentGestaltId == AgentGestaltEnum.Rod)
			{
				this.scale.y = this.scale.y * 2f;
				this.childPositionReferenceFrame.y = this.childPositionReferenceFrame.y * 0.5f;
				foreach (SerializedComponent serializedComponent in this.children)
				{
					serializedComponent.parentPositionReferenceFrame.y = serializedComponent.parentPositionReferenceFrame.y * 0.5f;
				}
			}
			if (this.version < 3)
			{
				this.parentSocketIndex = -1;
			}
			if (this.version < 4 && this.agentId.agentGestaltId == AgentGestaltEnum.Cube)
			{
				this.scale *= 4f;
				this.childPositionReferenceFrame *= 0.25f;
				SerializedComponent[] array = this.children;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].parentPositionReferenceFrame *= 0.25f;
				}
			}
		}

		// Token: 0x04000C12 RID: 3090
		public AgentId agentId;

		// Token: 0x04000C13 RID: 3091
		public Vector3 position;

		// Token: 0x04000C14 RID: 3092
		public Quaternion rotation;

		// Token: 0x04000C15 RID: 3093
		public Vector3 scale;

		// Token: 0x04000C16 RID: 3094
		public int colorIndex;

		// Token: 0x04000C17 RID: 3095
		public int altColorIndex;

		// Token: 0x04000C18 RID: 3096
		public int baseComponentInternalIndex;

		// Token: 0x04000C19 RID: 3097
		public AgentGestalt.MassCategories massCategory;

		// Token: 0x04000C1A RID: 3098
		public float massMultiplier;

		// Token: 0x04000C1B RID: 3099
		public float bounciness;

		// Token: 0x04000C1C RID: 3100
		public float dynamicFriction;

		// Token: 0x04000C1D RID: 3101
		public float staticFriction;

		// Token: 0x04000C1E RID: 3102
		public float audioVolume;

		// Token: 0x04000C1F RID: 3103
		public int version;

		// Token: 0x04000C20 RID: 3104
		public Vector3 parentPositionReferenceFrame;

		// Token: 0x04000C21 RID: 3105
		public Vector3 childPositionReferenceFrame;

		// Token: 0x04000C22 RID: 3106
		public Quaternion parentRotationReferenceFrame;

		// Token: 0x04000C23 RID: 3107
		public Quaternion childRotationReferenceFrame;

		// Token: 0x04000C24 RID: 3108
		public Quaternion attachmentReferenceFrame;

		// Token: 0x04000C25 RID: 3109
		public int parentComponentId;

		// Token: 0x04000C26 RID: 3110
		public int parentSubComponentId;

		// Token: 0x04000C27 RID: 3111
		public int childSubComponentId;

		// Token: 0x04000C28 RID: 3112
		public Vector3 pitchYawRoll;

		// Token: 0x04000C29 RID: 3113
		public float extrusionAmount;

		// Token: 0x04000C2A RID: 3114
		public int childSocketIndex;

		// Token: 0x04000C2B RID: 3115
		public int parentSnappingObjectIndex;

		// Token: 0x04000C2C RID: 3116
		public int parentSocketIndex;

		// Token: 0x04000C2D RID: 3117
		public SerializedComponent[] children;

		// Token: 0x04000C2E RID: 3118
		public SerializedSocket[] sockets;

		// Token: 0x04000C2F RID: 3119
		[NonSerialized]
		private const int currentVersion = 4;
	}
}
