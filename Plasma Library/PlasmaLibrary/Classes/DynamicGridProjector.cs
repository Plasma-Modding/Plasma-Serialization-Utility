using UnityEngine;
// Token: 0x02000148 RID: 328
public class DynamicGridProjector : MonoBehaviour
{
    // Token: 0x170000FD RID: 253
    // (get) Token: 0x06000D67 RID: 3431 RVA: 0x000446C4 File Offset: 0x000428C4
    public float circleRadius
    {
        get
        {
            return base.transform.lossyScale.x * 0.5f;
        }
    }

    // Token: 0x170000FE RID: 254
    // (get) Token: 0x06000D68 RID: 3432 RVA: 0x000446DC File Offset: 0x000428DC
    public SubComponentHandler owner
    {
        get
        {
            return this._owner;
        }
    }

    // Token: 0x170000FF RID: 255
    // (get) Token: 0x06000D69 RID: 3433 RVA: 0x000446E4 File Offset: 0x000428E4
    // (set) Token: 0x06000D6A RID: 3434 RVA: 0x000446EC File Offset: 0x000428EC
    public float distance { get; set; }

    // Token: 0x06000D6D RID: 3437 RVA: 0x00044DB0 File Offset: 0x00042FB0
    public void CalculateAndStoreGridPosition(Vector3 hitPosition, Vector3 hitNormal)
    {
        this._lastHitNormal = hitNormal;
        if (this.shape == DynamicGridProjector.Shape.Cylinder)
        {
            Vector3 a = new Vector3(1f / base.transform.lossyScale.x, 1f / base.transform.lossyScale.y, 1f / base.transform.lossyScale.z);
            int num = Mathf.RoundToInt(Vector3.Scale(base.transform.lossyScale, base.transform.InverseTransformPoint(hitPosition)).y / DynamicGridProjector.cellSize);
            int num2 = 0;
            float num3 = float.MaxValue;
            int num4 = this.ToNearest(Mathf.RoundToInt(this.owner.component.scale.x * this.cylinderUnitDiameter / this.cylinderSectionStepSize));
            this._cylinderGridMaterial.SetTextureScale("_MainTex", new Vector2((float)(num4 / 4), 1f));
            float num5 = 360f / (float)num4;
            for (int i = 0; i < num4; i++)
            {
                float x = Mathf.Sin((float)i * num5 * 0.017453292f);
                float z = Mathf.Cos((float)i * num5 * 0.017453292f);
                Vector3 b = base.transform.TransformPoint(Vector3.Scale(a, new Vector3(x, (float)num * DynamicGridProjector.cellSize, z)));
                float num6 = Vector3.Distance(hitPosition, b);
                if (num6 < num3)
                {
                    num3 = num6;
                    num2 = i;
                }
            }
            float num7 = Mathf.Sin((float)num2 * num5 * 0.017453292f);
            float num8 = Mathf.Cos((float)num2 * num5 * 0.017453292f);
            this._lastHitNormal = base.transform.rotation * new Vector3(num7, 0f, num8).normalized;
            this._lastHitPosition = base.transform.TransformPoint(Vector3.Scale(a, new Vector3(0.5f * num7 * base.transform.lossyScale.x, (float)num * DynamicGridProjector.cellSize, 0.5f * num8 * base.transform.lossyScale.z)));
            return;
        }
        Vector3 vector = Vector3.Scale(new Vector3(base.transform.lossyScale.x, base.transform.lossyScale.y, 1f), base.transform.InverseTransformPoint(hitPosition));
        int num9 = Mathf.RoundToInt(vector.x / DynamicGridProjector.cellSize);
        int num10 = Mathf.RoundToInt(vector.y / DynamicGridProjector.cellSize);
        int num11 = Mathf.FloorToInt(base.transform.lossyScale.x / DynamicGridProjector.cellSize * 0.5f);
        int num12 = Mathf.FloorToInt(base.transform.lossyScale.y / DynamicGridProjector.cellSize * 0.5f);
        if (num9 < -num11)
        {
            num9 = -num11;
        }
        else if (num9 > num11)
        {
            num9 = num11;
        }
        if (num10 < -num12)
        {
            num10 = -num12;
        }
        else if (num10 > num12)
        {
            num10 = num12;
        }
        Vector3 a2 = new Vector3(1f / base.transform.lossyScale.x, 1f / base.transform.lossyScale.y, 1f / base.transform.lossyScale.z);
        this._lastHitPosition = base.transform.TransformPoint(Vector3.Scale(a2, new Vector3((float)num9 * DynamicGridProjector.cellSize, (float)num10 * DynamicGridProjector.cellSize, 0f)));
    }

