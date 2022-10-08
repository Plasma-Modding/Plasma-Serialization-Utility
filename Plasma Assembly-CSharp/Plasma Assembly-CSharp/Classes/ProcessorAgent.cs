using Behavior;
using System.Collections.Generic;
using UnityEngine;
// Token: 0x020000BD RID: 189
public class ProcessorAgent : Agent
{
    // Token: 0x17000087 RID: 135
    // (get) Token: 0x0600089B RID: 2203 RVA: 0x0002FA43 File Offset: 0x0002DC43
    // (set) Token: 0x0600089C RID: 2204 RVA: 0x0002FA4B File Offset: 0x0002DC4B
    public Sketch sketch { get; private set; }

    // Token: 0x17000088 RID: 136
    // (get) Token: 0x0600089D RID: 2205 RVA: 0x0002FA54 File Offset: 0x0002DC54
    // (set) Token: 0x0600089E RID: 2206 RVA: 0x0002FA5C File Offset: 0x0002DC5C
    public VariablesContainerAgent variablesAgent { get; private set; }

    // Token: 0x17000089 RID: 137
    // (get) Token: 0x0600089F RID: 2207 RVA: 0x0002FA65 File Offset: 0x0002DC65
    // (set) Token: 0x060008A0 RID: 2208 RVA: 0x0002FA6D File Offset: 0x0002DC6D
    public ProcessorAgent.SketchMetaData sketchMetaData { get; private set; }

    // Token: 0x060008A1 RID: 2209 RVA: 0x0002FA78 File Offset: 0x0002DC78
    protected override void OnSetupFinished()
    {
        this.sketch = new Sketch(this);
        this._stateProperty = this._runtimeProperties[1];
        this._internalStateProperty = this._runtimeProperties[9];
        this._componentsProperty = this._runtimeProperties[10];
        this._deviceNameProperty = this._runtimeProperties[11];
        this._nodesProperty = this._runtimeProperties[12];
        this._connectionsProperty = this._runtimeProperties[13];
        this._massProperty = this._runtimeProperties[14];
        this.sketchMetaData = new ProcessorAgent.SketchMetaData();
    }

    // Token: 0x060008AA RID: 2218 RVA: 0x0002FD62 File Offset: 0x0002DF62
    public override void OnComponentDetachedDuringSimulation(AgentId theAgentId)
    {
        if (base.agentId != theAgentId)
        {
            this.sketch.BreakComponent(theAgentId);
        }
    }

    // Token: 0x060008AF RID: 2223 RVA: 0x0002FDEB File Offset: 0x0002DFEB
    public override void OnAgentInterfaceChanged(AgentId theAgentId)
    {
        this.sketch.UpdateNode(theAgentId);
    }

    // Token: 0x060008B0 RID: 2224 RVA: 0x0002FDF9 File Offset: 0x0002DFF9
    public override void StartSketchNodeRepeatOperation()
    {
        this._shouldRun = (this._receivedModuleOutputs != null);
        this._repeatIndex = 0;
    }

    // Token: 0x060008B2 RID: 2226 RVA: 0x0002FE7C File Offset: 0x0002E07C
    public override bool ShouldSketchNodeRepeat()
    {
        return this._shouldRun;
    }

    // Token: 0x040007FC RID: 2044
    private AgentProperty _stateProperty;

    // Token: 0x040007FD RID: 2045
    private AgentProperty _internalStateProperty;

    // Token: 0x040007FE RID: 2046
    private AgentProperty _componentsProperty;

    // Token: 0x040007FF RID: 2047
    private AgentProperty _deviceNameProperty;

    // Token: 0x04000800 RID: 2048
    private AgentProperty _nodesProperty;

    // Token: 0x04000801 RID: 2049
    private AgentProperty _connectionsProperty;

    // Token: 0x04000802 RID: 2050
    private AgentProperty _massProperty;

    // Token: 0x04000803 RID: 2051
    private int _repeatIndex;

