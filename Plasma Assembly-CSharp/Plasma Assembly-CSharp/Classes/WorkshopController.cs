using System;
using UnityEngine;
// Token: 0x02000050 RID: 80
public class WorkshopController : Controller
{
	// Token: 0x14000006 RID: 6
	// (add) Token: 0x0600029D RID: 669 RVA: 0x000135DC File Offset: 0x000117DC
	// (remove) Token: 0x0600029E RID: 670 RVA: 0x00013614 File Offset: 0x00011814
	public event WorkshopController.ItemDownloaded OnItemDownloadedEvent;

	// Token: 0x0400037A RID: 890
	public Texture2D previewPlaceholder;

	// Token: 0x0400037B RID: 891
	public const string storyWorldTag = "Story World";

	// Token: 0x0400037C RID: 892
	public const string challengeWorldTag = "Challenge World";

	// Token: 0x0400037D RID: 893
	public const string adventureWorldTag = "Adventure World";

	// Token: 0x0400037E RID: 894
	private WorkshopController.ItemPublished _itemPublishedHandler;

	// Token: 0x0400037F RID: 895
	private WorkshopController.ItemUnpublished _itemUnpublishedHandler;

	// Token: 0x04000380 RID: 896
	private WorkshopController.QueryCompleted _workshopQueryHandler;
	/*
	// Token: 0x04000381 RID: 897
	private Callback<ItemInstalled_t> _itemInstalled;

	// Token: 0x04000382 RID: 898
	private Callback<DownloadItemResult_t> _downloadItemResult;

	// Token: 0x04000383 RID: 899
	private Callback<RemoteStoragePublishedFileSubscribed_t> _remoteStoragePublishedFileSubscribed;

	// Token: 0x04000384 RID: 900
	private Callback<RemoteStoragePublishedFileUnsubscribed_t> _remoteStoragePublishedFileUnsubscribed;

	// Token: 0x04000385 RID: 901
	private CallResult<SteamUGCQueryCompleted_t> OnSteamUGCQueryCompletedCallResult;

	// Token: 0x04000386 RID: 902
	private CallResult<SteamUGCQueryCompleted_t> OnSteamUGCSpecificQueryCompletedCallResult;

	// Token: 0x04000387 RID: 903
	private CallResult<CreateItemResult_t> OnCreateDeviceItemResultCallResult;

	// Token: 0x04000388 RID: 904
	private CallResult<CreateItemResult_t> OnCreateWorldItemResultCallResult;

	// Token: 0x04000389 RID: 905
	private CallResult<SubmitItemUpdateResult_t> OnSubmitItemUpdateResultCallResult;

	// Token: 0x0400038A RID: 906
	private CallResult<RemoteStorageSubscribePublishedFileResult_t> OnRemoteStorageSubscribePublishedFileResultCallResult;

	// Token: 0x0400038B RID: 907
	private CallResult<RemoteStorageUnsubscribePublishedFileResult_t> OnRemoteStorageUnsubscribePublishedFileResultCallResult;

	// Token: 0x0400038C RID: 908
	private CallResult<DeleteItemResult_t> OnDeleteItemResultCallResult;

	// Token: 0x0400038D RID: 909
	private SerializedDeviceMetaData _uploadingDeviceMetaData;
	*/

	// Token: 0x0400038E RID: 910
	private SerializedWorldMetaData _uploadingWorldMetaData;

	// Token: 0x0400038F RID: 911
	private const int _metaDataFields = 3;
	/*
	// Token: 0x04000390 RID: 912
	private UGCQueryHandle_t _UGCQueryHandle;

	// Token: 0x04000391 RID: 913
	private PublishedFileId_t _publishedFileId;

	// Token: 0x04000392 RID: 914
	private UGCUpdateHandle_t _UGCUpdateHandle;
	*/
	// Token: 0x04000393 RID: 915
	private const string _deviceTag = "Device";

	// Token: 0x04000394 RID: 916
	private const string _moduleTag = "Module";

	// Token: 0x04000395 RID: 917
	private const string _worldTag = "World";

	// Token: 0x020002F4 RID: 756
	// (Invoke) Token: 0x06001EAF RID: 7855
	public delegate void ItemPublished(bool success, WorkshopController.WorkshopItemPublishResult result);

	// Token: 0x020002F5 RID: 757
	// (Invoke) Token: 0x06001EB3 RID: 7859
	public delegate void ItemUnpublished(bool success);

	// Token: 0x020002F6 RID: 758
	// (Invoke) Token: 0x06001EB7 RID: 7863
	public delegate void QueryCompleted(bool success, WorkshopController.WorkshopQueryResult result);

	// Token: 0x020002F7 RID: 759
	// (Invoke) Token: 0x06001EBB RID: 7867
	public delegate void ItemDownloaded(bool success, WorkshopController.WorkshopItemDownloadedResult result);

	// Token: 0x020002F8 RID: 760
	public enum Sorting
	{
		// Token: 0x040019F7 RID: 6647
		Date,
		// Token: 0x040019F8 RID: 6648
		Alphabetical,
		// Token: 0x040019F9 RID: 6649
		Subscriptions
	}

	// Token: 0x020002F9 RID: 761
	public class WorkshopQueryResult
	{
		// Token: 0x040019FA RID: 6650
		public uint totalQueryResultsCount;

		// Token: 0x040019FB RID: 6651
		public WorkshopController.WorkshopItemDetails[] items;
	}

	// Token: 0x020002FA RID: 762
	public class WorkshopItemDetails
	{
		// Token: 0x040019FC RID: 6652
		public ulong authorSteamId;

		// Token: 0x040019FD RID: 6653
		public ulong publishedFileId;

		// Token: 0x040019FE RID: 6654
		public string previewUrl;

		// Token: 0x040019FF RID: 6655
		public string title;

		// Token: 0x04001A00 RID: 6656
		public string description;

		// Token: 0x04001A01 RID: 6657
		public DateTime publishedDate;

		// Token: 0x04001A02 RID: 6658
		public DateTime updateDate;

		// Token: 0x04001A03 RID: 6659
		public EItemState itemState;

		// Token: 0x04001A04 RID: 6660
		public int numberOfComponents;

		// Token: 0x04001A05 RID: 6661
		public float mass;

		// Token: 0x04001A06 RID: 6662
		public bool isModule;

		// Token: 0x04001A07 RID: 6663
		public int numberOfDevices;

		// Token: 0x04001A08 RID: 6664
		public int biomeId;

		// Token: 0x04001A09 RID: 6665
		public bool isStaged;

		// Token: 0x04001A0A RID: 6666
		public string tag;
	}

	// Token: 0x020002FB RID: 763
	public class WorkshopItemPublishResult
	{
		// Token: 0x04001A0B RID: 6667
		public ulong publishedFileId;

		// Token: 0x04001A0C RID: 6668
		public bool userNeedsToAcceptLegalAgreement;
	}

	// Token: 0x020002FC RID: 764
	public class WorkshopItemDownloadedResult
	{
		// Token: 0x04001A0D RID: 6669
		public ulong publishedFileId;
	}

	public override void Init()
	{
		//throw new NotImplementedException();
	}
}
