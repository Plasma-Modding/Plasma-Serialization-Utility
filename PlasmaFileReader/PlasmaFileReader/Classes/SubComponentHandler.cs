using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlasmaFileReader.Plasma.Classes
{
    
    // Token: 0x02000151 RID: 337
    public class SubComponentHandler : MonoBehaviour
    {
        // Token: 0x1700010A RID: 266
        // (get) Token: 0x06000D9E RID: 3486 RVA: 0x00045A35 File Offset: 0x00043C35
        public bool isChildLink
        {
            get
            {
                return this.jointType > SubComponentHandler.Type.None;
            }
        }

        // Token: 0x1700010B RID: 267
        // (get) Token: 0x06000D9F RID: 3487 RVA: 0x00045A40 File Offset: 0x00043C40
        // (set) Token: 0x06000DA0 RID: 3488 RVA: 0x00045A48 File Offset: 0x00043C48
        public SubComponentHandler.Type jointType { get; private set; }

        // Token: 0x1700010C RID: 268
        // (get) Token: 0x06000DA1 RID: 3489 RVA: 0x00045A51 File Offset: 0x00043C51
        public bool isRootSubComponent
        {
            get
            {
                return this.component.IsSubComponentCurrentRoot(this);
            }
        }

        // Token: 0x1700010D RID: 269
        // (get) Token: 0x06000DA2 RID: 3490 RVA: 0x00045A5F File Offset: 0x00043C5F
        public int internalSubComponentIndex
        {
            get
            {
                return this._internalSubComponentIndex;
            }
        }

        // Token: 0x1700010E RID: 270
        // (get) Token: 0x06000DA3 RID: 3491 RVA: 0x00045A67 File Offset: 0x00043C67
        public IEnumerable<SubComponentHandler> childSubComponents
        {
            get
            {
                return this._childSubComponents;
            }
        }

        // Token: 0x1700010F RID: 271
        // (get) Token: 0x06000DA4 RID: 3492 RVA: 0x00045A6F File Offset: 0x00043C6F
        public bool hasChildren
        {
            get
            {
                return this._childSubComponents.Count > 0;
            }
        }

        // Token: 0x17000110 RID: 272
        // (get) Token: 0x06000DA5 RID: 3493 RVA: 0x00045A80 File Offset: 0x00043C80
        public IEnumerable<ComponentHandler> childComponents
        {
            get
            {
                List<ComponentHandler> list = new List<ComponentHandler>();
                foreach (SubComponentHandler subComponentHandler in this._childSubComponents)
                {
                    list.Add(subComponentHandler.component);
                }
                return list;
            }
        }

        // Token: 0x17000111 RID: 273
        // (get) Token: 0x06000DA6 RID: 3494 RVA: 0x00045AE0 File Offset: 0x00043CE0
        public Vector3 position
        {
            get
            {
                if (this.component.device.isSolid && !this.component.device.isMounted)
                {
                    return this.articulationCollidersGroup.position;
                }
                return this.rigidbody.transform.position;
            }
        }

        // Token: 0x17000112 RID: 274
        // (get) Token: 0x06000DA7 RID: 3495 RVA: 0x00045B2D File Offset: 0x00043D2D
        public Quaternion rotation
        {
            get
            {
                if (this.component.device.isSolid && !this.component.device.isMounted)
                {
                    return this.articulationCollidersGroup.rotation;
                }
                return this.rigidbodyCollidersGroup.rotation;
            }
        }

        // Token: 0x17000113 RID: 275
        // (get) Token: 0x06000DA8 RID: 3496 RVA: 0x00045B6A File Offset: 0x00043D6A
        // (set) Token: 0x06000DA9 RID: 3497 RVA: 0x00045B72 File Offset: 0x00043D72
        public ArticulationBody articulationBody { get; private set; }

        // Token: 0x17000114 RID: 276
        // (get) Token: 0x06000DAA RID: 3498 RVA: 0x00045B7B File Offset: 0x00043D7B
        // (set) Token: 0x06000DAB RID: 3499 RVA: 0x00045B83 File Offset: 0x00043D83
        public Rigidbody rigidbody { get; private set; }

        // Token: 0x17000115 RID: 277
        // (get) Token: 0x06000DAC RID: 3500 RVA: 0x00045B8C File Offset: 0x00043D8C
        // (set) Token: 0x06000DAD RID: 3501 RVA: 0x00045B94 File Offset: 0x00043D94
        public WireframeComponentListener wireframeComponentListener { get; private set; }

        // Token: 0x17000116 RID: 278
        // (get) Token: 0x06000DAE RID: 3502 RVA: 0x00045B9D File Offset: 0x00043D9D
        // (set) Token: 0x06000DAF RID: 3503 RVA: 0x00045BA5 File Offset: 0x00043DA5
        public Transform articulationCollidersGroup { get; private set; }

        // Token: 0x17000117 RID: 279
        // (get) Token: 0x06000DB0 RID: 3504 RVA: 0x00045BAE File Offset: 0x00043DAE
        // (set) Token: 0x06000DB1 RID: 3505 RVA: 0x00045BB6 File Offset: 0x00043DB6
        public Transform rigidbodyCollidersGroup { get; private set; }

        // Token: 0x17000118 RID: 280
        // (get) Token: 0x06000DB2 RID: 3506 RVA: 0x00045BBF File Offset: 0x00043DBF
        // (set) Token: 0x06000DB3 RID: 3507 RVA: 0x00045BC7 File Offset: 0x00043DC7
        public Transform renderGroup { get; set; }

        // Token: 0x17000119 RID: 281
        // (get) Token: 0x06000DB4 RID: 3508 RVA: 0x00045BD0 File Offset: 0x00043DD0
        // (set) Token: 0x06000DB5 RID: 3509 RVA: 0x00045BD8 File Offset: 0x00043DD8
        public ComponentMeshHandler meshHandler { get; set; }

        // Token: 0x1700011A RID: 282
        // (get) Token: 0x06000DB6 RID: 3510 RVA: 0x00045BE1 File Offset: 0x00043DE1
        public bool canSolidGrab
        {
            get
            {
                return this._canSolidGrab;
            }
        }

        // Token: 0x1700011B RID: 283
        // (get) Token: 0x06000DB7 RID: 3511 RVA: 0x00045BE9 File Offset: 0x00043DE9
        public Collider[] colliders
        {
            get
            {
                return this._colliders;
            }
        }

        // Token: 0x1700011C RID: 284
        // (get) Token: 0x06000DB8 RID: 3512 RVA: 0x00045BF1 File Offset: 0x00043DF1
        // (set) Token: 0x06000DB9 RID: 3513 RVA: 0x00045BF9 File Offset: 0x00043DF9
        public Device.Branch branch { get; set; }

        // Token: 0x06000DC1 RID: 3521 RVA: 0x000464ED File Offset: 0x000446ED
        public void AddChildSubComponent(SubComponentHandler subComponentHandler)
        {
            this._childSubComponents.Add(subComponentHandler);
        }

        // Token: 0x06000DC2 RID: 3522 RVA: 0x000464FB File Offset: 0x000446FB
        public void RemoveChildSubComponent(SubComponentHandler subComponentHandler)
        {
            this._childSubComponents.Remove(subComponentHandler);
        }

        // Token: 0x06000DC3 RID: 3523 RVA: 0x0004650C File Offset: 0x0004470C
        public void RemoveChildComponent(ComponentHandler componentHandler)
        {
            List<SubComponentHandler> list = new List<SubComponentHandler>();
            foreach (SubComponentHandler subComponentHandler in this._childSubComponents)
            {
                if (subComponentHandler.component == componentHandler)
                {
                    list.Add(subComponentHandler);
                }
            }
            foreach (SubComponentHandler item in list)
            {
                this._childSubComponents.Remove(item);
            }
        }

        // Token: 0x06000DC4 RID: 3524 RVA: 0x000465B8 File Offset: 0x000447B8
        public void GetChildSubComponentsRecursive(List<SubComponentHandler> list)
        {
            list.AddRange(this._childSubComponents);
            foreach (SubComponentHandler subComponentHandler in this._childSubComponents)
            {
                subComponentHandler.GetChildSubComponentsRecursive(list);
            }
        }

        // Token: 0x06000DC5 RID: 3525 RVA: 0x00046618 File Offset: 0x00044818
        public void UpdateScale()
        {
            this.articulationCollidersGroup.transform.localScale = this.component.scale;
            this.rigidbodyCollidersGroup.transform.localScale = this.component.scale;
            if (this.articulationBody != null)
            {
                this.articulationBody.anchorPosition = Vector3.Scale(this.component.scale, this.offset);
            }
        }

        // Token: 0x06000DC6 RID: 3526 RVA: 0x0004668A File Offset: 0x0004488A
        public int GetDofCount()
        {
            if (!this.isChildLink)
            {
                return 0;
            }
            return this.articulationBody.dofCount;
        }

        // Token: 0x06000DC7 RID: 3527 RVA: 0x000466A4 File Offset: 0x000448A4
        public void SetDofLimits(float lowerLimit, float upperLimit)
        {
            ArticulationDrive xDrive = this.articulationBody.xDrive;
            xDrive.lowerLimit = lowerLimit;
            xDrive.upperLimit = upperLimit;
            this.articulationBody.xDrive = xDrive;
        }

        // Token: 0x06000DC8 RID: 3528 RVA: 0x000466D9 File Offset: 0x000448D9
        public float GetDofOffset(int dof)
        {
            if (this.dofOffsets != null && this.dofOffsets.Count > dof)
            {
                return this.dofOffsets[dof];
            }
            return 0f;
        }

        // Token: 0x06000DC9 RID: 3529 RVA: 0x00046703 File Offset: 0x00044903
        public void MakingSolid()
        {
            this._canSolidGrab = !this.articulationCollidersGroup.GetComponentsInParent<ArticulationBody>(true)[0].isRoot;
        }

        // Token: 0x06000DCA RID: 3530 RVA: 0x00046724 File Offset: 0x00044924
        public void SetRenderGroupVisible(bool value)
        {
            MeshRenderer[] componentsInChildren = this.renderGroup.GetComponentsInChildren<MeshRenderer>(true);
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                componentsInChildren[i].enabled = value;
            }
        }

        // Token: 0x04000B07 RID: 2823
        public int subComponentIndex;

        // Token: 0x04000B08 RID: 2824
        public ComponentHandler component;

        // Token: 0x04000B09 RID: 2825
        public SubComponentHandler parentSubComponent;

        // Token: 0x04000B0A RID: 2826
        public Vector3 axis;

        // Token: 0x04000B0B RID: 2827
        public Vector3 offset;

        // Token: 0x04000B0C RID: 2828
        public List<float> dofOffsets;

        // Token: 0x04000B0D RID: 2829
        public float massRatio = 1f;

        // Token: 0x04000B0E RID: 2830
        private int _internalSubComponentIndex;

        // Token: 0x04000B0F RID: 2831
        private List<SubComponentHandler> _childSubComponents;

        // Token: 0x04000B10 RID: 2832
        private Vector3 _startingLocalPosition = Vector3.zero;

        // Token: 0x04000B11 RID: 2833
        private Quaternion _startingLocalRotation = Quaternion.identity;

        // Token: 0x04000B12 RID: 2834
        private bool _canSolidGrab;

        // Token: 0x04000B13 RID: 2835
        private Collider[] _colliders;

        // Token: 0x020003B0 RID: 944
        public enum Type
        {
            // Token: 0x04001CEF RID: 7407
            None,
            // Token: 0x04001CF0 RID: 7408
            Fixed,
            // Token: 0x04001CF1 RID: 7409
            Hinge,
            // Token: 0x04001CF2 RID: 7410
            Slider,
            // Token: 0x04001CF3 RID: 7411
            Ball
        }
    }

}
