using Behavior;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
// Token: 0x02000053 RID: 83
public class Agent
{
    // Token: 0x1700005C RID: 92
    // (get) Token: 0x060004DB RID: 1243 RVA: 0x0001E815 File Offset: 0x0001CA15
    // (set) Token: 0x060004DC RID: 1244 RVA: 0x0001E81D File Offset: 0x0001CA1D
    public AgentGestalt gestalt { get; private set; }

    // Token: 0x1700005D RID: 93
    // (get) Token: 0x060004DD RID: 1245 RVA: 0x0001E826 File Offset: 0x0001CA26
    // (set) Token: 0x060004DE RID: 1246 RVA: 0x0001E82E File Offset: 0x0001CA2E
    public AgentId agentId { get; private set; }

    // Token: 0x1700005E RID: 94
    // (get) Token: 0x060004DF RID: 1247 RVA: 0x0001E837 File Offset: 0x0001CA37
    public Dictionary<int, AgentProperty> runtimeProperties
    {
        get
        {
            return this._runtimeProperties;
        }
    }

    // Token: 0x1700005F RID: 95
    // (get) Token: 0x060004E0 RID: 1248 RVA: 0x0001E83F File Offset: 0x0001CA3F
    public Dictionary<int, AgentProperty> configuredProperties
    {
        get
        {
            return this._configuredProperties;
        }
    }

    // Token: 0x17000060 RID: 96
    // (get) Token: 0x060004E1 RID: 1249 RVA: 0x0001E847 File Offset: 0x0001CA47
    public Dictionary<AssetController.ResourceTypes, List<int>> resourceIds
    {
        get
        {
            return this._resourceIds;
        }
    }

    // Token: 0x17000061 RID: 97
    // (get) Token: 0x060004E2 RID: 1250 RVA: 0x0001E84F File Offset: 0x0001CA4F
    // (set) Token: 0x060004E3 RID: 1251 RVA: 0x0001E857 File Offset: 0x0001CA57
    public ComponentDriver driver
    {
        get
        {
            return this._driver;
        }
        set
        {
            this._driver = value;
            this.OnDriverSet();
        }
    }

    // Token: 0x17000062 RID: 98
    // (get) Token: 0x060004E4 RID: 1252 RVA: 0x0001E866 File Offset: 0x0001CA66
    // (set) Token: 0x060004E5 RID: 1253 RVA: 0x0001E86E File Offset: 0x0001CA6E
    public ComponentHandler component { get; set; }

    // Token: 0x17000063 RID: 99
    // (get) Token: 0x060004E6 RID: 1254 RVA: 0x0001E877 File Offset: 0x0001CA77
    // (set) Token: 0x060004E7 RID: 1255 RVA: 0x0001E87F File Offset: 0x0001CA7F
    public int lastSelectedPropertyPosition { get; set; }

    // Token: 0x17000064 RID: 100
    // (get) Token: 0x060004E8 RID: 1256 RVA: 0x0001E888 File Offset: 0x0001CA88
    public Device device
    {
        get
        {
            return this._device;
        }
    }

    // Token: 0x17000065 RID: 101
    // (get) Token: 0x060004E9 RID: 1257 RVA: 0x0001E890 File Offset: 0x0001CA90
    // (set) Token: 0x060004EA RID: 1258 RVA: 0x0001E898 File Offset: 0x0001CA98
    public ProcessorAgent processorAgent { get; set; }

    // Token: 0x17000066 RID: 102
    // (get) Token: 0x060004EB RID: 1259 RVA: 0x0001E8A1 File Offset: 0x0001CAA1
    // (set) Token: 0x060004EC RID: 1260 RVA: 0x0001E8A9 File Offset: 0x0001CAA9
    public SketchNode currentSketchNode { get; private set; }

