using System;
using System.Collections.Generic;
using System.Linq;
using Rewired;
using UnityEngine;

namespace PlasmaFileReader.Plasma.Classes
{
    // Token: 0x02000144 RID: 324
    public class ComponentHandler : MonoBehaviour
    {
        // Token: 0x17000096 RID: 150
        // (get) Token: 0x06000BFD RID: 3069 RVA: 0x0003D699 File Offset: 0x0003B899
        // (set) Token: 0x06000BFE RID: 3070 RVA: 0x0003D6A1 File Offset: 0x0003B8A1
        public AgentGestalt gestalt { get; set; }

        // Token: 0x17000097 RID: 151
        // (get) Token: 0x06000BFF RID: 3071 RVA: 0x0003D6AA File Offset: 0x0003B8AA
        public int guid
        {
            get
            {
                if (this.agentId != null)
                {
                    return this.agentId.guid;
                }
                return -1;
            }
        }

        // Token: 0x17000098 RID: 152
        // (get) Token: 0x06000C00 RID: 3072 RVA: 0x0003D6C7 File Offset: 0x0003B8C7
        // (set) Token: 0x06000C01 RID: 3073 RVA: 0x0003D6CF File Offset: 0x0003B8CF
        public Device device { get; set; }

        // Token: 0x17000099 RID: 153
        // (get) Token: 0x06000C02 RID: 3074 RVA: 0x0003D6D8 File Offset: 0x0003B8D8
        // (set) Token: 0x06000C03 RID: 3075 RVA: 0x0003D6E0 File Offset: 0x0003B8E0
        public Articulation articulation { get; set; }

        // Token: 0x1700009A RID: 154
        // (get) Token: 0x06000C04 RID: 3076 RVA: 0x0003D6E9 File Offset: 0x0003B8E9
        public AgentId agentId
        {
            get
            {
                return this.agent.agentId;
            }
        }

        // Token: 0x1700009B RID: 155
        // (get) Token: 0x06000C05 RID: 3077 RVA: 0x0003D6F6 File Offset: 0x0003B8F6
        // (set) Token: 0x06000C06 RID: 3078 RVA: 0x0003D6FE File Offset: 0x0003B8FE
        public Agent agent { get; set; }

        // Token: 0x1700009C RID: 156
        // (get) Token: 0x06000C07 RID: 3079 RVA: 0x0003D707 File Offset: 0x0003B907
        // (set) Token: 0x06000C08 RID: 3080 RVA: 0x0003D70F File Offset: 0x0003B90F
        public bool freshlySpawned { get; set; }

        // Token: 0x1700009D RID: 157
        // (get) Token: 0x06000C09 RID: 3081 RVA: 0x0003D718 File Offset: 0x0003B918
        // (set) Token: 0x06000C0A RID: 3082 RVA: 0x0003D720 File Offset: 0x0003B920
        public bool freshlyCloned { get; set; }

        // Token: 0x1700009E RID: 158
        // (get) Token: 0x06000C0B RID: 3083 RVA: 0x0003D72C File Offset: 0x0003B92C
        public IEnumerable<ComponentHandler> childComponents
        {
            get
            {
                List<ComponentHandler> list = new List<ComponentHandler>();
                foreach (SubComponentHandler subComponentHandler in this._subComponents.Values)
                {
                    list.AddRange(subComponentHandler.childComponents);
                }
                return list;
            }
        }

