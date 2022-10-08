using UnityEngine;
// Token: 0x02000149 RID: 329
public class FemaleSocketPoint : MonoBehaviour
{
    // Token: 0x17000100 RID: 256
    // (get) Token: 0x06000D76 RID: 3446 RVA: 0x000454B0 File Offset: 0x000436B0
    public SubComponentHandler owner
    {
        get
        {
            return this._owner;
        }
    }

    // Token: 0x17000101 RID: 257
    // (get) Token: 0x06000D77 RID: 3447 RVA: 0x000454B8 File Offset: 0x000436B8
    // (set) Token: 0x06000D78 RID: 3448 RVA: 0x000454C0 File Offset: 0x000436C0
    public SubComponentHandler parent { get; private set; }

    // Token: 0x17000102 RID: 258
    // (get) Token: 0x06000D79 RID: 3449 RVA: 0x000454C9 File Offset: 0x000436C9
    // (set) Token: 0x06000D7A RID: 3450 RVA: 0x000454D1 File Offset: 0x000436D1
    public SubComponentHandler child { get; private set; }

    // Token: 0x17000103 RID: 259
    // (get) Token: 0x06000D7B RID: 3451 RVA: 0x000454DA File Offset: 0x000436DA
    public bool isBusy
    {
        get
        {
            return this.parent != null || this.child != null;
        }
    }

    // Token: 0x17000104 RID: 260
    // (get) Token: 0x06000D7C RID: 3452 RVA: 0x000454F8 File Offset: 0x000436F8
    // (set) Token: 0x06000D7D RID: 3453 RVA: 0x00045500 File Offset: 0x00043700
    public bool isCustom
    {
        get
        {
            return this._isCustom;
        }
        set
        {
            this._isCustom = value;
            if (value)
            {
                this.meshGameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
            }
        }
    }

    // Token: 0x06000D80 RID: 3456 RVA: 0x00045600 File Offset: 0x00043800
    public void SetSubComponentParent(SubComponentHandler p)
    {
        if (p != null)
        {
            if (this.matchingSnappingObject != null)
            {
                this.matchingSnappingObject.occupiedBySocket = true;
            }
        }
        else if (this.matchingSnappingObject != null)
        {
            this.matchingSnappingObject.occupiedBySocket = false;
        }
        this.parent = p;
    }

    // Token: 0x06000D81 RID: 3457 RVA: 0x00045653 File Offset: 0x00043853
    public void SetSubComponentChild(SubComponentHandler p)
    {
        this.child = p;
    }

    // Token: 0x06000D83 RID: 3459 RVA: 0x00045744 File Offset: 0x00043944
    public void MakeFloating()
    {
        this.meshGameObject.GetComponent<Collider>().enabled = false;
        this.meshGameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
    }

    // Token: 0x06000D84 RID: 3460 RVA: 0x00045778 File Offset: 0x00043978
    public void UpdateColor(bool highlighted)
    {
        if (highlighted)
        {
            this.meshGameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            return;
        }
        this.meshGameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
    }

    // Token: 0x04000AEC RID: 2796
    public GameObject meshGameObject;

    // Token: 0x04000AED RID: 2797
    public SnappingGeneric.ChildOrientationPreference orientationPreference;

    // Token: 0x04000AEE RID: 2798
    public int index;

    // Token: 0x04000AEF RID: 2799
    public SnappingGeneric matchingSnappingObject;

    // Token: 0x04000AF0 RID: 2800
    public bool repositionOnScale;

    // Token: 0x04000AF1 RID: 2801
    public float repositionCenter;

    // Token: 0x04000AF2 RID: 2802
    public float repositionEdge;

    // Token: 0x04000AF3 RID: 2803
    private Vector3 _startingLocalPosition;

    // Token: 0x04000AF4 RID: 2804
    private SubComponentHandler _owner;

    // Token: 0x04000AF5 RID: 2805
    private bool _isCustom;
}
