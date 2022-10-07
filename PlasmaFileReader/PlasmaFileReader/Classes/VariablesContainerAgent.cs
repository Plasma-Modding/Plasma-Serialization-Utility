using System;
using System.Collections.Generic;

namespace PlasmaFileReader.Plasma.Classes
{
    // Token: 0x02000139 RID: 313
    public class VariablesContainerAgent : Agent
    {
        // Token: 0x06000B94 RID: 2964 RVA: 0x0003C669 File Offset: 0x0003A869
        protected override void OnSetupFinished()
        {
            this.configuredVariables = new Dictionary<string, AgentProperty>();
            this.runtimeVariables = new Dictionary<string, AgentProperty>();
        }


        // Token: 0x06000B97 RID: 2967 RVA: 0x0003C760 File Offset: 0x0003A960
        public bool RenameVariable(string name, string newName)
        {
            AgentProperty agentProperty;
            if (this.configuredVariables.TryGetValue(name, out agentProperty))
            {
                agentProperty.definition.name = newName;
                this.configuredVariables.Remove(name);
                this.configuredVariables.Add(newName, agentProperty);
                AgentProperty agentProperty2 = this.runtimeVariables[name];
                agentProperty2.definition.name = newName;
                this.runtimeVariables.Remove(name);
                this.runtimeVariables.Add(newName, agentProperty2);
                return true;
            }
            return false;
        }

        // Token: 0x04000A54 RID: 2644
        public Dictionary<string, AgentProperty> configuredVariables;

        // Token: 0x04000A55 RID: 2645
        public Dictionary<string, AgentProperty> runtimeVariables;
    }

}
