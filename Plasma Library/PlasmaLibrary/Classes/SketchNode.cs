using System;
using System.Collections.Generic;
using UnityEngine;

namespace Plasma.Classes
{

    // Token: 0x0200021F RID: 543
    public class SketchNode
    {
        // Token: 0x1400001C RID: 28
        // (add) Token: 0x060012E4 RID: 4836 RVA: 0x00061A08 File Offset: 0x0005FC08
        // (remove) Token: 0x060012E5 RID: 4837 RVA: 0x00061A3C File Offset: 0x0005FC3C
        public static event SketchNode.SketchNodeDebugEvent OnPreProcessInputPorts;

        // Token: 0x1400001D RID: 29
        // (add) Token: 0x060012E6 RID: 4838 RVA: 0x00061A70 File Offset: 0x0005FC70
        // (remove) Token: 0x060012E7 RID: 4839 RVA: 0x00061AA4 File Offset: 0x0005FCA4
        public static event SketchNode.SketchNodeDebugEvent OnPostGenericOperation;

        // Token: 0x170001FE RID: 510
        // (get) Token: 0x060012E8 RID: 4840 RVA: 0x00061AD7 File Offset: 0x0005FCD7
        public AgentGestalt agentGestalt
        {
            get
            {
                return this._agentGestalt;
            }
        }

        // Token: 0x170001FF RID: 511
        // (get) Token: 0x060012E9 RID: 4841 RVA: 0x00061ADF File Offset: 0x0005FCDF
        // (set) Token: 0x060012EA RID: 4842 RVA: 0x00061AE7 File Offset: 0x0005FCE7
        public AgentId agentId { get; set; }

        // Token: 0x17000200 RID: 512
        // (get) Token: 0x060012EB RID: 4843 RVA: 0x00061AF0 File Offset: 0x0005FCF0
        // (set) Token: 0x060012EC RID: 4844 RVA: 0x00061AF8 File Offset: 0x0005FCF8
        public Agent agent { get; private set; }

        // Token: 0x17000201 RID: 513
        // (get) Token: 0x060012ED RID: 4845 RVA: 0x00061B01 File Offset: 0x0005FD01
        public Dictionary<int, SketchNodePort> ports
        {
            get
            {
                return this._ports;
            }
        }

        // Token: 0x17000202 RID: 514
        // (get) Token: 0x060012EE RID: 4846 RVA: 0x00061B09 File Offset: 0x0005FD09
        public string name
        {
            get
            {
                return this.agentId.displayName;
            }
        }

        // Token: 0x17000203 RID: 515
        // (get) Token: 0x060012EF RID: 4847 RVA: 0x00061B16 File Offset: 0x0005FD16
        // (set) Token: 0x060012F0 RID: 4848 RVA: 0x00061B1E File Offset: 0x0005FD1E
        public Sketch sketch { get; private set; }

        // Token: 0x17000204 RID: 516
        // (get) Token: 0x060012F1 RID: 4849 RVA: 0x00061B27 File Offset: 0x0005FD27
        // (set) Token: 0x060012F2 RID: 4850 RVA: 0x00061B2F File Offset: 0x0005FD2F
        public Device device { get; set; }

        // Token: 0x17000205 RID: 517
        // (get) Token: 0x060012F3 RID: 4851 RVA: 0x00061B38 File Offset: 0x0005FD38
        // (set) Token: 0x060012F4 RID: 4852 RVA: 0x00061B40 File Offset: 0x0005FD40
        public bool isValid
        {
            get
            {
                return this._isValid;
            }
            private set
            {
                this._isValid = value;
                SketchNotifications.Notification notification = new SketchNotifications.Notification();
                notification.level = (value ? SketchNotifications.Levels.Normal : SketchNotifications.Levels.Warning);
                notification.type = (value ? SketchNotifications.Types.ValidationPassed : SketchNotifications.Types.ValidationFailed);
                notification.node = this;
                notification.propertyIds = this._validationPropertyIds;
                notification.log = this._validationMessage;
                this.sketch.SendNotification(notification);
            }
        }

        // Token: 0x17000206 RID: 518
        // (get) Token: 0x060012F5 RID: 4853 RVA: 0x00061B9F File Offset: 0x0005FD9F
        public string validationMessage
        {
            get
            {
                return this._validationMessage;
            }
        }

        // Token: 0x17000207 RID: 519
        // (get) Token: 0x060012F6 RID: 4854 RVA: 0x00061BA7 File Offset: 0x0005FDA7
        public List<int> validationPropertyIds
        {
            get
            {
                return this._validationPropertyIds;
            }
        }

        // Token: 0x17000208 RID: 520
        // (get) Token: 0x060012F7 RID: 4855 RVA: 0x00061BAF File Offset: 0x0005FDAF
        // (set) Token: 0x060012F8 RID: 4856 RVA: 0x00061BB7 File Offset: 0x0005FDB7
        public bool isBroken { get; set; }

