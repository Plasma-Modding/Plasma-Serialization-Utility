using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using FMOD.Studio;
using FMODUnity;
using Rewired;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Tayx.Graphy;
using Tayx.Graphy.Utils;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using static Rewired.InputMapper;
using static Rewired.Platforms.Custom.CustomInputSource;

namespace Plasma.Classes
{
    // Token: 0x02000051 RID: 81
    public class WorldController : Controller
    {

        // Token: 0x060002C5 RID: 709 RVA: 0x00014E57 File Offset: 0x00013057
        public bool ShouldStartDeleteComponent()
        {
            return this._operatingComponent.guid >= 0 && this._input.GetButtonDown("DeleteComponent") && this.DoesPlayerHavePermission(this._operatingComponent.device, WorldController.GlobalPermissions.DeviceDelete);
        }

        // Token: 0x060002C6 RID: 710 RVA: 0x00014E91 File Offset: 0x00013091
        private bool ShouldCancelDeleteComponent()
        {
            return this._input.GetButtonUp("DeleteComponent");
        }

        // Token: 0x060003E0 RID: 992 RVA: 0x0001A843 File Offset: 0x00018A43
        private void ClearUndoQueue()
        {
            this._undoQueue.Clear();
        }

        // Token: 0x060003E1 RID: 993 RVA: 0x0001A850 File Offset: 0x00018A50
        public bool ShouldStartUndo()
        {
            return this._input.GetButtonDown("Undo") && this._undoQueue.Count > 0;
        }

        // Token: 0x060003E2 RID: 994 RVA: 0x0001A874 File Offset: 0x00018A74
        private bool ShouldCancelUndo()
        {
            return this._input.GetButtonUp("Undo");
        }

        // Token: 0x14000007 RID: 7
        // (add) Token: 0x060003E4 RID: 996 RVA: 0x0001A894 File Offset: 0x00018A94
        // (remove) Token: 0x060003E5 RID: 997 RVA: 0x0001A8C8 File Offset: 0x00018AC8
        public static event WorldController.ComponentTargetEvent OnComponentTargetUpdate;

        // Token: 0x14000008 RID: 8
        // (add) Token: 0x060003E6 RID: 998 RVA: 0x0001A8FC File Offset: 0x00018AFC
        // (remove) Token: 0x060003E7 RID: 999 RVA: 0x0001A930 File Offset: 0x00018B30
        public static event WorldController.DevicesUpdateEvent OnPreUpdateDevices;

        // Token: 0x14000009 RID: 9
        // (add) Token: 0x060003E8 RID: 1000 RVA: 0x0001A964 File Offset: 0x00018B64
        // (remove) Token: 0x060003E9 RID: 1001 RVA: 0x0001A998 File Offset: 0x00018B98
        public static event WorldController.DevicesUpdateEvent OnPostUpdateDevices;

        // Token: 0x1400000A RID: 10
        // (add) Token: 0x060003EA RID: 1002 RVA: 0x0001A9CC File Offset: 0x00018BCC
        // (remove) Token: 0x060003EB RID: 1003 RVA: 0x0001AA00 File Offset: 0x00018C00
        public static event WorldController.DeviceEvent OnDeviceLoaded;

        // Token: 0x1400000B RID: 11
        // (add) Token: 0x060003EC RID: 1004 RVA: 0x0001AA34 File Offset: 0x00018C34
        // (remove) Token: 0x060003ED RID: 1005 RVA: 0x0001AA68 File Offset: 0x00018C68
        public static event WorldController.DeviceEvent OnDeviceDeleted;

        // Token: 0x1400000C RID: 12
        // (add) Token: 0x060003EE RID: 1006 RVA: 0x0001AA9C File Offset: 0x00018C9C
        // (remove) Token: 0x060003EF RID: 1007 RVA: 0x0001AAD0 File Offset: 0x00018CD0
        public static event WorldController.DeviceEvent OnDeviceGrabbed;

        // Token: 0x1400000D RID: 13
        // (add) Token: 0x060003F0 RID: 1008 RVA: 0x0001AB04 File Offset: 0x00018D04
        // (remove) Token: 0x060003F1 RID: 1009 RVA: 0x0001AB38 File Offset: 0x00018D38
        public static event WorldController.DeviceEvent OnDeviceReleased;

        // Token: 0x1400000E RID: 14
        // (add) Token: 0x060003F2 RID: 1010 RVA: 0x0001AB6C File Offset: 0x00018D6C
        // (remove) Token: 0x060003F3 RID: 1011 RVA: 0x0001ABA0 File Offset: 0x00018DA0
        public static event WorldController.DeviceEvent OnDeviceSpawned;

        // Token: 0x1400000F RID: 15
        // (add) Token: 0x060003F4 RID: 1012 RVA: 0x0001ABD4 File Offset: 0x00018DD4
        // (remove) Token: 0x060003F5 RID: 1013 RVA: 0x0001AC08 File Offset: 0x00018E08
        public static event WorldController.DeviceEvent OnDeviceStateChanged;

        // Token: 0x14000010 RID: 16
        // (add) Token: 0x060003F6 RID: 1014 RVA: 0x0001AC3C File Offset: 0x00018E3C
        // (remove) Token: 0x060003F7 RID: 1015 RVA: 0x0001AC70 File Offset: 0x00018E70
        public static event WorldController.DeviceEvent OnComponentAttached;

        // Token: 0x14000011 RID: 17
        // (add) Token: 0x060003F8 RID: 1016 RVA: 0x0001ACA4 File Offset: 0x00018EA4
        // (remove) Token: 0x060003F9 RID: 1017 RVA: 0x0001ACD8 File Offset: 0x00018ED8
        public static event WorldController.PhysicsEvent OnPhysicsTick;

        // Token: 0x14000012 RID: 18
        // (add) Token: 0x060003FA RID: 1018 RVA: 0x0001AD0C File Offset: 0x00018F0C
        // (remove) Token: 0x060003FB RID: 1019 RVA: 0x0001AD40 File Offset: 0x00018F40
        public static event WorldController.ComponentTransformUpdate OnComponentTransformUpdate;

        // Token: 0x14000013 RID: 19
        // (add) Token: 0x060003FC RID: 1020 RVA: 0x0001AD74 File Offset: 0x00018F74
        // (remove) Token: 0x060003FD RID: 1021 RVA: 0x0001ADA8 File Offset: 0x00018FA8
        public static event WorldController.ProjectileEvent OnProjectileDeleted;

        // Token: 0x14000014 RID: 20
        // (add) Token: 0x060003FE RID: 1022 RVA: 0x0001ADDC File Offset: 0x00018FDC
        // (remove) Token: 0x060003FF RID: 1023 RVA: 0x0001AE10 File Offset: 0x00019010
        public static event WorldController.GameEvent OnGamePaused;

        // Token: 0x14000015 RID: 21
        // (add) Token: 0x06000400 RID: 1024 RVA: 0x0001AE44 File Offset: 0x00019044
        // (remove) Token: 0x06000401 RID: 1025 RVA: 0x0001AE78 File Offset: 0x00019078
        public static event WorldController.GameEvent OnGameResumed;

        // Token: 0x17000033 RID: 51
        // (get) Token: 0x06000402 RID: 1026 RVA: 0x0001AEAB File Offset: 0x000190AB
        // (set) Token: 0x06000403 RID: 1027 RVA: 0x0001AEB3 File Offset: 0x000190B3
        public string exceptionCondition { get; private set; }

        // Token: 0x17000034 RID: 52
        // (get) Token: 0x06000404 RID: 1028 RVA: 0x0001AEBC File Offset: 0x000190BC
        // (set) Token: 0x06000405 RID: 1029 RVA: 0x0001AEC4 File Offset: 0x000190C4
        public string exceptionStackTrace { get; private set; }

        // Token: 0x17000035 RID: 53
        // (get) Token: 0x06000406 RID: 1030 RVA: 0x0001AECD File Offset: 0x000190CD
        public IEnumerable<Device> devices
        {
            get
            {
                return this._devices.Values;
            }
        }

        // Token: 0x17000036 RID: 54
        // (get) Token: 0x06000407 RID: 1031 RVA: 0x0001AEDA File Offset: 0x000190DA
        public int numberOfDevices
        {
            get
            {
                return this._devices.Count;
            }
        }

        // Token: 0x17000037 RID: 55
        // (get) Token: 0x06000408 RID: 1032 RVA: 0x0001AEE7 File Offset: 0x000190E7
        public IEnumerable<Light> lightSources
        {
            get
            {
                return this._lightSources;
            }
        }

