using UnityEngine;
// Token: 0x0200018C RID: 396
public class SimpleInterpolation : MonoBehaviour
{
	// Token: 0x06000E9F RID: 3743 RVA: 0x0004B2FF File Offset: 0x000494FF
	public virtual void Awake()
	{
		this._index = 0;
		this._storedTransforms = new SimpleInterpolation.StoredTransform[2];
	}

	// Token: 0x06000EA0 RID: 3744 RVA: 0x0004B314 File Offset: 0x00049514
	public void StoreState()
	{
		int num = (this._index + 1) % 2;
		this._storedTransforms[num].position = this.owner.position;
		this._storedTransforms[num].rotation = this.owner.rotation;
		this._index = num;
	}

	// Token: 0x06000EA1 RID: 3745 RVA: 0x0004B36C File Offset: 0x0004956C
	public virtual void Interpolate(float delta)
	{
		int num = (this._index + 1) % 2;
		base.transform.position = Vector3.Lerp(this._storedTransforms[num].position, this._storedTransforms[this._index].position, delta);
		base.transform.rotation = Quaternion.Slerp(this._storedTransforms[num].rotation, this._storedTransforms[this._index].rotation, delta);
	}

	// Token: 0x06000EA2 RID: 3746 RVA: 0x0004B3F4 File Offset: 0x000495F4
	public virtual void OverrideTransforms()
	{
		this._storedTransforms[0].position = this.owner.position;
		this._storedTransforms[1].position = this.owner.position;
		this._storedTransforms[0].rotation = this.owner.rotation;
		this._storedTransforms[1].rotation = this.owner.rotation;
	}

	// Token: 0x04000C83 RID: 3203
	public Transform owner;

	// Token: 0x04000C84 RID: 3204
	protected SimpleInterpolation.StoredTransform[] _storedTransforms;

	// Token: 0x04000C85 RID: 3205
	protected int _index;

	// Token: 0x020003C5 RID: 965
	protected struct StoredTransform
	{
		// Token: 0x04001D48 RID: 7496
		public Vector3 position;

		// Token: 0x04001D49 RID: 7497
		public Quaternion rotation;
	}
}