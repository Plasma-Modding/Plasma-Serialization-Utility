using UnityEngine;

// Token: 0x02000166 RID: 358
public class DynamicProp : MonoBehaviour
{
    // Token: 0x06000E05 RID: 3589 RVA: 0x0004767C File Offset: 0x0004587C
    private void Awake()
    {
        this._interpolator = base.gameObject.GetComponentInChildren<SimpleInterpolation>();
        //this._meshRenderer = Require.ComponentInChildren<MeshRenderer>(this, false, false);
        this._propertyBlock = new MaterialPropertyBlock();
        this.SetEffectEnabled(false);
    }

    // Token: 0x06000E06 RID: 3590 RVA: 0x000476AF File Offset: 0x000458AF
    public void StoreInterpolation()
    {
        if (this._interpolator != null)
        {
            this._interpolator.StoreState();
        }
    }

    // Token: 0x06000E07 RID: 3591 RVA: 0x000476CA File Offset: 0x000458CA
    public void Interpolate(float d)
    {
        if (this._interpolator != null)
        {
            this._interpolator.Interpolate(d);
        }
    }

    // Token: 0x06000E08 RID: 3592 RVA: 0x000476E8 File Offset: 0x000458E8
    public void SetEffectEnabled(bool value)
    {
        this._meshRenderer.GetPropertyBlock(this._propertyBlock);
        this._propertyBlock.SetColor("_RimColor", value ? Holder.instance.propsInteractiveColor : Color.black);
        this._meshRenderer.SetPropertyBlock(this._propertyBlock);
    }

    // Token: 0x04000B72 RID: 2930
    public int id = -1;

    // Token: 0x04000B73 RID: 2931
    private SimpleInterpolation _interpolator;

    // Token: 0x04000B74 RID: 2932
    private MaterialPropertyBlock _propertyBlock;

    // Token: 0x04000B75 RID: 2933
    private MeshRenderer _meshRenderer;
}