        // Token: 0x17000038 RID: 56
        // (get) Token: 0x06000409 RID: 1033 RVA: 0x0001AEEF File Offset: 0x000190EF
        public SubComponentHandler candidateParentSubComponentDuringOperation
        {
            get
            {
                return this._candidateParentSubComponent;
            }
        }

        // Token: 0x17000039 RID: 57
        // (get) Token: 0x0600040A RID: 1034 RVA: 0x0001AEF7 File Offset: 0x000190F7
        public Vector3 playerPosition
        {
            get
            {
                return this._localCharacter.position;
            }
        }

        // Token: 0x1700003A RID: 58
        // (get) Token: 0x0600040B RID: 1035 RVA: 0x0001AF04 File Offset: 0x00019104
        public Vector3 playerOrientation
        {
            get
            {
                return this._localCharacter.orientation;
            }
        }

        // Token: 0x1700003B RID: 59
        // (get) Token: 0x0600040C RID: 1036 RVA: 0x0001AF11 File Offset: 0x00019111
        public Vector3 playerVelocity
        {
            get
            {
                return this._localCharacter.velocity;
            }
        }

        // Token: 0x1700003C RID: 60
        // (get) Token: 0x0600040D RID: 1037 RVA: 0x0001AF1E File Offset: 0x0001911E
        // (set) Token: 0x0600040E RID: 1038 RVA: 0x0001AF26 File Offset: 0x00019126
        public bool shouldPlayerUndock { get; set; }

        // Token: 0x1700003D RID: 61
        // (get) Token: 0x0600040F RID: 1039 RVA: 0x0001AF2F File Offset: 0x0001912F
        public ComponentHandler mountedComponent
        {
            get
            {
                return this._mountedComponent;
            }
        }

        // Token: 0x1700003E RID: 62
        // (get) Token: 0x06000410 RID: 1040 RVA: 0x0001AF37 File Offset: 0x00019137
        // (set) Token: 0x06000411 RID: 1041 RVA: 0x0001AF3F File Offset: 0x0001913F
        public Transform world { get; private set; }

        // Token: 0x1700003F RID: 63
        // (get) Token: 0x06000412 RID: 1042 RVA: 0x0001AF48 File Offset: 0x00019148
        // (set) Token: 0x06000413 RID: 1043 RVA: 0x0001AF50 File Offset: 0x00019150
        public Transform biome { get; private set; }

        // Token: 0x17000040 RID: 64
        // (get) Token: 0x06000414 RID: 1044 RVA: 0x0001AF59 File Offset: 0x00019159
        // (set) Token: 0x06000415 RID: 1045 RVA: 0x0001AF61 File Offset: 0x00019161
        public bool shouldWorldBeSaved { get; set; }

        // Token: 0x17000041 RID: 65
        // (get) Token: 0x06000416 RID: 1046 RVA: 0x0001AF6A File Offset: 0x0001916A
        // (set) Token: 0x06000417 RID: 1047 RVA: 0x0001AF72 File Offset: 0x00019172
        public string lastSavedWorld { get; set; }

        // Token: 0x17000042 RID: 66
        // (get) Token: 0x06000418 RID: 1048 RVA: 0x0001AF7B File Offset: 0x0001917B
        // (set) Token: 0x06000419 RID: 1049 RVA: 0x0001AF83 File Offset: 0x00019183
        public bool isWorldTutorial { get; private set; }

        // Token: 0x17000043 RID: 67
        // (get) Token: 0x0600041A RID: 1050 RVA: 0x0001AF8C File Offset: 0x0001918C
        // (set) Token: 0x0600041B RID: 1051 RVA: 0x0001AF94 File Offset: 0x00019194
        public bool deviceWantsToCreateWorld { get; set; }

        // Token: 0x17000044 RID: 68
        // (get) Token: 0x0600041C RID: 1052 RVA: 0x0001AF9D File Offset: 0x0001919D
        // (set) Token: 0x0600041D RID: 1053 RVA: 0x0001AFA5 File Offset: 0x000191A5
        public BiomeGestaltEnum worldToBeCreated { get; set; }

        // Token: 0x17000045 RID: 69
        // (get) Token: 0x0600041E RID: 1054 RVA: 0x0001AFAE File Offset: 0x000191AE
        // (set) Token: 0x0600041F RID: 1055 RVA: 0x0001AFB8 File Offset: 0x000191B8
        public WorldController.WorldPermissions worldPermissions
        {
            get
            {
                return this._worldPermissions;
            }
            set
            {
                if (this._worldPermissions != value)
                {
                    WorldController.WorldPermissions worldPermissions = this._worldPermissions;
                    this._worldPermissions = value;
                }
            }
        }

        // Token: 0x17000046 RID: 70
        // (get) Token: 0x06000420 RID: 1056 RVA: 0x0001AFF7 File Offset: 0x000191F7
        // (set) Token: 0x06000421 RID: 1057 RVA: 0x0001AFFF File Offset: 0x000191FF
        public WorldController.GlobalPermissions globalPermissions
        {
            get
            {
                return this._globalPermissions;
            }
            set
            {
                this._globalPermissions = value;
            }
        }

        // Token: 0x17000047 RID: 71
        // (get) Token: 0x06000422 RID: 1058 RVA: 0x0001B008 File Offset: 0x00019208
        // (set) Token: 0x06000423 RID: 1059 RVA: 0x0001B010 File Offset: 0x00019210
        public WorldController.Stage stage { get; private set; }

        // Token: 0x17000048 RID: 72
        // (get) Token: 0x06000424 RID: 1060 RVA: 0x0001B019 File Offset: 0x00019219
        // (set) Token: 0x06000425 RID: 1061 RVA: 0x0001B021 File Offset: 0x00019221
        public SerializedWorldMetaData worldMetaData { get; private set; }

        // Token: 0x17000049 RID: 73
        // (get) Token: 0x06000426 RID: 1062 RVA: 0x0001B02A File Offset: 0x0001922A
        // (set) Token: 0x06000427 RID: 1063 RVA: 0x0001B032 File Offset: 0x00019232
        public bool isPlaytestingStage { get; private set; }

        // Token: 0x1700004A RID: 74
        // (get) Token: 0x06000428 RID: 1064 RVA: 0x0001B03B File Offset: 0x0001923B
        // (set) Token: 0x06000429 RID: 1065 RVA: 0x0001B043 File Offset: 0x00019243
        public SerializedWorld sandboxState { get; set; }

        // Token: 0x1700004B RID: 75
        // (get) Token: 0x0600042A RID: 1066 RVA: 0x0001B04C File Offset: 0x0001924C
        // (set) Token: 0x0600042B RID: 1067 RVA: 0x0001B054 File Offset: 0x00019254
        public bool userWorldScreenshotTaken { get; set; }

        // Token: 0x1700004C RID: 76
        // (get) Token: 0x0600042C RID: 1068 RVA: 0x0001B05D File Offset: 0x0001925D
        public DynamicPropsManager dynamicPropsManager
        {
            get
            {
                return this._dynamicPropsManager;
            }
        }

        // Token: 0x1700004D RID: 77
        // (get) Token: 0x0600042D RID: 1069 RVA: 0x0001B065 File Offset: 0x00019265
        public Texture2D deviceScreenshotTexture
        {
            get
            {
                return this._deviceScreenshotTexture;
            }
        }

        // Token: 0x1700004E RID: 78
        // (get) Token: 0x0600042E RID: 1070 RVA: 0x0001B06D File Offset: 0x0001926D
        public Texture2D worldScreenshotTexture
        {
            get
            {
                return this._worldScreenshotTexture;
            }
        }

        // Token: 0x1700004F RID: 79
        // (get) Token: 0x0600042F RID: 1071 RVA: 0x0001B075 File Offset: 0x00019275
        // (set) Token: 0x06000430 RID: 1072 RVA: 0x0001B07D File Offset: 0x0001927D
        public Device dockingStationDevice { get; private set; }

        // Token: 0x17000050 RID: 80
        // (get) Token: 0x06000431 RID: 1073 RVA: 0x0001B086 File Offset: 0x00019286
        // (set) Token: 0x06000432 RID: 1074 RVA: 0x0001B08E File Offset: 0x0001928E
        public BlockingTutorialGestaltEnum blockingTutorialToBeShown { get; set; }

        // Token: 0x17000051 RID: 81
        // (get) Token: 0x06000433 RID: 1075 RVA: 0x0001B097 File Offset: 0x00019297
        // (set) Token: 0x06000434 RID: 1076 RVA: 0x0001B09F File Offset: 0x0001929F
        public WorldController.Windows windowToBeOpened { get; set; }

