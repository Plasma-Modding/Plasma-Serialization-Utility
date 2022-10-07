using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using Behavior;
//using Common.Math;
//using FMOD.Studio;
//using FMODUnity;
//using Rewired;
using UnityEngine;

namespace Plasma.Classes
{
    // Token: 0x02000146 RID: 326
    public class Device : MonoBehaviour
    {
        // Token: 0x14000018 RID: 24
        // (add) Token: 0x06000CBA RID: 3258 RVA: 0x00040718 File Offset: 0x0003E918
        // (remove) Token: 0x06000CBB RID: 3259 RVA: 0x0004074C File Offset: 0x0003E94C
        public static event Device.DeviceEvent OnDeviceStateToggled;

        // Token: 0x170000D1 RID: 209
        // (get) Token: 0x06000CBC RID: 3260 RVA: 0x0004077F File Offset: 0x0003E97F
        // (set) Token: 0x06000CBD RID: 3261 RVA: 0x00040787 File Offset: 0x0003E987
        public string metaDataName { get; set; }

        // Token: 0x170000D2 RID: 210
        // (get) Token: 0x06000CBE RID: 3262 RVA: 0x00040790 File Offset: 0x0003E990
        // (set) Token: 0x06000CBF RID: 3263 RVA: 0x00040798 File Offset: 0x0003E998
        public int saveCounter { get; set; }

        // Token: 0x170000D3 RID: 211
        // (get) Token: 0x06000CC0 RID: 3264 RVA: 0x000407A1 File Offset: 0x0003E9A1
        public bool isWireframe
        {
            get
            {
                return this._state == Device.State.Wireframe;
            }
        }

        // Token: 0x170000D4 RID: 212
        // (get) Token: 0x06000CC1 RID: 3265 RVA: 0x000407AC File Offset: 0x0003E9AC
        public bool isSolid
        {
            get
            {
                return this._state == Device.State.Solid;
            }
        }

        // Token: 0x170000D5 RID: 213
        // (get) Token: 0x06000CC2 RID: 3266 RVA: 0x000407B7 File Offset: 0x0003E9B7
        // (set) Token: 0x06000CC3 RID: 3267 RVA: 0x000407BF File Offset: 0x0003E9BF
        public MechanicState mechanicState { get; set; }

        // Token: 0x170000D6 RID: 214
        // (get) Token: 0x06000CC4 RID: 3268 RVA: 0x000407C8 File Offset: 0x0003E9C8
        // (set) Token: 0x06000CC5 RID: 3269 RVA: 0x000407D0 File Offset: 0x0003E9D0
        public int guid { get; set; }

        // Token: 0x170000D7 RID: 215
        // (get) Token: 0x06000CC6 RID: 3270 RVA: 0x000407D9 File Offset: 0x0003E9D9
        public Device.State state
        {
            get
            {
                return this._state;
            }
        }

        // Token: 0x170000D8 RID: 216
        // (get) Token: 0x06000CC7 RID: 3271 RVA: 0x000407E1 File Offset: 0x0003E9E1
        // (set) Token: 0x06000CC8 RID: 3272 RVA: 0x000407E9 File Offset: 0x0003E9E9
        public string displayName { get; set; }

        // Token: 0x170000D9 RID: 217
        // (get) Token: 0x06000CC9 RID: 3273 RVA: 0x000407F2 File Offset: 0x0003E9F2
        // (set) Token: 0x06000CCA RID: 3274 RVA: 0x000407FA File Offset: 0x0003E9FA
        public List<ComponentHandler> allComponents { get; private set; }

        // Token: 0x170000DA RID: 218
        // (get) Token: 0x06000CCB RID: 3275 RVA: 0x00040803 File Offset: 0x0003EA03
        // (set) Token: 0x06000CCC RID: 3276 RVA: 0x0004080B File Offset: 0x0003EA0B
        public List<ComponentHandler> allComponentsHierarchy { get; private set; }

        // Token: 0x170000DB RID: 219
        // (get) Token: 0x06000CCD RID: 3277 RVA: 0x00040814 File Offset: 0x0003EA14
        // (set) Token: 0x06000CCE RID: 3278 RVA: 0x0004081C File Offset: 0x0003EA1C
        public List<ProcessorAgent> allControllers { get; private set; }

        // Token: 0x170000DC RID: 220
        // (get) Token: 0x06000CCF RID: 3279 RVA: 0x00040828 File Offset: 0x0003EA28
        private IEnumerable<ComponentDriver> allDrivers
        {
            get
            {
                List<ComponentDriver> list = new List<ComponentDriver>();
                foreach (Agent agent in this._agents.Values)
                {
                    if (agent.driver != null)
                    {
                        list.Add(agent.driver);
                    }
                }
                return list;
            }
        }

        // Token: 0x170000DD RID: 221
        // (get) Token: 0x06000CD0 RID: 3280 RVA: 0x0004089C File Offset: 0x0003EA9C
        public IEnumerable<AgentId> allAgentIds
        {
            get
            {
                return this._agents.Keys;
            }
        }

        // Token: 0x170000DE RID: 222
        // (get) Token: 0x06000CD1 RID: 3281 RVA: 0x000408A9 File Offset: 0x0003EAA9
        public IEnumerable<Agent> agents
        {
            get
            {
                return this._agents.Values;
            }
        }

