using UnityEngine;
// Token: 0x02000142 RID: 322
public class Colorizer : MonoBehaviour
{
    // Token: 0x17000092 RID: 146
    // (get) Token: 0x06000BC5 RID: 3013 RVA: 0x0003D4AA File Offset: 0x0003B6AA
    // (set) Token: 0x06000BC6 RID: 3014 RVA: 0x0003D4B2 File Offset: 0x0003B6B2
    public int colorId { get; private set; }

    // Token: 0x17000093 RID: 147
    // (get) Token: 0x06000BC7 RID: 3015 RVA: 0x0003D4BB File Offset: 0x0003B6BB
    // (set) Token: 0x06000BC8 RID: 3016 RVA: 0x0003D4C3 File Offset: 0x0003B6C3
    public int altColorId { get; private set; }

    // Token: 0x06000BC9 RID: 3017 RVA: 0x0003D4CC File Offset: 0x0003B6CC
    private void Awake()
    {
        this._propertyBlock = new MaterialPropertyBlock();
    }

    // Token: 0x06000BCA RID: 3018 RVA: 0x0003D4DC File Offset: 0x0003B6DC
    public void SetColorFromPalette(int theColorId, bool secondary)
    {
        foreach (MeshRenderer meshRenderer in this.renderers)
        {
            meshRenderer.GetPropertyBlock(this._propertyBlock);
            this._propertyBlock.SetColor(secondary ? "_StaticColor" : "_BaseColor", Holder.instance.componentPalette[theColorId]);
            meshRenderer.SetPropertyBlock(this._propertyBlock);
        }
        if (secondary)
        {
            this.altColorId = theColorId;
            return;
        }
        this.colorId = theColorId;
    }

    // Token: 0x06000BCB RID: 3019 RVA: 0x0003D57C File Offset: 0x0003B77C
    public void SetStructureColor()
    {
        foreach (MeshRenderer meshRenderer in this.renderers)
        {
            meshRenderer.GetPropertyBlock(this._propertyBlock);
            this._propertyBlock.SetColor("_StaticColor", Holder.instance.structureColor);
            meshRenderer.SetPropertyBlock(this._propertyBlock);
        }
    }

    // Token: 0x04000A67 RID: 2663
    public List<MeshRenderer> renderers;

    // Token: 0x04000A68 RID: 2664
    private MaterialPropertyBlock _propertyBlock;
}
