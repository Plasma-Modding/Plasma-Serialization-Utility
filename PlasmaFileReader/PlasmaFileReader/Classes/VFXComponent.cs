using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PlasmaFileReader.Plasma.Classes
{
    // Token: 0x02000193 RID: 403
    public class VFXComponent : SerializedMonoBehaviour
    {
        // Token: 0x17000137 RID: 311
        // (get) Token: 0x06000EF1 RID: 3825 RVA: 0x0004C9F7 File Offset: 0x0004ABF7
        // (set) Token: 0x06000EF2 RID: 3826 RVA: 0x0004C9FF File Offset: 0x0004ABFF
        public ComponentHandler componentHandler { get; private set; }

        // Token: 0x17000138 RID: 312
        // (get) Token: 0x06000EF3 RID: 3827 RVA: 0x0004CA08 File Offset: 0x0004AC08
        public IEnumerable<MeshRenderer> meshRenderers
        {
            get
            {
                return this._relevantMeshRenderers;
            }
        }

        // Token: 0x17000139 RID: 313
        // (get) Token: 0x06000EF4 RID: 3828 RVA: 0x0004CA10 File Offset: 0x0004AC10
        public Bounds bounds
        {
            get
            {
                return this._bounds;
            }
        }

        // Token: 0x06000F00 RID: 3840 RVA: 0x0004D131 File Offset: 0x0004B331
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(this._bounds.center, this._bounds.size);
        }

        // Token: 0x04000CA8 RID: 3240
        public Dictionary<MeshRenderer, VFXComponent.SpecialMaterial> specialMaterials;

        // Token: 0x04000CA9 RID: 3241
        private List<MeshRenderer> _relevantMeshRenderers;

        // Token: 0x04000CAA RID: 3242
        private List<MaterialPropertyBlock> _relevantPropertyBlocks;

        // Token: 0x04000CAB RID: 3243
        private Bounds _bounds;

        // Token: 0x04000CAC RID: 3244
        private bool _isTransparent;

        // Token: 0x04000CAD RID: 3245
        private bool _isColliding;

        // Token: 0x04000CAE RID: 3246
        private bool _isGrabbing;

        // Token: 0x04000CAF RID: 3247
        private Vector4 packPlane;

        // Token: 0x020003C8 RID: 968
        // (Invoke) Token: 0x060020EF RID: 8431
        public delegate void Callback();

        // Token: 0x020003C9 RID: 969
        public class SpecialMaterial
        {
            // Token: 0x04001D52 RID: 7506
            public Material wireframeSolid;

            // Token: 0x04001D53 RID: 7507
            public Material transparent;
        }
    }
}
