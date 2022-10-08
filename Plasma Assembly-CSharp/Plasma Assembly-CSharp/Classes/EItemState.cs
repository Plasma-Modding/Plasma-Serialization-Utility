// Token: 0x02000167 RID: 359
using System;

[Flags]
public enum EItemState
{
    // Token: 0x04000976 RID: 2422
    k_EItemStateNone = 0,
    // Token: 0x04000977 RID: 2423
    k_EItemStateSubscribed = 1,
    // Token: 0x04000978 RID: 2424
    k_EItemStateLegacyItem = 2,
    // Token: 0x04000979 RID: 2425
    k_EItemStateInstalled = 4,
    // Token: 0x0400097A RID: 2426
    k_EItemStateNeedsUpdate = 8,
    // Token: 0x0400097B RID: 2427
    k_EItemStateDownloading = 16,
    // Token: 0x0400097C RID: 2428
    k_EItemStateDownloadPending = 32
}