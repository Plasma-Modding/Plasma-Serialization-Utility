using Rewired;
using Sirenix.OdinInspector;
using UnityEngine;

// Token: 0x02000143 RID: 323
public class ComponentDriver : SerializedMonoBehaviour
{
    // Token: 0x17000094 RID: 148
    // (get) Token: 0x06000BCD RID: 3021 RVA: 0x0003D600 File Offset: 0x0003B800
    // (set) Token: 0x06000BCE RID: 3022 RVA: 0x0003D608 File Offset: 0x0003B808
    public Agent agent
    {
        get
        {
            return this._agent;
        }
        set
        {
            this._agent = value;
            if (value != null)
            {
                this.OnAgentSet();
            }
        }
    }

    // Token: 0x17000095 RID: 149
    // (get) Token: 0x06000BCF RID: 3023 RVA: 0x0003D61A File Offset: 0x0003B81A
    public ComponentHandler component
    {
        get
        {
            return this._component;
        }
    }

    // Token: 0x06000BD1 RID: 3025 RVA: 0x0003D630 File Offset: 0x0003B830
    public virtual void OnWireframeEnter()
    {
    }

    // Token: 0x06000BD2 RID: 3026 RVA: 0x0003D632 File Offset: 0x0003B832
    public virtual void OnSolidEnter()
    {
    }

    // Token: 0x06000BD3 RID: 3027 RVA: 0x0003D634 File Offset: 0x0003B834
    public virtual void OnReset()
    {
    }

    // Token: 0x06000BD4 RID: 3028 RVA: 0x0003D636 File Offset: 0x0003B836
    public virtual void OnResetFinished()
    {
    }

    // Token: 0x06000BD5 RID: 3029 RVA: 0x0003D638 File Offset: 0x0003B838
    protected virtual void OnAgentSet()
    {
    }

    // Token: 0x06000BD6 RID: 3030 RVA: 0x0003D63A File Offset: 0x0003B83A
    public virtual void OnScaleChanged()
    {
    }

    // Token: 0x06000BD7 RID: 3031 RVA: 0x0003D63C File Offset: 0x0003B83C
    public virtual void RunCommand(int commandId)
    {
    }

    // Token: 0x06000BD8 RID: 3032 RVA: 0x0003D63E File Offset: 0x0003B83E
    public virtual void Preprocess()
    {
    }

    // Token: 0x06000BD9 RID: 3033 RVA: 0x0003D640 File Offset: 0x0003B840
    public virtual void ProcessInput(Player input)
    {
    }

    // Token: 0x06000BDA RID: 3034 RVA: 0x0003D642 File Offset: 0x0003B842
    public virtual void UpdateConcreteProperties()
    {
    }

    // Token: 0x06000BDB RID: 3035 RVA: 0x0003D644 File Offset: 0x0003B844
    public virtual void SimulateMountedPhysics()
    {
    }

    // Token: 0x06000BDC RID: 3036 RVA: 0x0003D646 File Offset: 0x0003B846
    public virtual string GetRealValueStringForProperty(int propertyId, float value)
    {
        return null;
    }

    // Token: 0x06000BDD RID: 3037 RVA: 0x0003D649 File Offset: 0x0003B849
    public virtual void OnMount()
    {
    }

    // Token: 0x06000BDE RID: 3038 RVA: 0x0003D64B File Offset: 0x0003B84B
    public virtual void OnUnmount()
    {
    }

    // Token: 0x06000BDF RID: 3039 RVA: 0x0003D64D File Offset: 0x0003B84D
    public virtual void OnProjectileExplosion(Vector3 explosionForce)
    {
    }

    // Token: 0x06000BE0 RID: 3040 RVA: 0x0003D64F File Offset: 0x0003B84F
    public virtual bool CanInteractWithRaycast(Ray ray)
    {
        return false;
    }

    // Token: 0x06000BE1 RID: 3041 RVA: 0x0003D652 File Offset: 0x0003B852
    public virtual void OnInteractionDown(Ray ray)
    {
    }

    // Token: 0x06000BE2 RID: 3042 RVA: 0x0003D654 File Offset: 0x0003B854
    public virtual void OnInteractionHold(Camera theCamera, Vector2 movement)
    {
    }

    // Token: 0x06000BE3 RID: 3043 RVA: 0x0003D656 File Offset: 0x0003B856
    public virtual void OnInteractionUp()
    {
    }

    // Token: 0x06000BE4 RID: 3044 RVA: 0x0003D658 File Offset: 0x0003B858
    public virtual void OnPlasmaInteraction()
    {
    }

