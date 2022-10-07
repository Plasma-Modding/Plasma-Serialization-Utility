using UnityEngine;
// Token: 0x0200021E RID: 542
public class SketchConnection
{
    // Token: 0x170001FA RID: 506
    // (get) Token: 0x060012DC RID: 4828 RVA: 0x000618D2 File Offset: 0x0005FAD2
    public SketchNodePort output
    {
        get
        {
            return this._output;
        }
    }

    // Token: 0x170001FB RID: 507
    // (get) Token: 0x060012DD RID: 4829 RVA: 0x000618DA File Offset: 0x0005FADA
    public SketchNodePort input
    {
        get
        {
            return this._input;
        }
    }

    // Token: 0x170001FC RID: 508
    // (get) Token: 0x060012DE RID: 4830 RVA: 0x000618E2 File Offset: 0x0005FAE2
    // (set) Token: 0x060012DF RID: 4831 RVA: 0x000618EA File Offset: 0x0005FAEA
    public SketchConnection.States state { get; set; }

    // Token: 0x170001FD RID: 509
    // (get) Token: 0x060012E0 RID: 4832 RVA: 0x000618F3 File Offset: 0x0005FAF3
    // (set) Token: 0x060012E1 RID: 4833 RVA: 0x000618FB File Offset: 0x0005FAFB
    public SketchConnection.MetaData metaData { get; set; }

    // Token: 0x060012E2 RID: 4834 RVA: 0x00061904 File Offset: 0x0005FB04
    public SketchConnection(SketchNodePort output, SketchNodePort input)
    {
        this._output = output;
        this._input = input;
    }

    // Token: 0x060012E3 RID: 4835 RVA: 0x0006191C File Offset: 0x0005FB1C
    public static bool CheckValidity(AgentGestalt.Port output, AgentGestalt.Port input)
    {
        bool result = true;
        if (output.dataType == Data.Types.None && !output.allowsAnyData && input.type == AgentGestalt.Port.Types.Property && !input.allowsAnyData)
        {
            result = false;
        }
        else if (output.dataType == Data.Types.None && !output.allowsAnyData && input.type == AgentGestalt.Port.Types.Command && input.expectsData)
        {
            result = false;
        }
        else if (output.dataType != Data.Types.None && !output.allowsAnyData && output.injectedProperty == 0 && input.type == AgentGestalt.Port.Types.Property && !input.allowsAnyData && !Data.CheckCompatibility(output.dataType, input.dataType))
        {
            result = false;
        }
        else if (output.dataType != Data.Types.None && !output.allowsAnyData && output.injectedProperty == 0 && input.type == AgentGestalt.Port.Types.Command && input.expectsData && !input.allowsAnyData && !Data.CheckCompatibility(output.dataType, input.dataType))
        {
            result = false;
        }
        return result;
    }

    // Token: 0x04000F8C RID: 3980
    private SketchNodePort _output;

    // Token: 0x04000F8D RID: 3981
    private SketchNodePort _input;

    // Token: 0x0200042C RID: 1068
    public enum States
    {
        // Token: 0x04001E77 RID: 7799
        Normal,
        // Token: 0x04001E78 RID: 7800
        DisabledByUser,
        // Token: 0x04001E79 RID: 7801
        DisabledByVariable
    }

    // Token: 0x0200042D RID: 1069
    public class MetaData
    {
        // Token: 0x04001E7A RID: 7802
        public int outputPortId;

        // Token: 0x04001E7B RID: 7803
        public int inputPortId;

        // Token: 0x04001E7C RID: 7804
        public int outputNodeId;

        // Token: 0x04001E7D RID: 7805
        public int inputNodeId;

        // Token: 0x04001E7E RID: 7806
        public Color lineColor;

        // Token: 0x04001E7F RID: 7807
        public List<Vector3> pinPositions;

        // Token: 0x04001E80 RID: 7808
        public int index;
    }
}
