// Token: 0x02000221 RID: 545
using Behavior;
using System.Collections.Generic;

public static class SketchNotifications
{

    // Token: 0x02000434 RID: 1076
    public enum Levels
    {
        // Token: 0x04001E89 RID: 7817
        Log,
        // Token: 0x04001E8A RID: 7818
        Normal,
        // Token: 0x04001E8B RID: 7819
        Warning,
        // Token: 0x04001E8C RID: 7820
        Error
    }

    // Token: 0x02000435 RID: 1077
    public enum Types
    {
        // Token: 0x04001E8E RID: 7822
        Log = 1,
        // Token: 0x04001E8F RID: 7823
        InfiniteLoop,
        // Token: 0x04001E90 RID: 7824
        SetValueOnDisabledProperty,
        // Token: 0x04001E91 RID: 7825
        IncompatibleDataType,
        // Token: 0x04001E92 RID: 7826
        ValidationPassed,
        // Token: 0x04001E93 RID: 7827
        ValidationFailed,
        // Token: 0x04001E94 RID: 7828
        MaxCapacityReached
    }

    // Token: 0x02000436 RID: 1078
    public class Notification
    {
        // Token: 0x04001E95 RID: 7829
        public SketchNotifications.Levels level;

        // Token: 0x04001E96 RID: 7830
        public SketchNotifications.Types type;

        // Token: 0x04001E97 RID: 7831
        public SketchNode node;

        // Token: 0x04001E98 RID: 7832
        public int propertyId;

        // Token: 0x04001E99 RID: 7833
        public int portId;

        // Token: 0x04001E9A RID: 7834
        public Data payload;

        // Token: 0x04001E9B RID: 7835
        public Data data;

        // Token: 0x04001E9C RID: 7836
        public string log;

        // Token: 0x04001E9D RID: 7837
        public List<int> propertyIds;
    }
}
