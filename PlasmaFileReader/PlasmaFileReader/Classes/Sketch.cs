using System;
using System.Collections.Generic;
using System.Linq;

namespace PlasmaFileReader.Plasma.Classes
{


    // Token: 0x0200021D RID: 541
    public class Sketch
    {
        // Token: 0x14000019 RID: 25
        // (add) Token: 0x0600129D RID: 4765 RVA: 0x0005FB08 File Offset: 0x0005DD08
        // (remove) Token: 0x0600129E RID: 4766 RVA: 0x0005FB40 File Offset: 0x0005DD40
        public event Sketch.SketchNotificationEvent OnNotification;

        // Token: 0x1400001A RID: 26
        // (add) Token: 0x0600129F RID: 4767 RVA: 0x0005FB78 File Offset: 0x0005DD78
        // (remove) Token: 0x060012A0 RID: 4768 RVA: 0x0005FBB0 File Offset: 0x0005DDB0
        public event Sketch.SketchVariableEvent OnSketchNodeVariableUsageChanged;

        // Token: 0x1400001B RID: 27
        // (add) Token: 0x060012A1 RID: 4769 RVA: 0x0005FBE8 File Offset: 0x0005DDE8
        // (remove) Token: 0x060012A2 RID: 4770 RVA: 0x0005FC20 File Offset: 0x0005DE20
        public event Sketch.SketchVariableEvent OnSketchNodeVariableChanged;

        // Token: 0x170001F4 RID: 500
        // (get) Token: 0x060012A3 RID: 4771 RVA: 0x0005FC55 File Offset: 0x0005DE55
        public List<SketchNode> nodes
        {
            get
            {
                return this._nodes;
            }
        }

        // Token: 0x170001F5 RID: 501
        // (get) Token: 0x060012A4 RID: 4772 RVA: 0x0005FC5D File Offset: 0x0005DE5D
        public HashSet<SketchConnection> connections
        {
            get
            {
                return this._connections;
            }
        }

        // Token: 0x170001F6 RID: 502
        // (get) Token: 0x060012A5 RID: 4773 RVA: 0x0005FC65 File Offset: 0x0005DE65
        // (set) Token: 0x060012A6 RID: 4774 RVA: 0x0005FC6D File Offset: 0x0005DE6D
        public bool canValidate { get; private set; }

        // Token: 0x170001F7 RID: 503
        // (get) Token: 0x060012A7 RID: 4775 RVA: 0x0005FC76 File Offset: 0x0005DE76
        public IEnumerable<AgentProperty> allVariables
        {
            get
            {
                return this._processorAgent.variablesAgent.runtimeVariables.Values;
            }
        }

        // Token: 0x170001F8 RID: 504
        // (get) Token: 0x060012A8 RID: 4776 RVA: 0x0005FC8D File Offset: 0x0005DE8D
        public bool hasVariables
        {
            get
            {
                return this._processorAgent.variablesAgent.configuredVariables.Count > 0;
            }
        }

        // Token: 0x170001F9 RID: 505
        // (get) Token: 0x060012A9 RID: 4777 RVA: 0x0005FCA7 File Offset: 0x0005DEA7
        public ProcessorAgent processorAgent
        {
            get
            {
                return this._processorAgent;
            }
        }

        // Token: 0x060012AA RID: 4778 RVA: 0x0005FCB0 File Offset: 0x0005DEB0
        public Sketch(ProcessorAgent processorAgent)
        {
            this._nodes = new List<SketchNode>();
            this._connections = new HashSet<SketchConnection>();
            this._alwaysRunNodes = new List<SketchNode>();
            this._singleAlwaysRunNode = new List<SketchNode>();
            this._singleAlwaysRunNode.Add(new SketchNode());
            this._mappedVariables = new Dictionary<string, List<KeyValuePair<SketchNode, int>>>();
            this._reversedMappedVariables = new Dictionary<KeyValuePair<SketchNode, int>, string>();
            this._processorAgent = processorAgent;
            this._notifications = new Dictionary<SketchNotifications.Levels, List<SketchNotifications.Notification>>();
            this._currentVariableId = 1;
            this.canValidate = true;
        }

