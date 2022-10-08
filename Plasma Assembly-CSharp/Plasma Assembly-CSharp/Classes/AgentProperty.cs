using Behavior;
using System.Reflection;



// Token: 0x02000055 RID: 85
public class AgentProperty
{
    // Token: 0x14000016 RID: 22
    // (add) Token: 0x0600053A RID: 1338 RVA: 0x0001F860 File Offset: 0x0001DA60
    // (remove) Token: 0x0600053B RID: 1339 RVA: 0x0001F898 File Offset: 0x0001DA98
    public event AgentProperty.AgentPropertyEvent OnPropertyStateChanged;

    // Token: 0x14000017 RID: 23
    // (add) Token: 0x0600053C RID: 1340 RVA: 0x0001F8D0 File Offset: 0x0001DAD0
    // (remove) Token: 0x0600053D RID: 1341 RVA: 0x0001F908 File Offset: 0x0001DB08
    public event AgentProperty.AgentPropertyEvent OnPropertyValueChanged;

    // Token: 0x17000067 RID: 103
    // (get) Token: 0x0600053E RID: 1342 RVA: 0x0001F93D File Offset: 0x0001DB3D
    public AgentGestalt.Property definition
    {
        get
        {
            return this._definition;
        }
    }

    // Token: 0x17000068 RID: 104
    // (get) Token: 0x0600053F RID: 1343 RVA: 0x0001F945 File Offset: 0x0001DB45
    // (set) Token: 0x06000540 RID: 1344 RVA: 0x0001F94D File Offset: 0x0001DB4D
    public bool enabled
    {
        get
        {
            return this._enabled;
        }
        set
        {
            this._enabled = value;
            if (this.OnPropertyStateChanged != null)
            {
                this.OnPropertyStateChanged(this);
            }
        }
    }

    // Token: 0x17000069 RID: 105
    // (get) Token: 0x06000541 RID: 1345 RVA: 0x0001F96A File Offset: 0x0001DB6A
    public int id
    {
        get
        {
            return this._id;
        }
    }

    // Token: 0x1700006A RID: 106
    // (get) Token: 0x06000542 RID: 1346 RVA: 0x0001F972 File Offset: 0x0001DB72
    // (set) Token: 0x06000543 RID: 1347 RVA: 0x0001F97A File Offset: 0x0001DB7A
    public int ownedAssetIndex { get; set; }

    // Token: 0x1700006B RID: 107
    // (get) Token: 0x06000544 RID: 1348 RVA: 0x0001F983 File Offset: 0x0001DB83
    // (set) Token: 0x06000545 RID: 1349 RVA: 0x0001F98B File Offset: 0x0001DB8B
    public bool dontCopyAsset { get; set; }

    // Token: 0x06000546 RID: 1350 RVA: 0x0001F994 File Offset: 0x0001DB94
    public AgentProperty(AgentGestalt.Property definition, Agent agent, int id, bool isRuntime, bool isVariable = false)
    {
        this._definition = definition;
        this._agent = agent;
        this._id = id;
        this._data = new Data(definition.defaultData);
        this._oldData = new Data();
        this._isRuntime = isRuntime;
        this._isVariable = isVariable;
        this.enabled = true;
        /*
        if (this._isRuntime && this._definition.handler > 0)
        {
            foreach (MethodInfo methodInfo in this._agent.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public))
            {
                AgentPropertyHandlerAttribute agentPropertyHandlerAttribute = Attribute.GetCustomAttribute(methodInfo, typeof(AgentPropertyHandlerAttribute)) as AgentPropertyHandlerAttribute;
                if (agentPropertyHandlerAttribute != null && agentPropertyHandlerAttribute.id == this._definition.handler)
                {
                    this._processor = methodInfo;
                    return;
                }
            }
        }
        */
    }

    // Token: 0x0400048F RID: 1167
    private AgentGestalt.Property _definition;

    // Token: 0x04000490 RID: 1168
    private Agent _agent;

    // Token: 0x04000491 RID: 1169
    private int _id;

    // Token: 0x04000492 RID: 1170
    private MethodInfo _processor;

    // Token: 0x04000493 RID: 1171
    private Data _data;

    // Token: 0x04000494 RID: 1172
    private Data _oldData;

    // Token: 0x04000495 RID: 1173
    private bool _isRuntime;

    // Token: 0x04000496 RID: 1174
    private bool _enabled;

    // Token: 0x04000497 RID: 1175
    private bool _semaphore;

    // Token: 0x04000498 RID: 1176
    private bool _isVariable;

    // Token: 0x0200033B RID: 827
    public class Storage
    {
        // Token: 0x04001B29 RID: 6953
        public string name;

        // Token: 0x04001B2A RID: 6954
        public Data data;
    }

    // Token: 0x0200033C RID: 828
    // (Invoke) Token: 0x06001FEF RID: 8175
    public delegate void AgentPropertyEvent(AgentProperty property);
}
