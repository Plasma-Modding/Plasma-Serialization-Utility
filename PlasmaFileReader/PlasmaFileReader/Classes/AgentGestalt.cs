using System;
using System.Collections.Generic;
//using System.Diagnostics.Eventing.Reader;
using System.Reflection;
//using System.Windows.Markup;
//using Behavior;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PlasmaFileReader.Plasma.Classes
{
    // Token: 0x02000056 RID: 86
    [CreateAssetMenu(menuName = "Plasma/Agent Gestalt")]
    public class AgentGestalt : SerializedScriptableObject
    {

        // Token: 0x0400049B RID: 1179
        public const int propertyPortsStart = 64;

        // Token: 0x0400049C RID: 1180
        public const int moduleInterfaceCommandPortsStart = 128;

        // Token: 0x0400049D RID: 1181
        public const int moduleInterfacePropertyPortsStart = 192;

        // Token: 0x0400049E RID: 1182
        public const int moduleInterfaceOutputPortsStart = 256;

        // Token: 0x0400049F RID: 1183
        public const int remotePortId = 512;

        // Token: 0x040004A0 RID: 1184
        public const int moduleInterfacePropertiesStart = 256;

        // Token: 0x040004A1 RID: 1185
        public const int moduleInterfaceOperationId = 64;

        // Token: 0x040004A2 RID: 1186
        public const int moduleInterfacePropertyHandlerId = 64;

        // Token: 0x040004A3 RID: 1187
        public AgentGestalt.Types type;

        // Token: 0x040004A4 RID: 1188
        public Type agent;

        // Token: 0x040004A5 RID: 1189
        public string displayName;

        // Token: 0x040004A6 RID: 1190
        public bool needsResources;

        // Token: 0x040004A7 RID: 1191
        public string description;

        // Token: 0x040004A8 RID: 1192
        public string keywords;

        // Token: 0x040004A9 RID: 1193
        public bool developersOnly;

        // Token: 0x040004AA RID: 1194
        public bool hideDuringStage;

        // Token: 0x040004AB RID: 1195
        public bool canBeModule;

        // Token: 0x040004AC RID: 1196
        public bool processesModuleInterfaces;

        // Token: 0x040004AD RID: 1197
        public bool handlesModuleProperties;

        // Token: 0x040004AE RID: 1198
        public Dictionary<int, AgentGestalt.Property> properties;

        // Token: 0x040004AF RID: 1199
        public Dictionary<int, AgentGestalt.Port> ports;

        // Token: 0x040004B0 RID: 1200
        public AgentGestalt.ComponentCategories componentCategory;

        // Token: 0x040004B1 RID: 1201
        public float componentMass;

        // Token: 0x040004B2 RID: 1202
        public bool defaultsToKinematic;

        // Token: 0x040004B3 RID: 1203
        public AgentGestalt.MassCategories defaultMassCategory;

        // Token: 0x040004B4 RID: 1204
        public float componentPlasmaConsumption;

        // Token: 0x040004B5 RID: 1205
        public Type deviceComponentModifier;

        // Token: 0x040004B6 RID: 1206
        public bool modifierAffectsAllBodies;

        // Token: 0x040004B7 RID: 1207
        public bool componentInteractive;

        // Token: 0x040004B8 RID: 1208
        public bool componentInteractionLocksCamera;

        // Token: 0x040004B9 RID: 1209
        public bool componentPlasmaInteraction;

        // Token: 0x040004BA RID: 1210
        public bool componentCanHaveFocus;

        // Token: 0x040004BB RID: 1211
        public bool componentFocusLocksCamera;

        // Token: 0x040004BC RID: 1212
        public bool componentReactsToRaycast;

        // Token: 0x040004BD RID: 1213
        public bool componentHidesHintsUnderRaycast;

        // Token: 0x040004BE RID: 1214
        public bool componentAllowsDocking;

        // Token: 0x040004BF RID: 1215
        public bool componentAllowsMounting;

        // Token: 0x040004C0 RID: 1216
        public bool simulatedPhysicsWhenMounted;

        // Token: 0x040004C1 RID: 1217
        public bool componentSupportSecondarySnappingPointChild;

        // Token: 0x040004C2 RID: 1218
        public bool componentAllowNonUniformScale;

        // Token: 0x040004C3 RID: 1219
        public List<AgentGestalt.NUSDefinition> nusDefinitions;

        // Token: 0x040004C4 RID: 1220
        public Vector3 spawnScale;

        // Token: 0x040004C5 RID: 1221
        public FloatRange componentScaleXLimits;

        // Token: 0x040004C6 RID: 1222
        public FloatRange componentScaleYLimits;

        // Token: 0x040004C7 RID: 1223
        public FloatRange componentScaleZLimits;

        // Token: 0x040004C8 RID: 1224
        public bool componentReactsToScaling;

        // Token: 0x040004C9 RID: 1225
        public bool componentDisableScaling;

        // Token: 0x040004CA RID: 1226
        public bool componentVolumeControl;

        // Token: 0x040004CB RID: 1227
        public GameObject componentPrefab;

        // Token: 0x040004CC RID: 1228
        public Dictionary<int, string> componentIds;

        // Token: 0x040004CD RID: 1229
        public Sprite componentIcon;

        // Token: 0x040004CE RID: 1230
        public Sprite componentPreview;

        // Token: 0x040004CF RID: 1231
        public bool componentHidden;

        // Token: 0x040004D0 RID: 1232
        public bool affectedByProjectileExplosion;

        // Token: 0x040004D1 RID: 1233
        public AgentCategoryEnum nodeCategory;

        // Token: 0x040004D2 RID: 1234
        public bool nodeAlwaysRun;

        // Token: 0x040004D3 RID: 1235
        public bool nodeRepeatable;

        // Token: 0x040004D4 RID: 1236
        public bool advanced;

        // Token: 0x040004D5 RID: 1237
        public bool hideNode;

        // Token: 0x040004D6 RID: 1238
        public AgentGestaltEnum id;

        // Token: 0x040004D7 RID: 1239
        public static AgentGestalt instance;

        // Token: 0x0200033D RID: 829
        public class Property
        {

            // Token: 0x04001B2B RID: 6955
            public string name;

            // Token: 0x04001B2C RID: 6956
            public bool injectable;

            // Token: 0x04001B2D RID: 6957
            public bool allowsAnyData;

            // Token: 0x04001B2E RID: 6958
            public Data defaultData = new Data();

            // Token: 0x04001B2F RID: 6959
            public bool useLimits;

            // Token: 0x04001B30 RID: 6960
            public bool dynamicLimits;

            // Token: 0x04001B31 RID: 6961
            public FloatRange limits;

            // Token: 0x04001B32 RID: 6962
            public bool isVariable;

            // Token: 0x04001B33 RID: 6963
            public bool isScript;

            // Token: 0x04001B34 RID: 6964
            public bool isPowerBoolean;

            // Token: 0x04001B35 RID: 6965
            public bool configurable;

            // Token: 0x04001B36 RID: 6966
            public bool accessible;

            // Token: 0x04001B37 RID: 6967
            public int position;

            // Token: 0x04001B38 RID: 6968
            public string description;

            // Token: 0x04001B39 RID: 6969
            public int handler;

            // Token: 0x04001B3A RID: 6970
            public int driverCommand;

            // Token: 0x04001B3B RID: 6971
            public bool alwaysTrigger;

            // Token: 0x04001B3C RID: 6972
            public bool validateNodeOnSet;

            // Token: 0x04001B3D RID: 6973
            public bool assetOwnership;

            // Token: 0x04001B3E RID: 6974
            public bool hidePort;
        }

        // Token: 0x0200033E RID: 830
        public class Port
        {
            // Token: 0x06002001 RID: 8193 RVA: 0x0009F1F8 File Offset: 0x0009D3F8
            private void CreateInjectablePropertyAndAssignIt()
            {
                if (AgentGestalt.instance.properties == null)
                {
                    AgentGestalt.instance.properties = new Dictionary<int, AgentGestalt.Property>();
                }
                int num = 64;
                foreach (int num2 in AgentGestalt.instance.properties.Keys)
                {
                    if (num2 >= num)
                    {
                        num = num2 + 1;
                    }
                }
                AgentGestalt.Property property = new AgentGestalt.Property();
                property.name = this.name;
                property.injectable = true;
                AgentGestalt.instance.properties.Add(num, property);
                this.injectedProperty = num;
            }

            // Token: 0x06002002 RID: 8194 RVA: 0x0009F2A4 File Offset: 0x0009D4A4
            private bool ValidatePropertyValue(Data newPropertyValue)
            {
                return this.mappedProperty == 0 || (newPropertyValue != null && newPropertyValue.type != Data.Types.None);
            }

            // Token: 0x06002003 RID: 8195 RVA: 0x0009F2C2 File Offset: 0x0009D4C2
            private bool ShouldShowButton()
            {
                return this.injectedProperty == 0 && this.type == AgentGestalt.Port.Types.Output;
            }

            // Token: 0x06002004 RID: 8196 RVA: 0x0009F2D7 File Offset: 0x0009D4D7
            private bool ShouldShowOnlyAppliesData()
            {
                return this.type == AgentGestalt.Port.Types.Command && this.mappedProperty != 0;
            }

            // Token: 0x06002005 RID: 8197 RVA: 0x0009F2EC File Offset: 0x0009D4EC
            private bool ShouldShowExpectsData()
            {
                return this.type == AgentGestalt.Port.Types.Command && this.mappedProperty != 0 && !this.onlyAppliesData;
            }

            // Token: 0x06002006 RID: 8198 RVA: 0x0009F309 File Offset: 0x0009D509
            private bool ShouldShowAnyType()
            {
                return (this.type == AgentGestalt.Port.Types.Output && this.injectedProperty == 0) || (this.type == AgentGestalt.Port.Types.Command && this.mappedProperty != 0 && this.expectsData);
            }

            // Token: 0x06002007 RID: 8199 RVA: 0x0009F336 File Offset: 0x0009D536
            private bool ShouldShowDataType()
            {
                return !this.allowsAnyData && this.ShouldShowAnyType();
            }

            // Token: 0x06002008 RID: 8200 RVA: 0x0009F348 File Offset: 0x0009D548
            private bool ShouldShowMappedProperty()
            {
                return this.type == AgentGestalt.Port.Types.Command && AgentGestalt.instance.properties != null && AgentGestalt.instance.properties.Count > 0;
            }

            // Token: 0x06002009 RID: 8201 RVA: 0x0009F372 File Offset: 0x0009D572
            private bool ShouldShowInjectedProperty()
            {
                return this.type == AgentGestalt.Port.Types.Output && AgentGestalt.instance.properties != null && AgentGestalt.instance.properties.Count > 0;
            }

            // Token: 0x0600200A RID: 8202 RVA: 0x0009F3A0 File Offset: 0x0009D5A0
            private void UpdateDataType()
            {
                if (AgentGestalt.instance != null && AgentGestalt.instance.properties != null && this.mappedProperty != 0)
                {
                    AgentGestalt.Property property = AgentGestalt.instance.properties[this.mappedProperty];
                    this.allowsAnyData = property.allowsAnyData;
                    this.dataType = property.defaultData.type;
                }
            }

            // Token: 0x0600200B RID: 8203 RVA: 0x0009F401 File Offset: 0x0009D601
            private void UpdateExpectsData()
            {
                if (this.onlyAppliesData)
                {
                    this.expectsData = true;
                }
            }

            // Token: 0x0600200C RID: 8204 RVA: 0x0009F412 File Offset: 0x0009D612
            private IList<ValueDropdownItem<Data.Types>> ValidTypes()
            {
                return new ValueDropdownList<Data.Types>
                {
                    Data.Types.Boolean,
                Data.Types.Color,
                Data.Types.ComponentProperty,
                Data.Types.Image,
                Data.Types.Number,
                Data.Types.Selection,
                Data.Types.Sound,
                Data.Types.String
            };
            }

            // Token: 0x06002010 RID: 8208 RVA: 0x0009F64C File Offset: 0x0009D84C
            private bool ShouldShowPropertyValue()
            {
                return this.type == AgentGestalt.Port.Types.Command && !this.expectsData && !this.onlyAppliesData && this.mappedProperty != 0;
            }

            // Token: 0x06002011 RID: 8209 RVA: 0x0009F674 File Offset: 0x0009D874
            private bool ShouldMapCommand()
            {
                return AgentGestalt.instance.type == AgentGestalt.Types.Component && this.type == AgentGestalt.Port.Types.Command && this.mappedProperty == 0 && AgentGestalt.instance.agent != null && (AgentGestalt.instance.agent.GetNestedType("Commands") != null || AgentGestalt.instance.agent.BaseType.GetNestedType("Commands") != null || AgentGestalt.instance.agent.BaseType.BaseType.GetNestedType("Commands") != null);
            }

            // Token: 0x06002013 RID: 8211 RVA: 0x0009F868 File Offset: 0x0009DA68
            private bool ShouldMapOperation()
            {
                return this.type == AgentGestalt.Port.Types.Command && !this.onlyAppliesData && AgentGestalt.instance.agent != null;
            }

            // Token: 0x06002015 RID: 8213 RVA: 0x0009F9EA File Offset: 0x0009DBEA
            private bool ShouldShowWillRetrigger()
            {
                return (this.type == AgentGestalt.Port.Types.Command || this.type == AgentGestalt.Port.Types.Property) && AgentGestalt.instance.nodeAlwaysRun;
            }

            // Token: 0x04001B3F RID: 6975
            public AgentGestalt.Port.Types type;

            // Token: 0x04001B40 RID: 6976
            public string name;

            // Token: 0x04001B41 RID: 6977
            public int position;

            // Token: 0x04001B42 RID: 6978
            public string description;

            // Token: 0x04001B43 RID: 6979
            public int mappedProperty;

            // Token: 0x04001B44 RID: 6980
            public bool onlyAppliesData;

            // Token: 0x04001B45 RID: 6981
            public bool expectsData;

            // Token: 0x04001B46 RID: 6982
            public int injectedProperty;

            // Token: 0x04001B47 RID: 6983
            public bool allowsAnyData;

            // Token: 0x04001B48 RID: 6984
            public Data.Types dataType;

            // Token: 0x04001B49 RID: 6985
            public Data propertyValue;

            // Token: 0x04001B4A RID: 6986
            public int operation;

            // Token: 0x04001B4B RID: 6987
            public bool willRetrigger;

            // Token: 0x04001B4C RID: 6988
            public bool hidePort;

            // Token: 0x04001B4D RID: 6989
            [HideInInspector]
            public bool fromModuleInterface;

            // Token: 0x020004BC RID: 1212
            public enum Types
            {
                // Token: 0x040020D8 RID: 8408
                Command,
                // Token: 0x040020D9 RID: 8409
                Property,
                // Token: 0x040020DA RID: 8410
                Output
            }
        }

        // Token: 0x0200033F RID: 831
        public enum Types
        {
            // Token: 0x04001B4F RID: 6991
            Component,
            // Token: 0x04001B50 RID: 6992
            Logic
        }

        // Token: 0x02000340 RID: 832
        public enum ComponentCategories
        {
            // Token: 0x04001B52 RID: 6994
            Behavior,
            // Token: 0x04001B53 RID: 6995
            Mechanic,
            // Token: 0x04001B54 RID: 6996
            Basic,
            // Token: 0x04001B55 RID: 6997
            Structure,
            // Token: 0x04001B56 RID: 6998
            Decorative,
            // Token: 0x04001B57 RID: 6999
            All = 100
        }

        // Token: 0x02000341 RID: 833
        public enum MassCategories
        {
            // Token: 0x04001B59 RID: 7001
            Light,
            // Token: 0x04001B5A RID: 7002
            Medium,
            // Token: 0x04001B5B RID: 7003
            Heavy,
            // Token: 0x04001B5C RID: 7004
            Zero
        }

        // Token: 0x02000342 RID: 834
        public enum NUSLabel
        {
            // Token: 0x04001B5E RID: 7006
            None,
            // Token: 0x04001B5F RID: 7007
            Height,
            // Token: 0x04001B60 RID: 7008
            Width,
            // Token: 0x04001B61 RID: 7009
            Depth,
            // Token: 0x04001B62 RID: 7010
            Diameter
        }

        // Token: 0x02000343 RID: 835
        public enum NUSAxis
        {
            // Token: 0x04001B64 RID: 7012
            X,
            // Token: 0x04001B65 RID: 7013
            Y,
            // Token: 0x04001B66 RID: 7014
            Z
        }

        // Token: 0x02000344 RID: 836
        public class NUSDefinition
        {
            // Token: 0x04001B67 RID: 7015
            public AgentGestalt.NUSLabel label;

            // Token: 0x04001B68 RID: 7016
            public List<AgentGestalt.NUSAxis> axis;

            // Token: 0x04001B69 RID: 7017
            public float unitSize;

            // Token: 0x04001B6A RID: 7018
            public bool overrideCellSize;

            // Token: 0x04001B6B RID: 7019
            public float cellSize;
        }
    }

}