    // Token: 0x06000BE5 RID: 3045 RVA: 0x0003D65A File Offset: 0x0003B85A
    public virtual bool CanReactToRaycast(Ray ray)
    {
        return false;
    }

    // Token: 0x06000BE6 RID: 3046 RVA: 0x0003D65D File Offset: 0x0003B85D
    public virtual void OnRaycastInteraction(Ray ray)
    {
    }

    // Token: 0x06000BE7 RID: 3047 RVA: 0x0003D65F File Offset: 0x0003B85F
    public virtual void OnDock()
    {
    }

    // Token: 0x06000BE8 RID: 3048 RVA: 0x0003D661 File Offset: 0x0003B861
    public virtual void OnUndock()
    {
    }

    // Token: 0x06000BE9 RID: 3049 RVA: 0x0003D663 File Offset: 0x0003B863
    public virtual bool CanDock()
    {
        return false;
    }

    // Token: 0x06000BEA RID: 3050 RVA: 0x0003D666 File Offset: 0x0003B866
    public virtual void OnMountStartListening()
    {
    }

    // Token: 0x06000BEB RID: 3051 RVA: 0x0003D668 File Offset: 0x0003B868
    public virtual void OnMountStopListening()
    {
    }

    // Token: 0x06000BEC RID: 3052 RVA: 0x0003D66A File Offset: 0x0003B86A
    public virtual bool CanMount()
    {
        return false;
    }

    // Token: 0x06000BED RID: 3053 RVA: 0x0003D66D File Offset: 0x0003B86D
    public virtual void OnPropertyEditorOpen()
    {
    }

    // Token: 0x06000BEE RID: 3054 RVA: 0x0003D66F File Offset: 0x0003B86F
    public virtual void OnPropertyEditorClose()
    {
    }

    // Token: 0x06000BEF RID: 3055 RVA: 0x0003D671 File Offset: 0x0003B871
    public virtual bool CanFocusOnComponent(Ray ray)
    {
        return false;
    }

    // Token: 0x06000BF0 RID: 3056 RVA: 0x0003D674 File Offset: 0x0003B874
    public virtual void OnFocusDown(Ray ray)
    {
    }

    // Token: 0x06000BF1 RID: 3057 RVA: 0x0003D676 File Offset: 0x0003B876
    public virtual void OnFocusUpdate(Camera theCamera, Player input)
    {
    }

    // Token: 0x06000BF2 RID: 3058 RVA: 0x0003D678 File Offset: 0x0003B878
    public virtual void OnFocusUp()
    {
    }

    // Token: 0x06000BF3 RID: 3059 RVA: 0x0003D67A File Offset: 0x0003B87A
    public virtual void OnSensorTriggerEnter(Collider trigger)
    {
    }

    // Token: 0x06000BF4 RID: 3060 RVA: 0x0003D67C File Offset: 0x0003B87C
    public virtual void OnSensorTriggerStay(Collider trigger)
    {
    }

    // Token: 0x06000BF5 RID: 3061 RVA: 0x0003D67E File Offset: 0x0003B87E
    public virtual void OnSensorTriggerExit(Collider trigger)
    {
    }

    // Token: 0x06000BF6 RID: 3062 RVA: 0x0003D680 File Offset: 0x0003B880
    public virtual void OnSensorCollisionEnter(Collision collision, DeviceComponentModifier modifier)
    {
    }

    // Token: 0x06000BF7 RID: 3063 RVA: 0x0003D682 File Offset: 0x0003B882
    public virtual void OnSensorCollisionStay(Collision collision, DeviceComponentModifier modifier)
    {
    }

    // Token: 0x06000BF8 RID: 3064 RVA: 0x0003D684 File Offset: 0x0003B884
    public virtual void OnSensorCollisionExit(Collision collision, DeviceComponentModifier modifier)
    {
    }

    // Token: 0x06000BF9 RID: 3065 RVA: 0x0003D686 File Offset: 0x0003B886
    public virtual void OnTransparencyVFXEnter()
    {
    }

    // Token: 0x06000BFA RID: 3066 RVA: 0x0003D688 File Offset: 0x0003B888
    public virtual void OnTransparencyVFXExit()
    {
    }

    // Token: 0x06000BFB RID: 3067 RVA: 0x0003D68A File Offset: 0x0003B88A
    public virtual float GetLimit(bool unscaled = false)
    {
        return 0f;
    }

    // Token: 0x04000A6B RID: 2667
    protected ComponentHandler _component;

    // Token: 0x04000A6C RID: 2668
    private Agent _agent;
}
