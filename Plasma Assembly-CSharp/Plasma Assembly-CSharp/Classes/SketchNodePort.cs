// Token: 0x02000220 RID: 544
using Behavior;
using System.Collections.Generic;

public class SketchNodePort
{
    // Token: 0x1400001E RID: 30
    // (add) Token: 0x0600131F RID: 4895 RVA: 0x00062790 File Offset: 0x00060990
    // (remove) Token: 0x06001320 RID: 4896 RVA: 0x000627C4 File Offset: 0x000609C4
    public static event SketchNodePort.SketchNodePortDebugEvent OnPush;

    // Token: 0x1700020C RID: 524
    // (get) Token: 0x06001321 RID: 4897 RVA: 0x000627F7 File Offset: 0x000609F7
    public AgentGestalt.Port definition
    {
        get
        {
            return this._definition;
        }
    }

    // Token: 0x1700020D RID: 525
    // (get) Token: 0x06001322 RID: 4898 RVA: 0x000627FF File Offset: 0x000609FF
    public SketchNode node
    {
        get
        {
            return this._node;
        }
    }

    // Token: 0x1700020E RID: 526
    // (get) Token: 0x06001323 RID: 4899 RVA: 0x00062807 File Offset: 0x00060A07
    public bool isConnected
    {
        get
        {
            return this._connections.Count > 0;
        }
    }

    // Token: 0x1700020F RID: 527
    // (get) Token: 0x06001324 RID: 4900 RVA: 0x00062817 File Offset: 0x00060A17
    public int id { get; }

    // Token: 0x06001325 RID: 4901 RVA: 0x00062820 File Offset: 0x00060A20
    public SketchNodePort(KeyValuePair<int, AgentGestalt.Port> definition, SketchNode node)
    {
        this._definition = definition.Value;
        this._node = node;
        this._connections = new List<SketchConnection>();
        this._triggeredPorts = new List<SketchNodePort>();
        this._pendingSignals = new Dictionary<SketchNodePort, SketchNodePort.Signal>();
        this.id = definition.Key;
    }

    // Token: 0x06001326 RID: 4902 RVA: 0x00062875 File Offset: 0x00060A75
    public void AddConnection(SketchConnection connection)
    {
        this._connections.Add(connection);
    }

    // Token: 0x06001327 RID: 4903 RVA: 0x00062883 File Offset: 0x00060A83
    public void RemoveConnection(SketchConnection connection)
    {
        this._connections.Remove(connection);
    }

    // Token: 0x06001328 RID: 4904 RVA: 0x00062894 File Offset: 0x00060A94
    public bool IsConnectedTo(SketchNodePort port)
    {
        foreach (SketchConnection sketchConnection in this._connections)
        {
            if (((this._definition.type == AgentGestalt.Port.Types.Command || this._definition.type == AgentGestalt.Port.Types.Property) && sketchConnection.output == port) || (this._definition.type == AgentGestalt.Port.Types.Output && sketchConnection.input == port))
            {
                return true;
            }
        }
        return false;
    }

    // Token: 0x06001329 RID: 4905 RVA: 0x00062924 File Offset: 0x00060B24
    public void Clear()
    {
        this._acceptedSignal = null;
        this._pendingSignals.Clear();
    }

    // Token: 0x0600132B RID: 4907 RVA: 0x00062A6C File Offset: 0x00060C6C
    public void RemoteCommit(Data payload, SketchNodePort inputPort)
    {
        if (payload == null)
        {
            payload = new Data();
        }
        SketchNodePort.Signal signal = new SketchNodePort.Signal();
        signal.payload = new Data(payload);
        this._pendingSignals.Add(inputPort, signal);
    }

    // Token: 0x0600132C RID: 4908 RVA: 0x00062AA4 File Offset: 0x00060CA4
    public IEnumerable<SketchNodePort> Push()
    {
        this._triggeredPorts.Clear();
        foreach (KeyValuePair<SketchNodePort, SketchNodePort.Signal> keyValuePair in this._pendingSignals)
        {
            SketchNodePort key = keyValuePair.Key;
            key.Accept(keyValuePair.Value);
            if (SketchNodePort.OnPush != null)
            {
                SketchNodePort.OnPush(this._node.device.guid, this, key, keyValuePair.Value.payload);
            }
            this._triggeredPorts.Add(key);
        }
        this._pendingSignals.Clear();
        return this._triggeredPorts;
    }

    // Token: 0x0600132D RID: 4909 RVA: 0x00062B5C File Offset: 0x00060D5C
    private void Accept(SketchNodePort.Signal signal)
    {
        this._acceptedSignal = new SketchNodePort.Signal(signal);
    }

    // Token: 0x0600132F RID: 4911 RVA: 0x00062DB5 File Offset: 0x00060FB5
    public Data Peek()
    {
        if (this._definition.type == AgentGestalt.Port.Types.Property && this._acceptedSignal != null)
        {
            return this._acceptedSignal.payload;
        }
        return new Data();
    }

    // Token: 0x04000FA4 RID: 4004
    private AgentGestalt.Port _definition;

    // Token: 0x04000FA5 RID: 4005
    private SketchNode _node;

    // Token: 0x04000FA6 RID: 4006
    private List<SketchConnection> _connections;

    // Token: 0x04000FA7 RID: 4007
    private SketchNodePort.Signal _acceptedSignal;

    // Token: 0x04000FA8 RID: 4008
    private List<SketchNodePort> _triggeredPorts;

    // Token: 0x04000FA9 RID: 4009
    private Dictionary<SketchNodePort, SketchNodePort.Signal> _pendingSignals;

    // Token: 0x04000FAA RID: 4010
    private static Data _empty = new Data();

    // Token: 0x02000432 RID: 1074
    public class Signal
    {
        // Token: 0x06002256 RID: 8790 RVA: 0x000A4E0D File Offset: 0x000A300D
        public Signal()
        {
        }

        // Token: 0x06002257 RID: 8791 RVA: 0x000A4E15 File Offset: 0x000A3015
        public Signal(SketchNodePort.Signal signal)
        {
            this.payload = signal.payload;
        }

        // Token: 0x04001E87 RID: 7815
        public Data payload;
    }

    // Token: 0x02000433 RID: 1075
    // (Invoke) Token: 0x06002259 RID: 8793
    public delegate void SketchNodePortDebugEvent(int deviceGuid, SketchNodePort fromPort, SketchNodePort toPort, Data data);
}