    // Token: 0x060004EE RID: 1262 RVA: 0x0001EA50 File Offset: 0x0001CC50
    private void SetupModuleInterfaceAndProperties()
    {
        if (this.moduleInterface.isValid)
        {
            foreach (KeyValuePair<int, AgentGestalt.Property> keyValuePair in this.moduleInterface.properties)
            {
                this.gestalt.properties.Add(keyValuePair.Key, keyValuePair.Value);
            }
            foreach (KeyValuePair<int, AgentGestalt.Port> keyValuePair2 in this.moduleInterface.commandPorts)
            {
                this.gestalt.ports.Add(keyValuePair2.Key, keyValuePair2.Value);
            }
            foreach (KeyValuePair<int, AgentGestalt.Port> keyValuePair3 in this.moduleInterface.propertyPorts)
            {
                this.gestalt.ports.Add(keyValuePair3.Key, keyValuePair3.Value);
            }
            foreach (KeyValuePair<int, AgentGestalt.Port> keyValuePair4 in this.moduleInterface.outputPorts)
            {
                this.gestalt.ports.Add(keyValuePair4.Key, keyValuePair4.Value);
            }
        }
        this._runtimeProperties = new Dictionary<int, AgentProperty>();
        this._configuredProperties = new Dictionary<int, AgentProperty>();
        foreach (KeyValuePair<int, AgentGestalt.Property> keyValuePair5 in this.gestalt.properties)
        {
            AgentProperty value = new AgentProperty(keyValuePair5.Value, this, keyValuePair5.Key, true, false);
            this._runtimeProperties.Add(keyValuePair5.Key, value);
            AgentProperty value2 = new AgentProperty(keyValuePair5.Value, this, keyValuePair5.Key, false, false);
            this._configuredProperties.Add(keyValuePair5.Key, value2);
        }
    }

    // Token: 0x060004EF RID: 1263 RVA: 0x0001EC9C File Offset: 0x0001CE9C
    public void UpdatePortsAndProperties()
    {
        if (this.moduleInterface.isValid)
        {
            foreach (KeyValuePair<int, KeyValuePair<int, Agent.ModuleInterface.Actions>> keyValuePair in this.moduleInterface.propertyPortsActions)
            {
                int key = keyValuePair.Value.Key;
                switch (keyValuePair.Value.Value)
                {
                    case Agent.ModuleInterface.Actions.Added:
                        {
                            AgentGestalt.Property property = this.moduleInterface.properties[key];
                            this.gestalt.properties.Add(key, property);
                            AgentProperty value = new AgentProperty(property, this, key, true, false);
                            this._runtimeProperties.Add(key, value);
                            AgentProperty value2 = new AgentProperty(property, this, key, false, false);
                            this._configuredProperties.Add(key, value2);
                            AgentGestalt.Port value3 = this.moduleInterface.propertyPorts[keyValuePair.Key];
                            this.gestalt.ports.Add(keyValuePair.Key, value3);
                            break;
                        }
                    case Agent.ModuleInterface.Actions.Edited:
                        {
                            AgentGestalt.Property property2 = this.moduleInterface.properties[key];
                            this.gestalt.properties[key] = property2;
                            AgentProperty value4 = new AgentProperty(property2, this, key, true, false);
                            this._runtimeProperties[key] = value4;
                            AgentProperty value5 = new AgentProperty(property2, this, key, false, false);
                            this._configuredProperties[key] = value5;
                            AgentGestalt.Port value6 = this.moduleInterface.propertyPorts[keyValuePair.Key];
                            this.gestalt.ports[keyValuePair.Key] = value6;
                            break;
                        }
                    case Agent.ModuleInterface.Actions.Removed:
                        this.gestalt.properties.Remove(key);
                        this._runtimeProperties.Remove(key);
                        this._configuredProperties.Remove(key);
                        this.gestalt.ports.Remove(keyValuePair.Key);
                        break;
                }
            }
            foreach (KeyValuePair<int, KeyValuePair<AgentGestalt.Port.Types, Agent.ModuleInterface.Actions>> keyValuePair2 in this.moduleInterface.portsActions)
            {
                AgentGestalt.Port value7;
                this.GetModuleInterfaceFeature(keyValuePair2.Value.Key).TryGetValue(keyValuePair2.Key, out value7);
                switch (keyValuePair2.Value.Value)
                {
                    case Agent.ModuleInterface.Actions.Added:
                        this.gestalt.ports.Add(keyValuePair2.Key, value7);
                        break;
                    case Agent.ModuleInterface.Actions.Edited:
                        this.gestalt.ports[keyValuePair2.Key] = value7;
                        break;
                    case Agent.ModuleInterface.Actions.Removed:
                        this.gestalt.ports.Remove(keyValuePair2.Key);
                        break;
                }
            }
        }
    }

