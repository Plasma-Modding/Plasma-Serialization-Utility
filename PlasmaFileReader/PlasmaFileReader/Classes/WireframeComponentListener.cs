using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlasmaFileReader.Plasma.Classes
{
    // Token: 0x0200017A RID: 378
    public class WireframeComponentListener : MonoBehaviour
    {
        // Token: 0x17000127 RID: 295
        // (get) Token: 0x06000E69 RID: 3689 RVA: 0x00049B8A File Offset: 0x00047D8A
        // (set) Token: 0x06000E6A RID: 3690 RVA: 0x00049B92 File Offset: 0x00047D92
        public SubComponentHandler subComponent { get; set; }

        // Token: 0x17000128 RID: 296
        // (get) Token: 0x06000E6B RID: 3691 RVA: 0x00049B9B File Offset: 0x00047D9B
        public bool isColliding
        {
            get
            {
                return this._colliding;
            }
        }

        // Token: 0x17000129 RID: 297
        // (get) Token: 0x06000E6C RID: 3692 RVA: 0x00049BA3 File Offset: 0x00047DA3
        public bool listen
        {
            get
            {
                return this._listen;
            }
        }

        // Token: 0x06000E6D RID: 3693 RVA: 0x00049BAB File Offset: 0x00047DAB
        private void Awake()
        {
            this._entryCollisions = new List<WireframeComponentListener.CollisionPair>();
        }

        // Token: 0x06000E6E RID: 3694 RVA: 0x00049BB8 File Offset: 0x00047DB8
        public void SetListening(bool value, bool exemptEntryCollisions)
        {
            this._listen = value;
            if (value && exemptEntryCollisions)
            {
                this._hackCounter = 1;
                return;
            }
            this._entryCollisions.Clear();
        }

        // Token: 0x06000E70 RID: 3696 RVA: 0x00049CB8 File Offset: 0x00047EB8
        public void OnTriggerExit(Collider other)
        {
            bool listen = this._listen;
        }

        // Token: 0x06000E72 RID: 3698 RVA: 0x00049CFB File Offset: 0x00047EFB
        public void PreTick()
        {
            this._colliding = false;
        }

        // Token: 0x06000E73 RID: 3699 RVA: 0x00049D04 File Offset: 0x00047F04
        public void PostTick()
        {
            if (!this._colliding && this._shaderColliding)
            {
                //this.subComponent.component.SetWireframeColorColliding(false);
                this._shaderColliding = false;
            }
            if (this._hackCounter > 0)
            {
                this._hackCounter = 0;
            }
        }

        // Token: 0x04000BD6 RID: 3030
        private bool _colliding;

        // Token: 0x04000BD7 RID: 3031
        private bool _listen;

        // Token: 0x04000BD8 RID: 3032
        private int _hackCounter;

        // Token: 0x04000BD9 RID: 3033
        private bool _shaderColliding;

        // Token: 0x04000BDA RID: 3034
        private List<WireframeComponentListener.CollisionPair> _entryCollisions;

        // Token: 0x020003BE RID: 958
        private struct CollisionPair
        {
            // Token: 0x060020E8 RID: 8424 RVA: 0x000A1FE4 File Offset: 0x000A01E4
            public CollisionPair(GameObject myGameObject, GameObject otherGameObject)
            {
                this.myPosition = myGameObject.transform.position;
                this.myRotation = myGameObject.transform.rotation;
                this.myScale = myGameObject.transform.localScale;
                this.otherGameObject = otherGameObject;
                this.otherPosition = otherGameObject.transform.position;
                this.otherRotation = otherGameObject.transform.rotation;
                this.otherScale = otherGameObject.transform.localScale;
            }

            // Token: 0x04001D27 RID: 7463
            public Vector3 myPosition;

            // Token: 0x04001D28 RID: 7464
            public Quaternion myRotation;

            // Token: 0x04001D29 RID: 7465
            public Vector3 myScale;

            // Token: 0x04001D2A RID: 7466
            public GameObject otherGameObject;

            // Token: 0x04001D2B RID: 7467
            public Vector3 otherPosition;

            // Token: 0x04001D2C RID: 7468
            public Quaternion otherRotation;

            // Token: 0x04001D2D RID: 7469
            public Vector3 otherScale;
        }
    }

}
