using System;
using UnityEngine;
namespace PlasmaFileReader.Plasma.Classes
{
	// Token: 0x02000087 RID: 135
	public class GPSDriver : ComponentDriver
	{
		// Token: 0x060006E0 RID: 1760 RVA: 0x00027BF5 File Offset: 0x00025DF5
		protected override void OnAgentSet()
		{
			//this._positionXProperty = base.agent.GetRuntimeProperty(1);
			//this._positionZProperty = base.agent.GetRuntimeProperty(2);
			this._transform = this._component.GetSubComponent(0).articulationCollidersGroup;
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x00027C32 File Offset: 0x00025E32
		public override void OnMount()
		{
			this._transform = this._component.GetSubComponent(0).rigidbodyCollidersGroup;
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x00027C4B File Offset: 0x00025E4B
		public override void OnUnmount()
		{
			this._transform = this._component.GetSubComponent(0).articulationCollidersGroup;
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x00027C64 File Offset: 0x00025E64
		public override void UpdateConcreteProperties()
		{
			//this._positionXProperty.SetValueNumber(this._transform.position.x, false);
			//this._positionZProperty.SetValueNumber(this._transform.position.z, false);
		}

		// Token: 0x04000636 RID: 1590
		private AgentProperty _positionXProperty;

		// Token: 0x04000637 RID: 1591
		private AgentProperty _positionZProperty;

		// Token: 0x04000638 RID: 1592
		private Transform _transform;
	}
}
