using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlasmaFileReader.Plasma.Classes
{
	// Token: 0x0200004E RID: 78
	public class VisorUIController : Controller
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600028A RID: 650 RVA: 0x0001316A File Offset: 0x0001136A
		public dynamic visor
		{
			get
			{
				return this._visor;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600028B RID: 651 RVA: 0x00013172 File Offset: 0x00011372
		// (set) Token: 0x0600028C RID: 652 RVA: 0x0001317A File Offset: 0x0001137A
		public Dictionary<AgentGestaltEnum, GameObject> componentLibraryItems { get; private set; }

		// Token: 0x0600028D RID: 653 RVA: 0x00013184 File Offset: 0x00011384
		public override void Init()
		{
			//GameObject target = Require.UniqueGameObjectWithTag("VisorUI");
			//this._visor = Require.Component<Visor>(target);
			this.componentLibraryItems = new Dictionary<AgentGestaltEnum, GameObject>();
			foreach (AgentGestaltEnum agentGestaltEnum in Holder.componentGestalts.Keys)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.componentItemPrefab, base.transform, false);
				//Require.Component<ComponentItem>(gameObject).Setup(agentGestaltEnum);
				gameObject.SetActive(false);
				this.componentLibraryItems.Add(agentGestaltEnum, gameObject);
			}
		}

		// Token: 0x0600028F RID: 655 RVA: 0x00013284 File Offset: 0x00011484
		public void DeactivateAllComponentLibraryItems()
		{
			foreach (GameObject gameObject in this.componentLibraryItems.Values)
			{
				if (gameObject.activeSelf)
				{
					gameObject.transform.SetParent(base.transform, false);
					gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x06000290 RID: 656 RVA: 0x000132F8 File Offset: 0x000114F8
		public void RefreshAllComponentLibraryItemsQuickbarSlot()
		{
			foreach (GameObject gameObject in this.componentLibraryItems.Values)
			{
				//gameObject.GetComponent<ComponentItem>().UpdateQuickbarSlot();
			}
		}

		// Token: 0x04000372 RID: 882
		public GameObject componentItemPrefab;

		// Token: 0x04000373 RID: 883
		private dynamic _visor;
	}
}