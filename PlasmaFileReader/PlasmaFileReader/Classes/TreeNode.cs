using System;
using UnityEngine;

namespace PlasmaFileReader.Plasma.Classes
{
    // Token: 0x02000152 RID: 338
    public class TreeNode : MonoBehaviour
    {

        // Token: 0x06000DCD RID: 3533 RVA: 0x000467A4 File Offset: 0x000449A4
        public void UpdateVisuals(TreeNode.States state, Color color)
        {
            switch (state)
            {
                case TreeNode.States.Normal:
                    this._meshRenderer.GetPropertyBlock(this._propertyBlock);
                    this._propertyBlock.SetFloat("_Scale", 40f);
                    this._meshRenderer.SetPropertyBlock(this._propertyBlock);
                    this.iconRenderer.enabled = false;
                    break;
                case TreeNode.States.Base:
                    this._meshRenderer.GetPropertyBlock(this._propertyBlock);
                    this._propertyBlock.SetFloat("_Scale", 20f);
                    this._meshRenderer.SetPropertyBlock(this._propertyBlock);
                    this.iconRenderer.enabled = true;
                    this.iconRenderer.GetPropertyBlock(this._propertyBlock);
                    this._propertyBlock.SetTexture("_MainTex", this.baseIcon);
                    this.iconRenderer.SetPropertyBlock(this._propertyBlock);
                    break;
                case TreeNode.States.Highlighted:
                    this._meshRenderer.GetPropertyBlock(this._propertyBlock);
                    this._propertyBlock.SetFloat("_Scale", 20f);
                    this._meshRenderer.SetPropertyBlock(this._propertyBlock);
                    this.iconRenderer.enabled = true;
                    this.iconRenderer.GetPropertyBlock(this._propertyBlock);
                    this._propertyBlock.SetTexture("_MainTex", this.highlightedIcon);
                    this.iconRenderer.SetPropertyBlock(this._propertyBlock);
                    break;
            }
            this._meshRenderer.GetPropertyBlock(this._propertyBlock);
            this._propertyBlock.SetColor("_Color", color);
            this._meshRenderer.SetPropertyBlock(this._propertyBlock);
        }

        // Token: 0x04000B1D RID: 2845
        public MeshRenderer iconRenderer;

        // Token: 0x04000B1E RID: 2846
        public Texture baseIcon;

        // Token: 0x04000B1F RID: 2847
        public Texture highlightedIcon;

        // Token: 0x04000B20 RID: 2848
        private MeshRenderer _meshRenderer;

        // Token: 0x04000B21 RID: 2849
        private MaterialPropertyBlock _propertyBlock;

        // Token: 0x020003B1 RID: 945
        public enum States
        {
            // Token: 0x04001CF5 RID: 7413
            Normal,
            // Token: 0x04001CF6 RID: 7414
            Base,
            // Token: 0x04001CF7 RID: 7415
            Highlighted
        }
    }

}
