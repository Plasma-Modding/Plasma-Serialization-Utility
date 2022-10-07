using System;
using UnityEngine;

namespace PlasmaFileReader.Plasma.Classes
{
    // Token: 0x02000150 RID: 336
    public class SnappingGeneric : MonoBehaviour
    {
        // Token: 0x17000108 RID: 264
        // (get) Token: 0x06000D96 RID: 3478 RVA: 0x0004594E File Offset: 0x00043B4E
        // (set) Token: 0x06000D97 RID: 3479 RVA: 0x00045956 File Offset: 0x00043B56
        public SubComponentHandler owner { get; set; }

        // Token: 0x17000109 RID: 265
        // (get) Token: 0x06000D98 RID: 3480 RVA: 0x0004595F File Offset: 0x00043B5F
        // (set) Token: 0x06000D99 RID: 3481 RVA: 0x00045967 File Offset: 0x00043B67
        public bool occupiedBySocket { get; set; }

        // Token: 0x06000D9A RID: 3482 RVA: 0x00045970 File Offset: 0x00043B70
        public void DetachChild(ComponentHandler componentHandler)
        {
            if (this._primaryChild == componentHandler)
            {
                this._primaryChild = null;
            }
            if (this._secondaryChild == componentHandler)
            {
                this._secondaryChild = null;
            }
        }

        // Token: 0x06000D9B RID: 3483 RVA: 0x0004599C File Offset: 0x00043B9C
        public bool CanAttachChild(ComponentHandler componentHandler)
        {
            if (this.occupiedBySocket)
            {
                return false;
            }
            if (this._primaryChild != null)
            {
                if (this._primaryChild.gestalt.componentSupportSecondarySnappingPointChild)
                {
                    return false;
                }
                if (!componentHandler.gestalt.componentSupportSecondarySnappingPointChild)
                {
                    return false;
                }
            }
            return !(this._secondaryChild != null);
        }

        // Token: 0x06000D9C RID: 3484 RVA: 0x000459F5 File Offset: 0x00043BF5
        public void AttachChild(ComponentHandler componentHandler)
        {
            if (this._primaryChild == null)
            {
                this._primaryChild = componentHandler;
                return;
            }
            if (this._secondaryChild == null)
            {
                this._secondaryChild = componentHandler;
                return;
            }
        }

        // Token: 0x04000B00 RID: 2816
        public SnappingGeneric.ChildCompatibility childCompatibility;

        // Token: 0x04000B01 RID: 2817
        public SnappingGeneric.ChildOrientationPreference childOrientationPreference;

        // Token: 0x04000B02 RID: 2818
        public int index;

        // Token: 0x04000B03 RID: 2819
        private ComponentHandler _primaryChild;

        // Token: 0x04000B04 RID: 2820
        private ComponentHandler _secondaryChild;

        // Token: 0x020003AE RID: 942
        public enum ChildCompatibility
        {
            // Token: 0x04001CE6 RID: 7398
            Any,
            // Token: 0x04001CE7 RID: 7399
            Components,
            // Token: 0x04001CE8 RID: 7400
            Structures,
            // Token: 0x04001CE9 RID: 7401
            None
        }

        // Token: 0x020003AF RID: 943
        public enum ChildOrientationPreference
        {
            // Token: 0x04001CEB RID: 7403
            Any,
            // Token: 0x04001CEC RID: 7404
            Vertical,
            // Token: 0x04001CED RID: 7405
            Horizontal
        }
    }

}
