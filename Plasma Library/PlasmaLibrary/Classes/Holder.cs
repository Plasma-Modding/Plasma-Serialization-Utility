using Sirenix.OdinInspector;
using UnityEngine;
// Token: 0x0200003C RID: 60
public class Holder : SerializedMonoBehaviour
{
    // Token: 0x060001C7 RID: 455 RVA: 0x0000B868 File Offset: 0x00009A68
    public static int GetQuickbarSlotIndexForComponentId(AgentGestaltEnum componentGestaltId)
    {
        if (componentGestaltId == AgentGestaltEnum.Invalid)
        {
            return 0;
        }
        foreach (KeyValuePair<int, AgentGestaltEnum> keyValuePair in Holder.quickbarAssignments)
        {
            if (keyValuePair.Value == componentGestaltId)
            {
                return keyValuePair.Key;
            }
        }
        return 0;
    }

    // Token: 0x060001C8 RID: 456 RVA: 0x0000B8D0 File Offset: 0x00009AD0
    public static void AssignComponentIdToQuickbarSlot(AgentGestaltEnum componentGestaltId, int slotIndex)
    {
        if (componentGestaltId == AgentGestaltEnum.Invalid)
        {
            return;
        }
        List<int> list = new List<int>();
        foreach (KeyValuePair<int, AgentGestaltEnum> keyValuePair in Holder.quickbarAssignments)
        {
            if (keyValuePair.Value == componentGestaltId)
            {
                list.Add(keyValuePair.Key);
            }
        }
        bool flag = true;
        foreach (int num in list)
        {
            Holder.quickbarAssignments[num] = AgentGestaltEnum.Invalid;
            if (num == slotIndex)
            {
                flag = false;
            }
        }
        if (flag)
        {
            Holder.quickbarAssignments[slotIndex] = componentGestaltId;
        }
    }

    // Token: 0x060001C9 RID: 457 RVA: 0x0000B99C File Offset: 0x00009B9C
    public static void ToggleFavoriteNode(AgentGestaltEnum nodeGestaltId)
    {
        if (nodeGestaltId == AgentGestaltEnum.Invalid)
        {
            return;
        }
        if (!Holder.favoriteNodesAssignments.Contains(nodeGestaltId))
        {
            Holder.favoriteNodesAssignments.Add(nodeGestaltId);
        }
        else
        {
            Holder.favoriteNodesAssignments.Remove(nodeGestaltId);
        }
        Holder.favoriteNodesAssignments.Sort((AgentGestaltEnum a, AgentGestaltEnum b) => Holder.logicNodesByEnum[a].displayName.CompareTo(Holder.logicNodesByEnum[b].displayName));
    }

    // Token: 0x060001CA RID: 458 RVA: 0x0000B9FC File Offset: 0x00009BFC
    public static bool IsTimeAfterLastPlayed(DateTime dateTime)
    {
        string @string = PlayerPrefs.GetString("LastTimePlayed", null);
        if (string.IsNullOrEmpty(@string))
        {
            return false;
        }
        DateTime t = DateTime.FromBinary(Convert.ToInt64(@string));
        return dateTime > t;
    }

    // Token: 0x060001CB RID: 459 RVA: 0x0000BA32 File Offset: 0x00009C32
    private void DeleteAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    // Token: 0x060001CC RID: 460 RVA: 0x0000BA39 File Offset: 0x00009C39
    private void DeleteVideoSettings()
    {
        PlayerPrefs.DeleteKey("ScreenHeight");
        PlayerPrefs.DeleteKey("ScreenRefresh");
        PlayerPrefs.DeleteKey("ScreenWidth");
        PlayerPrefs.DeleteKey("FOV");
        PlayerPrefs.DeleteKey("ScreenID");
        PlayerPrefs.DeleteKey("ScreenVSync");
    }

    // Token: 0x060001CD RID: 461 RVA: 0x0000BA77 File Offset: 0x00009C77
    private void DeleteMiscSettings()
    {
        PlayerPrefs.DeleteKey("HUDHintsVisibility");
    }

    // Token: 0x060001CE RID: 462 RVA: 0x0000BA83 File Offset: 0x00009C83
    private void DeleteWelcomeSettings()
    {
        PlayerPrefs.DeleteKey("WelcomeShown");
    }

    // Token: 0x060001CF RID: 463 RVA: 0x0000BA8F File Offset: 0x00009C8F
    private void DeleteLastSavedWorld()
    {
        PlayerPrefs.DeleteKey("LastSavedWorld");
    }

    // Token: 0x060001D0 RID: 464 RVA: 0x0000BA9C File Offset: 0x00009C9C
    private void DeleteTutorialImpressions()
    {
        foreach (object obj in Enum.GetValues(typeof(AgentGestaltEnum)))
        {
            int num = (int)obj;
            if (PlayerPrefs.HasKey("TutorialImpressions_" + num.ToString()))
            {
                PlayerPrefs.DeleteKey("TutorialImpressions_" + num.ToString());
            }
        }
    }

