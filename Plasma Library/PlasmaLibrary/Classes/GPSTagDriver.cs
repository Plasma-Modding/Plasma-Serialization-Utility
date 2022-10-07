using System;
using UnityEngine;
namespace Plasma.Classes
{
	// Token: 0x02000089 RID: 137
	public class GPSTagDriver : ComponentDriver
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060006E6 RID: 1766 RVA: 0x00027CB0 File Offset: 0x00025EB0
		public Vector3 worldPosition
		{
			get
			{
				return this._transform.position;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060006E7 RID: 1767 RVA: 0x00027CBD File Offset: 0x00025EBD
		public Vector3 velocity
		{
			get
			{
				return this._articulationBody.velocity;
			}
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00027CCA File Offset: 0x00025ECA
		protected override void OnAgentSet()
		{
			//this._idProperty = base.agent.GetRuntimeProperty(1);
			this._transform = this._component.GetSubComponent(0).articulationCollidersGroup;
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x00027CF5 File Offset: 0x00025EF5
		public override void OnSolidEnter()
		{
			//Controllers.gpsController.RegisterTag(this._idProperty.GetValueNumberAsInteger(), this);
			this._articulationBody = this._component.ownerArticulationBody;
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x00027D1E File Offset: 0x00025F1E
		public override void OnWireframeEnter()
		{
			//Controllers.gpsController.UnregisterTag(this._idProperty.GetValueNumberAsInteger());
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x00027D35 File Offset: 0x00025F35
		public override void OnMount()
		{
			this._transform = this._component.GetSubComponent(0).rigidbodyCollidersGroup;
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x00027D4E File Offset: 0x00025F4E
		public override void OnUnmount()
		{
			this._transform = this._component.GetSubComponent(0).articulationCollidersGroup;
		}

		// Token: 0x04000639 RID: 1593
		private AgentProperty _idProperty;

		// Token: 0x0400063A RID: 1594
		private Transform _transform;

		// Token: 0x0400063B RID: 1595
		private ArticulationBody _articulationBody;
	}
}