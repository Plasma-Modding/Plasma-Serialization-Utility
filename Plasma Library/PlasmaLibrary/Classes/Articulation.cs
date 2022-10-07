using System;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Plasma.Classes
{
        // Token: 0x02000141 RID: 321
    public class Articulation : MonoBehaviour
    {
        // Token: 0x1700008F RID: 143
        // (get) Token: 0x06000BC0 RID: 3008 RVA: 0x0003D45B File Offset: 0x0003B65B
        // (set) Token: 0x06000BC1 RID: 3009 RVA: 0x0003D463 File Offset: 0x0003B663
        public ComponentHandler rootComponent { get; set; }

        // Token: 0x17000090 RID: 144
        // (get) Token: 0x06000BC2 RID: 3010 RVA: 0x0003D46C File Offset: 0x0003B66C
        public List<ComponentHandler> allComponentsHierarchy
        {
            get
            {
                List<ComponentHandler> list = new List<ComponentHandler>();
                list.Add(this.rootComponent);
                this.rootComponent.GetChildren(true, list);
                return list;
            }
        }

        // Token: 0x17000091 RID: 145
        // (get) Token: 0x06000BC3 RID: 3011 RVA: 0x0003D49A File Offset: 0x0003B69A
        public ArticulationBody[] allArticulationBodies
        {
            get
            {
                return base.GetComponentsInChildren<ArticulationBody>();
            }
        }
    }

}
