using System;

namespace Plasma.Classes
{
    public struct ArticulationDrive
    {
        // Token: 0x0400006E RID: 110
        public float lowerLimit;

        // Token: 0x0400006F RID: 111
        public float upperLimit;

        // Token: 0x04000070 RID: 112
        public float stiffness;

        // Token: 0x04000071 RID: 113
        public float damping;

        // Token: 0x04000072 RID: 114
        public float forceLimit;

        // Token: 0x04000073 RID: 115
        public float target;

        // Token: 0x04000074 RID: 116
        public float targetVelocity;
    }
}