    // Token: 0x060004F0 RID: 1264 RVA: 0x0001EFA0 File Offset: 0x0001D1A0
    public Dictionary<int, AgentGestalt.Port> GetModuleInterfaceFeature(AgentGestalt.Port.Types type)
    {
        Dictionary<int, AgentGestalt.Port> result;
        switch (type)
        {
            case AgentGestalt.Port.Types.Command:
                result = this.moduleInterface.commandPorts;
                break;
            case AgentGestalt.Port.Types.Property:
                result = this.moduleInterface.propertyPorts;
                break;
            case AgentGestalt.Port.Types.Output:
                result = this.moduleInterface.outputPorts;
                break;
            default:
                result = null;
                break;
        }
        return result;
    }

    // Token: 0x060004F1 RID: 1265 RVA: 0x0001EFEE File Offset: 0x0001D1EE
    public void SetDevice(Device theDevice)
    {
        this._device = theDevice;
    }

    // Token: 0x060004F7 RID: 1271 RVA: 0x0001F258 File Offset: 0x0001D458
    public void RunDriverCommand(int commandId)
    {
        if (this._driver != null)
        {
            this._driver.RunCommand(commandId);
        }
    }

    // Token: 0x060004F8 RID: 1272 RVA: 0x0001F274 File Offset: 0x0001D474
    public virtual FloatRange GetLimitsForProperty(int propertyId)
    {
        return new FloatRange(float.NegativeInfinity, float.PositiveInfinity);
    }

    // Token: 0x060004F9 RID: 1273 RVA: 0x0001F285 File Offset: 0x0001D485
    protected virtual void OnSetupStarted()
    {
    }

    // Token: 0x060004FA RID: 1274 RVA: 0x0001F287 File Offset: 0x0001D487
    protected virtual void OnSetupFinished()
    {
    }

    // Token: 0x060004FB RID: 1275 RVA: 0x0001F289 File Offset: 0x0001D489
    protected virtual void OnCleanUpFinished()
    {
    }

    // Token: 0x060004FC RID: 1276 RVA: 0x0001F28B File Offset: 0x0001D48B
    public virtual void OnComponentSpawned()
    {
    }

    // Token: 0x060004FD RID: 1277 RVA: 0x0001F28D File Offset: 0x0001D48D
    protected virtual void OnDriverSet()
    {
    }

    // Token: 0x060004FE RID: 1278 RVA: 0x0001F28F File Offset: 0x0001D48F
    protected virtual void OnBeforeCopyConfiguredToRuntimeProperties()
    {
    }

    // Token: 0x060004FF RID: 1279 RVA: 0x0001F291 File Offset: 0x0001D491
    public virtual void OnAgentsChangedIdsDuringMove(List<AgentId> agentIds, List<AgentId> newAgentIds)
    {
    }

    // Token: 0x06000500 RID: 1280 RVA: 0x0001F293 File Offset: 0x0001D493
    public virtual void OnDeviceStateChanged(Device.State state)
    {
    }

    // Token: 0x06000501 RID: 1281 RVA: 0x0001F295 File Offset: 0x0001D495
    public virtual void OnDeviceReset()
    {
        if (this._driver != null)
        {
            this._driver.OnReset();
        }
    }

    // Token: 0x06000502 RID: 1282 RVA: 0x0001F2B0 File Offset: 0x0001D4B0
    public virtual void OnDeviceResetFinished()
    {
        if (this._driver != null)
        {
            this._driver.OnResetFinished();
        }
    }

    // Token: 0x06000503 RID: 1283 RVA: 0x0001F2CB File Offset: 0x0001D4CB
    public virtual void OnDeviceLoaded()
    {
    }

    // Token: 0x06000504 RID: 1284 RVA: 0x0001F2CD File Offset: 0x0001D4CD
    public virtual void OnComponentDetachedDuringSimulation(AgentId theAgentId)
    {
    }

    // Token: 0x06000505 RID: 1285 RVA: 0x0001F2CF File Offset: 0x0001D4CF
    public virtual void OnAgentRenamed(AgentId theAgentId)
    {
    }

    // Token: 0x06000506 RID: 1286 RVA: 0x0001F2D1 File Offset: 0x0001D4D1
    public virtual void OnAgentInterfaceChanged(AgentId theAgentId)
    {
    }

    // Token: 0x06000507 RID: 1287 RVA: 0x0001F2D3 File Offset: 0x0001D4D3
    public virtual void OnAgentsChanged()
    {
    }