        // Token: 0x060012AC RID: 4780 RVA: 0x0005FDAC File Offset: 0x0005DFAC
        public bool DoesVariableExist(string name)
        {
            name = name.ToLowerInvariant();
            return this._processorAgent.variablesAgent.configuredVariables.ContainsKey(name);
        }

        // Token: 0x060012AD RID: 4781 RVA: 0x0005FDCC File Offset: 0x0005DFCC
        public AgentProperty GetConfiguredVariable(string name)
        {
            name = name.ToLowerInvariant();
            AgentProperty result;
            if (this._processorAgent.variablesAgent.configuredVariables.TryGetValue(name, out result))
            {
                return result;
            }
            //this.LogError("Couldn't find variable with name: " + name);
            return null;
        }

        // Token: 0x060012AE RID: 4782 RVA: 0x0005FE10 File Offset: 0x0005E010
        public AgentProperty GetRuntimeVariable(string name)
        {
            name = name.ToLowerInvariant();
            AgentProperty result;
            if (this._processorAgent.variablesAgent.runtimeVariables.TryGetValue(name, out result))
            {
                return result;
            }
            return null;
        }

        // Token: 0x060012B0 RID: 4784 RVA: 0x0005FF40 File Offset: 0x0005E140
        private void NotifyProperty(SketchNode node, int propertyId)
        {
            if (this.OnSketchNodeVariableChanged != null)
            {
                this.OnSketchNodeVariableChanged(node, propertyId);
            }
        }

        // Token: 0x060012B4 RID: 4788 RVA: 0x00060100 File Offset: 0x0005E300
        public void UnmapVariable(string name, SketchNode node, int propertyId)
        {
            name = name.ToLowerInvariant();
            List<KeyValuePair<SketchNode, int>> list;
            if (this._mappedVariables.TryGetValue(name, out list))
            {
                KeyValuePair<SketchNode, int> keyValuePair = new KeyValuePair<SketchNode, int>(node, propertyId);
                list.Remove(keyValuePair);
                if (list.Count == 0)
                {
                    this._mappedVariables.Remove(name);
                }
                if (this._reversedMappedVariables.ContainsKey(keyValuePair))
                {
                    this._reversedMappedVariables.Remove(keyValuePair);
                    if (this.OnSketchNodeVariableUsageChanged != null)
                    {
                        this.OnSketchNodeVariableUsageChanged(node, propertyId);
                    }
                }
            }
        }

        // Token: 0x060012B5 RID: 4789 RVA: 0x0006017B File Offset: 0x0005E37B
        public bool DoesPropertyUseVariable(SketchNode node, int propertyId)
        {
            return this._reversedMappedVariables.ContainsKey(new KeyValuePair<SketchNode, int>(node, propertyId));
        }

        // Token: 0x060012B6 RID: 4790 RVA: 0x00060190 File Offset: 0x0005E390
        public string GetMappedVariableForProperty(SketchNode node, int propertyId)
        {
            string result;
            this._reversedMappedVariables.TryGetValue(new KeyValuePair<SketchNode, int>(node, propertyId), out result);
            return result;
        }

        // Token: 0x060012B7 RID: 4791 RVA: 0x000601B4 File Offset: 0x0005E3B4
        public int GetUsagesForVariable(string name)
        {
            name = name.ToLowerInvariant();
            List<KeyValuePair<SketchNode, int>> list;
            if (this._mappedVariables.TryGetValue(name, out list))
            {
                return list.Count;
            }
            return 0;
        }


        // Token: 0x060012BE RID: 4798 RVA: 0x0006070C File Offset: 0x0005E90C
        public void HandleOnAgentsChangedIdsDuringMove(List<AgentId> oldAgentIds, List<AgentId> newAgentIds)
        {
            foreach (SketchNode sketchNode in this._nodes)
            {
                for (int i = 0; i < oldAgentIds.Count; i++)
                {
                    if (sketchNode.agentId == oldAgentIds[i])
                    {
                        sketchNode.agentId = newAgentIds[i];
                        break;
                    }
                }
            }
        }

        // Token: 0x060012C2 RID: 4802 RVA: 0x000609C4 File Offset: 0x0005EBC4
        public SketchNode GetNodeById(AgentId nodeId, bool includeUnmapped = true)
        {
            foreach (SketchNode sketchNode in this._nodes)
            {
                if (sketchNode.agentId == nodeId && (sketchNode.isMapped || includeUnmapped))
                {
                    return sketchNode;
                }
            }
            return null;
        }

