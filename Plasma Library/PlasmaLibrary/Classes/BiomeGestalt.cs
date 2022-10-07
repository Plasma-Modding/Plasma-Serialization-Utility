using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Plasma.Classes
{
    // Token: 0x02000163 RID: 355
    [CreateAssetMenu(menuName = "Plasma/Biome Gestalt")]
    public class BiomeGestalt : SerializedScriptableObject
    {
        // Token: 0x06000DFF RID: 3583 RVA: 0x0004764C File Offset: 0x0004584C
        private void SetAsInvalid()
        {
            this.id = BiomeGestaltEnum.Invalid;
        }

        // Token: 0x04000B53 RID: 2899
        public string displayName;

        // Token: 0x04000B54 RID: 2900
        public string description;

        // Token: 0x04000B55 RID: 2901
        public string sceneName;

        // Token: 0x04000B56 RID: 2902
        public Vector3 position;

        // Token: 0x04000B57 RID: 2903
        public float radius;

        // Token: 0x04000B58 RID: 2904
        public float playableRadius;

        // Token: 0x04000B59 RID: 2905
        public Vector3 surfaceCenter;

        // Token: 0x04000B5A RID: 2906
        public Vector3 playerPosition;

        // Token: 0x04000B5B RID: 2907
        public Vector3 playerOrientation;

        // Token: 0x04000B5C RID: 2908
        public Vector3 deviceRespawnPosition;

        // Token: 0x04000B5D RID: 2909
        public float gravity;

        // Token: 0x04000B5E RID: 2910
        public Vector3 cameraPosition;

        // Token: 0x04000B5F RID: 2911
        public Vector3 cameraRotation;

        // Token: 0x04000B60 RID: 2912
        public float cameraSize;

        // Token: 0x04000B61 RID: 2913
        public Texture2D preview;

        // Token: 0x04000B62 RID: 2914
        public int listPosition;

        // Token: 0x04000B63 RID: 2915
        public bool hidden;

        // Token: 0x04000B64 RID: 2916
        public BiomeGestaltEnum id;
    }

}