        // Token: 0x170000DF RID: 223
        // (get) Token: 0x06000CD2 RID: 3282 RVA: 0x000408B6 File Offset: 0x0003EAB6
        public Dictionary<AgentId, Agent> agentsDictionary
        {
            get
            {
                return this._agents;
            }
        }

        // Token: 0x170000E0 RID: 224
        // (get) Token: 0x06000CD3 RID: 3283 RVA: 0x000408BE File Offset: 0x0003EABE
        // (set) Token: 0x06000CD4 RID: 3284 RVA: 0x000408C6 File Offset: 0x0003EAC6
        public int numberOfControllers { get; private set; }

        // Token: 0x170000E1 RID: 225
        // (get) Token: 0x06000CD5 RID: 3285 RVA: 0x000408CF File Offset: 0x0003EACF
        // (set) Token: 0x06000CD6 RID: 3286 RVA: 0x000408D7 File Offset: 0x0003EAD7
        public ComponentHandler onlyController { get; private set; }

        // Token: 0x170000E2 RID: 226
        // (get) Token: 0x06000CD7 RID: 3287 RVA: 0x000408E0 File Offset: 0x0003EAE0
        // (set) Token: 0x06000CD8 RID: 3288 RVA: 0x000408E8 File Offset: 0x0003EAE8
        public ComponentHandler rootComponent { get; set; }

        // Token: 0x170000E3 RID: 227
        // (get) Token: 0x06000CD9 RID: 3289 RVA: 0x000408F1 File Offset: 0x0003EAF1
        // (set) Token: 0x06000CDA RID: 3290 RVA: 0x000408F9 File Offset: 0x0003EAF9
        public VFXDevice vfxDevice { get; private set; }

        // Token: 0x170000E4 RID: 228
        // (get) Token: 0x06000CDB RID: 3291 RVA: 0x00040902 File Offset: 0x0003EB02
        // (set) Token: 0x06000CDC RID: 3292 RVA: 0x0004090A File Offset: 0x0003EB0A
        public bool needsToEnabledColliders { get; set; }

        // Token: 0x170000E5 RID: 229
        // (get) Token: 0x06000CDD RID: 3293 RVA: 0x00040913 File Offset: 0x0003EB13
        public Articulation firstArticulation
        {
            get
            {
                return this._articulations[0];
            }
        }

        // Token: 0x170000E6 RID: 230
        // (get) Token: 0x06000CDE RID: 3294 RVA: 0x00040921 File Offset: 0x0003EB21
        public List<Articulation> articulations
        {
            get
            {
                return this._articulations;
            }
        }

        // Token: 0x170000E7 RID: 231
        // (get) Token: 0x06000CDF RID: 3295 RVA: 0x00040929 File Offset: 0x0003EB29
        // (set) Token: 0x06000CE0 RID: 3296 RVA: 0x00040931 File Offset: 0x0003EB31
        public bool dirtyHierarchy { get; set; }

        // Token: 0x170000E8 RID: 232
        // (get) Token: 0x06000CE1 RID: 3297 RVA: 0x0004093A File Offset: 0x0003EB3A
        // (set) Token: 0x06000CE2 RID: 3298 RVA: 0x00040942 File Offset: 0x0003EB42
        public bool dirtyBounds { get; set; }

        // Token: 0x170000E9 RID: 233
        // (get) Token: 0x06000CE3 RID: 3299 RVA: 0x0004094B File Offset: 0x0003EB4B
        // (set) Token: 0x06000CE4 RID: 3300 RVA: 0x00040953 File Offset: 0x0003EB53
        public Quaternion wireframeRootComponentRotation { get; set; }

        // Token: 0x170000EA RID: 234
        // (get) Token: 0x06000CE5 RID: 3301 RVA: 0x0004095C File Offset: 0x0003EB5C
        // (set) Token: 0x06000CE6 RID: 3302 RVA: 0x00040964 File Offset: 0x0003EB64
        public float wireframeDistanceFromTerrain { get; set; }

        // Token: 0x170000EB RID: 235
        // (get) Token: 0x06000CE7 RID: 3303 RVA: 0x0004096D File Offset: 0x0003EB6D
        // (set) Token: 0x06000CE8 RID: 3304 RVA: 0x00040975 File Offset: 0x0003EB75
        public bool kinematicBase { get; set; }

        // Token: 0x170000EC RID: 236
        // (get) Token: 0x06000CE9 RID: 3305 RVA: 0x0004097E File Offset: 0x0003EB7E
        // (set) Token: 0x06000CEA RID: 3306 RVA: 0x00040986 File Offset: 0x0003EB86
        public bool shouldShowTreeLines { get; set; }

        // Token: 0x170000ED RID: 237
        // (get) Token: 0x06000CEB RID: 3307 RVA: 0x00040990 File Offset: 0x0003EB90
        public Vector3 worldCenter
        {
            get
            {
                int num = 0;
                Vector3 a = Vector3.zero;
                foreach (ComponentHandler componentHandler in this.allComponents)
                {
                    a += (this.isWireframe ? componentHandler.wireframePosition : componentHandler.solidPosition);
                    num++;
                }
                return a / (float)num;
            }
        }

        // Token: 0x170000EE RID: 238
        // (get) Token: 0x06000CEC RID: 3308 RVA: 0x00040A10 File Offset: 0x0003EC10
        // (set) Token: 0x06000CED RID: 3309 RVA: 0x00040A18 File Offset: 0x0003EC18
        public DateTime creationDate { get; set; }