        // Token: 0x060012C4 RID: 4804 RVA: 0x00060B5C File Offset: 0x0005ED5C
        public void UpdateNode(AgentId agentId)
        {
            SketchNode nodeById = this.GetNodeById(agentId, true);
            if (nodeById != null)
            {
                nodeById.UpdateModuleInterfacePorts();
            }
        }

        // Token: 0x060012CA RID: 4810 RVA: 0x00060D4C File Offset: 0x0005EF4C
        public bool DoesConnectionExist(SketchNodePort output, SketchNodePort input)
        {
            foreach (SketchConnection sketchConnection in this._connections)
            {
                if (sketchConnection.output == output && sketchConnection.input == input)
                {
                    return true;
                }
            }
            return false;
        }

        // Token: 0x060012CC RID: 4812 RVA: 0x00060F2C File Offset: 0x0005F12C
        public void Reset()
        {
            foreach (SketchNode sketchNode in this._nodes)
            {
                if (sketchNode.agent != null)
                {
                    sketchNode.ClearPorts();
                    sketchNode.isBroken = false;
                }
            }
        }

        // Token: 0x060012D0 RID: 4816 RVA: 0x000611E4 File Offset: 0x0005F3E4
        public void SendNotification(SketchNotifications.Notification notification)
        {
            List<SketchNotifications.Notification> list;
            if (!this._notifications.TryGetValue(notification.level, out list))
            {
                list = new List<SketchNotifications.Notification>();
                this._notifications.Add(notification.level, list);
            }
            list.Add(notification);
            if (this.OnNotification != null)
            {
                this.OnNotification(notification);
            }
        }

        // Token: 0x060012D1 RID: 4817 RVA: 0x00061239 File Offset: 0x0005F439
        public void ClearNotifications()
        {
            this._notifications.Clear();
        }

        // Token: 0x060012D2 RID: 4818 RVA: 0x00061248 File Offset: 0x0005F448
        public List<SketchNotifications.Notification> GetNotificationsForLevel(SketchNotifications.Levels level)
        {
            List<SketchNotifications.Notification> result;
            this._notifications.TryGetValue(level, out result);
            return result;
        }

        // Token: 0x060012D3 RID: 4819 RVA: 0x00061268 File Offset: 0x0005F468
        public int GetNotificationCountForLevel(SketchNotifications.Levels level)
        {
            List<SketchNotifications.Notification> list;
            if (this._notifications.TryGetValue(level, out list))
            {
                return list.Count;
            }
            return 0;
        }

        // Token: 0x060012D4 RID: 4820 RVA: 0x00061290 File Offset: 0x0005F490
        public object ComposeStorage()
        {
            Sketch.Storage storage = new Sketch.Storage();
            storage.name = this._name;
            storage.nodes = new List<object>();
            for (int i = 0; i < this._nodes.Count; i++)
            {
                object item = this._nodes[i].ComposeStorage();
                storage.nodes.Add(item);
            }
            storage.connections = new List<Sketch.Storage.Connection>();
            using (HashSet<SketchConnection>.Enumerator enumerator = this._connections.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    SketchConnection connection = enumerator.Current;
                    Sketch.Storage.Connection connection2 = new Sketch.Storage.Connection();
                    connection2.outputNodeIndex = this._nodes.IndexOf(connection.output.node);
                    connection2.outputPortId = connection.output.node.ports.FirstOrDefault((KeyValuePair<int, SketchNodePort> x) => x.Value == connection.output).Key;
                    connection2.inputNodeIndex = this._nodes.IndexOf(connection.input.node);
                    connection2.inputPortId = connection.input.node.ports.FirstOrDefault((KeyValuePair<int, SketchNodePort> x) => x.Value == connection.input).Key;
                    connection2.state = (int)connection.state;
                    connection2.metaData = connection.metaData;
                    storage.connections.Add(connection2);
                }
            }
            storage.mappedVariables = new Dictionary<string, List<KeyValuePair<AgentId, int>>>();
            foreach (KeyValuePair<string, List<KeyValuePair<SketchNode, int>>> keyValuePair in this._mappedVariables)
            {
                List<KeyValuePair<AgentId, int>> list;
                if (!storage.mappedVariables.TryGetValue(keyValuePair.Key, out list))
                {
                    list = new List<KeyValuePair<AgentId, int>>();
                    storage.mappedVariables.Add(keyValuePair.Key, list);
                }
                foreach (KeyValuePair<SketchNode, int> keyValuePair2 in keyValuePair.Value)
                {
                    list.Add(new KeyValuePair<AgentId, int>(keyValuePair2.Key.agentId, keyValuePair2.Value));
                }
            }
            storage.currentVariableId = this._currentVariableId;
            return storage;
        }

