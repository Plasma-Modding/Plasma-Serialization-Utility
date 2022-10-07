using System;
//using System.Windows.Controls;

namespace Plasma.Classes
{
    // Token: 0x02000054 RID: 84
    public class AgentId
    {
        // Token: 0x06000533 RID: 1331 RVA: 0x0001F76B File Offset: 0x0001D96B
        public AgentId()
        {
        }

        // Token: 0x06000534 RID: 1332 RVA: 0x0001F773 File Offset: 0x0001D973
        public AgentId(AgentId id)
        {
            this.type = id.type;
            this.agentGestaltId = id.agentGestaltId;
            this.guid = id.guid;
            this.displayName = id.displayName;
        }

        // Token: 0x06000535 RID: 1333 RVA: 0x0001F7AB File Offset: 0x0001D9AB
        public override bool Equals(object obj)
        {
            return obj is AgentId && this == (AgentId)obj;
        }

        // Token: 0x06000536 RID: 1334 RVA: 0x0001F7C3 File Offset: 0x0001D9C3
        public static bool operator ==(AgentId i1, AgentId i2)
        {
            return (i1 == null && i2 == null) || (i1 != null && i2 != null && i1.type == i2.type && i1.agentGestaltId == i2.agentGestaltId && i1.guid == i2.guid);
        }

        // Token: 0x06000537 RID: 1335 RVA: 0x0001F801 File Offset: 0x0001DA01
        public static bool operator !=(AgentId i1, AgentId i2)
        {
            return !(i1 == i2);
        }

        // Token: 0x06000538 RID: 1336 RVA: 0x0001F80D File Offset: 0x0001DA0D
        public override int GetHashCode()
        {
            return this.agentGestaltId.GetHashCode() ^ this.guid.GetHashCode();
        }

        // Token: 0x06000539 RID: 1337 RVA: 0x0001F82C File Offset: 0x0001DA2C
        public override string ToString()
        {
            return Holder.agentGestalts[this.agentGestaltId].displayName + "(" + this.guid.ToString() + ")";
        }

        // Token: 0x04000489 RID: 1161
        public AgentGestalt.Types type;

        // Token: 0x0400048A RID: 1162
        public AgentGestaltEnum agentGestaltId;

        // Token: 0x0400048B RID: 1163
        public int guid;

        // Token: 0x0400048C RID: 1164
        public string displayName;
    }

}
