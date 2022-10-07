// Token: 0x0200004F RID: 79
public class WifiController : Controller
{
	// Token: 0x06000292 RID: 658 RVA: 0x0001335C File Offset: 0x0001155C
	public override void Init()
	{
		this._dataBuffer1 = new Dictionary<int, WifiController.Packet[]>();
		this._dataBuffer2 = new Dictionary<int, WifiController.Packet[]>();
		this._queuedBroadcastData = this._dataBuffer1;
		this._broadcastData = this._dataBuffer2;
		WorldController.OnPreUpdateDevices += this.HandleOnPreUpdateDevices;
	}

	// Token: 0x06000293 RID: 659 RVA: 0x000133A8 File Offset: 0x000115A8
	public void Purge()
	{
		this._dataBuffer1.Clear();
		this._dataBuffer2.Clear();
		this._queuedBroadcastData = this._dataBuffer1;
		this._broadcastData = this._dataBuffer2;
	}

	// Token: 0x06000294 RID: 660 RVA: 0x000133D8 File Offset: 0x000115D8
	private void OnDestroy()
	{
		WorldController.OnPreUpdateDevices -= this.HandleOnPreUpdateDevices;
	}

	// Token: 0x06000295 RID: 661 RVA: 0x000133EC File Offset: 0x000115EC
	private void HandleOnPreUpdateDevices()
	{
		//this.LogVerbose("Swapping buffers");
		Dictionary<int, WifiController.Packet[]> broadcastData = this._broadcastData;
		Dictionary<int, WifiController.Packet[]> queuedBroadcastData = this._queuedBroadcastData;
		this._queuedBroadcastData = broadcastData;
		this._broadcastData = queuedBroadcastData;
		this._queuedBroadcastData.Clear();
	}

	// Token: 0x06000296 RID: 662 RVA: 0x0001342C File Offset: 0x0001162C
	public void Broadcast(int senderDeviceGuid, int frequency, WifiController.Channels channel, Data data)
	{
		/*this.Log(string.Concat(new string[]
		{
		"Device ",
		senderDeviceGuid.ToString(),
		" broadcast '",
		(data != null) ? data.ToString() : null,
		"' on frequency ",
		frequency.ToString(),
		" and channel ",
		channel.ToString()
		}));*/
		WifiController.Packet[] array;
		if (!this._queuedBroadcastData.TryGetValue(frequency, out array))
		{
			array = new WifiController.Packet[3];
			this._queuedBroadcastData.Add(frequency, array);
		}
		if (array[(int)channel] == null)
		{
			array[(int)channel] = new WifiController.Packet();
		}
		array[(int)channel].deviceGuid = senderDeviceGuid;
		array[(int)channel].data = data;
	}

	// Token: 0x06000297 RID: 663 RVA: 0x000134E4 File Offset: 0x000116E4
	public Data FetchData(int deviceGuid, int frequency, WifiController.Channels channel)
	{
		WifiController.Packet[] array;
		if (this._broadcastData != null && this._broadcastData.TryGetValue(frequency, out array) && array[(int)channel] != null && array[(int)channel].deviceGuid != deviceGuid)
		{
			string[] array2 = new string[8];
			array2[0] = "Device ";
			array2[1] = deviceGuid.ToString();
			array2[2] = " is fetching '";
			int num = 3;
			Data data = array[(int)channel].data;
			array2[num] = ((data != null) ? data.ToString() : null);
			array2[4] = "' on frequency ";
			array2[5] = frequency.ToString();
			array2[6] = " and channel ";
			array2[7] = channel.ToString();
			//this.Log(string.Concat(array2));
			return array[(int)channel].data;
		}
		return null;
	}

	// Token: 0x04000375 RID: 885
	private Dictionary<int, WifiController.Packet[]> _dataBuffer1;

	// Token: 0x04000376 RID: 886
	private Dictionary<int, WifiController.Packet[]> _dataBuffer2;

	// Token: 0x04000377 RID: 887
	private Dictionary<int, WifiController.Packet[]> _queuedBroadcastData;

	// Token: 0x04000378 RID: 888
	private Dictionary<int, WifiController.Packet[]> _broadcastData;

	// Token: 0x020002F2 RID: 754
	public enum Channels
	{
		// Token: 0x040019F1 RID: 6641
		One,
		// Token: 0x040019F2 RID: 6642
		Two,
		// Token: 0x040019F3 RID: 6643
		Three
	}

	// Token: 0x020002F3 RID: 755
	private class Packet
	{
		// Token: 0x040019F4 RID: 6644
		public int deviceGuid;

		// Token: 0x040019F5 RID: 6645
		public Data data;
	}
}