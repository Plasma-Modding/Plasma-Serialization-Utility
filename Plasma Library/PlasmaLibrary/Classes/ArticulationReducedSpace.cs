using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;

namespace Plasma.Classes
{
    // Token: 0x0200001B RID: 27
    public struct ArticulationReducedSpace
    {
        /*
        // Token: 0x17000031 RID: 49
        public unsafe float this[int i]
        {
            get
            {
                bool flag = i < 0 || i >= this.dofCount;
                if (flag)
                {
                    throw new IndexOutOfRangeException();
                }
                return *(ref this.x.FixedElementField + (IntPtr)i * 4);
            }
            set
            {
                bool flag = i < 0 || i >= this.dofCount;
                if (flag)
                {
                    throw new IndexOutOfRangeException();
                }
                *(ref this.x.FixedElementField + (IntPtr)i * 4) = value;
            }
        }
        */

        // Token: 0x06000056 RID: 86 RVA: 0x00002855 File Offset: 0x00000A55
        public ArticulationReducedSpace(float a)
        {
            //this.x.FixedElementField = a;
            this.dofCount = 1;
        }
        /*
        // Token: 0x06000057 RID: 87 RVA: 0x0000286C File Offset: 0x00000A6C
        public unsafe ArticulationReducedSpace(float a, float b)
        {
            this.x.FixedElementField = a;
            *(ref this.x.FixedElementField + 4) = b;
            this.dofCount = 2;
        }

        // Token: 0x06000058 RID: 88 RVA: 0x00002892 File Offset: 0x00000A92
        public unsafe ArticulationReducedSpace(float a, float b, float c)
        {
            this.x.FixedElementField = a;
            *(ref this.x.FixedElementField + 4) = b;
            *(ref this.x.FixedElementField + (IntPtr)2 * 4) = c;
            this.dofCount = 3;
        }
        */
        // Token: 0x04000075 RID: 117
        //[FixedBuffer(typeof(float), 3)]
        //private ArticulationReducedSpace.<x>e__FixedBuffer x;
        
        // Token: 0x04000076 RID: 118
        public int dofCount;
    }
}