        // Token: 0x17000052 RID: 82
        // (get) Token: 0x06000435 RID: 1077 RVA: 0x0001B0A8 File Offset: 0x000192A8
        // (set) Token: 0x06000436 RID: 1078 RVA: 0x0001B0B0 File Offset: 0x000192B0
        public Device deviceToSolidify { get; set; }

        // Token: 0x17000053 RID: 83
        // (get) Token: 0x06000437 RID: 1079 RVA: 0x0001B0B9 File Offset: 0x000192B9
        // (set) Token: 0x06000438 RID: 1080 RVA: 0x0001B0C1 File Offset: 0x000192C1
        public bool temporarilyDisableGuruMeditation { get; set; }

        // Token: 0x17000054 RID: 84
        // (get) Token: 0x06000439 RID: 1081 RVA: 0x0001B0CA File Offset: 0x000192CA
        // (set) Token: 0x0600043A RID: 1082 RVA: 0x0001B0D2 File Offset: 0x000192D2
        public int numberOfComponentsLoaded { get; set; }

        // Token: 0x17000055 RID: 85
        // (get) Token: 0x0600043B RID: 1083 RVA: 0x0001B0DB File Offset: 0x000192DB
        // (set) Token: 0x0600043C RID: 1084 RVA: 0x0001B0E3 File Offset: 0x000192E3
        public int paintPrimaryColor { get; set; }

        // Token: 0x17000056 RID: 86
        // (get) Token: 0x0600043D RID: 1085 RVA: 0x0001B0EC File Offset: 0x000192EC
        // (set) Token: 0x0600043E RID: 1086 RVA: 0x0001B0F4 File Offset: 0x000192F4
        public int paintSecondaryColor { get; set; }

        // Token: 0x17000057 RID: 87
        // (get) Token: 0x0600043F RID: 1087 RVA: 0x0001B0FD File Offset: 0x000192FD
        public dynamic blackboard
        {
            get
            {
                return this._stateMachine.blackboard;
            }
        }

        // Token: 0x17000058 RID: 88
        // (get) Token: 0x06000440 RID: 1088 RVA: 0x0001B10A File Offset: 0x0001930A
        public RigidbodyCharacter character
        {
            get
            {
                return this._localCharacter;
            }
        }

        // Token: 0x17000059 RID: 89
        // (get) Token: 0x06000441 RID: 1089 RVA: 0x0001B112 File Offset: 0x00019312
        public dynamic visor
        {
            get
            {
                return this._visor;
            }
        }

        // Token: 0x1700005A RID: 90
        // (get) Token: 0x06000442 RID: 1090 RVA: 0x0001B11A File Offset: 0x0001931A
        public Camera currentCamera
        {
            get
            {
                if (this._localCharacter.cameraIsTaken && this._dummyCamera != null)
                {
                    return this._dummyCamera;
                }
                return this._camera;
            }
        }

        // Token: 0x1700005B RID: 91
        // (get) Token: 0x06000443 RID: 1091 RVA: 0x0001B144 File Offset: 0x00019344
        public Device targetDevice
        {
            get
            {
                return this._targetDevice;
            }
        }

        // Token: 0x06000444 RID: 1092 RVA: 0x0001B14C File Offset: 0x0001934C
        public override void Init()
        {

        }

        // Token: 0x06000445 RID: 1093 RVA: 0x0001B52C File Offset: 0x0001972C
        private void HandleLogMessageReceived(string condition, string stacktrace, LogType type)
        {
            if (!this.temporarilyDisableGuruMeditation && !this._exceptionUnhandled && (type == LogType.Error || type == LogType.Exception) && !condition.StartsWith("<RI") && !condition.StartsWith("Compute shader") && !condition.StartsWith("Failed to save a temporary cursor") && !condition.StartsWith("Infinity or NaN floating point numbers appear") && !condition.StartsWith("Initializing Microsoft Media Foundation failed") && !condition.StartsWith("ERROR - Could not find specified") && !condition.StartsWith("Screen position out of view frustum") && !condition.StartsWith("RenderTexture.Create failed: format") && !condition.StartsWith("Error capturing camera feed. Maybe") && !condition.StartsWith("Rotation quaternions must be unit") && !condition.StartsWith("Could not start graph") && !condition.StartsWith("RenderTexture.Create failed: format unsupported") && !condition.StartsWith("Platform does not support compute") && !condition.StartsWith("Could not connect pins"))
            {
                this._exceptionUnhandled = true;
                this.exceptionCondition = condition;
                this.exceptionStackTrace = stacktrace;
            }
        }

        // Token: 0x06000446 RID: 1094
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        // Token: 0x06000447 RID: 1095
        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hwnd, IntPtr proccess);

        // Token: 0x06000448 RID: 1096
        [DllImport("user32.dll")]
        private static extern IntPtr GetKeyboardLayout(uint thread);

        // Token: 0x06000449 RID: 1097 RVA: 0x0001B634 File Offset: 0x00019834
        private static int GetCurrentKeyboardLayout()
        {
            int result;
            try
            {
                int num = WorldController.GetKeyboardLayout(WorldController.GetWindowThreadProcessId(WorldController.GetForegroundWindow(), IntPtr.Zero)).ToInt32() & 65535;
                if (num == 0)
                {
                    num = 1033;
                }
                result = num;
            }
            catch (Exception)
            {
                result = 1033;
            }
            return result;
        }

        // Token: 0x0600044A RID: 1098 RVA: 0x0001B68C File Offset: 0x0001988C
        public void Run()
        {
            this._stateMachine.StartBehaviour(!this.manualUpdate, null);
            if (PlayerPrefs.GetInt("WelcomeShown", 0) == 1)
            {
                this.CheckNews();
            }
        }

        // Token: 0x0600044B RID: 1099 RVA: 0x0001B6B7 File Offset: 0x000198B7
        public void PauseGame()
        {

        }

        // Token: 0x0600044C RID: 1100 RVA: 0x0001B6EB File Offset: 0x000198EB
        public void ResumGame()
        {

        }

        // Token: 0x0600044D RID: 1101 RVA: 0x0001B71F File Offset: 0x0001991F
        public static int GetTicksForTime(float time)
        {
            return Mathf.RoundToInt(time / WorldController.fixedTimeStep);
        }

        // Token: 0x0600044E RID: 1102 RVA: 0x0001B72D File Offset: 0x0001992D
        public static float GetTimeForTicks(int ticks)
        {
            return (float)ticks * WorldController.fixedTimeStep;
        }

        // Token: 0x0600044F RID: 1103 RVA: 0x0001B738 File Offset: 0x00019938
        private void OnDestroy()
        {

        }

        // Token: 0x06000450 RID: 1104 RVA: 0x0001B788 File Offset: 0x00019988
        public bool IsInFirstPersonMode()
        {
            return true;
        }

        // Token: 0x06000451 RID: 1105 RVA: 0x0001B7BC File Offset: 0x000199BC
        private bool IsInMoveWireframeComponent()
        {
            return true;
        }

        // Token: 0x06000452 RID: 1106 RVA: 0x0001B7F0 File Offset: 0x000199F0
        private void CheckNews()
        {
        }

        // Token: 0x06000454 RID: 1108 RVA: 0x0001B801 File Offset: 0x00019A01
        private void HandleWorkshopItemDownloaded(bool success, WorkshopController.WorkshopItemDownloadedResult result)
        {
            if (success && result.publishedFileId != this.tutorialPublishedFileId)
            {
                this._visor.ShowTutorialMessage(TutorialGestaltEnum.Item_Downloaded, null);
            }
        }

        // Token: 0x06000455 RID: 1109 RVA: 0x0001B822 File Offset: 0x00019A22
        public bool DoesPlayerHaveAuthority()
        {
            return true;
            //return !this.worldMetaData.isStaged || (this.worldMetaData.authorSteamId == GlobalControllers.steamController.GetPlayerSteamId() && !this.isPlaytestingStage);
        }

        // Token: 0x06000457 RID: 1111 RVA: 0x0001B870 File Offset: 0x00019A70
        public bool DoesPlayerHavePermission(Device device, WorldController.GlobalPermissions permission)
        {
            if (this.DoesPlayerHaveAuthority())
            {
                return true;
            }
            if (device == null)
            {
                return (this._globalPermissions & permission) > (WorldController.GlobalPermissions)0;
            }
            WorldController.GlobalPermissionStates globalPermissionStates = device.permissions[permission];
            return globalPermissionStates == WorldController.GlobalPermissionStates.True || (globalPermissionStates == WorldController.GlobalPermissionStates.Global && (this._globalPermissions & permission) > (WorldController.GlobalPermissions)0);
        }

