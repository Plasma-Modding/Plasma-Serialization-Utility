using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Plasma.Classes
{
    // Token: 0x02000145 RID: 325
    public class ComponentMeshHandler : MonoBehaviour
    {
        // Token: 0x06000CB0 RID: 3248 RVA: 0x00040389 File Offset: 0x0003E589
        private void Awake()
        {
            this.meshColliderGameObjects = new List<GameObject>();
            this._index = 0;
            this._storedTransforms = new ComponentMeshHandler.StoredTransform[2];
        }

        // Token: 0x06000CB1 RID: 3249 RVA: 0x000403AC File Offset: 0x0003E5AC
        public void SetMeshColliderObjectEnabled(bool value)
        {
            foreach (GameObject gameObject in this.meshColliderGameObjects)
            {
                gameObject.SetActive(value);
            }
        }

        // Token: 0x06000CB2 RID: 3250 RVA: 0x00040400 File Offset: 0x0003E600
        public void SetMeshCollidersLayer(int layer)
        {
            foreach (GameObject gameObject in this.meshColliderGameObjects)
            {
                gameObject.layer = layer;
            }
        }

        // Token: 0x06000CB3 RID: 3251 RVA: 0x00040454 File Offset: 0x0003E654
        public void StoreState()
        {
            int num = (this._index + 1) % 2;
            this._storedTransforms[num].position = this.subComponent.position;
            this._storedTransforms[num].rotation = this.subComponent.rotation;
            this._index = num;
        }

        // Token: 0x06000CB4 RID: 3252 RVA: 0x000404AC File Offset: 0x0003E6AC
        public void StoreLocalTransform()
        {
            if (this.subComponent.component.parentSubComponent != null)
            {
                this._localPosition = this.subComponent.component.parentSubComponent.rigidbody.transform.InverseTransformPoint(this.subComponent.rigidbody.transform.position);
                this._localRotation = Quaternion.Inverse(this.subComponent.component.parentSubComponent.rigidbody.transform.rotation) * this.subComponent.rigidbody.transform.rotation;
            }
        }

        // Token: 0x06000CB5 RID: 3253 RVA: 0x00040550 File Offset: 0x0003E750
        public void Interpolate(float delta)
        {
            int num = (this._index + 1) % 2;
            this.subComponent.transform.position = Vector3.Lerp(this._storedTransforms[num].position, this._storedTransforms[this._index].position, delta);
            this.subComponent.transform.rotation = Quaternion.Slerp(this._storedTransforms[num].rotation, this._storedTransforms[this._index].rotation, delta);
        }

        // Token: 0x06000CB6 RID: 3254 RVA: 0x000405E4 File Offset: 0x0003E7E4
        public void UpdateLocalTransform()
        {
            this.subComponent.transform.position = this.subComponent.component.parentSubComponent.transform.TransformPoint(this._localPosition);
            this.subComponent.transform.rotation = this.subComponent.component.parentSubComponent.transform.rotation * this._localRotation;
        }

        // Token: 0x06000CB7 RID: 3255 RVA: 0x00040656 File Offset: 0x0003E856
        public void RenderOntoSolidComponent()
        {
            this.subComponent.transform.position = this.subComponent.position;
            this.subComponent.transform.rotation = this.subComponent.rotation;
        }

        // Token: 0x06000CB8 RID: 3256 RVA: 0x00040690 File Offset: 0x0003E890
        public void OverrideTransforms()
        {
            this._storedTransforms[0].position = this.subComponent.position;
            this._storedTransforms[1].position = this.subComponent.position;
            this._storedTransforms[0].rotation = this.subComponent.rotation;
            this._storedTransforms[1].rotation = this.subComponent.rotation;
        }

        // Token: 0x04000A97 RID: 2711
        public SubComponentHandler subComponent;

        // Token: 0x04000A98 RID: 2712
        public List<GameObject> meshColliderGameObjects;

        // Token: 0x04000A99 RID: 2713
        protected ComponentMeshHandler.StoredTransform[] _storedTransforms;

        // Token: 0x04000A9A RID: 2714
        private int _index;

        // Token: 0x04000A9B RID: 2715
        public Vector3 _localPosition;

        // Token: 0x04000A9C RID: 2716
        public Quaternion _localRotation;

        // Token: 0x020003A4 RID: 932
        protected struct StoredTransform
        {
            // Token: 0x04001CAE RID: 7342
            public Vector3 position;

            // Token: 0x04001CAF RID: 7343
            public Quaternion rotation;
        }
    }

}
