using UnityEngine;
// Token: 0x02000147 RID: 327
public class DeviceComponentModifier : MonoBehaviour
{
    // Token: 0x170000FC RID: 252
    // (get) Token: 0x06000D63 RID: 3427 RVA: 0x000446A3 File Offset: 0x000428A3
    // (set) Token: 0x06000D64 RID: 3428 RVA: 0x000446AB File Offset: 0x000428AB
    public ComponentHandler componentHandler
    {
        get
        {
            return this._componentHandler;
        }
        set
        {
            this._componentHandler = value;
            this.OnComponentSet();
        }
    }

    // Token: 0x06000D65 RID: 3429 RVA: 0x000446BA File Offset: 0x000428BA
    public virtual void OnComponentSet()
    {
    }

    // Token: 0x04000ADC RID: 2780
    private ComponentHandler _componentHandler;
}