        // Token: 0x1700009F RID: 159
        // (get) Token: 0x06000C0C RID: 3084 RVA: 0x0003D790 File Offset: 0x0003B990
        public bool hasChildren
        {
            get
            {
                using (Dictionary<int, SubComponentHandler>.ValueCollection.Enumerator enumerator = this._subComponents.Values.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        if (enumerator.Current.hasChildren)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        // Token: 0x170000A0 RID: 160
        // (get) Token: 0x06000C0D RID: 3085 RVA: 0x0003D7F0 File Offset: 0x0003B9F0
        // (set) Token: 0x06000C0E RID: 3086 RVA: 0x0003D7F8 File Offset: 0x0003B9F8
        public bool isBroken { get; set; }

        // Token: 0x170000A1 RID: 161
        // (get) Token: 0x06000C0F RID: 3087 RVA: 0x0003D801 File Offset: 0x0003BA01
        public ComponentDriver componentDriver
        {
            get
            {
                return this._componentDriver;
            }
        }

        // Token: 0x170000A2 RID: 162
        // (get) Token: 0x06000C10 RID: 3088 RVA: 0x0003D809 File Offset: 0x0003BA09
        // (set) Token: 0x06000C11 RID: 3089 RVA: 0x0003D811 File Offset: 0x0003BA11
        public Colorizer colorizer { get; private set; }

        // Token: 0x170000A3 RID: 163
        // (get) Token: 0x06000C12 RID: 3090 RVA: 0x0003D81A File Offset: 0x0003BA1A
        // (set) Token: 0x06000C13 RID: 3091 RVA: 0x0003D822 File Offset: 0x0003BA22
        public TreeNode treeNode { get; private set; }

        // Token: 0x170000A4 RID: 164
        // (get) Token: 0x06000C14 RID: 3092 RVA: 0x0003D82B File Offset: 0x0003BA2B
        // (set) Token: 0x06000C15 RID: 3093 RVA: 0x0003D833 File Offset: 0x0003BA33
        public int treeNodeIndex { get; set; }

        // Token: 0x170000A5 RID: 165
        // (get) Token: 0x06000C16 RID: 3094 RVA: 0x0003D83C File Offset: 0x0003BA3C
        // (set) Token: 0x06000C17 RID: 3095 RVA: 0x0003D844 File Offset: 0x0003BA44
        public VFXComponent vfxComponent { get; private set; }

        // Token: 0x170000A6 RID: 166
        // (get) Token: 0x06000C18 RID: 3096 RVA: 0x0003D84D File Offset: 0x0003BA4D
        // (set) Token: 0x06000C19 RID: 3097 RVA: 0x0003D855 File Offset: 0x0003BA55
        public Vector3 scale { get; set; }

        // Token: 0x170000A7 RID: 167
        // (get) Token: 0x06000C1A RID: 3098 RVA: 0x0003D85E File Offset: 0x0003BA5E
        // (set) Token: 0x06000C1B RID: 3099 RVA: 0x0003D866 File Offset: 0x0003BA66
        public Vector3 pitchYawRoll { get; set; }

        // Token: 0x170000A8 RID: 168
        // (get) Token: 0x06000C1C RID: 3100 RVA: 0x0003D870 File Offset: 0x0003BA70
        // (set) Token: 0x06000C1D RID: 3101 RVA: 0x0003D88B File Offset: 0x0003BA8B
        public Quaternion attachmentPivotLocalRotation
        {
            get
            {
                Vector3 pitchYawRoll = this.pitchYawRoll;
                return QuaternionUtil.PlasmaAngles(pitchYawRoll);
            }
            set
            {
                this.pitchYawRoll = value.GetPlasmaAngles();
            }
        }

        // Token: 0x170000A9 RID: 169
        // (get) Token: 0x06000C1E RID: 3102 RVA: 0x0003D89A File Offset: 0x0003BA9A
        // (set) Token: 0x06000C1F RID: 3103 RVA: 0x0003D8A2 File Offset: 0x0003BAA2
        public float attachmentPointOffset { get; set; }

        // Token: 0x170000AA RID: 170
        // (get) Token: 0x06000C20 RID: 3104 RVA: 0x0003D8AB File Offset: 0x0003BAAB
        // (set) Token: 0x06000C21 RID: 3105 RVA: 0x0003D8BE File Offset: 0x0003BABE
        public FemaleSocketPoint currentLocalFemaleSocketPoint
        {
            get
            {
                return this._femaleSocketPoints[this._currentLocalFemaleSocketPointID];
            }
            set
            {
                this._currentLocalFemaleSocketPointID = this._femaleSocketPoints.IndexOf(value);
            }
        }

        // Token: 0x170000AB RID: 171
        // (get) Token: 0x06000C22 RID: 3106 RVA: 0x0003D8D2 File Offset: 0x0003BAD2
        // (set) Token: 0x06000C23 RID: 3107 RVA: 0x0003D8DA File Offset: 0x0003BADA
        public Dictionary<AssetController.ResourceTypes, List<int>> resourceIDs { get; set; }

        // Token: 0x170000AC RID: 172
        // (get) Token: 0x06000C24 RID: 3108 RVA: 0x0003D8E3 File Offset: 0x0003BAE3
        // (set) Token: 0x06000C25 RID: 3109 RVA: 0x0003D900 File Offset: 0x0003BB00
        public Vector3 wireframePosition
        {
            get
            {
                return this._subComponents[0].rigidbody.transform.position;
            }
            set
            {
                this._subComponents[0].rigidbody.transform.position = value;
                this._subComponents[0].rigidbody.position = value;
            }
        }

        // Token: 0x170000AD RID: 173
        // (get) Token: 0x06000C26 RID: 3110 RVA: 0x0003D935 File Offset: 0x0003BB35
        // (set) Token: 0x06000C27 RID: 3111 RVA: 0x0003D94D File Offset: 0x0003BB4D
        public Quaternion wireframeRotation
        {
            get
            {
                return this._subComponents[0].rigidbody.rotation;
            }
            set
            {
                this._subComponents[0].rigidbody.transform.rotation = value;
                this._subComponents[0].rigidbody.rotation = value;
            }
        }

        // Token: 0x170000AE RID: 174
        // (get) Token: 0x06000C28 RID: 3112 RVA: 0x0003D982 File Offset: 0x0003BB82
        public Transform wireframeTransform
        {
            get
            {
                return this._subComponents[0].rigidbodyCollidersGroup.transform;
            }
        }

        // Token: 0x170000AF RID: 175
        // (get) Token: 0x06000C29 RID: 3113 RVA: 0x0003D99A File Offset: 0x0003BB9A
        public Vector3 solidPosition
        {
            get
            {
                return this._subComponents[0].articulationCollidersGroup.position;
            }
        }

        // Token: 0x170000B0 RID: 176
        // (get) Token: 0x06000C2A RID: 3114 RVA: 0x0003D9B2 File Offset: 0x0003BBB2
        public Quaternion solidRotation
        {
            get
            {
                return this._subComponents[0].articulationCollidersGroup.rotation;
            }
        }

        // Token: 0x170000B1 RID: 177
        // (get) Token: 0x06000C2B RID: 3115 RVA: 0x0003D9CA File Offset: 0x0003BBCA
        public Transform solidTransform
        {
            get
            {
                return this._subComponents[0].articulationCollidersGroup.transform;
            }
        }

        // Token: 0x170000B2 RID: 178
        // (get) Token: 0x06000C2C RID: 3116 RVA: 0x0003D9E2 File Offset: 0x0003BBE2
        public Vector3 livePosition
        {
            get
            {
                if (!this.device.isSolid)
                {
                    return this.wireframePosition;
                }
                return this.solidPosition;
            }
        }

        // Token: 0x170000B3 RID: 179
        // (get) Token: 0x06000C2D RID: 3117 RVA: 0x0003DA00 File Offset: 0x0003BC00
        public Quaternion interpolatedRotation
        {
            get
            {
                foreach (ComponentMeshHandler componentMeshHandler in this._componentMeshHandlers)
                {
                    if (componentMeshHandler.subComponent == this.baseSubComponent)
                    {
                        return componentMeshHandler.transform.rotation;
                    }
                }
                return Quaternion.identity;
            }
        }

        // Token: 0x170000B4 RID: 180
        // (get) Token: 0x06000C2E RID: 3118 RVA: 0x0003DA74 File Offset: 0x0003BC74
        public IEnumerable<SubComponentHandler> allSubComponents
        {
            get
            {
                return this._subComponents.Values;
            }
        }

        // Token: 0x170000B5 RID: 181
        // (get) Token: 0x06000C2F RID: 3119 RVA: 0x0003DA81 File Offset: 0x0003BC81
        public int subComponentsCount
        {
            get
            {
                return this._subComponents.Count;
            }
        }

        // Token: 0x170000B6 RID: 182
        // (get) Token: 0x06000C30 RID: 3120 RVA: 0x0003DA8E File Offset: 0x0003BC8E
        public bool shouldSaveToMechanicState
        {
            get
            {
                return this._subComponents.Count > 1;
            }
        }

        // Token: 0x170000B7 RID: 183
        // (get) Token: 0x06000C31 RID: 3121 RVA: 0x0003DA9E File Offset: 0x0003BC9E
        public Vector3 parentWorldPositionReferenceFrame
        {
            get
            {
                return this.parentSubComponent.rigidbody.transform.TransformPoint(Vector3.Scale(this.parentSubComponent.component.scale, this.parentReferenceFrame.position));
            }
        }

        // Token: 0x170000B8 RID: 184
        // (get) Token: 0x06000C32 RID: 3122 RVA: 0x0003DAD5 File Offset: 0x0003BCD5
        public Quaternion parentWorldRotationReferenceFrame
        {
            get
            {
                return this.parentSubComponent.rigidbody.transform.rotation * this.parentReferenceFrame.rotation;
            }
        }

        // Token: 0x170000B9 RID: 185
        // (get) Token: 0x06000C33 RID: 3123 RVA: 0x0003DAFC File Offset: 0x0003BCFC
        public Quaternion childWorldRotationReferenceFrame
        {
            get
            {
                return this.childSubComponent.rigidbody.transform.rotation * this.childReferenceFrame.rotation;
            }
        }

        // Token: 0x170000BA RID: 186
        // (get) Token: 0x06000C34 RID: 3124 RVA: 0x0003DB23 File Offset: 0x0003BD23
        // (set) Token: 0x06000C35 RID: 3125 RVA: 0x0003DB2B File Offset: 0x0003BD2B
        public Quaternion attachmentReferenceFrame { get; private set; }

        // Token: 0x170000BB RID: 187
        // (get) Token: 0x06000C36 RID: 3126 RVA: 0x0003DB34 File Offset: 0x0003BD34
        // (set) Token: 0x06000C37 RID: 3127 RVA: 0x0003DB3C File Offset: 0x0003BD3C
        public FemaleSocketPoint childAttachmentSocket { get; private set; }

        // Token: 0x170000BC RID: 188
        // (get) Token: 0x06000C38 RID: 3128 RVA: 0x0003DB45 File Offset: 0x0003BD45
        // (set) Token: 0x06000C39 RID: 3129 RVA: 0x0003DB4D File Offset: 0x0003BD4D
        public FemaleSocketPoint parentAttachmentSocket { get; private set; }

        // Token: 0x170000BD RID: 189
        // (get) Token: 0x06000C3A RID: 3130 RVA: 0x0003DB56 File Offset: 0x0003BD56
        // (set) Token: 0x06000C3B RID: 3131 RVA: 0x0003DB5E File Offset: 0x0003BD5E
        public SubComponentHandler lastDetachedParent { get; set; }

        // Token: 0x170000BE RID: 190
        // (get) Token: 0x06000C3C RID: 3132 RVA: 0x0003DB67 File Offset: 0x0003BD67
        public bool isRootComponent
        {
            get
            {
                return this.device.rootComponent == this;
            }
        }

        // Token: 0x170000BF RID: 191
        // (get) Token: 0x06000C3D RID: 3133 RVA: 0x0003DB7A File Offset: 0x0003BD7A
        public SubComponentHandler baseSubComponent
        {
            get
            {
                return this._subComponents[0];
            }
        }

        // Token: 0x170000C0 RID: 192
        // (get) Token: 0x06000C3E RID: 3134 RVA: 0x0003DB88 File Offset: 0x0003BD88
        // (set) Token: 0x06000C3F RID: 3135 RVA: 0x0003DB90 File Offset: 0x0003BD90
        public SubComponentHandler parentSubComponent { get; private set; }

        // Token: 0x170000C1 RID: 193
        // (get) Token: 0x06000C40 RID: 3136 RVA: 0x0003DB99 File Offset: 0x0003BD99
        // (set) Token: 0x06000C41 RID: 3137 RVA: 0x0003DBA1 File Offset: 0x0003BDA1
        public SubComponentHandler childSubComponent { get; private set; }

        // Token: 0x170000C2 RID: 194
        // (get) Token: 0x06000C42 RID: 3138 RVA: 0x0003DBAC File Offset: 0x0003BDAC
        public bool isTriggerColliding
        {
            get
            {
                foreach (SubComponentHandler subComponentHandler in this.allSubComponents)
                {
                    if (subComponentHandler.wireframeComponentListener != null && subComponentHandler.wireframeComponentListener.isColliding)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        // Token: 0x170000C3 RID: 195
        // (get) Token: 0x06000C43 RID: 3139 RVA: 0x0003DC14 File Offset: 0x0003BE14
        public IEnumerable<ComponentMeshHandler> componentMeshHandlers
        {
            get
            {
                return this._componentMeshHandlers;
            }
        }

        // Token: 0x170000C4 RID: 196
        // (get) Token: 0x06000C44 RID: 3140 RVA: 0x0003DC1C File Offset: 0x0003BE1C
        public AgentGestalt.MassCategories massCategory
        {
            get
            {
                return this._massCategory;
            }
        }

        // Token: 0x170000C5 RID: 197
        // (get) Token: 0x06000C46 RID: 3142 RVA: 0x0003DC2D File Offset: 0x0003BE2D
        // (set) Token: 0x06000C45 RID: 3141 RVA: 0x0003DC24 File Offset: 0x0003BE24
        public float massMultiplier
        {
            get
            {
                return this._massMultiplier;
            }
            set
            {
                this._massMultiplier = value;
            }
        }

        // Token: 0x170000C6 RID: 198
        // (get) Token: 0x06000C47 RID: 3143 RVA: 0x0003DC35 File Offset: 0x0003BE35
        // (set) Token: 0x06000C48 RID: 3144 RVA: 0x0003DC42 File Offset: 0x0003BE42
        public float bounciness
        {
            get
            {
                return this._physicMaterial.bounciness;
            }
            set
            {
                this._physicMaterial.bounciness = value;
            }
        }

        // Token: 0x170000C7 RID: 199
        // (get) Token: 0x06000C49 RID: 3145 RVA: 0x0003DC50 File Offset: 0x0003BE50
        // (set) Token: 0x06000C4A RID: 3146 RVA: 0x0003DC5D File Offset: 0x0003BE5D
        public float dynamicFriction
        {
            get
            {
                return this._physicMaterial.dynamicFriction;
            }
            set
            {
                this._physicMaterial.dynamicFriction = value;
            }
        }

        // Token: 0x170000C8 RID: 200
        // (get) Token: 0x06000C4B RID: 3147 RVA: 0x0003DC6B File Offset: 0x0003BE6B
        // (set) Token: 0x06000C4C RID: 3148 RVA: 0x0003DC78 File Offset: 0x0003BE78
        public float staticFriction
        {
            get
            {
                return this._physicMaterial.staticFriction;
            }
            set
            {
                this._physicMaterial.staticFriction = value;
            }
        }

        // Token: 0x170000C9 RID: 201
        // (get) Token: 0x06000C4D RID: 3149 RVA: 0x0003DC86 File Offset: 0x0003BE86
        // (set) Token: 0x06000C4E RID: 3150 RVA: 0x0003DC8E File Offset: 0x0003BE8E
        public float audioVolume { get; set; }

        // Token: 0x170000CA RID: 202
        // (get) Token: 0x06000C4F RID: 3151 RVA: 0x0003DC97 File Offset: 0x0003BE97
        public AgentGestalt.NUSDefinition nusDefinition
        {
            get
            {
                if (this.gestalt.nusDefinitions != null && this._currentNusLabelIndex != -1)
                {
                    return this.gestalt.nusDefinitions[this._currentNusLabelIndex];
                }
                return null;
            }
        }

        // Token: 0x170000CB RID: 203
        // (get) Token: 0x06000C50 RID: 3152 RVA: 0x0003DCC7 File Offset: 0x0003BEC7
        public bool uniformScalingActive
        {
            get
            {
                return this._currentNusLabelIndex == -1;
            }
        }

        // Token: 0x170000CC RID: 204
        // (get) Token: 0x06000C51 RID: 3153 RVA: 0x0003DCD2 File Offset: 0x0003BED2
        public int currentNusLabelIndex
        {
            get
            {
                return this._currentNusLabelIndex;
            }
        }

        // Token: 0x170000CD RID: 205
        // (get) Token: 0x06000C52 RID: 3154 RVA: 0x0003DCDA File Offset: 0x0003BEDA
        // (set) Token: 0x06000C53 RID: 3155 RVA: 0x0003DCE2 File Offset: 0x0003BEE2
        public SnappingGeneric parentSnappingObject { get; set; }

        // Token: 0x170000CE RID: 206
        // (get) Token: 0x06000C54 RID: 3156 RVA: 0x0003DCEB File Offset: 0x0003BEEB
        // (set) Token: 0x06000C55 RID: 3157 RVA: 0x0003DCF3 File Offset: 0x0003BEF3
        public Transform dockingPoint { get; private set; }

        // Token: 0x170000CF RID: 207
        // (get) Token: 0x06000C56 RID: 3158 RVA: 0x0003DCFC File Offset: 0x0003BEFC
        public ArticulationBody ownerArticulationBody
        {
            get
            {
                return this.GetSubComponent(0).articulationCollidersGroup.GetComponentsInParent<ArticulationBody>(true)[0];
            }
        }

        // Token: 0x170000D0 RID: 208
        // (get) Token: 0x06000C57 RID: 3159 RVA: 0x0003DD12 File Offset: 0x0003BF12
        public PhysicMaterial physicMaterial
        {
            get
            {
                return this._physicMaterial;
            }
        }

        // Token: 0x06000C5A RID: 3162 RVA: 0x0003E188 File Offset: 0x0003C388
        public bool ShouldBreak()
        {
            if (!this.isRootComponent && this.gestalt.componentCategory != AgentGestalt.ComponentCategories.Structure)
            {
                bool isChildLink = this.baseSubComponent.isChildLink;
            }
            return false;
        }

        // Token: 0x06000C5B RID: 3163 RVA: 0x0003E1AD File Offset: 0x0003C3AD
        public void Paint(int colorIndex, bool secondary)
        {
            if (this.colorizer != null)
            {
                if (secondary && this.gestalt.componentCategory == AgentGestalt.ComponentCategories.Structure)
                {
                    this.colorizer.SetStructureColor();
                    return;
                }
                this.colorizer.SetColorFromPalette(colorIndex, secondary);
            }
        }

        // Token: 0x06000C5C RID: 3164 RVA: 0x0003E1E7 File Offset: 0x0003C3E7
        public int GetPaintColor()
        {
            if (this.colorizer != null)
            {
                return this.colorizer.colorId;
            }
            return -1;
        }

        // Token: 0x06000C5D RID: 3165 RVA: 0x0003E204 File Offset: 0x0003C404
        public int GetAltPaintColor()
        {
            if (this.colorizer != null)
            {
                return this.colorizer.altColorId;
            }
            return -1;
        }

        // Token: 0x06000C5F RID: 3167 RVA: 0x0003E49C File Offset: 0x0003C69C
        public Quaternion CycleRootComponentRotation(bool forward)
        {
            if (forward)
            {
                this._rootComponentAxisRotation++;
                if (this._rootComponentAxisRotation >= ComponentHandler._cycleRotations.Count)
                {
                    this._rootComponentAxisRotation = 0;
                }
            }
            else
            {
                this._rootComponentAxisRotation--;
                if (this._rootComponentAxisRotation < 0)
                {
                    this._rootComponentAxisRotation = ComponentHandler._cycleRotations.Count - 1;
                }
            }
            return Quaternion.LookRotation(ComponentHandler._cycleRotations[this._rootComponentAxisRotation]);
        }

        // Token: 0x06000C60 RID: 3168 RVA: 0x0003E513 File Offset: 0x0003C713
        public void ResetRootComponentRotationCycle()
        {
            this._rootComponentAxisRotation = -1;
        }

        // Token: 0x06000C62 RID: 3170 RVA: 0x0003E5DC File Offset: 0x0003C7DC
        public float GetScaleValueForLabelIndex(int labelIndex)
        {
            if (!this.gestalt.componentAllowNonUniformScale || this.gestalt.nusDefinitions == null || this.gestalt.nusDefinitions.Count <= labelIndex)
            {
                return this.scale.x * 100f;
            }
            AgentGestalt.NUSDefinition nusdefinition = this.gestalt.nusDefinitions[labelIndex];
            float num;
            switch (nusdefinition.axis[0])
            {
                case AgentGestalt.NUSAxis.X:
                    num = this.scale.x;
                    break;
                case AgentGestalt.NUSAxis.Y:
                    num = this.scale.y;
                    break;
                case AgentGestalt.NUSAxis.Z:
                    num = this.scale.z;
                    break;
                default:
                    num = 0f;
                    break;
            }
            return num * nusdefinition.unitSize;
        }

        // Token: 0x06000C65 RID: 3173 RVA: 0x0003EA88 File Offset: 0x0003CC88
        public void CycleNUSLabel(bool forward)
        {
            if (forward)
            {
                this._currentNusLabelIndex++;
                if (this._currentNusLabelIndex >= this.gestalt.nusDefinitions.Count)
                {
                    this._currentNusLabelIndex = -1;
                    return;
                }
            }
            else
            {
                this._currentNusLabelIndex--;
                if (this._currentNusLabelIndex < -1)
                {
                    this._currentNusLabelIndex = this.gestalt.nusDefinitions.Count - 1;
                }
            }
        }

        // Token: 0x06000C66 RID: 3174 RVA: 0x0003EAF8 File Offset: 0x0003CCF8
        public Vector3 ClampScale(Vector3 theScale)
        {
            theScale.x = Mathf.Clamp(theScale.x, this.gestalt.componentScaleXLimits.min, this.gestalt.componentScaleXLimits.max);
            theScale.y = Mathf.Clamp(theScale.y, this.gestalt.componentScaleYLimits.min, this.gestalt.componentScaleYLimits.max);
            theScale.z = Mathf.Clamp(theScale.z, this.gestalt.componentScaleZLimits.min, this.gestalt.componentScaleZLimits.max);
            return theScale;
        }

        // Token: 0x06000C67 RID: 3175 RVA: 0x0003EB9C File Offset: 0x0003CD9C
        public float GetScaleRatio()
        {
            return this.gestalt.componentScaleXLimits.ToRatio(this.scale.x);
        }

        // Token: 0x06000C68 RID: 3176 RVA: 0x0003EBBC File Offset: 0x0003CDBC
        public void StoreInterpolationState()
        {
            foreach (ComponentMeshHandler componentMeshHandler in this._componentMeshHandlers)
            {
                componentMeshHandler.StoreState();
            }
        }

        // Token: 0x06000C69 RID: 3177 RVA: 0x0003EC0C File Offset: 0x0003CE0C
        public void Interpolate(float delta, bool mounted = false)
        {
            foreach (ComponentMeshHandler componentMeshHandler in this._componentMeshHandlers)
            {
                if (mounted)
                {
                    componentMeshHandler.RenderOntoSolidComponent();
                }
                else
                {
                    componentMeshHandler.Interpolate(delta);
                }
            }
        }

        // Token: 0x06000C6A RID: 3178 RVA: 0x0003EC6C File Offset: 0x0003CE6C
        public void OverrideInterpolationTransforms()
        {
            foreach (ComponentMeshHandler componentMeshHandler in this._componentMeshHandlers)
            {
                componentMeshHandler.OverrideTransforms();
            }
        }

        // Token: 0x06000C6D RID: 3181 RVA: 0x0003EE38 File Offset: 0x0003D038
        public float GetMass()
        {
            return this._massMultiplier * ((this.gestalt != null) ? this.gestalt.componentMass : 1f) * this.scale.x * this.scale.y * this.scale.z;
        }

        // Token: 0x06000C70 RID: 3184 RVA: 0x0003EF7C File Offset: 0x0003D17C
        public void SetWireframeCollidersEnabled(bool value)
        {
            foreach (SubComponentHandler subComponentHandler in this.allSubComponents)
            {
                Collider[] componentsInChildren = subComponentHandler.rigidbodyCollidersGroup.GetComponentsInChildren<Collider>(true);
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    componentsInChildren[i].gameObject.SetActive(value);
                }
                foreach (Collider collider in subComponentHandler.renderGroup.GetComponentsInChildren<Collider>(true))
                {
                    if (collider.gameObject.layer != LayerMask.NameToLayer("WireframeComponent") && collider.gameObject.layer != LayerMask.NameToLayer("SolidComponent") && collider.gameObject.layer != LayerMask.NameToLayer("DynamicGrid"))
                    {
                        collider.gameObject.SetActive(value);
                    }
                }
            }
        }

        // Token: 0x06000C71 RID: 3185 RVA: 0x0003F068 File Offset: 0x0003D268
        public void SetWireframeCollidersToTrigger(bool value)
        {
            foreach (SubComponentHandler subComponentHandler in this.allSubComponents)
            {
                Collider[] componentsInChildren = subComponentHandler.rigidbodyCollidersGroup.GetComponentsInChildren<Collider>(true);
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    componentsInChildren[i].isTrigger = value;
                }
                foreach (Collider collider in subComponentHandler.renderGroup.GetComponentsInChildren<Collider>(true))
                {
                    if (collider.gameObject.layer != LayerMask.NameToLayer("WireframeComponent") && collider.gameObject.layer != LayerMask.NameToLayer("SolidComponent") && collider.gameObject.layer != LayerMask.NameToLayer("DynamicGrid"))
                    {
                        collider.isTrigger = value;
                    }
                }
            }
        }

        // Token: 0x06000C72 RID: 3186 RVA: 0x0003F148 File Offset: 0x0003D348
        public void SetPhysicsCollidersEnabled(bool value, bool exemptEntryCollisions = true)
        {
            foreach (SubComponentHandler subComponentHandler in this.allSubComponents)
            {
                Collider[] componentsInChildren = subComponentHandler.rigidbodyCollidersGroup.GetComponentsInChildren<Collider>();
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    componentsInChildren[i].isTrigger = !value;
                }
                if (subComponentHandler.wireframeComponentListener != null)
                {
                    subComponentHandler.wireframeComponentListener.SetListening(!value, exemptEntryCollisions);
                }
            }
        }

        // Token: 0x06000C73 RID: 3187 RVA: 0x0003F1D4 File Offset: 0x0003D3D4
        public void TransitioningToWireframe()
        {
        }

        // Token: 0x06000C74 RID: 3188 RVA: 0x0003F1D8 File Offset: 0x0003D3D8
        public void MakingSolid()
        {
            foreach (SubComponentHandler subComponentHandler in this.allSubComponents)
            {
                subComponentHandler.MakingSolid();
            }
        }

        // Token: 0x06000C76 RID: 3190 RVA: 0x0003F250 File Offset: 0x0003D450
        public void PreTickTriggerListeners()
        {
            foreach (SubComponentHandler subComponentHandler in this.allSubComponents)
            {
                if (subComponentHandler.wireframeComponentListener != null)
                {
                    subComponentHandler.wireframeComponentListener.PreTick();
                }
            }
        }

        // Token: 0x06000C77 RID: 3191 RVA: 0x0003F2B0 File Offset: 0x0003D4B0
        public void PostTickTriggerListeners()
        {
            foreach (SubComponentHandler subComponentHandler in this.allSubComponents)
            {
                if (subComponentHandler.wireframeComponentListener != null)
                {
                    subComponentHandler.wireframeComponentListener.PostTick();
                }
            }
        }

        // Token: 0x06000C78 RID: 3192 RVA: 0x0003F310 File Offset: 0x0003D510
        public SubComponentHandler GetSubComponent(int subComponentIndex)
        {
            return this._subComponents[subComponentIndex];
        }

        // Token: 0x06000C7B RID: 3195 RVA: 0x0003F598 File Offset: 0x0003D798
        public bool IsSubComponentCurrentRoot(SubComponentHandler subComponentHandler)
        {
            return subComponentHandler.subComponentIndex == 0;
        }

        // Token: 0x06000C7C RID: 3196 RVA: 0x0003F5A3 File Offset: 0x0003D7A3
        public bool CanHaveZeroMass()
        {
            return !this.isRootComponent && this.subComponentsCount == 1 && this.GetChildren(false, null).Count<ComponentHandler>() == 0;
        }

        // Token: 0x06000C7D RID: 3197 RVA: 0x0003F5C8 File Offset: 0x0003D7C8
        public void SetParentSubComponent(SubComponentHandler subComponentHandler)
        {
            this.parentSubComponent = subComponentHandler;
        }

        // Token: 0x06000C7E RID: 3198 RVA: 0x0003F5D1 File Offset: 0x0003D7D1
        public void SetChildSubComponent(SubComponentHandler subComponentHandler)
        {
            this.childSubComponent = subComponentHandler;
        }

        // Token: 0x06000C7F RID: 3199 RVA: 0x0003F5DA File Offset: 0x0003D7DA
        public void SetAttachmentReferenceFrame(Quaternion q)
        {
            this.attachmentReferenceFrame = q;
        }

        // Token: 0x06000C80 RID: 3200 RVA: 0x0003F5E3 File Offset: 0x0003D7E3
        public void SetChildAttachmentSocketPoint(FemaleSocketPoint femaleSocketPoint)
        {
            this.childAttachmentSocket = femaleSocketPoint;
            this.lastDetachedParent = null;
        }

        // Token: 0x06000C81 RID: 3201 RVA: 0x0003F5F3 File Offset: 0x0003D7F3
        public void SetParentAttachmentSocketPoint(FemaleSocketPoint femaleSocketPoint)
        {
            this.parentAttachmentSocket = femaleSocketPoint;
        }

        // Token: 0x06000C82 RID: 3202 RVA: 0x0003F5FC File Offset: 0x0003D7FC
        public void RemoveChildFromAllSubComponents(SubComponentHandler childToRemove)
        {
            foreach (SubComponentHandler subComponentHandler in this.allSubComponents)
            {
                subComponentHandler.RemoveChildSubComponent(childToRemove);
            }
        }

        // Token: 0x06000C85 RID: 3205 RVA: 0x0003F7F0 File Offset: 0x0003D9F0
        public void StoreLocalTransforms()
        {
            foreach (SubComponentHandler subComponentHandler in this.allSubComponents)
            {
                subComponentHandler.meshHandler.StoreLocalTransform();
            }
        }

        // Token: 0x06000C88 RID: 3208 RVA: 0x0003FA90 File Offset: 0x0003DC90
        public void SetCurrentFemaleSocketIndex(int value)
        {
            this._currentLocalFemaleSocketPointID = value;
        }

        // Token: 0x06000C89 RID: 3209 RVA: 0x0003FA99 File Offset: 0x0003DC99
        public int GetCurrentFemaleSocketIndex()
        {
            return this._currentLocalFemaleSocketPointID;
        }

        // Token: 0x06000C8A RID: 3210 RVA: 0x0003FAA1 File Offset: 0x0003DCA1
        public bool CanHandleInteraction(Ray ray)
        {
            return this._componentDriver != null && this._componentDriver.CanInteractWithRaycast(ray);
        }

        // Token: 0x06000C8B RID: 3211 RVA: 0x0003FABF File Offset: 0x0003DCBF
        public bool InteractionAllowsPlayerLookAround()
        {
            return !this.gestalt.componentInteractionLocksCamera;
        }

        // Token: 0x06000C8C RID: 3212 RVA: 0x0003FACF File Offset: 0x0003DCCF
        public bool InteractionTriggeredByPlasma()
        {
            return this.gestalt.componentPlasmaInteraction;
        }

        // Token: 0x06000C8D RID: 3213 RVA: 0x0003FADC File Offset: 0x0003DCDC
        public void HandleInteractionDown(Ray ray)
        {
            if (this._componentDriver != null)
            {
                this._componentDriver.OnInteractionDown(ray);
            }
        }

        // Token: 0x06000C8E RID: 3214 RVA: 0x0003FAF8 File Offset: 0x0003DCF8
        public void HandleInteractionUp()
        {
            if (this._componentDriver != null)
            {
                this._componentDriver.OnInteractionUp();
            }
        }

        // Token: 0x06000C8F RID: 3215 RVA: 0x0003FB13 File Offset: 0x0003DD13
        public void HandleInteractionHold(Camera theCamera, Vector2 movement)
        {
            if (this._componentDriver != null)
            {
                this._componentDriver.OnInteractionHold(theCamera, movement);
            }
        }

        // Token: 0x06000C90 RID: 3216 RVA: 0x0003FB30 File Offset: 0x0003DD30
        public void HandlePlasmaInteraction()
        {
            if (this._componentDriver != null)
            {
                this._componentDriver.OnPlasmaInteraction();
            }
        }

        // Token: 0x06000C91 RID: 3217 RVA: 0x0003FB4B File Offset: 0x0003DD4B
        public bool CanHandleRaycast(Ray ray)
        {
            return this._componentDriver != null && this._componentDriver.CanReactToRaycast(ray);
        }

        // Token: 0x06000C92 RID: 3218 RVA: 0x0003FB69 File Offset: 0x0003DD69
        public void HandleRaycast(Ray ray)
        {
            if (this._componentDriver != null)
            {
                this._componentDriver.OnRaycastInteraction(ray);
            }
        }

        // Token: 0x06000C93 RID: 3219 RVA: 0x0003FB85 File Offset: 0x0003DD85
        public bool CanHandleFocus(Ray ray)
        {
            return this._componentDriver != null && this._componentDriver.CanFocusOnComponent(ray);
        }

        // Token: 0x06000C94 RID: 3220 RVA: 0x0003FBA3 File Offset: 0x0003DDA3
        public bool FocusAllowsPlayerLookAround()
        {
            return !this.gestalt.componentFocusLocksCamera;
        }

        // Token: 0x06000C95 RID: 3221 RVA: 0x0003FBB3 File Offset: 0x0003DDB3
        public void HandleFocusDown(Ray ray)
        {
            if (this._componentDriver != null)
            {
                this._componentDriver.OnFocusDown(ray);
            }
        }

        // Token: 0x06000C96 RID: 3222 RVA: 0x0003FBCF File Offset: 0x0003DDCF
        public void HandleFocusUp()
        {
            if (this._componentDriver != null)
            {
                this._componentDriver.OnFocusUp();
            }
        }

        // Token: 0x06000C97 RID: 3223 RVA: 0x0003FBEA File Offset: 0x0003DDEA
        public void HandleFocusHold(Camera theCamera, Player input)
        {
            if (this._componentDriver != null)
            {
                this._componentDriver.OnFocusUpdate(theCamera, input);
            }
        }

        // Token: 0x06000C98 RID: 3224 RVA: 0x0003FC08 File Offset: 0x0003DE08
        public IEnumerable<ComponentHandler> GetChildren(bool recursive, List<ComponentHandler> list = null)
        {
            List<ComponentHandler> list2 = list ?? new List<ComponentHandler>();
            foreach (ComponentHandler componentHandler in this.childComponents)
            {
                list2.Add(componentHandler);
                if (recursive)
                {
                    componentHandler.GetChildren(true, list2);
                }
            }
            return list2;
        }

        // Token: 0x06000C99 RID: 3225 RVA: 0x0003FC70 File Offset: 0x0003DE70
        public void HandleDocking()
        {
            if (this._componentDriver != null)
            {
                this._componentDriver.OnDock();
            }
        }

        // Token: 0x06000C9A RID: 3226 RVA: 0x0003FC8B File Offset: 0x0003DE8B
        public void HandleUndocking()
        {
            if (this._componentDriver != null)
            {
                this._componentDriver.OnUndock();
            }
        }

        // Token: 0x06000C9B RID: 3227 RVA: 0x0003FCA6 File Offset: 0x0003DEA6
        public bool CanDock()
        {
            return this._componentDriver != null && this._componentDriver.CanDock();
        }

        // Token: 0x06000C9C RID: 3228 RVA: 0x0003FCC3 File Offset: 0x0003DEC3
        public bool CanMount()
        {
            return this._componentDriver != null && this._componentDriver.CanMount();
        }

        // Token: 0x06000C9D RID: 3229 RVA: 0x0003FCE0 File Offset: 0x0003DEE0
        public void HandleMountModifierStart()
        {
            if (this._componentDriver != null)
            {
                this._componentDriver.OnMountStartListening();
            }
        }

        // Token: 0x06000C9E RID: 3230 RVA: 0x0003FCFB File Offset: 0x0003DEFB
        public void HandleMountModifierStop()
        {
            if (this._componentDriver != null)
            {
                this._componentDriver.OnMountStopListening();
            }
        }

        // Token: 0x06000C9F RID: 3231 RVA: 0x0003FD18 File Offset: 0x0003DF18
        public IEnumerable<VFXComponent> GetVFXChildren(bool recursive, bool includeBroken = true, bool includeSelf = true)
        {
            List<VFXComponent> list = new List<VFXComponent>();
            if (includeSelf)
            {
                list.Add(this.vfxComponent);
            }
            foreach (ComponentHandler componentHandler in this.childComponents)
            {
                if (includeBroken || (!componentHandler.isBroken && componentHandler.baseSubComponent.isChildLink))
                {
                    list.Add(componentHandler.vfxComponent);
                    if (recursive)
                    {
                        list.AddRange(componentHandler.GetVFXChildren(true, includeBroken, false));
                    }
                }
            }
            return list;
        }

        // Token: 0x06000CA1 RID: 3233 RVA: 0x0003FDBC File Offset: 0x0003DFBC
        public void SetSocketsEnabled(bool value)
        {
            foreach (FemaleSocketPoint femaleSocketPoint in this._femaleSocketPoints)
            {
                femaleSocketPoint.gameObject.SetActive(value);
            }
        }

        // Token: 0x06000CA2 RID: 3234 RVA: 0x0003FE14 File Offset: 0x0003E014
        public void AddSocket(FemaleSocketPoint newSocket, bool fromLoad = false)
        {
            if (!fromLoad)
            {
                int num = 0;
                foreach (FemaleSocketPoint femaleSocketPoint in this._femaleSocketPoints)
                {
                    if (femaleSocketPoint.index > num)
                    {
                        num = femaleSocketPoint.index;
                    }
                }
                newSocket.index = num + 1;
            }
            this._femaleSocketPoints.Add(newSocket);
        }

        // Token: 0x06000CA3 RID: 3235 RVA: 0x0003FE8C File Offset: 0x0003E08C
        public void DeleteSocket(GameObject socketToDelete)
        {
            FemaleSocketPoint component = socketToDelete.GetComponent<FemaleSocketPoint>();
            foreach (FemaleSocketPoint femaleSocketPoint in this._femaleSocketPoints)
            {
                if (femaleSocketPoint.index > component.index)
                {
                    femaleSocketPoint.index--;
                }
            }
            if (this._currentLocalFemaleSocketPointID >= component.index)
            {
                this._currentLocalFemaleSocketPointID--;
            }
            this._femaleSocketPoints.Remove(component);
            UnityEngine.Object.Destroy(socketToDelete);
        }

        // Token: 0x06000CA4 RID: 3236 RVA: 0x0003FF2C File Offset: 0x0003E12C
        public List<FemaleSocketPoint> GetCustomSockets()
        {
            List<FemaleSocketPoint> list = new List<FemaleSocketPoint>();
            foreach (FemaleSocketPoint femaleSocketPoint in this._femaleSocketPoints)
            {
                if (femaleSocketPoint.isCustom)
                {
                    list.Add(femaleSocketPoint);
                }
            }
            return list;
        }

        // Token: 0x06000CA5 RID: 3237 RVA: 0x0003FF90 File Offset: 0x0003E190
        public SnappingGeneric GetSnappingObjectByIndex(int index)
        {
            foreach (SnappingGeneric snappingGeneric in this._snappingObjects)
            {
                if (snappingGeneric.index == index)
                {
                    return snappingGeneric;
                }
            }
            return null;
        }

        // Token: 0x06000CA6 RID: 3238 RVA: 0x0003FFEC File Offset: 0x0003E1EC
        public FemaleSocketPoint GetSocketByIndex(int index)
        {
            foreach (FemaleSocketPoint femaleSocketPoint in this._femaleSocketPoints)
            {
                if (femaleSocketPoint.index == index)
                {
                    return femaleSocketPoint;
                }
            }
            return null;
        }

        // Token: 0x06000CA8 RID: 3240 RVA: 0x00040104 File Offset: 0x0003E304
        public bool ValidateCurrentFemaleSocket(int increment)
        {
            int num = (this._currentLocalFemaleSocketPointID < 0) ? 0 : this.currentLocalFemaleSocketPoint.index;
            if (increment != 0)
            {
                this.StepFemaleSocketPoint(increment);
            }
            while (this.currentLocalFemaleSocketPoint.isBusy)
            {
                this.StepFemaleSocketPoint(increment);
                if (this.currentLocalFemaleSocketPoint.index == num)
                {
                    return false;
                }
            }
            return true;
        }

        // Token: 0x06000CA9 RID: 3241 RVA: 0x0004015C File Offset: 0x0003E35C
        private void StepFemaleSocketPoint(int increment)
        {
            if (increment > 0)
            {
                this._currentLocalFemaleSocketPointID = (this._currentLocalFemaleSocketPointID + 1) % this._femaleSocketPoints.Count<FemaleSocketPoint>();
                return;
            }
            if (this._currentLocalFemaleSocketPointID == 0)
            {
                this._currentLocalFemaleSocketPointID = this._femaleSocketPoints.Count<FemaleSocketPoint>() - 1;
                return;
            }
            this._currentLocalFemaleSocketPointID--;
        }

        // Token: 0x06000CAA RID: 3242 RVA: 0x000401B4 File Offset: 0x0003E3B4
        public void SetSnappingPointsEnabled(bool value)
        {
            for (int i = 0; i < this._snappingObjects.Count; i++)
            {
                this._snappingObjects[i].gameObject.SetActive(value);
            }
        }

        // Token: 0x06000CAB RID: 3243 RVA: 0x000401F0 File Offset: 0x0003E3F0
        public void SetSnappingPointsEnabled(bool value, ComponentHandler child, SnappingGeneric.ChildCompatibility snappingPointChildCompatibility)
        {
            for (int i = 0; i < this._snappingObjects.Count; i++)
            {
                if (!value)
                {
                    this._snappingObjects[i].gameObject.SetActive(false);
                }
                else if ((this._snappingObjects[i].childCompatibility == snappingPointChildCompatibility || this._snappingObjects[i].childCompatibility == SnappingGeneric.ChildCompatibility.Any) && this._snappingObjects[i].CanAttachChild(child))
                {
                    this._snappingObjects[i].gameObject.SetActive(true);
                }
            }
        }

        // Token: 0x06000CAC RID: 3244 RVA: 0x00040284 File Offset: 0x0003E484
        public void SetRenderGroupVisible(bool value)
        {
            foreach (SubComponentHandler subComponentHandler in this.allSubComponents)
            {
                subComponentHandler.SetRenderGroupVisible(value);
            }
        }

        // Token: 0x06000CAD RID: 3245 RVA: 0x000402D0 File Offset: 0x0003E4D0
        public void SetRaycastMeshColliderEnabled(bool value)
        {
            foreach (SubComponentHandler subComponentHandler in this.allSubComponents)
            {
                subComponentHandler.meshHandler.gameObject.SetActive(value);
            }
        }

        // Token: 0x04000A6D RID: 2669
        public QTransform parentReferenceFrame;

        // Token: 0x04000A6E RID: 2670
        public QTransform childReferenceFrame;

        // Token: 0x04000A6F RID: 2671
        public const float maxMassMultiplier = 14f;

        // Token: 0x04000A70 RID: 2672
        private Dictionary<int, SubComponentHandler> _subComponents;

        // Token: 0x04000A71 RID: 2673
        private ComponentDriver _componentDriver;

        // Token: 0x04000A72 RID: 2674
        private PlasmaPhysicsConstraint[] _physicsConstraints;

        // Token: 0x04000A73 RID: 2675
        private List<FemaleSocketPoint> _femaleSocketPoints;

        // Token: 0x04000A74 RID: 2676
        private int _currentLocalFemaleSocketPointID;

        // Token: 0x04000A75 RID: 2677
        private List<SnappingGeneric> _snappingObjects;

        // Token: 0x04000A76 RID: 2678
        private List<ComponentMeshHandler> _componentMeshHandlers;

        // Token: 0x04000A77 RID: 2679
        private DynamicGridProjector[] _dynamicGridProjectors;

        // Token: 0x04000A78 RID: 2680
        private StructureTriggerListener[] _structureTriggerListener;

        // Token: 0x04000A79 RID: 2681
        private int _rootComponentAxisRotation;

        // Token: 0x04000A7A RID: 2682
        private AgentGestalt.MassCategories _massCategory;

        // Token: 0x04000A7B RID: 2683
        private float _massMultiplier;

        // Token: 0x04000A7C RID: 2684
        private int _currentNusLabelIndex;

        // Token: 0x04000A7D RID: 2685
        private PhysicMaterial _physicMaterial;

        // Token: 0x04000A7E RID: 2686
        private static List<Vector3> _cycleRotations = new List<Vector3>
    {
        Vector3.forward,
        Vector3.down,
        Vector3.back,
        Vector3.left,
        Vector3.up,
        Vector3.right
    };
    }

}