    // Token: 0x06000D70 RID: 3440 RVA: 0x000452DC File Offset: 0x000434DC
    private static Quaternion RotationHelper(Vector3 meshNormal, Vector3 referenceVector, Vector3 a, Vector3 b, Transform targetTransform)
    {
        float num = Vector3.Dot(referenceVector, targetTransform.rotation * a);
        float num2 = Vector3.Dot(referenceVector, targetTransform.rotation * b);
        if (Mathf.Abs(num) > Mathf.Abs(num2))
        {
            if (num > 0f)
            {
                return Quaternion.LookRotation(meshNormal, -(targetTransform.rotation * a));
            }
            return Quaternion.LookRotation(meshNormal, targetTransform.rotation * a);
        }
        else
        {
            if (num2 > 0f)
            {
                return Quaternion.LookRotation(meshNormal, -(targetTransform.rotation * b));
            }
            return Quaternion.LookRotation(meshNormal, targetTransform.rotation * b);
        }
    }

    // Token: 0x06000D71 RID: 3441 RVA: 0x00045388 File Offset: 0x00043588
    public void UpdateScale(Vector3 scale)
    {
        if (this.rotateOnScale)
        {
            float num = Mathf.Sqrt(scale.x * scale.x + scale.z * scale.z);
            base.transform.localRotation = Quaternion.Euler(0f, 180f - Mathf.Asin(scale.z / num) * 57.29578f, 0f);
        }
        if (this.shape == DynamicGridProjector.Shape.Rectangle || this.shape == DynamicGridProjector.Shape.Circle)
        {
            Vector3 localScale = base.transform.localScale;
            localScale.z = Mathf.Abs(1f / (base.transform.localRotation * scale).z);
            base.transform.localScale = localScale;
        }
    }

    // Token: 0x06000D72 RID: 3442 RVA: 0x00045442 File Offset: 0x00043642
    private int ToNextNearest(int x)
    {
        if (x < 0)
        {
            return 0;
        }
        x--;
        x |= x >> 1;
        x |= x >> 2;
        x |= x >> 4;
        x |= x >> 8;
        x |= x >> 16;
        return x + 1;
    }

    // Token: 0x06000D73 RID: 3443 RVA: 0x00045478 File Offset: 0x00043678
    private int ToNearest(int x)
    {
        int num = this.ToNextNearest(x);
        int num2 = num >> 1;
        if (num - x >= x - num2)
        {
            return num2;
        }
        return num;
    }

    // Token: 0x04000ADD RID: 2781
    public static float cellSize = 0.0625f;

    // Token: 0x04000ADE RID: 2782
    public DynamicGridProjector.Shape shape;

    // Token: 0x04000ADF RID: 2783
    public float cylinderSectionStepSize;

    // Token: 0x04000AE0 RID: 2784
    public float cylinderUnitDiameter;

    // Token: 0x04000AE1 RID: 2785
    public bool rotateOnScale;

    // Token: 0x04000AE2 RID: 2786
    private SubComponentHandler _owner;

    // Token: 0x04000AE3 RID: 2787
    private Material _gridMaterial;

    // Token: 0x04000AE4 RID: 2788
    private Material _cylinderGridMaterial;

    // Token: 0x04000AE5 RID: 2789
    private float _innerRadius;

    // Token: 0x04000AE6 RID: 2790
    private float _outerBorder;

    // Token: 0x04000AE7 RID: 2791
    private Vector3 _rectScale;

    // Token: 0x04000AE8 RID: 2792
    private Vector3 _lastHitPosition;

    // Token: 0x04000AE9 RID: 2793
    private Vector3 _lastHitNormal;

    // Token: 0x04000AEA RID: 2794
    private Vector3 _gridPosition;

    // Token: 0x020003AD RID: 941
    public enum Shape
    {
        // Token: 0x04001CE2 RID: 7394
        Rectangle,
        // Token: 0x04001CE3 RID: 7395
        Circle,
        // Token: 0x04001CE4 RID: 7396
        Cylinder
    }
}