    // Token: 0x040001D2 RID: 466
    public Dictionary<int, UIColorEntity> UIColorPalette;

    // Token: 0x040001D3 RID: 467
    public Dictionary<AgentGestalt.ComponentCategories, Holder.SketchColorTheme> sketchColorThemes;

    // Token: 0x040001D4 RID: 468
    public Holder.SketchColorTheme sketchLogicColorTheme;

    // Token: 0x040001D5 RID: 469
    public Dictionary<string, GameObject> keyboardIcons;

    // Token: 0x040001D6 RID: 470
    public Dictionary<AgentCategoryEnum, string> agentCategories;

    // Token: 0x040001D7 RID: 471
    public float UITransitionDuration;

    // Token: 0x040001D8 RID: 472
    public Dictionary<Holder.TransitionDurations, float> transitionDurations;

    // Token: 0x040001D9 RID: 473
    public Texture2D normalCursor;

    // Token: 0x040001DA RID: 474
    public Texture2D panCursor;

    // Token: 0x040001DB RID: 475
    public Texture2D pinCursor;

    // Token: 0x040001DC RID: 476
    public int sketchViewWidth;

    // Token: 0x040001DD RID: 477
    public int sketchViewHeight;

    // Token: 0x040001DE RID: 478
    public Dictionary<Data.Types, Holder.DataTypeDescriptor> dataTypeDescriptors;

    // Token: 0x040001DF RID: 479
    public Holder.DataTypeDescriptor anyDataDescriptor;

    // Token: 0x040001E0 RID: 480
    public Sprite variableIcon;

    // Token: 0x040001E1 RID: 481
    public Sprite variableSketchIcon;

    // Token: 0x040001E2 RID: 482
    public Sprite moduleInterfaceIcon;

    // Token: 0x040001E3 RID: 483
    public float buttonAnimationInDuration;

    // Token: 0x040001E4 RID: 484
    public float buttonAnimationOutDuration;

    // Token: 0x040001E5 RID: 485
    public BiomeGestaltEnum DemoDefaultBiome;

    // Token: 0x040001E6 RID: 486
    public Dictionary<int, TextAsset> demoImageAssets;

    // Token: 0x040001E7 RID: 487
    public List<VideoClip> tutorialVideos;

    // Token: 0x040001E8 RID: 488
    [ColorUsage(true, true)]
    public Color LEDPowerColor;

    // Token: 0x040001E9 RID: 489
    [ColorUsage(true, true)]
    public Color LEDBusyColor;

    // Token: 0x040001EA RID: 490
    [ColorUsage(true, true)]
    public Color LEDWarningColor;

    // Token: 0x040001EB RID: 491
    [ColorUsage(true, true)]
    public Color LEDErrorColor;

    // Token: 0x040001EC RID: 492
    [ColorUsage(true, true)]
    public Color LEDConfirmColor;

    // Token: 0x040001ED RID: 493
    [ColorUsage(true, true)]
    public Color overlayNormalColor;

    // Token: 0x040001EE RID: 494
    [ColorUsage(true, true)]
    public Color overlaySelectedColor;

    // Token: 0x040001EF RID: 495
    [ColorUsage(true, true)]
    public Color propsInteractiveColor;

    // Token: 0x040001F0 RID: 496
    [ColorUsage(false, false)]
    public Color structureColor;

    // Token: 0x040001F1 RID: 497
    public List<Color> componentPalette;

    // Token: 0x040001F2 RID: 498
    public List<Color> componentPaletteUI;

    // Token: 0x040001F3 RID: 499
    public static Holder instance;

    // Token: 0x040001F4 RID: 500
    public static Dictionary<string, List<AgentGestalt>> sortedComponentGestaltsByKeyword;

    // Token: 0x040001F5 RID: 501
    public static Dictionary<AgentGestaltEnum, AgentGestalt> agentGestalts;

    // Token: 0x040001F6 RID: 502
    public static Dictionary<AgentGestaltEnum, AgentGestalt> componentGestalts;

    // Token: 0x040001F7 RID: 503
    public static Dictionary<AgentGestaltEnum, AgentId> logicNodesByEnum;

    // Token: 0x040001F8 RID: 504
    public static Dictionary<AgentCategoryEnum, List<AgentId>> logicNodesByCategory;

    // Token: 0x040001F9 RID: 505
    public static Dictionary<string, List<AgentId>> sortedlogicNodesByKeyword;

    // Token: 0x040001FA RID: 506
    public static Dictionary<HintGestaltEnum, HintGestalt> hintGestalts;

    // Token: 0x040001FB RID: 507
    public static Dictionary<TutorialGestaltEnum, TutorialGestalt> tutorialGestalts;

    // Token: 0x040001FC RID: 508
    public static Dictionary<BlockingTutorialGestaltEnum, BlockingTutorialGestalt> blockingTutorialGestalts;

    // Token: 0x040001FD RID: 509
    public static Dictionary<BiomeGestaltEnum, BiomeGestalt> biomeGestalts;

    // Token: 0x040001FE RID: 510
    public static List<Color> colorSwatches;

