using System;
using UnityEngine;


namespace Plasma.Classes
{
    // Token: 0x0200022B RID: 555
    public static class QuaternionUtil
    {
        // Token: 0x06000069 RID: 105 RVA: 0x00003F20 File Offset: 0x00002120
        public static Quaternion PlasmaAngles(in Vector3 pitchYawRoll)
        {
            Vector3 euler = pitchYawRoll;
            euler.z += pitchYawRoll.x / 90f * pitchYawRoll.y;
            return Quaternion.Euler(euler);
        }

        // Token: 0x0600006A RID: 106 RVA: 0x00003F58 File Offset: 0x00002158
        public static bool AreEqualEnough(in Quaternion a, in Quaternion b)
        {
            return (Quaternion.Inverse(a) * b).GetSafeSignedEuler().sqrMagnitude < 9.848419E-09f;
        }

        // Token: 0x0600006B RID: 107 RVA: 0x00003F90 File Offset: 0x00002190
        public static void AssertEqualEnough(in Quaternion a, in Quaternion b)
        {
            float sqrMagnitude = (Quaternion.Inverse(a) * b).GetSafeSignedEuler().sqrMagnitude;
        }

        // Token: 0x04000C23 RID: 3107
        public static readonly Quaternion rotate180AroundX = Quaternion.Euler(180f, 0f, 0f);
    }
}
