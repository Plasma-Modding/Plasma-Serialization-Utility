using System;
using System.Collections.Generic;

namespace PlasmaFileReader.Plasma.Classes
{
	// Token: 0x02000222 RID: 546
	public class EasyFSMController : Controller
	{
		// Token: 0x06001337 RID: 4919 RVA: 0x0006319A File Offset: 0x0006139A
		public override void Init()
		{
			//this._stateMachines = new Dictionary<EasyFSMEnum, EasyStateMachine>();
			//this._instancedStateMachines = new Dictionary<int, EasyStateMachine>();
		}

		// Token: 0x04000FAC RID: 4012
		private Dictionary<EasyFSMEnum, EasyStateMachine> _stateMachines;

		// Token: 0x04000FAD RID: 4013
		private Dictionary<int, EasyStateMachine> _instancedStateMachines;

		// Token: 0x04000FAE RID: 4014
		private int _instanceIndex;
	}
}