        // Token: 0x17000209 RID: 521
        // (get) Token: 0x060012F9 RID: 4857 RVA: 0x00061BC0 File Offset: 0x0005FDC0
        // (set) Token: 0x060012FA RID: 4858 RVA: 0x00061BC8 File Offset: 0x0005FDC8
        public bool isMapped { get; private set; }

        // Token: 0x1700020A RID: 522
        // (get) Token: 0x060012FB RID: 4859 RVA: 0x00061BD1 File Offset: 0x0005FDD1
        // (set) Token: 0x060012FC RID: 4860 RVA: 0x00061BD9 File Offset: 0x0005FDD9
        public bool alwaysRunProcessed { get; set; }

        // Token: 0x1700020B RID: 523
        // (get) Token: 0x060012FD RID: 4861 RVA: 0x00061BE2 File Offset: 0x0005FDE2
        // (set) Token: 0x060012FE RID: 4862 RVA: 0x00061BEA File Offset: 0x0005FDEA
        public SketchNode.MetaData metaData { get; set; }

        // Token: 0x06001300 RID: 4864 RVA: 0x00061D88 File Offset: 0x0005FF88
        public void UpdateModuleInterfacePorts()
        {
            if (this.isMapped)
            {
                foreach (KeyValuePair<int, KeyValuePair<int, Agent.ModuleInterface.Actions>> keyValuePair in this.agent.moduleInterface.propertyPortsActions)
                {
                    AgentGestalt.Port port;
                    this.agent.moduleInterface.propertyPorts.TryGetValue(keyValuePair.Key, out port);
                    this.ProcessPortActions(keyValuePair.Key, port, keyValuePair.Value.Value);
                }
                foreach (KeyValuePair<int, KeyValuePair<AgentGestalt.Port.Types, Agent.ModuleInterface.Actions>> keyValuePair2 in this.agent.moduleInterface.portsActions)
                {
                    AgentGestalt.Port port2;
                    this.agent.GetModuleInterfaceFeature(keyValuePair2.Value.Key).TryGetValue(keyValuePair2.Key, out port2);
                    this.ProcessPortActions(keyValuePair2.Key, port2, keyValuePair2.Value.Value);
                }
            }
        }

        // Token: 0x06001301 RID: 4865 RVA: 0x00061EB8 File Offset: 0x000600B8
        private void ProcessPortActions(int portId, AgentGestalt.Port port, Agent.ModuleInterface.Actions action)
        {
            switch (action)
            {
                case Agent.ModuleInterface.Actions.Added:
                    {
                        SketchNodePort value = new SketchNodePort(new KeyValuePair<int, AgentGestalt.Port>(portId, port), this);
                        this._ports.Add(portId, value);
                        return;
                    }
                case Agent.ModuleInterface.Actions.Edited:
                    {
                        //this.sketch.RemoveConnectionsToFromPort(this._ports[portId]);
                        this._ports.Remove(portId);
                        SketchNodePort value2 = new SketchNodePort(new KeyValuePair<int, AgentGestalt.Port>(portId, port), this);
                        this._ports.Add(portId, value2);
                        return;
                    }
                case Agent.ModuleInterface.Actions.Removed:
                    //this.sketch.RemoveConnectionsToFromPort(this._ports[portId]);
                    this._ports.Remove(portId);
                    return;
                default:
                    return;
            }
        }

        // Token: 0x06001302 RID: 4866 RVA: 0x00061F58 File Offset: 0x00060158
        public void RemoveConnectionsFromToModuleInterfacePorts()
        {
            foreach (SketchNodePort sketchNodePort in this._ports.Values)
            {
                if (sketchNodePort.definition.fromModuleInterface)
                {
                    //this.sketch.RemoveConnectionsToFromPort(sketchNodePort);
                }
            }
        }

        // Token: 0x06001308 RID: 4872 RVA: 0x00062149 File Offset: 0x00060349
        public SketchNodePort GetPort(int id)
        {
            if (this._ports.ContainsKey(id))
            {
                return this._ports[id];
            }
            //this.LogError("Called with an invalid id (" + id.ToString() + ")");
            return null;
        }

        // Token: 0x06001309 RID: 4873 RVA: 0x00062184 File Offset: 0x00060384
        public List<SketchNodePort> GetPortsOfType(AgentGestalt.Port.Types type, bool fromModuleInterface)
        {
            List<SketchNodePort> list = new List<SketchNodePort>();
            foreach (KeyValuePair<int, SketchNodePort> keyValuePair in this._ports)
            {
                if (((!fromModuleInterface && !keyValuePair.Value.definition.fromModuleInterface) || (fromModuleInterface && keyValuePair.Value.definition.fromModuleInterface)) && keyValuePair.Value.definition.type == type && (type != AgentGestalt.Port.Types.Output || !keyValuePair.Value.definition.hidePort))
                {
                    list.Add(keyValuePair.Value);
                }
            }
            list.Sort((SketchNodePort x, SketchNodePort y) => x.definition.position.CompareTo(y.definition.position));
            return list;
        }

