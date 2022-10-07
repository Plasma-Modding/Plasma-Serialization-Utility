using UnityEngine;
// Token: 0x0200022A RID: 554
public static class QuaternionExtension
{
    // Token: 0x06000065 RID: 101 RVA: 0x00003E60 File Offset: 0x00002060
    public static Vector3 GetSafeSignedEuler(this Quaternion q)
    {
        Vector3 eulerAngles = q.eulerAngles;
        eulerAngles.x = QuaternionExtension.AngleToSigned180(eulerAngles.x);
        eulerAngles.y = QuaternionExtension.AngleToSigned180(eulerAngles.y);
        eulerAngles.z = QuaternionExtension.AngleToSigned180(eulerAngles.z);
        return eulerAngles;
    }

    // Token: 0x06000066 RID: 102 RVA: 0x00003EAC File Offset: 0x000020AC
    public static float AngleToSigned180(float angle)
    {
        if (angle > 180f)
        {
            angle -= 360f;
        }
        else if (angle <= -180f)
        {
            angle += 360f;
        }
        return angle;
    }

    // Token: 0x06000067 RID: 103 RVA: 0x00003ED4 File Offset: 0x000020D4
    public static Vector3 GetPlasmaAngles(this Quaternion q)
    {
        Vector3 safeSignedEuler = q.GetSafeSignedEuler();
        safeSignedEuler.z -= safeSignedEuler.x / 90f * safeSignedEuler.y;
        return safeSignedEuler;
    }

    // Token: 0x06000068 RID: 104 RVA: 0x00003F0F File Offset: 0x0000210F
    public static void SetFromPlasmaAngles(this Quaternion q, in Vector3 pitchYawRoll)
    {
        q = QuaternionUtil.PlasmaAngles(pitchYawRoll);
    }
}