        // Token: 0x170000EF RID: 239
        // (get) Token: 0x06000CEE RID: 3310 RVA: 0x00040A21 File Offset: 0x0003EC21
        // (set) Token: 0x06000CEF RID: 3311 RVA: 0x00040A29 File Offset: 0x0003EC29
        public Dictionary<WorldController.GlobalPermissions, WorldController.GlobalPermissionStates> permissions { get; set; }

        // Token: 0x170000F0 RID: 240
        // (get) Token: 0x06000CF0 RID: 3312 RVA: 0x00040A34 File Offset: 0x0003EC34
        public static Dictionary<WorldController.GlobalPermissions, WorldController.GlobalPermissionStates> defaultPermissions
        {
            get
            {
                return new Dictionary<WorldController.GlobalPermissions, WorldController.GlobalPermissionStates>
            {
                {
                    WorldController.GlobalPermissions.Clone,
                    WorldController.GlobalPermissionStates.Global
                },
                {
                    WorldController.GlobalPermissions.AttachDetach,
                    WorldController.GlobalPermissionStates.Global
                },
                {
                    WorldController.GlobalPermissions.ComponentManipulation,
                    WorldController.GlobalPermissionStates.Global
                },
                {
                    WorldController.GlobalPermissions.DeviceDelete,
                    WorldController.GlobalPermissionStates.Global
                },
                {
                    WorldController.GlobalPermissions.GrabSolid,
                    WorldController.GlobalPermissionStates.Global
                },
                {
                    WorldController.GlobalPermissions.GrabWireframe,
                    WorldController.GlobalPermissionStates.Global
                },
                {
                    WorldController.GlobalPermissions.PaintComponent,
                    WorldController.GlobalPermissionStates.Global
                },
                {
                    WorldController.GlobalPermissions.ResetDevice,
                    WorldController.GlobalPermissionStates.Global
                },
                {
                    WorldController.GlobalPermissions.SketchAccess,
                    WorldController.GlobalPermissionStates.Global
                },
                {
                    WorldController.GlobalPermissions.SketchEdit,
                    WorldController.GlobalPermissionStates.Global
                },
                {
                    WorldController.GlobalPermissions.StateToggle,
                    WorldController.GlobalPermissionStates.Global
                },
                {
                    WorldController.GlobalPermissions.PropertyEditorAccess,
                    WorldController.GlobalPermissionStates.Global
                }
            };
            }
        }

        // Token: 0x170000F1 RID: 241
        // (get) Token: 0x06000CF1 RID: 3313 RVA: 0x00040AC1 File Offset: 0x0003ECC1
        public bool isMounted
        {
            get
            {
                return this._mounted;
            }
        }

        // Token: 0x170000F2 RID: 242
        // (get) Token: 0x06000CF2 RID: 3314 RVA: 0x00040AC9 File Offset: 0x0003ECC9
        // (set) Token: 0x06000CF3 RID: 3315 RVA: 0x00040AD1 File Offset: 0x0003ECD1
        public bool freshlySplit { get; set; }

        // Token: 0x170000F3 RID: 243
        // (get) Token: 0x06000CF4 RID: 3316 RVA: 0x00040ADA File Offset: 0x0003ECDA
        // (set) Token: 0x06000CF5 RID: 3317 RVA: 0x00040AE2 File Offset: 0x0003ECE2
        public bool freshlyLoaded { get; set; }

        // Token: 0x170000F4 RID: 244
        // (get) Token: 0x06000CF6 RID: 3318 RVA: 0x00040AEB File Offset: 0x0003ECEB
        // (set) Token: 0x06000CF7 RID: 3319 RVA: 0x00040AF3 File Offset: 0x0003ECF3
        public Device.State stageState { get; set; }

        // Token: 0x170000F5 RID: 245
        // (get) Token: 0x06000CF8 RID: 3320 RVA: 0x00040AFC File Offset: 0x0003ECFC
        // (set) Token: 0x06000CF9 RID: 3321 RVA: 0x00040B04 File Offset: 0x0003ED04
        public bool stageReset { get; set; }

        // Token: 0x170000F6 RID: 246
        // (get) Token: 0x06000CFA RID: 3322 RVA: 0x00040B0D File Offset: 0x0003ED0D
        // (set) Token: 0x06000CFB RID: 3323 RVA: 0x00040B15 File Offset: 0x0003ED15
        public int primaryColorId { get; set; }

        // Token: 0x170000F7 RID: 247
        // (get) Token: 0x06000CFC RID: 3324 RVA: 0x00040B1E File Offset: 0x0003ED1E
        // (set) Token: 0x06000CFD RID: 3325 RVA: 0x00040B26 File Offset: 0x0003ED26
        public int secondaryColorId { get; set; }

