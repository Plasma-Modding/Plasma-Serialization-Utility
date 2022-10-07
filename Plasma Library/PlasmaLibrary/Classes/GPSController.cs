using UnityEngine;
// Token: 0x0200003B RID: 59
public class GPSController : Controller
{
	// Token: 0x060001BB RID: 443 RVA: 0x0000AE8D File Offset: 0x0000908D
	public override void Init()
	{
		this._tags = new Dictionary<int, GPSTagDriver>();
	}

	// Token: 0x060001BC RID: 444 RVA: 0x0000AE9A File Offset: 0x0000909A
	public void Purge()
	{
		this._tags.Clear();
	}

	// Token: 0x060001BD RID: 445 RVA: 0x0000AEA7 File Offset: 0x000090A7
	public void RegisterTag(int tagId, GPSTagDriver tagDriver)
	{
		if (!this._tags.ContainsKey(tagId))
		{
			this._tags.Add(tagId, tagDriver);
		}
	}

	// Token: 0x060001BE RID: 446 RVA: 0x0000AEC4 File Offset: 0x000090C4
	public void UnregisterTag(int tagId)
	{
		this._tags.Remove(tagId);
	}

	// Token: 0x060001BF RID: 447 RVA: 0x0000AED4 File Offset: 0x000090D4
	public bool FetchPosition(int tagId, out Vector3 position)
	{
		GPSTagDriver gpstagDriver;
		if (this._tags.TryGetValue(tagId, out gpstagDriver))
		{
			position = gpstagDriver.worldPosition;
			return true;
		}
		position = Vector3.zero;
		return false;
	}

	// Token: 0x060001C0 RID: 448 RVA: 0x0000AF0C File Offset: 0x0000910C
	public bool FetchVelocity(int tagId, out Vector3 velocity)
	{
		GPSTagDriver gpstagDriver;
		if (this._tags.TryGetValue(tagId, out gpstagDriver))
		{
			velocity = gpstagDriver.velocity;
			return true;
		}
		velocity = Vector3.zero;
		return false;
	}

	// Token: 0x040001D1 RID: 465
	private Dictionary<int, GPSTagDriver> _tags;
}
