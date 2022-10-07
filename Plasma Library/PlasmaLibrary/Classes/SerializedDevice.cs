using System.Runtime.Serialization;
using UnityEngine;
// Token: 0x02000180 RID: 384
public class SerializedDevice
{
    // Token: 0x06000E87 RID: 3719 RVA: 0x0004A756 File Offset: 0x00048956
    public SerializedDevice()
    {
        this.version = 2;
    }

    // Token: 0x06000E88 RID: 3720 RVA: 0x0004A768 File Offset: 0x00048968
    [OnDeserialized]
    public void Defaults()
    {
        if (this.version < 1)
        {
            this.devicePermissions = new Dictionary<WorldController.GlobalPermissions, WorldController.GlobalPermissionStates>();
            foreach (object obj in Enum.GetValues(typeof(Device.Permissions)))
            {
                Device.Permissions permissions = (Device.Permissions)obj;
                int key = (int)permissions;
                if ((this.permissions & permissions) != (Device.Permissions)0)
                {
                    this.devicePermissions[(WorldController.GlobalPermissions)key] = WorldController.GlobalPermissionStates.True;
                }
                else
                {
                    this.devicePermissions[(WorldController.GlobalPermissions)key] = WorldController.GlobalPermissionStates.False;
                }
            }
            this.devicePermissions[WorldController.GlobalPermissions.All] = WorldController.GlobalPermissionStates.Global;
        }
    }

    // Token: 0x04000BF5 RID: 3061
    public int guid;

    // Token: 0x04000BF6 RID: 3062
    public string displayName;

    // Token: 0x04000BF7 RID: 3063
    public Device.State state;

    // Token: 0x04000BF8 RID: 3064
    public Quaternion wireframeRootComponentRotation;

    // Token: 0x04000BF9 RID: 3065
    public float wireframeDistanceFromTerrain;

    // Token: 0x04000BFA RID: 3066
    public bool kinematicBase;

    // Token: 0x04000BFB RID: 3067
    public float rotationOffset;

    // Token: 0x04000BFC RID: 3068
    public int primaryColorId;

    // Token: 0x04000BFD RID: 3069
    public int secondaryColorId;

    // Token: 0x04000BFE RID: 3070
    public bool showTreeLine;

    // Token: 0x04000BFF RID: 3071
    public int version;

    // Token: 0x04000C00 RID: 3072
    public SerializedAgent[] agents;

    // Token: 0x04000C01 RID: 3073
    public SerializedComponent rootComponent;

    // Token: 0x04000C02 RID: 3074
    public SerializedComponent[] components;

    // Token: 0x04000C03 RID: 3075
    public MechanicState mechanicState;

    // Token: 0x04000C04 RID: 3076
    public Device.Permissions permissions;

    // Token: 0x04000C05 RID: 3077
    public Dictionary<WorldController.GlobalPermissions, WorldController.GlobalPermissionStates> devicePermissions;

    // Token: 0x04000C06 RID: 3078
    public Device.State stageState;

    // Token: 0x04000C07 RID: 3079
    public bool stageReset;

    // Token: 0x04000C08 RID: 3080
    [NonSerialized]
    private const int currentVersion = 2;
}
