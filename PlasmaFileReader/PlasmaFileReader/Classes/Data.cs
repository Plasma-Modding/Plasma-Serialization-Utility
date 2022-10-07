using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlasmaFileReader.Plasma.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Sirenix.OdinInspector;
    using Sirenix.Utilities;
    using UnityEngine;

    // Token: 0x0200021A RID: 538
    [Serializable]
    public class Data
    {
        // Token: 0x06001284 RID: 4740 RVA: 0x0005E740 File Offset: 0x0005C940
        private IList<ValueDropdownItem<Data.Types>> ValidTypes()
        {
            return new ValueDropdownList<Data.Types>
            {
                Data.Types.Boolean,
                Data.Types.Color,
                Data.Types.ComponentProperty,
                Data.Types.Image,
                Data.Types.ModuleInterface,
                Data.Types.Number,
                Data.Types.Selection,
                Data.Types.Sound,
                Data.Types.String
            };
        }

        // Token: 0x06001285 RID: 4741 RVA: 0x0005E794 File Offset: 0x0005C994
        public Data()
        {
            this.type = Data.Types.None;
            this.booleanValue = false;
            this.colorValue = Color.black;
            this.componentPropertyValue = default(Data.ComponentProperty);
            this.numberValue = 0f;
            this.imageValue = default(Data.Image);
            this.imageValue.index = 0;
            this.moduleInterfaceValue = default(Data.ModuleInterface);
            this.moduleInterfaceValue.label = "<NONE>";
            this.selectionValue = default(Data.Selection);
            this.soundValue = default(Data.Sound);
            this.soundValue.soundEvent = "";
            this.stringValue = "";
        }

        // Token: 0x06001286 RID: 4742 RVA: 0x0005E83F File Offset: 0x0005CA3F
        public Data(Data data)
        {
            this.Copy(data);
        }

        // Token: 0x06001287 RID: 4743 RVA: 0x0005E84E File Offset: 0x0005CA4E
        public Data(bool value) : this()
        {
            this.type = Data.Types.Boolean;
            this.booleanValue = value;
        }

        // Token: 0x06001288 RID: 4744 RVA: 0x0005E864 File Offset: 0x0005CA64
        public Data(Color value) : this()
        {
            this.type = Data.Types.Color;
            this.colorValue = value;
        }

        // Token: 0x06001289 RID: 4745 RVA: 0x0005E87A File Offset: 0x0005CA7A
        public Data(Data.ComponentProperty value) : this()
        {
            this.type = Data.Types.ComponentProperty;
            this.componentPropertyValue = value;
        }

        // Token: 0x0600128A RID: 4746 RVA: 0x0005E890 File Offset: 0x0005CA90
        public Data(float value) : this()
        {
            this.type = Data.Types.Number;
            this.numberValue = value;
            this.numberDecorator = Data.NumberDecorators.Generic;
        }

        // Token: 0x0600128B RID: 4747 RVA: 0x0005E8AD File Offset: 0x0005CAAD
        public Data(float value, Data.NumberDecorators decorator) : this()
        {
            this.type = Data.Types.Number;
            this.numberValue = value;
            this.numberDecorator = decorator;
        }

        // Token: 0x0600128C RID: 4748 RVA: 0x0005E8CA File Offset: 0x0005CACA
        public Data(Data.Image value) : this()
        {
            this.type = Data.Types.Image;
            this.imageValue = value;
        }

        // Token: 0x0600128D RID: 4749 RVA: 0x0005E8E0 File Offset: 0x0005CAE0
        public Data(int value) : this()
        {
            this.type = Data.Types.Number;
            this.numberValue = (float)value;
            this.numberDecorator = Data.NumberDecorators.Integer;
        }

        // Token: 0x0600128E RID: 4750 RVA: 0x0005E8FE File Offset: 0x0005CAFE
        public Data(int value, Data.NumberDecorators decorator) : this()
        {
            this.type = Data.Types.Number;
            this.numberValue = (float)value;
            this.numberDecorator = decorator;
        }

        // Token: 0x0600128F RID: 4751 RVA: 0x0005E91C File Offset: 0x0005CB1C
        public Data(Data.ModuleInterface value) : this()
        {
            this.type = Data.Types.ModuleInterface;
            this.moduleInterfaceValue = value;
        }

        // Token: 0x06001290 RID: 4752 RVA: 0x0005E933 File Offset: 0x0005CB33
        public Data(Data.Selection value) : this()
        {
            this.type = Data.Types.Selection;
            this.selectionValue = value;
        }

        // Token: 0x06001291 RID: 4753 RVA: 0x0005E949 File Offset: 0x0005CB49
        public Data(Data.Sound value) : this()
        {
            this.type = Data.Types.Sound;
            this.soundValue = value;
        }

        // Token: 0x06001292 RID: 4754 RVA: 0x0005E95F File Offset: 0x0005CB5F
        public Data(string value) : this()
        {
            this.type = Data.Types.String;
            this.stringValue = value;
        }

        // Token: 0x06001293 RID: 4755 RVA: 0x0005E978 File Offset: 0x0005CB78
        public void Copy(Data data)
        {
            this.type = data.type;
            this.booleanValue = data.booleanValue;
            this.colorValue = data.colorValue;
            this.componentPropertyValue = data.componentPropertyValue;
            this.numberValue = data.numberValue;
            this.numberDecorator = data.numberDecorator;
            this.imageValue.index = data.imageValue.index;
            this.imageValue.md5Hash = data.imageValue.md5Hash;
            this.moduleInterfaceValue = data.moduleInterfaceValue;
            this.selectionValue = data.selectionValue;
            this.soundValue = data.soundValue;
            this.stringValue = data.stringValue;
        }

        // Token: 0x06001294 RID: 4756 RVA: 0x0005EA2C File Offset: 0x0005CC2C
        public bool IsEqualTo(Data data, bool compareDecorators = true)
        {
            if (this.type == data.type)
            {
                Data.Types types = this.type;
                switch (types)
                {
                    case Data.Types.Boolean:
                        return this.booleanValue == data.booleanValue;
                    case Data.Types.Number:
                        if (!compareDecorators || this.numberDecorator == data.numberDecorator)
                        {
                            if (this.numberDecorator == Data.NumberDecorators.Integer)
                            {
                                if (data.numberDecorator == Data.NumberDecorators.Integer)
                                {
                                    return this.numberValue == data.numberValue;
                                }
                                return this.numberValue == (float)Mathf.RoundToInt(data.numberValue);
                            }
                            else
                            {
                                if (data.numberDecorator == Data.NumberDecorators.Integer)
                                {
                                    return (float)Mathf.RoundToInt(this.numberValue) == data.numberValue;
                                }
                                return Mathf.Approximately(this.numberValue, data.numberValue);
                            }
                        }
                        break;
                    case Data.Types.Color:
                        return this.colorValue == data.colorValue;
                    case (Data.Types)3:
                        break;
                    case Data.Types.String:
                        return this.stringValue.Equals(data.stringValue);
                    case Data.Types.Image:
                        return this.imageValue == data.imageValue;
                    case Data.Types.ComponentProperty:
                        return this.componentPropertyValue == data.componentPropertyValue;
                    case Data.Types.Selection:
                        return this.selectionValue == data.selectionValue;
                    case Data.Types.Sound:
                        return this.soundValue == data.soundValue;
                    case Data.Types.ModuleInterface:
                        return this.moduleInterfaceValue == data.moduleInterfaceValue;
                    default:
                        if (types == Data.Types.None)
                        {
                            return true;
                        }
                        break;
                }
            }
            return false;
        }

        // Token: 0x06001295 RID: 4757 RVA: 0x0005EB96 File Offset: 0x0005CD96
        public static bool CheckCompatibility(Data.Types type1, Data.Types type2)
        {
            return type1 == type2;
        }

        // Token: 0x06001296 RID: 4758 RVA: 0x0005EB9C File Offset: 0x0005CD9C
        public static string TypeToString(Data.Types type)
        {
            switch (type)
            {
                case Data.Types.Boolean:
                    return "<BOOLEAN>";
                case Data.Types.Number:
                    return "<NUMBER>";
                case Data.Types.Color:
                    return "<COLOR>";
                case (Data.Types)3:
                    break;
                case Data.Types.String:
                    return "<TEXT>";
                case Data.Types.Image:
                    return "<IMAGE>";
                case Data.Types.ComponentProperty:
                    return "<COMPONENT PROPERTY>";
                case Data.Types.Selection:
                    return "<SELECTION>";
                case Data.Types.Sound:
                    return "<SOUND>";
                case Data.Types.ModuleInterface:
                    return "<MODULE INTERFACE>";
                default:
                    if (type == Data.Types.None)
                    {
                        return "<NONE>";
                    }
                    break;
            }
            return "";
        }

        // Token: 0x06001298 RID: 4760 RVA: 0x0005EE8C File Offset: 0x0005D08C
        public override string ToString()
        {
            Data.Types types = this.type;
            switch (types)
            {
                case Data.Types.Boolean:
                    return "(Boolean) " + (this.booleanValue ? "TRUE" : "FALSE");
                case Data.Types.Number:
                    return string.Concat(new string[]
                    {
                    "(Number) ",
                    this.numberValue.ToString(),
                    "  [",
                    this.numberDecorator.ToString(),
                    "]"
                    });
                case Data.Types.Color:
                    {
                        string str = "(Color) ";
                        Color color = this.colorValue;
                        return str + color.ToString();
                    }
                case (Data.Types)3:
                    break;
                case Data.Types.String:
                    return "(String) " + this.stringValue;
                case Data.Types.Image:
                    return "(Image) " + this.imageValue.index.ToString() + ((this.imageValue.md5Hash != null) ? (":" + this.imageValue.md5Hash) : "");
                case Data.Types.ComponentProperty:
                    return "(ComponentProperty) " + this.componentPropertyValue.componentDisplayName + ":" + this.componentPropertyValue.property.ToString();
                case Data.Types.Selection:
                    {
                        string[] array = new string[7];
                        array[0] = "(Selection) ";
                        int num = 1;
                        Type provider = this.selectionValue.provider;
                        array[num] = ((provider != null) ? provider.ToString() : null);
                        array[2] = ":";
                        array[3] = this.selectionValue.id.ToString();
                        array[4] = " [";
                        array[5] = this.selectionValue.category.ToString();
                        array[6] = "]";
                        return string.Concat(array);
                    }
                case Data.Types.Sound:
                    return "(Sound) " + this.soundValue.soundEvent;
                case Data.Types.ModuleInterface:
                    return string.Concat(new string[]
                    {
                    "(ModuleInterface) ",
                    this.moduleInterfaceValue.type.ToString(),
                    ":",
                    this.moduleInterfaceValue.id.ToString(),
                    "(",
                    this.moduleInterfaceValue.label,
                    ")"
                    });
                default:
                    if (types == Data.Types.None)
                    {
                        return "(NONE)";
                    }
                    break;
            }
            return "";
        }

        // Token: 0x04000F6E RID: 3950
        public Data.Types type;

        // Token: 0x04000F6F RID: 3951
        public bool booleanValue;

        // Token: 0x04000F70 RID: 3952
        public Color colorValue;

        // Token: 0x04000F71 RID: 3953
        public Data.ComponentProperty componentPropertyValue;

        // Token: 0x04000F72 RID: 3954
        public Data.Image imageValue;

        // Token: 0x04000F73 RID: 3955
        public Data.ModuleInterface moduleInterfaceValue;

        // Token: 0x04000F74 RID: 3956
        public float numberValue;

        // Token: 0x04000F75 RID: 3957
        public Data.NumberDecorators numberDecorator;

        // Token: 0x04000F76 RID: 3958
        public Data.Selection selectionValue;

        // Token: 0x04000F77 RID: 3959
        public Data.Sound soundValue;

        // Token: 0x04000F78 RID: 3960
        public string stringValue;

        // Token: 0x0200041F RID: 1055
        public enum Types
        {
            // Token: 0x04001E54 RID: 7764
            Boolean,
            // Token: 0x04001E55 RID: 7765
            Number,
            // Token: 0x04001E56 RID: 7766
            Color,
            // Token: 0x04001E57 RID: 7767
            String = 4,
            // Token: 0x04001E58 RID: 7768
            Image,
            // Token: 0x04001E59 RID: 7769
            ComponentProperty,
            // Token: 0x04001E5A RID: 7770
            Selection,
            // Token: 0x04001E5B RID: 7771
            Sound,
            // Token: 0x04001E5C RID: 7772
            ModuleInterface,
            // Token: 0x04001E5D RID: 7773
            None = 101
        }

        // Token: 0x02000420 RID: 1056
        public enum NumberDecorators
        {
            // Token: 0x04001E5F RID: 7775
            Generic,
            // Token: 0x04001E60 RID: 7776
            Percentage,
            // Token: 0x04001E61 RID: 7777
            Time,
            // Token: 0x04001E62 RID: 7778
            Integer
        }

        // Token: 0x02000421 RID: 1057
        public struct ComponentProperty
        {
            // Token: 0x17000535 RID: 1333
            // (get) Token: 0x06002221 RID: 8737 RVA: 0x000A49DB File Offset: 0x000A2BDB
            public bool isProper
            {
                get
                {
                    return !string.IsNullOrEmpty(this.componentDisplayName) && this.property > 0;
                }
            }

            // Token: 0x06002222 RID: 8738 RVA: 0x000A49F5 File Offset: 0x000A2BF5
            public override bool Equals(object obj)
            {
                return obj is Data.ComponentProperty && this == (Data.ComponentProperty)obj;
            }

            // Token: 0x06002223 RID: 8739 RVA: 0x000A4A14 File Offset: 0x000A2C14
            public static bool operator ==(Data.ComponentProperty c1, Data.ComponentProperty c2)
            {
                if (c1.property != c2.property)
                {
                    return false;
                }
                if (string.IsNullOrEmpty(c1.componentDisplayName))
                {
                    return string.IsNullOrEmpty(c2.componentDisplayName);
                }
                return !string.IsNullOrEmpty(c2.componentDisplayName) && c1.componentDisplayName.ToUpperInvariant().Equals(c2.componentDisplayName.ToUpperInvariant());
            }

            // Token: 0x06002224 RID: 8740 RVA: 0x000A4A79 File Offset: 0x000A2C79
            public static bool operator !=(Data.ComponentProperty c1, Data.ComponentProperty c2)
            {
                return !(c1 == c2);
            }

            // Token: 0x06002225 RID: 8741 RVA: 0x000A4A85 File Offset: 0x000A2C85
            public override int GetHashCode()
            {
                return this.componentDisplayName.GetHashCode() ^ this.property.GetHashCode();
            }

            // Token: 0x04001E63 RID: 7779
            public string componentDisplayName;

            // Token: 0x04001E64 RID: 7780
            public int property;
        }

        // Token: 0x02000422 RID: 1058
        public struct Selection
        {
            // Token: 0x06002226 RID: 8742 RVA: 0x000A4A9E File Offset: 0x000A2C9E
            public override bool Equals(object obj)
            {
                return obj is Data.Selection && this == (Data.Selection)obj;
            }

            // Token: 0x06002227 RID: 8743 RVA: 0x000A4ABB File Offset: 0x000A2CBB
            public static bool operator ==(Data.Selection s1, Data.Selection s2)
            {
                return s1.id == s2.id && s1.category == s2.category && s1.provider == s2.provider;
            }

            // Token: 0x06002228 RID: 8744 RVA: 0x000A4AEC File Offset: 0x000A2CEC
            public static bool operator !=(Data.Selection s1, Data.Selection s2)
            {
                return !(s1 == s2);
            }

            // Token: 0x06002229 RID: 8745 RVA: 0x000A4AF8 File Offset: 0x000A2CF8
            public override int GetHashCode()
            {
                return this.id.GetHashCode() ^ this.category.GetHashCode() ^ this.provider.GetHashCode();
            }

            // Token: 0x04001E65 RID: 7781
            public Type provider;

            // Token: 0x04001E66 RID: 7782
            public int category;

            // Token: 0x04001E67 RID: 7783
            public int id;
        }

        // Token: 0x02000423 RID: 1059
        public struct Image
        {
            // Token: 0x0600222B RID: 8747 RVA: 0x000A4B68 File Offset: 0x000A2D68
            public override bool Equals(object obj)
            {
                return obj is Data.Image && this == (Data.Image)obj;
            }

            // Token: 0x0600222C RID: 8748 RVA: 0x000A4B85 File Offset: 0x000A2D85
            public static bool operator ==(Data.Image i1, Data.Image i2)
            {
                return i1.index == i2.index;
            }

            // Token: 0x0600222D RID: 8749 RVA: 0x000A4B95 File Offset: 0x000A2D95
            public static bool operator !=(Data.Image i1, Data.Image i2)
            {
                return !(i1 == i2);
            }

            // Token: 0x0600222E RID: 8750 RVA: 0x000A4BA1 File Offset: 0x000A2DA1
            public override int GetHashCode()
            {
                return this.index.GetHashCode();
            }

            // Token: 0x04001E68 RID: 7784
            public int index;

            // Token: 0x04001E69 RID: 7785
            public string md5Hash;
        }

        // Token: 0x02000424 RID: 1060
        public struct Sound
        {
            // Token: 0x17000536 RID: 1334
            // (get) Token: 0x0600222F RID: 8751 RVA: 0x000A4BAE File Offset: 0x000A2DAE
            public bool isEmpty
            {
                get
                {
                    return string.IsNullOrEmpty(this.soundEvent);
                }
            }

            // Token: 0x06002230 RID: 8752 RVA: 0x000A4BBB File Offset: 0x000A2DBB
            public void SetAsEmpty()
            {
                this.soundEvent = null;
            }

            // Token: 0x06002231 RID: 8753 RVA: 0x000A4BC4 File Offset: 0x000A2DC4
            public override bool Equals(object obj)
            {
                return obj is Data.Sound && this == (Data.Sound)obj;
            }

            // Token: 0x06002232 RID: 8754 RVA: 0x000A4BE1 File Offset: 0x000A2DE1
            public static bool operator ==(Data.Sound i1, Data.Sound i2)
            {
                return (!string.IsNullOrEmpty(i1.soundEvent) && i1.soundEvent.Equals(i2.soundEvent)) || (string.IsNullOrEmpty(i1.soundEvent) && string.IsNullOrEmpty(i2.soundEvent));
            }

            // Token: 0x06002233 RID: 8755 RVA: 0x000A4C1F File Offset: 0x000A2E1F
            public static bool operator !=(Data.Sound i1, Data.Sound i2)
            {
                return !(i1 == i2);
            }

            // Token: 0x06002234 RID: 8756 RVA: 0x000A4C2B File Offset: 0x000A2E2B
            public override int GetHashCode()
            {
                return this.soundEvent.GetHashCode();
            }

            // Token: 0x04001E6A RID: 7786
            public string soundEvent;
        }

        // Token: 0x02000425 RID: 1061
        public struct ModuleInterface
        {
            // Token: 0x06002235 RID: 8757 RVA: 0x000A4C38 File Offset: 0x000A2E38
            public override bool Equals(object obj)
            {
                return obj is Data.ModuleInterface && this == (Data.ModuleInterface)obj;
            }

            // Token: 0x06002236 RID: 8758 RVA: 0x000A4C55 File Offset: 0x000A2E55
            public static bool operator ==(Data.ModuleInterface i1, Data.ModuleInterface i2)
            {
                return i1.id == i2.id && i1.type == i2.type && i1.label.Equals(i2.label);
            }

            // Token: 0x06002237 RID: 8759 RVA: 0x000A4C86 File Offset: 0x000A2E86
            public static bool operator !=(Data.ModuleInterface i1, Data.ModuleInterface i2)
            {
                return !(i1 == i2);
            }

            // Token: 0x06002238 RID: 8760 RVA: 0x000A4C92 File Offset: 0x000A2E92
            public override int GetHashCode()
            {
                return this.id.GetHashCode() ^ this.type.GetHashCode();
            }

            // Token: 0x04001E6B RID: 7787
            public AgentGestalt.Port.Types type;

            // Token: 0x04001E6C RID: 7788
            public int id;

            // Token: 0x04001E6D RID: 7789
            public string label;
        }
    }
}