        // Token: 0x0600130B RID: 4875 RVA: 0x00062294 File Offset: 0x00060494
        public void ApplyOutwardConnection(SketchConnection connection)
        {
            int id = connection.output.id;
            if (!this._connectedOutputPorts.Contains(id))
            {
                this._connectedOutputPorts.Add(id);
            }
        }

        // Token: 0x0600130C RID: 4876 RVA: 0x000622C8 File Offset: 0x000604C8
        public void ApplyInwardConnection(SketchConnection connection)
        {
            int id = connection.input.id;
            if (!this._connectedInputPorts.Contains(id))
            {
                this._connectedInputPorts.Add(id);
            }
        }

        // Token: 0x0600130D RID: 4877 RVA: 0x000622FB File Offset: 0x000604FB
        public void ApplyInwardRemoteConnection(int inputPortId)
        {
            if (!this._connectedInputPorts.Contains(inputPortId))
            {
                this._connectedInputPorts.Add(inputPortId);
            }
        }

        // Token: 0x0600130E RID: 4878 RVA: 0x00062317 File Offset: 0x00060517
        public void ApplyOutwardRemoteConnection()
        {
            if (this._ports.ContainsKey(512))
            {
                this._connectedOutputPorts.Add(512);
            }
        }

        // Token: 0x0600130F RID: 4879 RVA: 0x0006233C File Offset: 0x0006053C
        public void ClearPorts()
        {
            foreach (int key in this._connectedInputPorts)
            {
                this._ports[key].Clear();
            }
        }

        // Token: 0x06001311 RID: 4881 RVA: 0x000624B0 File Offset: 0x000606B0
        public void PrepareForRetrigger()
        {
            this.agent.PrepareForSketchRetrigger();
        }

        // Token: 0x06001314 RID: 4884 RVA: 0x000624F6 File Offset: 0x000606F6
        public void RunModuleInterfaceProcess()
        {
            this.agent.ProcessModuleInterface();
        }

        // Token: 0x06001315 RID: 4885 RVA: 0x00062503 File Offset: 0x00060703
        public void StartRepeatOperation()
        {
            this.agent.StartSketchNodeRepeatOperation();
        }

        // Token: 0x06001317 RID: 4887 RVA: 0x0006251E File Offset: 0x0006071E
        public bool CheckRepeatOperation()
        {
            return this.agent.ShouldSketchNodeRepeat();
        }

        // Token: 0x06001318 RID: 4888 RVA: 0x0006252C File Offset: 0x0006072C
        public IEnumerable<SketchNode> ProcessOutputPorts()
        {
            this._triggeredNodes.Clear();
            foreach (int key in this._connectedOutputPorts)
            {
                foreach (SketchNodePort sketchNodePort in this._ports[key].Push())
                {
                    if (!this._triggeredNodes.Contains(sketchNodePort.node))
                    {
                        this._triggeredNodes.Add(sketchNodePort.node);
                    }
                }
            }
            return this._triggeredNodes;
        }

        // Token: 0x06001319 RID: 4889 RVA: 0x000625F0 File Offset: 0x000607F0
        public object ComposeStorage()
        {
            return new SketchNode.Storage
            {
                agentId = new AgentId(this.agentId),
                agentGestaltId = this._agentGestaltId,
                metaData = this.metaData
            };
        }

        // Token: 0x04000F92 RID: 3986
        private AgentGestalt _agentGestalt;

        // Token: 0x04000F93 RID: 3987
        private AgentGestaltEnum _agentGestaltId;

        // Token: 0x04000F94 RID: 3988
        private Dictionary<int, SketchNodePort> _ports;

        // Token: 0x04000F95 RID: 3989
        private List<int> _connectedOutputPorts;

        // Token: 0x04000F96 RID: 3990
        private List<int> _connectedInputPorts;

        // Token: 0x04000F97 RID: 3991
        private List<SketchNode> _triggeredNodes;

        // Token: 0x04000F98 RID: 3992
        private bool _isValid;

        // Token: 0x04000F99 RID: 3993
        private string _validationMessage;

        // Token: 0x04000F9A RID: 3994
        private List<int> _validationPropertyIds;

        // Token: 0x0200042E RID: 1070
        public class Storage
        {
            // Token: 0x04001E81 RID: 7809
            public AgentId agentId;

            // Token: 0x04001E82 RID: 7810
            public AgentGestaltEnum agentGestaltId;

            // Token: 0x04001E83 RID: 7811
            public SketchNode.MetaData metaData;
        }

        // Token: 0x0200042F RID: 1071
        public class MetaData
        {
            // Token: 0x04001E84 RID: 7812
            public List<SketchNode.MetaData.Node> nodes;

            // Token: 0x020004C0 RID: 1216
            public class Node
            {
                // Token: 0x040020EB RID: 8427
                public Vector2 position;

                // Token: 0x040020EC RID: 8428
                public int id;

                // Token: 0x040020ED RID: 8429
                public bool[] connectionPriorities;
            }
        }

        // Token: 0x02000430 RID: 1072
        // (Invoke) Token: 0x06002250 RID: 8784
        public delegate void SketchNodeDebugEvent(int deviceGuid, SketchNode sketchNode);
    }
}