    // Token: 0x06000508 RID: 1288 RVA: 0x0001F2D5 File Offset: 0x0001D4D5
    public virtual IEnumerable<Agent> GetDependentAgents()
    {
        return null;
    }

    // Token: 0x06000509 RID: 1289 RVA: 0x0001F2D8 File Offset: 0x0001D4D8
    public void PreprocessDriver()
    {
        if (this.driver != null)
        {
            this.driver.Preprocess();
        }
    }

    // Token: 0x0600050A RID: 1290 RVA: 0x0001F2F4 File Offset: 0x0001D4F4
    public void UpdateDriver()
    {
        if (this.driver != null)
        {
            if (this._device.isMounted && this.gestalt.simulatedPhysicsWhenMounted)
            {
                this.driver.SimulateMountedPhysics();
                return;
            }
            this.driver.UpdateConcreteProperties();
        }
    }

    // Token: 0x0600050B RID: 1291 RVA: 0x0001F340 File Offset: 0x0001D540
    public void ProcessModuleInterface()
    {
        if (this.gestalt.processesModuleInterfaces && this.processorAgent != null)
        {
            AgentId agentId = this.processorAgent.agentId;
            //this._changedModuleProperties = this._device.FetchPropertiesOnModule(agentId);
            this._receivedModuleCommands = this._device.FetchCommandPortsOnModule(agentId);
            return;
        }
        if (this.gestalt.canBeModule)
        {
            //this._receivedModuleOutputs = this._device.FetchOutputPortsOnModule(this.agentId);
        }
    }

    // Token: 0x0600050D RID: 1293 RVA: 0x0001F3D3 File Offset: 0x0001D5D3
    public virtual void OnTicksPreprocess()
    {
    }

    // Token: 0x0600050E RID: 1294 RVA: 0x0001F3D5 File Offset: 0x0001D5D5
    public virtual void OnTicksPostprocess()
    {
    }

    // Token: 0x0600050F RID: 1295 RVA: 0x0001F3D7 File Offset: 0x0001D5D7
    public virtual void OnPreWorldTickUpdate()
    {
    }

    // Token: 0x06000510 RID: 1296 RVA: 0x0001F3D9 File Offset: 0x0001D5D9
    public virtual void OnWorldTickUpdate()
    {
    }

    // Token: 0x06000511 RID: 1297 RVA: 0x0001F3DB File Offset: 0x0001D5DB
    public virtual void OnLateWorldTickUpdate(bool firstStep)
    {
    }

    // Token: 0x06000512 RID: 1298 RVA: 0x0001F3DD File Offset: 0x0001D5DD
    public virtual void OnPostWorldTickUpdate()
    {
    }

    // Token: 0x06000513 RID: 1299 RVA: 0x0001F3DF File Offset: 0x0001D5DF
    public virtual string GetStringForPercentageProperty(int propertyId, float percentage)
    {
        if (this.driver != null)
        {
            return this.driver.GetRealValueStringForProperty(propertyId, percentage / 100f);
        }
        return null;
    }

    // Token: 0x06000514 RID: 1300 RVA: 0x0001F404 File Offset: 0x0001D604
    public void OnProjectileExplosion(Vector3 explosionForce)
    {
        if (this.driver != null)
        {
            this.driver.OnProjectileExplosion(explosionForce);
        }
    }

    // Token: 0x06000515 RID: 1301 RVA: 0x0001F420 File Offset: 0x0001D620
    public virtual void AllocResources()
    {
    }

    // Token: 0x06000516 RID: 1302 RVA: 0x0001F422 File Offset: 0x0001D622
    public virtual void DeallocResources()
    {
    }

    // Token: 0x06000517 RID: 1303 RVA: 0x0001F424 File Offset: 0x0001D624
    public virtual List<KeyValuePair<int, int>> GetSerializableTexturesLabels()
    {
        return null;
    }

    // Token: 0x06000518 RID: 1304 RVA: 0x0001F427 File Offset: 0x0001D627
    public virtual int GetTextureIndexForLabel(int label)
    {
        return -1;
    }

    // Token: 0x06000519 RID: 1305 RVA: 0x0001F42A File Offset: 0x0001D62A
    public virtual bool NeedsRemoteOutputPort()
    {
        return false;
    }

    // Token: 0x0600051A RID: 1306 RVA: 0x0001F42D File Offset: 0x0001D62D
    public virtual void HandleRemotePortLogic()
    {
    }