    // Token: 0x040001FF RID: 511
    public static Dictionary<int, AgentGestaltEnum> quickbarAssignments;

    // Token: 0x04000200 RID: 512
    public static List<AgentGestaltEnum> favoriteNodesAssignments;

    // Token: 0x04000201 RID: 513
    public static readonly Dictionary<Data.Types, List<int>> sketchViewNodePreviewWidths = new Dictionary<Data.Types, List<int>>
    {
        {
            Data.Types.Boolean,
            new List<int>
            {
                2,
                1
            }
        },
        {
            Data.Types.Color,
            new List<int>
            {
                3,
                2
            }
        },
        {
            Data.Types.Image,
            new List<int>
            {
                5,
                4
            }
        },
        {
            Data.Types.None,
            new List<int>
            {
                5,
                3
            }
        },
        {
            Data.Types.Number,
            new List<int>
            {
                5,
                4
            }
        },
        {
            Data.Types.Sound,
            new List<int>
            {
                5,
                4
            }
        },
        {
            Data.Types.String,
            new List<int>
            {
                5,
                4
            }
        },
        {
            Data.Types.Selection,
            new List<int>
            {
                5,
                4
            }
        },
        {
            Data.Types.ModuleInterface,
            new List<int>
            {
                5,
                4
            }
        },
        {
            Data.Types.ComponentProperty,
            new List<int>
            {
                5,
                4
            }
        }
    };

    // Token: 0x04000202 RID: 514
    public const int sketchViewVariableWidth = 4;

    // Token: 0x04000203 RID: 515
    public const int sketchViewNodeMaxTypeWidth = 5;

    // Token: 0x04000204 RID: 516
    public static char decimalSeparator;

    // Token: 0x04000205 RID: 517
    public static string importPath;

    // Token: 0x04000206 RID: 518
    public static string devicesPath;

    // Token: 0x04000207 RID: 519
    public static string worldsPath;

    // Token: 0x04000208 RID: 520
    public static string progressPath;

    // Token: 0x04000209 RID: 521
    public static string screenshotsPath;

    // Token: 0x0400020A RID: 522
    private static string _colorSwatchesPath;

    // Token: 0x0400020B RID: 523
    private static string _quickbarAssignmentsPath;

    // Token: 0x0400020C RID: 524
    private static string _favoriteNodesAssignmentsPath;

    // Token: 0x020002DF RID: 735
    public enum TransitionDurations
    {
        // Token: 0x04001966 RID: 6502
        Short,
        // Token: 0x04001967 RID: 6503
        Normal,
        // Token: 0x04001968 RID: 6504
        Long,
        // Token: 0x04001969 RID: 6505
        Shorter,
        // Token: 0x0400196A RID: 6506
        Shortest,
        // Token: 0x0400196B RID: 6507
        Longer,
        // Token: 0x0400196C RID: 6508
        Longest,
        // Token: 0x0400196D RID: 6509
        Shortish,
        // Token: 0x0400196E RID: 6510
        Longish
    }

    // Token: 0x020002E0 RID: 736
    public class DataTypeDescriptor
    {
        // Token: 0x0400196F RID: 6511
        public string name;

        // Token: 0x04001970 RID: 6512
        public string description;

        // Token: 0x04001971 RID: 6513
        public Sprite icon;

        // Token: 0x04001972 RID: 6514
        public Sprite sketchIcon;
    }

    // Token: 0x020002E1 RID: 737
    public class SketchColorTheme
    {
        // Token: 0x06001E6B RID: 7787 RVA: 0x00095EE0 File Offset: 0x000940E0
        private IList<ValueDropdownItem<int>> ColorList()
        {
            ValueDropdownList<int> valueDropdownList = new ValueDropdownList<int>();
            foreach (KeyValuePair<int, UIColorEntity> keyValuePair in UnityEngine.Object.FindObjectOfType<Holder>().UIColorPalette)
            {
                valueDropdownList.Add(keyValuePair.Value.label + " (" + keyValuePair.Value.description.Replace("\n", " ") + ")", keyValuePair.Key);
            }
            return valueDropdownList;
        }

        // Token: 0x04001973 RID: 6515
        [ColorEntity]
        public int normal;

        // Token: 0x04001974 RID: 6516
        [ColorEntity]
        public int alternate;

        // Token: 0x04001975 RID: 6517
        [ColorEntity]
        public int highlighted;

        // Token: 0x04001976 RID: 6518
        [ColorEntity]
        public int disabled;

        // Token: 0x04001977 RID: 6519
        [ColorEntity]
        public int lighterShade;

        // Token: 0x04001978 RID: 6520
        [ColorEntity]
        public int typeStatic;

        // Token: 0x04001979 RID: 6521
        [ColorEntity]
        public int typeDynamic;

        // Token: 0x0400197A RID: 6522
        [ColorEntity]
        public int safetyBufferWarning;

        // Token: 0x0400197B RID: 6523
        [ColorEntity]
        public int moduleInterface;
    }
}