        // Token: 0x060012D6 RID: 4822 RVA: 0x0006173C File Offset: 0x0005F93C
        public void BreakComponent(AgentId id)
        {
            foreach (SketchNode sketchNode in this._nodes)
            {
                if (sketchNode.agentId == id)
                {
                    sketchNode.isBroken = true;
                }
            }
        }

        // Token: 0x060012D7 RID: 4823 RVA: 0x000617A0 File Offset: 0x0005F9A0
        public void RestoreBrokenComponents()
        {
            foreach (SketchNode sketchNode in this._nodes)
            {
                sketchNode.isBroken = false;
            }
        }

        // Token: 0x04000F7D RID: 3965
        public const string noVar = "---";

        // Token: 0x04000F7E RID: 3966
        private string _name;

        // Token: 0x04000F7F RID: 3967
        private List<SketchNode> _nodes;

        // Token: 0x04000F80 RID: 3968
        private HashSet<SketchConnection> _connections;

        // Token: 0x04000F81 RID: 3969
        private List<SketchNode> _alwaysRunNodes;

        // Token: 0x04000F82 RID: 3970
        private List<SketchNode> _singleAlwaysRunNode;

        // Token: 0x04000F83 RID: 3971
        private Dictionary<string, List<KeyValuePair<SketchNode, int>>> _mappedVariables;

        // Token: 0x04000F84 RID: 3972
        private Dictionary<KeyValuePair<SketchNode, int>, string> _reversedMappedVariables;

        // Token: 0x04000F85 RID: 3973
        private int _processedNodes;

        // Token: 0x04000F86 RID: 3974
        private ProcessorAgent _processorAgent;

        // Token: 0x04000F87 RID: 3975
        private Dictionary<SketchNotifications.Levels, List<SketchNotifications.Notification>> _notifications;

        // Token: 0x04000F88 RID: 3976
        private int _currentVariableId;

        // Token: 0x04000F89 RID: 3977
        private static int _maxProcessedNodes = 65535;

        // Token: 0x04000F8A RID: 3978
        private static string _variablePrefix = "MyVar";

        // Token: 0x02000426 RID: 1062
        public class Storage
        {
            // Token: 0x04001E6E RID: 7790
            public string name;

            // Token: 0x04001E6F RID: 7791
            public List<object> nodes;

            // Token: 0x04001E70 RID: 7792
            public List<Sketch.Storage.Connection> connections;

            // Token: 0x04001E71 RID: 7793
            public Dictionary<string, List<KeyValuePair<AgentId, int>>> mappedVariables;

            // Token: 0x04001E72 RID: 7794
            public int currentVariableId;

            // Token: 0x020004BF RID: 1215
            public class Connection
            {
                // Token: 0x040020E5 RID: 8421
                public int outputNodeIndex;

                // Token: 0x040020E6 RID: 8422
                public int outputPortId;

                // Token: 0x040020E7 RID: 8423
                public int inputNodeIndex;

                // Token: 0x040020E8 RID: 8424
                public int inputPortId;

                // Token: 0x040020E9 RID: 8425
                public int state;

                // Token: 0x040020EA RID: 8426
                public SketchConnection.MetaData metaData;
            }
        }

        // Token: 0x02000427 RID: 1063
        // (Invoke) Token: 0x0600223B RID: 8763
        public delegate void SketchDebugEvent(int deviceGuid, int index);

        // Token: 0x02000428 RID: 1064
        // (Invoke) Token: 0x0600223F RID: 8767
        public delegate void SketchNotificationEvent(SketchNotifications.Notification notification);

        // Token: 0x02000429 RID: 1065
        // (Invoke) Token: 0x06002243 RID: 8771
        public delegate void SketchVariableEvent(SketchNode sketchNode, int propertyId);
    }
}