    // Token: 0x04000804 RID: 2052
    private bool _shouldRun;

    // Token: 0x0200037B RID: 891
    public class ProcessorStorage : Agent.Storage
    {
        // Token: 0x04001C1F RID: 7199
        public object sketchStorage;

        // Token: 0x04001C20 RID: 7200
        public ProcessorAgent.SketchMetaData sketchMetaData;

        // Token: 0x04001C21 RID: 7201
        public AgentId variablesContainerAgentId;
    }

    // Token: 0x0200037C RID: 892
    public class SketchMetaData
    {
        // Token: 0x06002064 RID: 8292 RVA: 0x000A0523 File Offset: 0x0009E723
        public SketchMetaData()
        {
            this.labels = new List<ProcessorAgent.SketchMetaData.Label>();
            this.nodePriorities = new bool[256];
            this.labelPriorities = new bool[64];
        }

        // Token: 0x06002065 RID: 8293 RVA: 0x000A0554 File Offset: 0x0009E754
        public SketchMetaData(ProcessorAgent.SketchMetaData sketchMetaData)
        {
            this.labels = new List<ProcessorAgent.SketchMetaData.Label>();
            foreach (ProcessorAgent.SketchMetaData.Label label in sketchMetaData.labels)
            {
                this.labels.Add(new ProcessorAgent.SketchMetaData.Label(label));
            }
            this.viewPosition = sketchMetaData.viewPosition;
            this.nodePriorities = (bool[])sketchMetaData.nodePriorities.Clone();
            this.labelPriorities = (bool[])sketchMetaData.labelPriorities.Clone();
            this.showMinimap = sketchMetaData.showMinimap;
        }

        // Token: 0x04001C22 RID: 7202
        public List<ProcessorAgent.SketchMetaData.Label> labels;

        // Token: 0x04001C23 RID: 7203
        public Vector2 viewPosition;

        // Token: 0x04001C24 RID: 7204
        public bool[] nodePriorities;

        // Token: 0x04001C25 RID: 7205
        public bool[] labelPriorities;

        // Token: 0x04001C26 RID: 7206
        public bool showMinimap;

        // Token: 0x020004BD RID: 1213
        public class Label
        {
            // Token: 0x06002437 RID: 9271 RVA: 0x000AB976 File Offset: 0x000A9B76
            public Label()
            {
            }

            // Token: 0x06002438 RID: 9272 RVA: 0x000AB980 File Offset: 0x000A9B80
            public Label(ProcessorAgent.SketchMetaData.Label label)
            {
                this.position = label.position;
                this.size = label.size;
                this.title = label.title;
                this.comment = label.comment;
                this.demoImageAsset = label.demoImageAsset;
                this.index = label.index;
            }

            // Token: 0x040020DB RID: 8411
            public Vector2 position;

            // Token: 0x040020DC RID: 8412
            public Vector2 size;

            // Token: 0x040020DD RID: 8413
            public string title;

            // Token: 0x040020DE RID: 8414
            public string comment;

            // Token: 0x040020DF RID: 8415
            public int demoImageAsset;

            // Token: 0x040020E0 RID: 8416
            public int index;
        }
    }

    // Token: 0x0200037D RID: 893
    public enum Commands
    {
        // Token: 0x04001C28 RID: 7208
        UpdateVisuals = 4
    }

    // Token: 0x0200037E RID: 894
    public enum Channels
    {
        // Token: 0x04001C2A RID: 7210
        One,
        // Token: 0x04001C2B RID: 7211
        Two,
        // Token: 0x04001C2C RID: 7212
        Three
    }

    // Token: 0x0200037F RID: 895
    private enum InternalStates
    {
        // Token: 0x04001C2E RID: 7214
        Normal = 1,
        // Token: 0x04001C2F RID: 7215
        Warning,
        // Token: 0x04001C30 RID: 7216
        Error
    }
}
