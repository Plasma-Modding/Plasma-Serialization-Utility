using System;
using UnityEngine;

namespace PlasmaFileReader.Plasma.Classes
{
    // Token: 0x02000009 RID: 9
    [Serializable]
    public struct FloatRange
    {
        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000024 RID: 36 RVA: 0x00002CB0 File Offset: 0x00000EB0
        public float length
        {
            get
            {
                return this.max - this.min;
            }
        }

        // Token: 0x06000025 RID: 37 RVA: 0x00002CBF File Offset: 0x00000EBF
        public FloatRange(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        // Token: 0x06000026 RID: 38 RVA: 0x00002CCF File Offset: 0x00000ECF
        public float Random()
        {
            return UnityEngine.Random.Range(this.min, this.max);
        }

        // Token: 0x06000027 RID: 39 RVA: 0x00002CE2 File Offset: 0x00000EE2
        public float ToRatio(float value)
        {
            if (value < this.min)
            {
                return 0f;
            }
            if (value > this.max)
            {
                return 1f;
            }
            return (value - this.min) / this.length;
        }

        // Token: 0x06000028 RID: 40 RVA: 0x00002D11 File Offset: 0x00000F11
        public float FromRatio(float ratio)
        {
            ratio = Mathf.Clamp(ratio, 0f, 1f);
            return this.min + ratio * this.length;
        }

        // Token: 0x06000029 RID: 41 RVA: 0x00002D34 File Offset: 0x00000F34
        public float MidPoint()
        {
            return this.min + this.length / 2f;
        }

        // Token: 0x0600002A RID: 42 RVA: 0x00002D49 File Offset: 0x00000F49
        public bool IsWithin(float value, bool inclusive = true)
        {
            if (inclusive)
            {
                return value >= this.min && value <= this.max;
            }
            return value > this.min && value < this.max;
        }

        // Token: 0x0600002B RID: 43 RVA: 0x00002D7A File Offset: 0x00000F7A
        public float Clamp(float value)
        {
            if (this.IsWithin(value, true))
            {
                return value;
            }
            if (value < this.min)
            {
                return this.min;
            }
            return this.max;
        }

        // Token: 0x0400000B RID: 11
        public float min;

        // Token: 0x0400000C RID: 12
        public float max;
    }

}