        // Token: 0x170000F8 RID: 248
        // (get) Token: 0x06000CFE RID: 3326 RVA: 0x00040B30 File Offset: 0x0003ED30
        public bool canBeModule
        {
            get
            {
                using (List<ProcessorAgent>.Enumerator enumerator = this.allControllers.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        if (enumerator.Current.moduleInterface.isValid)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        // Token: 0x170000F9 RID: 249
        // (get) Token: 0x06000CFF RID: 3327 RVA: 0x00040B90 File Offset: 0x0003ED90
        // (set) Token: 0x06000D00 RID: 3328 RVA: 0x00040B98 File Offset: 0x0003ED98
        public bool isModule { get; set; }

        // Token: 0x170000FA RID: 250
        // (get) Token: 0x06000D01 RID: 3329 RVA: 0x00040BA1 File Offset: 0x0003EDA1
        public bool isEditingSockets
        {
            get
            {
                return this._editingSockets;
            }
        }

        // Token: 0x170000FB RID: 251
        // (get) Token: 0x06000D02 RID: 3330 RVA: 0x00040BA9 File Offset: 0x0003EDA9
        // (set) Token: 0x06000D03 RID: 3331 RVA: 0x00040BB1 File Offset: 0x0003EDB1
        public bool isLoading { get; set; }

        // Token: 0x06000D28 RID: 3368 RVA: 0x0004238E File Offset: 0x0004058E
        private void PlayVFXSolid()
        {
            this.vfxDevice.command = VFXDevice.Commands.GoToSolidAnimated;
        }

        // Token: 0x06000D29 RID: 3369 RVA: 0x0004239C File Offset: 0x0004059C
        private void PlayVFXWireframe()
        {
            this.vfxDevice.command = VFXDevice.Commands.GoToWireframeAnimated;
        }

        // Token: 0x06000D2A RID: 3370 RVA: 0x000423AA File Offset: 0x000405AA
        private void UpdateVFXWireframe()
        {
            this.dirtyBounds = true;
            this.vfxDevice.command = VFXDevice.Commands.Update;
        }

        // Token: 0x06000D33 RID: 3379 RVA: 0x00042AD0 File Offset: 0x00040CD0
        public string GetDisplayNameFor(AgentGestalt componentGestalt)
        {
            int num = 1;
            while (!this.IsDisplayNameAvailable(Device.DisplayName(componentGestalt, num), null))
            {
                num++;
            }
            return Device.DisplayName(componentGestalt, num);
        }

        // Token: 0x06000D34 RID: 3380 RVA: 0x00042AFC File Offset: 0x00040CFC
        private static string DisplayName(AgentGestalt componentGestalt, int i)
        {
            if (i == 1)
            {
                return componentGestalt.displayName;
            }
            return componentGestalt.displayName + " " + i.ToString("D2");
        }

        // Token: 0x06000D35 RID: 3381 RVA: 0x00042B28 File Offset: 0x00040D28
        public string GetDisplayNameForExcluding(AgentGestalt componentGestalt, List<string> exclusionList)
        {
            int num = 1;
            while (exclusionList.Contains(Device.DisplayName(componentGestalt, num)) || !this.IsDisplayNameAvailable(Device.DisplayName(componentGestalt, num), null))
            {
                num++;
            }
            return Device.DisplayName(componentGestalt, num);
        }

        // Token: 0x06000D36 RID: 3382 RVA: 0x00042B64 File Offset: 0x00040D64
        public bool IsDisplayNameAvailable(string theDisplayName, ComponentHandler currentComponent = null)
        {
            if (string.IsNullOrEmpty(theDisplayName))
            {
                return false;
            }
            foreach (ComponentHandler componentHandler in this.allComponents)
            {
                if (componentHandler.agentId.displayName.ToUpper().Equals(theDisplayName.ToUpper()) && currentComponent != componentHandler)
                {
                    return false;
                }
            }
            return true;
        }

        // Token: 0x06000D37 RID: 3383 RVA: 0x00042BE8 File Offset: 0x00040DE8
        public void MoveAgent(Agent agent)
        {
            if (!this._agents.ContainsKey(agent.agentId))
            {
                agent.SetDevice(this);
                this._agents.Add(agent.agentId, agent);
                if (agent.agentId.guid >= this._guidIndex)
                {
                    this._guidIndex = agent.agentId.guid + 1;
                }
                string str = "Moved agent for ";
                AgentId agentId = agent.agentId;
                //this.LogVerbose(str + ((agentId != null) ? agentId.ToString() : null));
                return;
            }
            string str2 = "Cannot move agent because of an existing id: ";
            AgentId agentId2 = agent.agentId;
            //this.LogWarning(str2 + ((agentId2 != null) ? agentId2.ToString() : null));
        }

        // Token: 0x06000D3B RID: 3387 RVA: 0x00042DE5 File Offset: 0x00040FE5
        public bool DoesAgentExist(AgentId agentId)
        {
            return this._agents.ContainsKey(agentId);
        }

        // Token: 0x06000D3C RID: 3388 RVA: 0x00042DF4 File Offset: 0x00040FF4
        public Agent GetComponentAgentByDisplayName(string componentDisplayName)
        {
            componentDisplayName = componentDisplayName.ToUpperInvariant();
            foreach (KeyValuePair<AgentId, Agent> keyValuePair in this._agents)
            {
                if (keyValuePair.Key.type == AgentGestalt.Types.Component && keyValuePair.Key.displayName.ToUpperInvariant().Equals(componentDisplayName))
                {
                    return keyValuePair.Value;
                }
            }
            return null;
        }
        
        // Token: 0x06000D3E RID: 3390 RVA: 0x00042FA0 File Offset: 0x000411A0
        public void QueuePropertyOnModule(AgentId agentId, int propertyId, Data propertyData)
        {
            Dictionary<int, List<Data>> dictionary;
            if (!this._writeableAgentModuleQueues.properties.TryGetValue(agentId, out dictionary))
            {
                dictionary = new Dictionary<int, List<Data>>();
                this._writeableAgentModuleQueues.properties.Add(agentId, dictionary);
            }
            List<Data> list;
            if (!dictionary.TryGetValue(propertyId, out list))
            {
                list = new List<Data>();
                dictionary.Add(propertyId, list);
            }
            list.Add(propertyData);
        }

        // Token: 0x06000D3F RID: 3391 RVA: 0x00042FFC File Offset: 0x000411FC
        public Dictionary<int, List<Data>> FetchPropertiesOnModule(AgentId agentId)
        {
            Dictionary<int, List<Data>> result;
            this._readableAgentModuleQueues.properties.TryGetValue(agentId, out result);
            return result;
        }

        // Token: 0x06000D40 RID: 3392 RVA: 0x00043020 File Offset: 0x00041220
        public void QueueCommandPortOnModule(AgentId agentId, int commandPortId)
        {
            Dictionary<int, int> dictionary;
            if (!this._writeableAgentModuleQueues.commandPorts.TryGetValue(agentId, out dictionary))
            {
                dictionary = new Dictionary<int, int>();
                this._writeableAgentModuleQueues.commandPorts.Add(agentId, dictionary);
            }
            int num;
            if (dictionary.TryGetValue(commandPortId, out num))
            {
                dictionary[commandPortId] = num + 1;
                return;
            }
            dictionary.Add(commandPortId, 1);
        }

        // Token: 0x06000D41 RID: 3393 RVA: 0x00043078 File Offset: 0x00041278
        public Dictionary<int, int> FetchCommandPortsOnModule(AgentId agentId)
        {
            Dictionary<int, int> result;
            this._readableAgentModuleQueues.commandPorts.TryGetValue(agentId, out result);
            return result;
        }

        // Token: 0x06000D42 RID: 3394 RVA: 0x0004309C File Offset: 0x0004129C
        public void QueueOutputPortOnModule(AgentId agentId, int outputPortId, Data payload)
        {
            List<KeyValuePair<int, Data>> list;
            if (!this._writeableAgentModuleQueues.outputPorts.TryGetValue(agentId, out list))
            {
                list = new List<KeyValuePair<int, Data>>();
                this._writeableAgentModuleQueues.outputPorts.Add(agentId, list);
            }
            list.Add(new KeyValuePair<int, Data>(outputPortId, payload));
        }

        // Token: 0x06000D43 RID: 3395 RVA: 0x000430E4 File Offset: 0x000412E4
        public List<KeyValuePair<int, Data>> FetchOutputPortsOnModule(AgentId agentId)
        {
            List<KeyValuePair<int, Data>> result;
            this._readableAgentModuleQueues.outputPorts.TryGetValue(agentId, out result);
            return result;
        }

        // Token: 0x06000D44 RID: 3396 RVA: 0x00043108 File Offset: 0x00041308
        private void SwapModuleQueues()
        {
            if (this._writeableAgentModuleQueues == this._agentModuleQueues1)
            {
                this._readableAgentModuleQueues = this._agentModuleQueues1;
                this._agentModuleQueues2.Clear();
                this._writeableAgentModuleQueues = this._agentModuleQueues2;
                return;
            }
            this._readableAgentModuleQueues = this._agentModuleQueues2;
            this._agentModuleQueues1.Clear();
            this._writeableAgentModuleQueues = this._agentModuleQueues1;
        }

        // Token: 0x06000D45 RID: 3397 RVA: 0x0004316C File Offset: 0x0004136C
        public ComponentHandler FindComponentByGuid(int theGuid)
        {
            foreach (ComponentHandler componentHandler in this.allComponents)
            {
                if (componentHandler.guid == theGuid)
                {
                    return componentHandler;
                }
            }
            return null;
        }

        // Token: 0x06000D46 RID: 3398 RVA: 0x000431C8 File Offset: 0x000413C8
        public MechanicState GetMechanicStateForSnapshot()
        {
            if (this._state == Device.State.Solid && !this.isMounted)
            {
                this.GenerateMechanicState();
                return this.mechanicState;
            }
            if (this.mechanicState != null)
            {
                this.mechanicState.articulations[0].baseRawPosition = this.rootComponent.wireframePosition;
                this.mechanicState.articulations[0].baseRawOrientation = this.rootComponent.wireframeRotation;
                return this.mechanicState;
            }
            MechanicState mechanicState = new MechanicState();
            mechanicState.CreateDefaults(this.rootComponent.wireframePosition, this.rootComponent.wireframeRotation, this.rootComponent.guid, null);
            return mechanicState;
        }


        // Token: 0x06000D48 RID: 3400 RVA: 0x00043364 File Offset: 0x00041564
        public void ResetDevice(Vector3 resetPosition, bool updateWireframeVFX, bool forcePositionReset = false)
        {
            
        }

        // Token: 0x06000D49 RID: 3401 RVA: 0x000435D8 File Offset: 0x000417D8
        private void CopyConfiguredToRuntimeProperties()
        {

        }

        // Token: 0x06000D4A RID: 3402 RVA: 0x00043630 File Offset: 0x00041830
        private void OnDestroy()
        {

        }

        // Token: 0x06000D4B RID: 3403 RVA: 0x0004369C File Offset: 0x0004189C
        public void SetComponentsEnabledAfterLoad()
        {

        }

        // Token: 0x06000D4C RID: 3404 RVA: 0x000436F4 File Offset: 0x000418F4
        public void MakeWireframe()
        {
            foreach (ComponentHandler componentHandler in this.allComponentsHierarchy)
            {
                foreach (SubComponentHandler subComponentHandler in componentHandler.allSubComponents)
                {
                    if (subComponentHandler.rigidbody != null)
                    {
                        subComponentHandler.rigidbody.transform.position = subComponentHandler.articulationCollidersGroup.position;
                        subComponentHandler.rigidbody.transform.rotation = subComponentHandler.articulationCollidersGroup.rotation;
                        subComponentHandler.rigidbody.transform.gameObject.SetActive(true);
                    }
                    if (subComponentHandler.articulationBody != null)
                    {
                        subComponentHandler.articulationBody.gameObject.SetActive(false);
                    }
                }
            }
            this.SetSelfCollisionEnabled(true);
        }

        // Token: 0x06000D4D RID: 3405 RVA: 0x00043800 File Offset: 0x00041A00
        private void MakeSolid()
        {
            this._cachedMass = 0f;
            foreach (ComponentHandler componentHandler in this.allComponentsHierarchy)
            {
                foreach (SubComponentHandler subComponentHandler in componentHandler.allSubComponents)
                {
                    if (subComponentHandler.articulationBody != null)
                    {
                        subComponentHandler.articulationBody.transform.position = subComponentHandler.rigidbody.transform.position;
                        subComponentHandler.articulationBody.transform.rotation = subComponentHandler.rigidbody.transform.rotation;
                        subComponentHandler.articulationBody.gameObject.SetActive(true);
                        subComponentHandler.articulationBody.enabled = true;
                        subComponentHandler.articulationBody.sleepThreshold = 0f;
                    }
                    else
                    {
                        subComponentHandler.articulationCollidersGroup.position = subComponentHandler.rigidbodyCollidersGroup.transform.position;
                        subComponentHandler.articulationCollidersGroup.rotation = subComponentHandler.rigidbodyCollidersGroup.transform.rotation;
                    }
                    if (subComponentHandler.rigidbody != null)
                    {
                        subComponentHandler.rigidbody.transform.gameObject.SetActive(false);
                    }
                }
                componentHandler.MakingSolid();
                this._cachedMass += componentHandler.GetMass();
            }
            this.rootComponent.baseSubComponent.articulationBody.immovable = this.kinematicBase;
            this.rootComponent.baseSubComponent.articulationBody.WakeUp();
            this.SetSelfCollisionEnabled(false);
            this.RefreshDeviceComponentModifiers();
            if (this.kinematicBase)
            {
                this._restorePosition = this.rootComponent.baseSubComponent.rigidbody.transform.position;
                this._restoreRotation = this.rootComponent.baseSubComponent.rigidbody.transform.rotation;
                this._restoreKinematicTransforms = true;
            }
        }

        // Token: 0x06000D4E RID: 3406 RVA: 0x00043A2C File Offset: 0x00041C2C
        private void SetWireframeCollidersEnabled(bool value)
        {
            foreach (ComponentHandler componentHandler in this.allComponents)
            {
                componentHandler.SetWireframeCollidersEnabled(value);
            }
        }

        // Token: 0x06000D4F RID: 3407 RVA: 0x00043A80 File Offset: 0x00041C80
        private void SetWireframeCollidersToTrigger(bool value)
        {
            foreach (ComponentHandler componentHandler in this.allComponents)
            {
                componentHandler.SetWireframeCollidersToTrigger(value);
            }
        }

        // Token: 0x06000D50 RID: 3408 RVA: 0x00043AD4 File Offset: 0x00041CD4
        public void SetCollidersEnabled(bool value, bool exemptEntryCollisions = true)
        {
            foreach (ComponentHandler componentHandler in this.allComponents)
            {
                componentHandler.SetPhysicsCollidersEnabled(value, exemptEntryCollisions);
            }
        }

        // Token: 0x06000D51 RID: 3409 RVA: 0x00043B28 File Offset: 0x00041D28
        public void SetMeshCollidersEnabled(bool value)
        {

        }

        // Token: 0x06000D52 RID: 3410 RVA: 0x00043B7C File Offset: 0x00041D7C
        private void SetMeshCollidersLayer(int layer)
        {

        }

        // Token: 0x06000D54 RID: 3412 RVA: 0x00043C15 File Offset: 0x00041E15
        private void GenerateMechanicState()
        {
            //this.mechanicState = MechanicState.GenerateMechanicState(this);
        }

        // Token: 0x06000D55 RID: 3413 RVA: 0x00043C24 File Offset: 0x00041E24
        public void ApplyMechanicState(MechanicState theMechanicState, Articulation targetArticulation = null)
        {
            
        }

        // Token: 0x06000D56 RID: 3414 RVA: 0x00043E90 File Offset: 0x00042090
        private Articulation ArticulationForRootComponentWithGuid(int theGuid)
        {
            foreach (Articulation articulation in this._articulations)
            {
                if (articulation.rootComponent.guid == theGuid)
                {
                    return articulation;
                }
            }
            return null;
        }

        // Token: 0x06000D57 RID: 3415 RVA: 0x00043EF4 File Offset: 0x000420F4
        public float GetTotalMass()
        {
            float num = 0f;
            foreach (ComponentHandler componentHandler in this.allComponents)
            {
                num += componentHandler.GetMass();
            }
            return num;
        }

        // Token: 0x06000D58 RID: 3416 RVA: 0x00043F50 File Offset: 0x00042150
        private void SetSelfCollisionEnabled(bool value)
        {
            for (int i = 0; i < this._branches.Count<Device.Branch>(); i++)
            {
                for (int j = i + 1; j < this._branches.Count<Device.Branch>(); j++)
                {
                    foreach (Collider collider in this._branches[i].colliders)
                    {
                        foreach (Collider collider2 in this._branches[j].colliders)
                        {
                            Physics.IgnoreCollision(collider, collider2, !value);
                        }
                    }
                }
            }
        }

        // Token: 0x06000D59 RID: 3417 RVA: 0x00044038 File Offset: 0x00042238
        private void RefreshDeviceComponentModifiers()
        {
            ArticulationBody[] componentsInChildren = this.articulationsContainer.GetComponentsInChildren<ArticulationBody>(true);
            ArticulationBody[] array = componentsInChildren;
            for (int i = 0; i < array.Length; i++)
            {
                DeviceComponentModifier component = array[i].GetComponent<DeviceComponentModifier>();
                if (component != null)
                {
                    UnityEngine.Object.DestroyImmediate(component);
                }
            }
            List<ComponentHandler> list = new List<ComponentHandler>();
            foreach (ComponentHandler componentHandler in this.allComponents)
            {
                if (componentHandler.gestalt.deviceComponentModifier != null)
                {
                    list.Add(componentHandler);
                }
            }
            foreach (ArticulationBody articulationBody in componentsInChildren)
            {
                foreach (ComponentHandler componentHandler2 in list)
                {
                    if (componentHandler2.gestalt.modifierAffectsAllBodies || componentHandler2.ownerArticulationBody == articulationBody)
                    {
                        ((DeviceComponentModifier)articulationBody.gameObject.AddComponent(componentHandler2.gestalt.deviceComponentModifier)).componentHandler = componentHandler2;
                    }
                }
            }
            foreach (ArticulationBody articulationBody2 in componentsInChildren)
            {

            }
        }

        // Token: 0x06000D5A RID: 3418 RVA: 0x000441A8 File Offset: 0x000423A8
        private void CalculateCenterOfGravity()
        {
            
        }

        // Token: 0x06000D5B RID: 3419 RVA: 0x00044444 File Offset: 0x00042644
        public void ImpactSFXCollisionEnter(SubComponentHandler subComponentHandler, Collision collision)
        {
            float magnitude = collision.impulse.magnitude;
            float num = Time.realtimeSinceStartup - this._lastImpactSFXTime;
            if (magnitude < this.impactMinForce || num < this.impactSleepTime)
            {
                return;
            }
            string text = this.impactComponentDefault;
            string text2 = "";
            Vector3 point = collision.GetContact(0).point;
            if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain"))
            {
                text2 = this.impactTerrain;
            }
            float value = Mathf.Clamp01(magnitude / this.impactMaxValue);
            if (!string.IsNullOrEmpty(text))
            {

            }
            if (!string.IsNullOrEmpty(text2))
            {
                Controllers.audioController.Play3DSound(text2, point).setParameterByName("ImpactForce", value, false);
            }
            this._lastImpactSFXTime = Time.realtimeSinceStartup;
        }

        // Token: 0x06000D5C RID: 3420 RVA: 0x0004454C File Offset: 0x0004274C
        public void ToggleSocketEditing()
        {
            this._editingSockets = !this._editingSockets;
            foreach (ComponentHandler componentHandler in this.allComponents)
            {
                componentHandler.SetSocketsEnabled(this._editingSockets);
            }
        }

        // Token: 0x04000A9E RID: 2718
        public GameObject componentDriversContainer;

        // Token: 0x04000A9F RID: 2719
        public GameObject articulationsContainer;

        // Token: 0x04000AA0 RID: 2720
        public GameObject rigidbodiesContainer;

        // Token: 0x04000AA1 RID: 2721
        //[EventRef]
        public string toSolidSound;

        // Token: 0x04000AA2 RID: 2722
        //[EventRef]
        public string toWireframeSound;

        // Token: 0x04000AA3 RID: 2723
        //[EventRef]
        public string impactComponentDefault;

        // Token: 0x04000AA4 RID: 2724
        //[EventRef]
        public string impactTerrain;

        // Token: 0x04000AA5 RID: 2725
        public float impactMinForce;

        // Token: 0x04000AA6 RID: 2726
        public float impactSleepTime;

        // Token: 0x04000AA7 RID: 2727
        public float impactMaxValue;

        // Token: 0x04000AA8 RID: 2728
        public float impactMaxMass;

        // Token: 0x04000AA9 RID: 2729
        private static int maxArticulationChildCount = 64;

        // Token: 0x04000AAA RID: 2730
        private Device.State _state;

        // Token: 0x04000AAB RID: 2731
        private Dictionary<AgentId, Agent> _agents;

        // Token: 0x04000AAC RID: 2732
        private Device.AgentModuleQueues _agentModuleQueues1;

        // Token: 0x04000AAD RID: 2733
        private Device.AgentModuleQueues _agentModuleQueues2;

        // Token: 0x04000AAE RID: 2734
        private Device.AgentModuleQueues _writeableAgentModuleQueues;

        // Token: 0x04000AAF RID: 2735
        private Device.AgentModuleQueues _readableAgentModuleQueues;

        // Token: 0x04000AB0 RID: 2736
        private int _guidIndex;

        // Token: 0x04000AB1 RID: 2737
        private List<Articulation> _articulations;

        // Token: 0x04000AB2 RID: 2738
        private bool _locked;

        // Token: 0x04000AB3 RID: 2739
        private float _lastStateToggleTime;

        // Token: 0x04000AB4 RID: 2740
        private List<float> _dofPositions;

        // Token: 0x04000AB5 RID: 2741
        private List<float> _dofVelocities;

        // Token: 0x04000AB6 RID: 2742
        private List<int> _dofStartIndices;

        // Token: 0x04000AB7 RID: 2743
        private bool _restoreKinematicTransforms;

        // Token: 0x04000AB8 RID: 2744
        private Vector3 _restorePosition;

        // Token: 0x04000AB9 RID: 2745
        private Quaternion _restoreRotation;

        // Token: 0x04000ABA RID: 2746
        private bool _mounted;

        // Token: 0x04000ABB RID: 2747
        private float _lastImpactSFXTime;

        // Token: 0x04000ABC RID: 2748
        private float _cachedMass;

        // Token: 0x04000ABD RID: 2749
        private List<Device.Branch> _branches;

        // Token: 0x04000ABE RID: 2750
        private bool _editingSockets;

        // Token: 0x020003A5 RID: 933
        public enum State
        {
            // Token: 0x04001CB1 RID: 7345
            None,
            // Token: 0x04001CB2 RID: 7346
            Wireframe,
            // Token: 0x04001CB3 RID: 7347
            Solid
        }

        // Token: 0x020003A6 RID: 934
        public class Branch
        {
            // Token: 0x060020AA RID: 8362 RVA: 0x000A0DE0 File Offset: 0x0009EFE0
            public Branch(SubComponentHandler subComponentHandler)
            {
                this.children = new List<SubComponentHandler>();
                this.rootSubComponent = subComponentHandler;
                this.rootSubComponent.branch = this;
                subComponentHandler.GetChildSubComponentsRecursive(this.children);
                this.colliders = new List<Collider>();
                this.colliders.AddRange(this.rootSubComponent.colliders);
                foreach (SubComponentHandler subComponentHandler2 in this.children)
                {
                    this.colliders.AddRange(subComponentHandler2.colliders);
                    subComponentHandler2.branch = this;
                }
            }

            // Token: 0x060020AB RID: 8363 RVA: 0x000A0E98 File Offset: 0x0009F098
            public void SaveLastTransforms()
            {
                this._lastPosition = this.rootSubComponent.transform.position;
                this._lastRotation = this.rootSubComponent.transform.rotation;
            }

            // Token: 0x060020AC RID: 8364 RVA: 0x000A0EC6 File Offset: 0x0009F0C6
            public void ForceRefresh()
            {
                this._force = true;
            }

            // Token: 0x060020AD RID: 8365 RVA: 0x000A0ED0 File Offset: 0x0009F0D0
            public bool HasChanged()
            {
                if (this._force || !this._lastPosition.Equals(this.rootSubComponent.transform.position) || this._lastRotation != this.rootSubComponent.transform.rotation)
                {
                    this._force = false;
                    return true;
                }
                return false;
            }

            // Token: 0x04001CB4 RID: 7348
            public SubComponentHandler rootSubComponent;

            // Token: 0x04001CB5 RID: 7349
            public List<SubComponentHandler> children;

            // Token: 0x04001CB6 RID: 7350
            public List<Collider> colliders;

            // Token: 0x04001CB7 RID: 7351
            private Vector3 _lastPosition;

            // Token: 0x04001CB8 RID: 7352
            private Quaternion _lastRotation;

            // Token: 0x04001CB9 RID: 7353
            private bool _force;
        }

        // Token: 0x020003A7 RID: 935
        [Flags]
        public enum Permissions
        {
            // Token: 0x04001CBB RID: 7355
            ComponentManipulation = 2,
            // Token: 0x04001CBC RID: 7356
            StateToggle = 4,
            // Token: 0x04001CBD RID: 7357
            GrabWireframe = 8,
            // Token: 0x04001CBE RID: 7358
            PropertyEditorAccess = 16,
            // Token: 0x04001CBF RID: 7359
            SketchAccess = 32,
            // Token: 0x04001CC0 RID: 7360
            SketchEdit = 64,
            // Token: 0x04001CC1 RID: 7361
            DeviceDelete = 128,
            // Token: 0x04001CC2 RID: 7362
            AttachDetach = 256,
            // Token: 0x04001CC3 RID: 7363
            GrabSolid = 512,
            // Token: 0x04001CC4 RID: 7364
            Clone = 1024,
            // Token: 0x04001CC5 RID: 7365
            ResetDevice = 2048,
            // Token: 0x04001CC6 RID: 7366
            PaintComponent = 4096,
            // Token: 0x04001CC7 RID: 7367
            All = 8190
        }

        // Token: 0x020003A8 RID: 936
        private class AgentModuleQueues
        {
            // Token: 0x060020AE RID: 8366 RVA: 0x000A0F29 File Offset: 0x0009F129
            public void Clear()
            {
                this.properties.Clear();
                this.commandPorts.Clear();
                this.outputPorts.Clear();
            }

            // Token: 0x04001CC8 RID: 7368
            public Dictionary<AgentId, Dictionary<int, List<Data>>> properties;

            // Token: 0x04001CC9 RID: 7369
            public Dictionary<AgentId, Dictionary<int, int>> commandPorts;

            // Token: 0x04001CCA RID: 7370
            public Dictionary<AgentId, List<KeyValuePair<int, Data>>> outputPorts;
        }

        // Token: 0x020003A9 RID: 937
        // (Invoke) Token: 0x060020B1 RID: 8369
        public delegate void DeviceEvent(int guid);
    }

}