        // Token: 0x06000458 RID: 1112 RVA: 0x0001B8C5 File Offset: 0x00019AC5
        public bool DoesPlayerHaveWorldPermission(WorldController.WorldPermissions permission)
        {
            return this.DoesPlayerHaveAuthority() || (this._worldPermissions & permission) > (WorldController.WorldPermissions)0;
        }

        // Token: 0x0600045A RID: 1114 RVA: 0x0001B904 File Offset: 0x00019B04
        public Dictionary<WorldController.GlobalPermissions, bool> ValidateSpecialGlobalPermissions()
        {
            Dictionary<WorldController.GlobalPermissions, bool> dictionary = new Dictionary<WorldController.GlobalPermissions, bool>();
            if ((this.globalPermissions & WorldController.GlobalPermissions.StateToggle) == (WorldController.GlobalPermissions)0)
            {
                this.globalPermissions &= (WorldController.GlobalPermissions)(-2049);
                dictionary.Add(WorldController.GlobalPermissions.ResetDevice, false);
            }
            else
            {
                dictionary.Add(WorldController.GlobalPermissions.ResetDevice, true);
            }
            if ((this.globalPermissions & WorldController.GlobalPermissions.SketchAccess) == (WorldController.GlobalPermissions)0)
            {
                this.globalPermissions &= (WorldController.GlobalPermissions)(-65);
                dictionary.Add(WorldController.GlobalPermissions.SketchEdit, false);
            }
            else
            {
                dictionary.Add(WorldController.GlobalPermissions.SketchEdit, true);
            }
            if ((this.globalPermissions & WorldController.GlobalPermissions.GrabWireframe) == (WorldController.GlobalPermissions)0 && (this.globalPermissions & WorldController.GlobalPermissions.AttachDetach) == (WorldController.GlobalPermissions)0)
            {
                this.globalPermissions &= (WorldController.GlobalPermissions)(-129);
                dictionary.Add(WorldController.GlobalPermissions.DeviceDelete, false);
            }
            else
            {
                dictionary.Add(WorldController.GlobalPermissions.DeviceDelete, true);
            }
            return dictionary;
        }

        // Token: 0x0600045B RID: 1115 RVA: 0x0001B9C0 File Offset: 0x00019BC0
        public Device GetDeviceByName(string deviceName)
        {
            deviceName = deviceName.ToUpperInvariant();
            foreach (Device device in this._devices.Values)
            {
                if (string.Equals(deviceName, device.displayName.ToUpperInvariant()))
                {
                    return device;
                }
            }
            return null;
        }

        // Token: 0x0600045C RID: 1116 RVA: 0x0001BA34 File Offset: 0x00019C34
        public void EnableStage()
        {

        }

        // Token: 0x0600045D RID: 1117 RVA: 0x0001BAB2 File Offset: 0x00019CB2
        public void DisableStage()
        {
            this.worldMetaData.isStaged = false;
            this.stage = null;
            this.TakeScreenshotOfWorld();
            this.SaveWorldToDisk(true, null);
        }

        // Token: 0x0600045E RID: 1118 RVA: 0x0001BAD8 File Offset: 0x00019CD8
        private void ApplyStageInitialState(bool fromLoadWorld)
        {

            this._localCharacter.SetInitialPositionAndOrientation(this.stage.playerPosition, this.stage.playerOrientation[0], this.stage.playerOrientation[1]);
            this._cameraPitch = this.stage.playerOrientation[0];
            this._cameraYaw = this.stage.playerOrientation[1];
        }

        // Token: 0x0600045F RID: 1119 RVA: 0x0001BC38 File Offset: 0x00019E38
        public bool IsWithinBiome(Vector3 position)
        {
            return (this._biomeSurfaceCenter - position).sqrMagnitude < this._biomeRadiusSquared;
        }

        // Token: 0x06000460 RID: 1120 RVA: 0x0001BC64 File Offset: 0x00019E64
        private bool IsWithinBiomeOuterArea(Vector3 position)
        {
            float sqrMagnitude = (this._biomeSurfaceCenter - position).sqrMagnitude;
            return sqrMagnitude < this._biomeRadiusSquared && sqrMagnitude > this._biomeRadiusSquared - this.outerBiomeAreaWidth;
        }

        // Token: 0x06000461 RID: 1121 RVA: 0x0001BCA4 File Offset: 0x00019EA4
        private void DoTargetRaycast()
        {
            
        }

        // Token: 0x06000462 RID: 1122 RVA: 0x0001BFC0 File Offset: 0x0001A1C0
        private void HandleRaycastTargetUpdate(ComponentHandler componentHandler, SubComponentHandler subComponentHandler, Device.State state, DynamicProp dynamicProp, bool interactive, bool dockable, bool reactsToRaycast)
        {
            
        }

        // Token: 0x06000463 RID: 1123 RVA: 0x0001C0A4 File Offset: 0x0001A2A4
        private void DoSocketRaycast()
        {
            
        }

        // Token: 0x06000464 RID: 1124 RVA: 0x0001C220 File Offset: 0x0001A420
        private void UpdateCameraMouseLook()
        {
            Vector2 vector = new Vector2(this._input.GetAxis("CameraHorizontal"), this._input.GetAxis("CameraVertical"));
            float b = this._cameraPitch - this.mouseSensitivity * vector.y;
            this._cameraPitch = Mathf.Lerp(this._cameraPitch, b, this.cameraSmoothing);
            this._cameraPitch = this.cameraPitchLimits.Clamp(this._cameraPitch);
            float b2 = this._cameraYaw + this.mouseSensitivity * vector.x;
            this._cameraYaw = Mathf.Lerp(this._cameraYaw, b2, this.cameraSmoothing);
            this._localCharacter.UpdateLookAngle(this._cameraPitch, this._cameraYaw);
        }

        // Token: 0x06000465 RID: 1125 RVA: 0x0001C2DC File Offset: 0x0001A4DC
        private void UpdateFirstPersonMovement()
        {
            this._localCharacter.UpdateMovement(this._input);
        }

        // Token: 0x06000466 RID: 1126 RVA: 0x0001C2F0 File Offset: 0x0001A4F0
        private void SetBackgroundTasks(WorldController.BackgroundTasks tasks)
        {
            
        }

        // Token: 0x06000467 RID: 1127 RVA: 0x0001C369 File Offset: 0x0001A569
        private void DisableSnappingPoints()
        {
            if (this._operatingComponent != null)
            {
                this._operatingComponent.SetSnappingPointsEnabled(false);
            }
            if (this._targetComponent != null)
            {
                this._targetComponent.SetSnappingPointsEnabled(false);
            }
        }

        // Token: 0x06000468 RID: 1128 RVA: 0x0001C39F File Offset: 0x0001A59F
        public void AddLightSource(Light spotlight)
        {
            this._lightSources.Add(spotlight);
        }

        // Token: 0x06000469 RID: 1129 RVA: 0x0001C3AD File Offset: 0x0001A5AD
        public void RemoveLightSource(Light spotlight)
        {
            this._lightSources.Remove(spotlight);
        }

        // Token: 0x0600046C RID: 1132 RVA: 0x0001C3EB File Offset: 0x0001A5EB
        public void SetMouseSensitivity(float value)
        {
            this.mouseSensitivity = value;
            PlayerPrefs.SetFloat("MouseSensitivity", value);
        }

        // Token: 0x0600046D RID: 1133 RVA: 0x0001C3FF File Offset: 0x0001A5FF
        public void SetHudHintsVisibility(bool value)
        {
            this.hudHintsVisibility = value;
            PlayerPrefs.SetInt("HUDHintsVisibility", value ? 1 : 0);
        }

