using System;
using UnityEngine;

namespace PlasmaFileReader.Plasma.Classes
{
    // Token: 0x02000218 RID: 536
    [Serializable]
    public struct QTransform
    {
        // Token: 0x0600126F RID: 4719 RVA: 0x0005E248 File Offset: 0x0005C448
        public QTransform(in Vector3 position, in Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }

        // Token: 0x06001270 RID: 4720 RVA: 0x0005E264 File Offset: 0x0005C464
        public static implicit operator QTransform(Transform t)
        {
            return new QTransform
            {
                position = t.position,
                rotation = t.rotation
            };
        }

        // Token: 0x06001271 RID: 4721 RVA: 0x0005E294 File Offset: 0x0005C494
        public void SetFrom(Transform t)
        {
            this = t;
        }

        // Token: 0x06001272 RID: 4722 RVA: 0x0005E2A2 File Offset: 0x0005C4A2
        public void ApplyTo(Transform t)
        {
            t.position = this.position;
            t.rotation = this.rotation;
        }

        // Token: 0x06001273 RID: 4723 RVA: 0x0005E2BC File Offset: 0x0005C4BC
        public static QTransform operator *(in QTransform t0, in QTransform t1)
        {
            return new QTransform
            {
                position = t0.position + t0.rotation * t1.position,
                rotation = t0.rotation * t1.rotation
            };
        }

        // Token: 0x06001274 RID: 4724 RVA: 0x0005E310 File Offset: 0x0005C510
        public static QTransform InvMul(in QTransform t0, in QTransform t1)
        {
            QTransform result = default(QTransform);
            Quaternion lhs = Quaternion.Inverse(t0.rotation);
            result.position = lhs * (t1.position - t0.position);
            result.rotation = lhs * t1.rotation;
            return result;
        }

        // Token: 0x170001F1 RID: 497
        // (get) Token: 0x06001275 RID: 4725 RVA: 0x0005E363 File Offset: 0x0005C563
        public Vector3 forward
        {
            get
            {
                return this.rotation * Vector3.forward;
            }
        }

        // Token: 0x170001F2 RID: 498
        // (get) Token: 0x06001276 RID: 4726 RVA: 0x0005E375 File Offset: 0x0005C575
        public Vector3 right
        {
            get
            {
                return this.rotation * Vector3.right;
            }
        }

        // Token: 0x170001F3 RID: 499
        // (get) Token: 0x06001277 RID: 4727 RVA: 0x0005E387 File Offset: 0x0005C587
        public Vector3 up
        {
            get
            {
                return this.rotation * Vector3.up;
            }
        }

        // Token: 0x06001278 RID: 4728 RVA: 0x0005E39C File Offset: 0x0005C59C
        public override bool Equals(object obj)
        {
            if (obj is QTransform)
            {
                QTransform qtransform = (QTransform)obj;
                return this == qtransform;
            }
            return false;
        }

        // Token: 0x06001279 RID: 4729 RVA: 0x0005E3C2 File Offset: 0x0005C5C2
        public override int GetHashCode()
        {
            return this.position.GetHashCode() ^ this.rotation.GetHashCode();
        }

        // Token: 0x0600127A RID: 4730 RVA: 0x0005E3E7 File Offset: 0x0005C5E7
        public static bool operator ==(in QTransform a, in QTransform b)
        {
            return a.position == b.position && a.rotation == b.rotation;
        }

        // Token: 0x0600127B RID: 4731 RVA: 0x0005E410 File Offset: 0x0005C610
        public static bool operator ==(in QTransform qt, Transform t)
        {
            QTransform qtransform = t;
            return qt == qtransform;
        }

        // Token: 0x0600127C RID: 4732 RVA: 0x0005E42C File Offset: 0x0005C62C
        public static bool operator !=(in QTransform a, in QTransform b)
        {
            return !(a == b);
        }

        // Token: 0x0600127D RID: 4733 RVA: 0x0005E438 File Offset: 0x0005C638
        public static bool operator !=(in QTransform qt, Transform t)
        {
            return !(qt == t);
        }

        // Token: 0x04000F6C RID: 3948
        public Vector3 position;

        // Token: 0x04000F6D RID: 3949
        public Quaternion rotation;
    }
}