    // Token: 0x0600051B RID: 1307 RVA: 0x0001F42F File Offset: 0x0001D62F
    public virtual void PrepareForSketchFirstTick()
    {
    }

    // Token: 0x0600051C RID: 1308 RVA: 0x0001F431 File Offset: 0x0001D631
    public virtual void PrepareForSketchRetrigger()
    {
    }


    // Token: 0x06000520 RID: 1312 RVA: 0x0001F464 File Offset: 0x0001D664
    public virtual void StartSketchNodeRepeatOperation()
    {
    }

    // Token: 0x06000522 RID: 1314 RVA: 0x0001F468 File Offset: 0x0001D668
    public virtual bool ShouldSketchNodeRepeat()
    {
        return false;
    }

    // Token: 0x06000524 RID: 1316 RVA: 0x0001F478 File Offset: 0x0001D678
    public virtual void ValidateSelectionForProperty(int id)
    {
    }

    // Token: 0x0600052E RID: 1326 RVA: 0x0001F714 File Offset: 0x0001D914
    public virtual void PrepareForComposeRuntimeStorage()
    {
    }

    // Token: 0x04000478 RID: 1144
    public Agent.ModuleInterface moduleInterface;

    // Token: 0x04000479 RID: 1145
    protected Dictionary<int, AgentProperty> _runtimeProperties;

    // Token: 0x0400047A RID: 1146
    protected Dictionary<int, AgentProperty> _configuredProperties;

    // Token: 0x0400047B RID: 1147
    protected Device _device;

    // Token: 0x0400047C RID: 1148
    protected Dictionary<AssetController.ResourceTypes, List<int>> _resourceIds;

    // Token: 0x0400047D RID: 1149
    protected Dictionary<int, int> _receivedModuleCommands;

    // Token: 0x0400047E RID: 1150
    protected List<KeyValuePair<int, Data>> _receivedModuleOutputs;

    // Token: 0x0400047F RID: 1151
    protected Dictionary<int, List<Data>> _changedModuleProperties;

    // Token: 0x04000480 RID: 1152
    private int _currentlyProcessedPortId;

    // Token: 0x04000481 RID: 1153
    private Dictionary<int, MethodInfo> _operations;

    // Token: 0x04000482 RID: 1154
    private ComponentDriver _driver;

    // Token: 0x02000339 RID: 825
    public class Storage
    {
        // Token: 0x04001B1E RID: 6942
        public Dictionary<int, object> properties;
    }

    // Token: 0x0200033A RID: 826
    public class ModuleInterface
    {
        // Token: 0x170004C5 RID: 1221
        // (get) Token: 0x06001FEB RID: 8171 RVA: 0x0009ECB2 File Offset: 0x0009CEB2
        public bool isValid
        {
            get
            {
                return this.commandPorts.Count > 0 || this.propertyPorts.Count > 0 || this.outputPorts.Count > 0;
            }
        }

        // Token: 0x04001B1F RID: 6943
        public Dictionary<int, AgentGestalt.Port> commandPorts;

        // Token: 0x04001B20 RID: 6944
        public Dictionary<int, AgentGestalt.Port> propertyPorts;

        // Token: 0x04001B21 RID: 6945
        public Dictionary<int, AgentGestalt.Port> outputPorts;

        // Token: 0x04001B22 RID: 6946
        public Dictionary<int, AgentGestalt.Property> properties;

        // Token: 0x04001B23 RID: 6947
        public int nextCommandId;

        // Token: 0x04001B24 RID: 6948
        public int nextPropertyPortId;

        // Token: 0x04001B25 RID: 6949
        public int nextOutputId;

        // Token: 0x04001B26 RID: 6950
        public int nextPropertyId;

        // Token: 0x04001B27 RID: 6951
        [NonSerialized]
        public Dictionary<int, KeyValuePair<AgentGestalt.Port.Types, Agent.ModuleInterface.Actions>> portsActions;

        // Token: 0x04001B28 RID: 6952
        [NonSerialized]
        public Dictionary<int, KeyValuePair<int, Agent.ModuleInterface.Actions>> propertyPortsActions;

        // Token: 0x020004BB RID: 1211
        public enum Actions
        {
            // Token: 0x040020D4 RID: 8404
            Added,
            // Token: 0x040020D5 RID: 8405
            Edited,
            // Token: 0x040020D6 RID: 8406
            Removed
        }
    }
}