        // Token: 0x0600046E RID: 1134 RVA: 0x0001C41C File Offset: 0x0001A61C
        private void TakeScreenshotOfTargetDevice()
        {
            float magnitude = this._targetDevice.vfxDevice.bounds.extents.magnitude;
            this._deviceScreenshotCamera.transform.position = this._targetDevice.worldCenter + new Vector3(0f, magnitude / 4f, magnitude / 2f);
            this._deviceScreenshotCamera.transform.rotation = Quaternion.LookRotation(this._targetDevice.worldCenter - this._deviceScreenshotCamera.transform.position, Vector3.up);
            float d = magnitude * 0.8f / Mathf.Sin(0.017453292f * this._deviceScreenshotCamera.fieldOfView / 2f);
            this._deviceScreenshotCamera.transform.position -= this._deviceScreenshotCamera.transform.forward * d;
            this._deviceScreenshotCamera.Render();
            Rect source = new Rect(0f, 0f, (float)this._deviceScreenshotCamera.targetTexture.width, (float)this._deviceScreenshotCamera.targetTexture.height);
            RenderTexture.active = this._deviceScreenshotCamera.targetTexture;
            this._deviceScreenshotTexture.ReadPixels(source, 0, 0);
            Color[] pixels = this._deviceScreenshotTexture.GetPixels();
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = pixels[i].gamma;
                pixels[i].a = 1f;
            }
            this._deviceScreenshotTexture.SetPixels(pixels);
            this._deviceScreenshotTexture.Apply();
        }

        // Token: 0x0600046F RID: 1135 RVA: 0x0001C5CC File Offset: 0x0001A7CC
        private void TakeScreenshotOfWorld()
        {
            BiomeGestalt biomeGestalt = Holder.biomeGestalts[this.worldMetaData.biomeId];
            this._worldScreenshotCamera.transform.position = biomeGestalt.cameraPosition;
            this._worldScreenshotCamera.transform.rotation = Quaternion.Euler(biomeGestalt.cameraRotation);
            this._worldScreenshotCamera.orthographicSize = biomeGestalt.cameraSize;
            this._worldScreenshotCamera.Render();
            Rect source = new Rect(0f, 0f, (float)this._worldScreenshotCamera.targetTexture.width, (float)this._worldScreenshotCamera.targetTexture.height);
            RenderTexture.active = this._worldScreenshotCamera.targetTexture;
            this._worldScreenshotTexture.ReadPixels(source, 0, 0);
            Color[] pixels = this._worldScreenshotTexture.GetPixels();
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = pixels[i].gamma;
                pixels[i].a = 1f;
            }
            this._worldScreenshotTexture.SetPixels(pixels);
            this._worldScreenshotTexture.Apply();
        }

        // Token: 0x06000470 RID: 1136 RVA: 0x0001C6E4 File Offset: 0x0001A8E4
        private void Update()
        {
            this._stateMachine.UpdateBehaviour();
            if ((this._enabledBackgroundTasks & WorldController.BackgroundTasks.PlayerLookAround) != (WorldController.BackgroundTasks)0)
            {
                this.UpdateCameraMouseLook();
            }
            if ((this._enabledBackgroundTasks & WorldController.BackgroundTasks.PlayerMove) != (WorldController.BackgroundTasks)0)
            {
                this.UpdateFirstPersonMovement();
            }
            if ((this._enabledBackgroundTasks & WorldController.BackgroundTasks.PlayerRaycast) != (WorldController.BackgroundTasks)0)
            {
                this.DoTargetRaycast();
                if (this._targetDevice != null && this._targetDevice.isEditingSockets)
                {
                    this.DoSocketRaycast();
                }
                else if (this._floatingSocket.activeSelf)
                {
                    this._floatingSocket.SetActive(false);
                }
            }
            if (this._input.GetButtonDown("FPSToggle"))
            {
                GraphyManager.ModuleState fpsModuleState = G_Singleton<GraphyManager>.Instance.FpsModuleState;
                G_Singleton<GraphyManager>.Instance.FpsModuleState = ((fpsModuleState == GraphyManager.ModuleState.BACKGROUND) ? GraphyManager.ModuleState.FULL : GraphyManager.ModuleState.BACKGROUND);
            }
        }

        // Token: 0x06000475 RID: 1141 RVA: 0x0001CF80 File Offset: 0x0001B180
        public Device GetDeviceWithGuid(int guid)
        {
            Device result;
            this._devices.TryGetValue(guid, out result);
            return result;
        }

        // Token: 0x06000476 RID: 1142 RVA: 0x0001CFA0 File Offset: 0x0001B1A0
        private int GenerateDeviceGUID()
        {
            int uniqueDeviceID = this._uniqueDeviceID;
            this._uniqueDeviceID = uniqueDeviceID + 1;
            return uniqueDeviceID;
        }

        // Token: 0x06000477 RID: 1143 RVA: 0x0001CFC0 File Offset: 0x0001B1C0
        public Device InstantiateDevice(int guid, string displayName)
        {
            Device component = UnityEngine.Object.Instantiate<GameObject>(this.devicePrefab).GetComponent<Device>();
            component.transform.SetParent(this.world, true);
            component.guid = guid;
            component.gameObject.name = displayName;
            component.displayName = displayName;
            if (guid >= 0)
            {
                this._devices.Add(guid, component);
            }
            return component;
        }

        // Token: 0x06000479 RID: 1145 RVA: 0x0001D254 File Offset: 0x0001B454
        public void SaveDeviceToDisk(string deviceName, string deviceDescription, bool overwrite)
        {
            //this._targetDevice.Lock();
            //GlobalControllers.storageHelperController.SaveDeviceToDiskAsync(this._targetDevice, deviceName, deviceDescription, overwrite, this._cameraYaw);
            //WorldController.Log("Attempting Async Saving Device (" + this._targetDevice.displayName + ")");
        }

        // Token: 0x0600047A RID: 1146 RVA: 0x0001D2A4 File Offset: 0x0001B4A4
        private void BuildDeviceFromDeviceBlueprint(string deviceFilename)
        {
        
        }

        // Token: 0x0600047B RID: 1147 RVA: 0x0001D39C File Offset: 0x0001B59C
        private void BuildDeviceFromWorkshop(ulong publishedFileId)
        {
            
        }

        // Token: 0x0600047C RID: 1148 RVA: 0x0001D530 File Offset: 0x0001B730
        private void FinishedLoadingDevice(Device device)
        {

        }

        // Token: 0x0600047E RID: 1150 RVA: 0x0001D698 File Offset: 0x0001B898
        public void SaveWorldToDisk(bool overwrite, Action handler = null)
        {
            
        }

        // Token: 0x0600047F RID: 1151 RVA: 0x0001D6F8 File Offset: 0x0001B8F8
        private static HashSet<string> GetExistingWorldsNames()
        {
            string[] files = Directory.GetFiles(Holder.worldsPath, "*.metadata");
            HashSet<string> hashSet = new HashSet<string>();
            return hashSet;
        }

        // Token: 0x06000480 RID: 1152 RVA: 0x0001D74C File Offset: 0x0001B94C
        public static string GetFirstAvailableWorldName()
        {
            string text = "NEW WORLD";
            int num = 1;
            HashSet<string> existingWorldsNames = WorldController.GetExistingWorldsNames();
            while (existingWorldsNames.Contains(text))
            {
                text = "NEW WORLD " + num.ToString("D2");
                num++;
            }
            return text;
        }
            
        // Token: 0x06000484 RID: 1156 RVA: 0x0001D890 File Offset: 0x0001BA90
        private void FinishedCloningComponent(Device device)
        {
            
        }

        // Token: 0x06000485 RID: 1157 RVA: 0x0001DAA8 File Offset: 0x0001BCA8
        public void RemoveDevice(Device device, bool immediate = false)
        {
            if (WorldController.OnDeviceDeleted != null)
            {
                WorldController.OnDeviceDeleted(device.guid, device.displayName.ToUpperInvariant(), null);
            }
            if (immediate)
            {
                UnityEngine.Object.DestroyImmediate(device.gameObject);
            }
            else
            {
                UnityEngine.Object.Destroy(device.gameObject);
            }
            if (device.guid >= 0)
            {
                this._devices.Remove(device.guid);
            }
        }

        // Token: 0x06000486 RID: 1158 RVA: 0x0001DB10 File Offset: 0x0001BD10
        private void Purge()
        {

        }

        // Token: 0x06000487 RID: 1159 RVA: 0x0001DB84 File Offset: 0x0001BD84
        private string GetNewDisplayNameForDevice()
        {
            string str = "Device";
            int num = this._devices.Count + 1;
            while (!this.IsDisplayNameAvailable(str + " " + num.ToString("D2")))
            {
                num++;
            }
            return str + " " + num.ToString("D2");
        }

        // Token: 0x06000488 RID: 1160 RVA: 0x0001DBE4 File Offset: 0x0001BDE4
        private bool IsDisplayNameAvailable(string displayName)
        {
            using (Dictionary<int, Device>.ValueCollection.Enumerator enumerator = this._devices.Values.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.displayName.ToUpper() == displayName.ToUpper())
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        // Token: 0x06000489 RID: 1161 RVA: 0x0001DC54 File Offset: 0x0001BE54
        private void CleanUpState()
        {
            this._visor.hud.HideInfoPanel();
            this._hintGestaltIds.Clear();
            this._advancedHintGestaltIds.Clear();
            this._targetSubComponent = null;
            this._targetComponent = null;
            this._targetDevice = null;
            this._targetState = Device.State.None;
            this._targetInteractive = false;
            this._targetReactsToRaycast = false;
            this._targetDockable = false;
            this._targetDynamicProp = null;
        }

        // Token: 0x0600048A RID: 1162 RVA: 0x0001DCC0 File Offset: 0x0001BEC0
        public void BuildReplayPinOrAttachComponent(SubComponentHandler parentSubComponent)
        {

        }

        // Token: 0x0600048C RID: 1164 RVA: 0x0001DD52 File Offset: 0x0001BF52
        public void BuildReplayToggleDevice()
        {
            if (this._targetDevice != null)
            {
                //this._targetDevice.ToggleState(this._targetDevice.rootComponent, false);
            }
        }

        // Token: 0x0600048F RID: 1167 RVA: 0x0001DF10 File Offset: 0x0001C110
        public void BuildReplayExplodeDevice()
        {

        }

        // Token: 0x06000490 RID: 1168 RVA: 0x0001DF94 File Offset: 0x0001C194
        public void BuildReplayOpenSketch(ComponentHandler componentHandler)
        {

        }

        // Token: 0x06000491 RID: 1169 RVA: 0x0001DFC5 File Offset: 0x0001C1C5
        public void BuildReplayRotateComponent(Vector3 value)
        {

        }

        // Token: 0x06000492 RID: 1170 RVA: 0x0001DFEC File Offset: 0x0001C1EC
        public BuildReplayCanvas.SerializedData BuildReplaySave()
        {
            if (this._buildReplayCanvas != null)
            {
                return this._buildReplayCanvas.GetComponent<BuildReplayCanvas>().Save();
            }
            return null;
        }

        // Token: 0x06000493 RID: 1171 RVA: 0x0001E00E File Offset: 0x0001C20E
        public void BuildReplayLoad(BuildReplayCanvas.SerializedData serializedData)
        {

        }

        // Token: 0x06000494 RID: 1172 RVA: 0x0001E02F File Offset: 0x0001C22F
        public void BuildReplayStart()
        {

        }

        // Token: 0x06000495 RID: 1173 RVA: 0x0001E04F File Offset: 0x0001C24F
        public void BuildReplayScaleComponent(Vector3 value)
        {

        }

        // Token: 0x06000496 RID: 1174 RVA: 0x0001E081 File Offset: 0x0001C281
        public void BuildReplayOffsetComponent(float value)
        {

        }

        // Token: 0x06000497 RID: 1175 RVA: 0x0001E0A8 File Offset: 0x0001C2A8
        public void BuildReplayColorComponent(int primary, int secondary)
        {
            if (this._lastSpawnedComponent != null)
            {
                this._lastSpawnedComponent.Paint(primary, false);
                this._lastSpawnedComponent.Paint(secondary, true);
            }
        }

        // Token: 0x06000498 RID: 1176 RVA: 0x0001E0D4 File Offset: 0x0001C2D4
        public void BuildReplaySetTransform(Vector3 position, Quaternion rotation)
        {
            if (this._lastSpawnedComponent != null)
            {
                this._lastSpawnedComponent.wireframePosition = position;
                this._lastSpawnedComponent.wireframeRotation = rotation;
                this._lastSpawnedComponent.OverrideInterpolationTransforms();
                this._lastSpawnedComponent.Interpolate(1f, false);
            }
        }

        // Token: 0x06000499 RID: 1177 RVA: 0x0001E123 File Offset: 0x0001C323
        public void BuildReplaySelectSocket(int index)
        {
            if (this._lastSpawnedComponent != null)
            {
                this._lastSpawnedComponent.SetCurrentFemaleSocketIndex(index);
            }
        }

        // Token: 0x04000396 RID: 918
        private bool _errorPopupClosed;

        // Token: 0x04000397 RID: 919
        private string _paintKey;

        // Token: 0x04000398 RID: 920
        private bool _inSelector;

        // Token: 0x040003A8 RID: 936
        public static float fixedTimeStep = 0.033333335f;

        // Token: 0x040003A9 RID: 937
        public bool manualUpdate;

        // Token: 0x040003AA RID: 938
        public bool skipLoadingScreen;

        // Token: 0x040003AB RID: 939
        public bool skipWelcomePopup;

        // Token: 0x040003AC RID: 940
        public bool skipTutorial;

        // Token: 0x040003AD RID: 941
        public bool disableGuruMeditation;

        // Token: 0x040003AE RID: 942
        public ulong slowDownFactor;

        // Token: 0x040003AF RID: 943
        public FloatRange cameraPitchLimits;

        // Token: 0x040003B0 RID: 944
        public float cameraSmoothing;

        // Token: 0x040003B1 RID: 945
        public GameObject localPlayerPrefab;

        // Token: 0x040003B2 RID: 946
        public GameObject devicePrefab;

        // Token: 0x040003B3 RID: 947
        public GameObject articulationPrefab;

        // Token: 0x040003B4 RID: 948
        public GameObject socketPrefab;

        // Token: 0x040003B5 RID: 949
        public bool skipPhysics;

        // Token: 0x040003B6 RID: 950
        public Dictionary<AgentGestalt.MassCategories, float> massCategoriesMultipliers;

        // Token: 0x040003B7 RID: 951
        public float mouseSensitivity;

        // Token: 0x040003B8 RID: 952
        public FloatRange offsetLimits;

        // Token: 0x040003B9 RID: 953
        public float translationSpeed;

        // Token: 0x040003BA RID: 954
        public float scaleSpeed;

        // Token: 0x040003BB RID: 955
        public float componentOffsetSpeed;

        // Token: 0x040003BC RID: 956
        public float genericRaycastDistance;

        // Token: 0x040003BD RID: 957
        public float interactionRaycastDistance;

        // Token: 0x040003BE RID: 958
        public float surfingRaycastDistanceOffset;

        // Token: 0x040003BF RID: 959
        public float deviceRotationMultiplier;

        // Token: 0x040003C0 RID: 960
        public float snapRotateInterval;

        // Token: 0x040003C1 RID: 961
        public float spawnDistance;

        // Token: 0x040003C2 RID: 962
        public float worldSeaLevelOffset;

        // Token: 0x040003C3 RID: 963
        public float volumeToMassRatio;

        // Token: 0x040003C4 RID: 964
        public float outerBiomeAreaWidth;

        // Token: 0x040003C5 RID: 965
        public float socketCycleAnimationDuration;

        // Token: 0x040003C6 RID: 966
        public bool hudHintsVisibility;

        // Token: 0x040003C7 RID: 967
        public int componentsCountPerFrameOnLoad;

        // Token: 0x040003C8 RID: 968
        public int devicesCountPerFrameOnSave;

        // Token: 0x040003C9 RID: 969
        public float characterProjectileForceMultiplier;

        // Token: 0x040003CA RID: 970
        public bool disableNonInterpolatedComponents;

        // Token: 0x040003CB RID: 971
        public float shortHoldActionDuration;

        // Token: 0x040003CC RID: 972
        public float normalHoldActionDuration;

        // Token: 0x040003CD RID: 973
        public float longHoldActionDuration;

        // Token: 0x040003CE RID: 974
        public float veryLongHoldActionDuration;

        // Token: 0x040003CF RID: 975
        public float quickbarHideDelay;

        // Token: 0x040003D0 RID: 976
        public float welcomePopupDelay;

        // Token: 0x040003D1 RID: 977
        public float saveReminderInteraval;

        // Token: 0x040003D2 RID: 978
        public float playtestingFadeDuration;

        // Token: 0x040003D3 RID: 979
        [EventRef]
        public string attachSound;

        // Token: 0x040003D4 RID: 980
        [EventRef]
        public string detachSound;

        // Token: 0x040003D5 RID: 981
        [EventRef]
        public string paintPrimarySound;

        // Token: 0x040003D6 RID: 982
        [EventRef]
        public string paintSecondarySound;

        // Token: 0x040003D7 RID: 983
        [EventRef]
        public string cloneSound;

        // Token: 0x040003D8 RID: 984
        [EventRef]
        public string deviceStaticSound;

        // Token: 0x040003D9 RID: 985
        [EventRef]
        public string deviceDynamicSound;

        // Token: 0x040003DA RID: 986
        [EventRef]
        public string pinSound;

        // Token: 0x040003DB RID: 987
        [EventRef]
        public string grabSound;

        // Token: 0x040003DC RID: 988
        [EventRef]
        public string deleteSound;

        // Token: 0x040003DD RID: 989
        [EventRef]
        public string resetDeviceSound;

        // Token: 0x040003DE RID: 990
        [EventRef]
        public string treeLineOnSound;

        // Token: 0x040003DF RID: 991
        [EventRef]
        public string treeLineOffSound;

        // Token: 0x040003E0 RID: 992
        [EventRef]
        public string enterAdvancedHudSound;

        // Token: 0x040003E1 RID: 993
        [EventRef]
        public string exitAdvancedHudSound;

        // Token: 0x040003E2 RID: 994
        [EventRef]
        public string confirmAdvancedHudSound;

        // Token: 0x040003E3 RID: 995
        [EventRef]
        public string stepAdvancedHudSound;

        // Token: 0x040003E4 RID: 996
        [EventRef]
        public string rebaseSound;

        // Token: 0x040003E5 RID: 997
        [EventRef]
        public string rotateStepsSound;

        // Token: 0x040003E6 RID: 998
        [EventRef]
        public string rotateLinearSound;

        // Token: 0x040003E7 RID: 999
        [EventRef]
        public string offsetLinearSound;

        // Token: 0x040003E8 RID: 1000
        [EventRef]
        public string scaleUpSound;

        // Token: 0x040003E9 RID: 1001
        [EventRef]
        public string scaleDownSound;

        // Token: 0x040003EA RID: 1002
        public Dictionary<AgentGestaltEnum, TutorialGestaltEnum> componentTutorials;

        // Token: 0x040003EB RID: 1003
        public ulong tutorialAuthorSteamId;

        // Token: 0x040003EC RID: 1004
        public ulong tutorialPublishedFileId;

        // Token: 0x040003ED RID: 1005
        public string tutorialFilename;

        // Token: 0x040003EE RID: 1006
        private dynamic _stateMachine;

        // Token: 0x040003EF RID: 1007
        private dynamic _visor;

        // Token: 0x040003F0 RID: 1008
        private RigidbodyCharacter _localCharacter;

        // Token: 0x040003F1 RID: 1009
        private Player _input;

        // Token: 0x040003F2 RID: 1010
        private bool _playerIsDocked;

        // Token: 0x040003F3 RID: 1011
        private bool _playerIsInPhotoMode;

        // Token: 0x040003F4 RID: 1012
        private bool _playerTookPhoto;

        // Token: 0x040003F5 RID: 1013
        private Camera _camera;

        // Token: 0x040003F6 RID: 1014
        private Camera _dummyCamera;

        // Token: 0x040003F7 RID: 1015
        private float _cameraPitch;

        // Token: 0x040003F8 RID: 1016
        private float _cameraYaw;

        // Token: 0x040003F9 RID: 1017
        private Camera _deviceScreenshotCamera;

        // Token: 0x040003FA RID: 1018
        private Camera _worldScreenshotCamera;

        // Token: 0x040003FB RID: 1019
        private int _uniqueDeviceID;

        // Token: 0x040003FC RID: 1020
        private Dictionary<int, Device> _devices;

        // Token: 0x040003FD RID: 1021
        private Transform _safeZone;

        // Token: 0x040003FE RID: 1022
        private bool _firstWorldLoad;

        // Token: 0x040003FF RID: 1023
        private bool _shouldOpenWelcome;

        // Token: 0x04000400 RID: 1024
        private bool _playerIsWithinOuterArea;

        // Token: 0x04000401 RID: 1025
        private Device _targetDevice;

        // Token: 0x04000402 RID: 1026
        private ComponentHandler _targetComponent;

        // Token: 0x04000403 RID: 1027
        private SubComponentHandler _targetSubComponent;

        // Token: 0x04000404 RID: 1028
        private DynamicProp _targetDynamicProp;

        // Token: 0x04000405 RID: 1029
        private Vector3 _targetPosition;

        // Token: 0x04000406 RID: 1030
        private Vector3 _targetAngle;

        // Token: 0x04000407 RID: 1031
        private DynamicProp _enabledDynamicProp;

        // Token: 0x04000408 RID: 1032
        private SubComponentHandler _candidateParentSubComponent;

        // Token: 0x04000409 RID: 1033
        private SnappingGeneric _candidateSnappingObject;

        // Token: 0x0400040A RID: 1034
        private Device.State _targetState;

        // Token: 0x0400040B RID: 1035
        private bool _targetInteractive;

        // Token: 0x0400040C RID: 1036
        private bool _targetReactsToRaycast;

        // Token: 0x0400040D RID: 1037
        private bool _targetDockable;

        // Token: 0x0400040E RID: 1038
        private float _targetDistance;

        // Token: 0x0400040F RID: 1039
        private Vector3 _candidatePositionReferenceFrame;

        // Token: 0x04000410 RID: 1040
        private Quaternion _candidateRotationReferenceFrame;

        // Token: 0x04000411 RID: 1041
        private ComponentHandler _previousTargetComponent;

        // Token: 0x04000412 RID: 1042
        private Device.State _previousTargetState;

        // Token: 0x04000413 RID: 1043
        private Device _previousTargetDevice;

        // Token: 0x04000414 RID: 1044
        private ComponentHandler _operatingComponent;

        // Token: 0x04000415 RID: 1045
        private SubComponentHandler _operatingSubComponent;

        // Token: 0x04000416 RID: 1046
        private ComponentHandler _dockingStationComponent;

        // Token: 0x04000417 RID: 1047
        private Device _operatingDevice;

        // Token: 0x04000418 RID: 1048
        private dynamic _componentRotator;

        // Token: 0x04000419 RID: 1049
        private float _moveComponentDistance;

        // Token: 0x0400041A RID: 1050
        private Quaternion _cameraViewOffset;

        // Token: 0x0400041B RID: 1051
        private float _sketchTimeAccumulator;

        // Token: 0x0400041C RID: 1052
        private float _biomeRadiusSquared;

        // Token: 0x0400041D RID: 1053
        private Vector3 _biomeDefaultSpawnPosition;

        // Token: 0x0400041E RID: 1054
        private Vector3 _biomeDeviceRespawnPosition;

        // Token: 0x0400041F RID: 1055
        private Vector3 _biomeSurfaceCenter;

        // Token: 0x04000420 RID: 1056
        private int _componentColorId;

        // Token: 0x04000421 RID: 1057
        private int _altComponentColorId;

        // Token: 0x04000422 RID: 1058
        private List<HintGestaltEnum> _hintGestaltIds;

        // Token: 0x04000423 RID: 1059
        private List<HintGestaltEnum> _advancedHintGestaltIds;

        // Token: 0x04000424 RID: 1060
        private WorldController.BackgroundTasks _enabledBackgroundTasks;

        // Token: 0x04000425 RID: 1061
        private List<Light> _lightSources;

        // Token: 0x04000426 RID: 1062
        private List<dynamic> _projectiles;

        // Token: 0x04000427 RID: 1063
        private dynamic _rotationOverlay;

        // Token: 0x04000428 RID: 1064
        private dynamic _scaleOverlay;

        // Token: 0x04000429 RID: 1065
        private DynamicGridProjector _dynamicGridProjector;

        // Token: 0x0400042A RID: 1066
        private bool _forceRaycastNotification;

        // Token: 0x0400042B RID: 1067
        private int _deviceRotationAxis;

        // Token: 0x0400042C RID: 1068
        private bool _freePlacementWhileMovingWireframe;

        // Token: 0x0400042D RID: 1069
        private Quaternion _componentToCameraYRootOffset;

        // Token: 0x0400042E RID: 1070
        private bool _tickPhysics;

        // Token: 0x0400042F RID: 1071
        private ulong _tickNumber;

        // Token: 0x04000430 RID: 1072
        private bool _canRun;

        // Token: 0x04000431 RID: 1073
        private bool _isPaused;

        // Token: 0x04000432 RID: 1074
        private DynamicPropsManager _dynamicPropsManager;

        // Token: 0x04000433 RID: 1075
        private List<GameObject> _toDestroyLateUpdate;

        // Token: 0x04000434 RID: 1076
        private Texture2D _deviceScreenshotTexture;

        // Token: 0x04000435 RID: 1077
        private Texture2D _worldScreenshotTexture;

        // Token: 0x04000436 RID: 1078
        private bool _waitForTick;

        // Token: 0x04000437 RID: 1079
        private bool _shouldRecloneComponent;

        // Token: 0x04000438 RID: 1080
        private bool _isDeletingComponent;

        // Token: 0x04000439 RID: 1081
        private WorldController.WorldPermissions _worldPermissions;

        // Token: 0x0400043A RID: 1082
        private WorldController.GlobalPermissions _globalPermissions;

        // Token: 0x0400043B RID: 1083
        private bool _exceptionUnhandled;

        // Token: 0x0400043C RID: 1084
        private float _lastFrameScrollAmount;

        // Token: 0x0400043D RID: 1085
        private bool _deviceWasSolidBeforeProcessorUI;

        // Token: 0x0400043E RID: 1086
        private bool _deviceWasSolidBeforePropertyEditor;

        // Token: 0x0400043F RID: 1087
        private float _timeSinceLastSaveReminder;

        // Token: 0x04000440 RID: 1088
        private bool _errorLoadingTutorial;

        // Token: 0x04000441 RID: 1089
        private List<dynamic> _undoQueue;

        // Token: 0x04000442 RID: 1090
        private Vector3 _positionAtStartOfOperation;

        // Token: 0x04000443 RID: 1091
        private Quaternion _rotationAtStartOfOperation;

        // Token: 0x04000444 RID: 1092
        private Vector3 _pitchYawRollAtStartOfOperation;

        // Token: 0x04000445 RID: 1093
        private Vector3 _scaleAtStartOfOperation;

        // Token: 0x04000446 RID: 1094
        private float _offsetAtStartOfOperation;

        // Token: 0x04000447 RID: 1095
        private bool _hudForceUpdate;

        // Token: 0x04000448 RID: 1096
        private ComponentHandler _mountedComponent;

        // Token: 0x04000449 RID: 1097
        private Device _movingDevice;

        // Token: 0x0400044A RID: 1098
        private Vector3 _movingDevicePosOffset;

        // Token: 0x0400044B RID: 1099
        private Quaternion _movingDeviceBaseRotation;

        // Token: 0x0400044C RID: 1100
        private int _numberOfComponentsToLoad;

        // Token: 0x0400044D RID: 1101
        private bool _inGhostMode;

        // Token: 0x0400044E RID: 1102
        private bool _showingAdvancedActions;

        // Token: 0x0400044F RID: 1103
        private dynamic _freeRotationSoundInstance;

        // Token: 0x04000450 RID: 1104
        private dynamic _offsetSoundInstance;

        // Token: 0x04000451 RID: 1105
        private FemaleSocketPoint _candidateSocket;

        // Token: 0x04000452 RID: 1106
        private Vector3 _socketPlacementPosition;

        // Token: 0x04000453 RID: 1107
        private Quaternion _socketPlacementRotation;

        // Token: 0x04000454 RID: 1108
        private GameObject _floatingSocket;

        // Token: 0x04000455 RID: 1109
        private GameObject _buildReplayCanvas;

        // Token: 0x04000456 RID: 1110
        private ComponentHandler _lastSpawnedComponent;

        // Token: 0x04000457 RID: 1111
        private const string _bbWorldToLoad = "worldToLoad";

        // Token: 0x04000458 RID: 1112
        private const string _bbWorldTypeToLoad = "worldTypeToLoad";

        // Token: 0x04000459 RID: 1113
        public const string bbPublishedWorldInfo = "publishedWorldInfo";

        // Token: 0x0400045A RID: 1114
        public const string bbShouldSaveWorld = "shouldSaveWorld";

        // Token: 0x0400045B RID: 1115
        public const string bbShouldQuit = "shouldQuit";

        // Token: 0x0400045C RID: 1116
        private const float _maxRotationDelta = 10f;

        // Token: 0x0400045D RID: 1117
        private const float _maxOffsetMultiplier = 1500f;

        // Token: 0x0400045E RID: 1118
        private const int _frenchKeyboardId = 1036;

        // Token: 0x0400045F RID: 1119
        private const int _germanKeyboardId = 1031;

        // Token: 0x020002FD RID: 765
        private class WaitActionParameters
        {
            // Token: 0x04001A0E RID: 6670
            public float duration;

            // Token: 0x04001A0F RID: 6671
            public Color color;

            // Token: 0x04001A10 RID: 6672
            public string message;

            // Token: 0x04001A11 RID: 6673
            public bool playSound = true;
        }

        // Token: 0x020002FE RID: 766
        [Serializable]
        private class NewsMetaData
        {
            // Token: 0x04001A12 RID: 6674
            public int id;

            // Token: 0x04001A13 RID: 6675
            public string text;
        }

        // Token: 0x020002FF RID: 767
        public class Stage
        {
            // Token: 0x04001A14 RID: 6676
            public float timeOfDay;

            // Token: 0x04001A15 RID: 6677
            public float timeSpeed;

            // Token: 0x04001A16 RID: 6678
            public Vector3 playerPosition;

            // Token: 0x04001A17 RID: 6679
            public Vector3 playerOrientation;
        }

        // Token: 0x02000300 RID: 768
        [Flags]
        public enum WorldPermissions
        {
            // Token: 0x04001A19 RID: 6681
            PlayerCanSpawnComponents = 1,
            // Token: 0x04001A1A RID: 6682
            PlayerCanFly = 2,
            // Token: 0x04001A1B RID: 6683
            PlayerCanMakeNewDevices = 4,
            // Token: 0x04001A1C RID: 6684
            All = 7
        }

        // Token: 0x02000301 RID: 769
        public enum GlobalPermissions
        {
            // Token: 0x04001A1E RID: 6686
            ComponentManipulation = 2,
            // Token: 0x04001A1F RID: 6687
            StateToggle = 4,
            // Token: 0x04001A20 RID: 6688
            GrabWireframe = 8,
            // Token: 0x04001A21 RID: 6689
            PropertyEditorAccess = 16,
            // Token: 0x04001A22 RID: 6690
            SketchAccess = 32,
            // Token: 0x04001A23 RID: 6691
            SketchEdit = 64,
            // Token: 0x04001A24 RID: 6692
            DeviceDelete = 128,
            // Token: 0x04001A25 RID: 6693
            AttachDetach = 256,
            // Token: 0x04001A26 RID: 6694
            GrabSolid = 512,
            // Token: 0x04001A27 RID: 6695
            Clone = 1024,
            // Token: 0x04001A28 RID: 6696
            ResetDevice = 2048,
            // Token: 0x04001A29 RID: 6697
            PaintComponent = 4096,
            // Token: 0x04001A2A RID: 6698
            All = 8190
        }

        // Token: 0x02000302 RID: 770
        public enum GlobalPermissionStates
        {
            // Token: 0x04001A2C RID: 6700
            True,
            // Token: 0x04001A2D RID: 6701
            Global,
            // Token: 0x04001A2E RID: 6702
            False
        }

        // Token: 0x02000303 RID: 771
        public enum Windows
        {
            // Token: 0x04001A30 RID: 6704
            Invalid,
            // Token: 0x04001A31 RID: 6705
            DeviceBrowser,
            // Token: 0x04001A32 RID: 6706
            WorldBrowser,
            // Token: 0x04001A33 RID: 6707
            News,
            // Token: 0x04001A34 RID: 6708
            Feedback
        }

        // Token: 0x02000304 RID: 772
        public enum WorldTypes
        {
            // Token: 0x04001A36 RID: 6710
            Local,
            // Token: 0x04001A37 RID: 6711
            Workshop,
            // Token: 0x04001A38 RID: 6712
            Progress,
            // Token: 0x04001A39 RID: 6713
            Temporary,
            // Token: 0x04001A3A RID: 6714
            Tutorial,
            // Token: 0x04001A3B RID: 6715
            TutorialProgress
        }

        // Token: 0x02000305 RID: 773
        // (Invoke) Token: 0x06001EC6 RID: 7878
        public delegate void ComponentTargetEvent(ComponentHandler component, SubComponentHandler subComponent, Device.State state, DynamicProp dynamicProp, bool interactive, bool dockable, bool reactsToRaycast);

        // Token: 0x02000306 RID: 774
        // (Invoke) Token: 0x06001ECA RID: 7882
        public delegate void DevicesUpdateEvent();

        // Token: 0x02000307 RID: 775
        // (Invoke) Token: 0x06001ECE RID: 7886
        public delegate void DeviceEvent(int guid, string deviceName, string componentName);

        // Token: 0x02000308 RID: 776
        // (Invoke) Token: 0x06001ED2 RID: 7890
        public delegate void PhysicsEvent();

        // Token: 0x02000309 RID: 777
        // (Invoke) Token: 0x06001ED6 RID: 7894
        public delegate void ProjectileEvent(dynamic projectile);

        // Token: 0x0200030A RID: 778
        // (Invoke) Token: 0x06001EDA RID: 7898
        public delegate void ComponentTransformUpdate(ComponentHandler componentHandler);

        // Token: 0x0200030B RID: 779
        // (Invoke) Token: 0x06001EDE RID: 7902
        public delegate void GameEvent();

        // Token: 0x0200030C RID: 780
        [Flags]
        private enum BackgroundTasks
        {
            // Token: 0x04001A3D RID: 6717
            PlayerLookAround = 1,
            // Token: 0x04001A3E RID: 6718
            PlayerMove = 2,
            // Token: 0x04001A3F RID: 6719
            PlayerRaycast = 4,
            // Token: 0x04001A40 RID: 6720
            Outline = 8,
            // Token: 0x04001A41 RID: 6721
            TreeLine = 16
        }
    }

}
