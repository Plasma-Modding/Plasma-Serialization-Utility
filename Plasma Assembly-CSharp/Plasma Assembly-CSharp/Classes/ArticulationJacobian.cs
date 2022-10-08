using System;
using System.Collections.Generic;
public struct ArticulationJacobian
{
    // Token: 0x06000059 RID: 89 RVA: 0x000028CC File Offset: 0x00000ACC
    public ArticulationJacobian(int rows, int cols)
    {
        this.rowsCount = rows;
        this.colsCount = cols;
        this.matrixData = new List<float>(rows * cols);
        for (int i = 0; i < rows * cols; i++)
        {
            this.matrixData.Add(0f);
        }
    }

    // Token: 0x17000032 RID: 50
    public float this[int row, int col]
    {
        get
        {
            bool flag = row < 0 || row >= this.rowsCount;
            if (flag)
            {
                throw new IndexOutOfRangeException();
            }
            bool flag2 = col < 0 || col >= this.colsCount;
            if (flag2)
            {
                throw new IndexOutOfRangeException();
            }
            return this.matrixData[row * this.colsCount + col];
        }
        set
        {
            bool flag = row < 0 || row >= this.rowsCount;
            if (flag)
            {
                throw new IndexOutOfRangeException();
            }
            bool flag2 = col < 0 || col >= this.colsCount;
            if (flag2)
            {
                throw new IndexOutOfRangeException();
            }
            this.matrixData[row * this.colsCount + col] = value;
        }
    }

    // Token: 0x17000033 RID: 51
    // (get) Token: 0x0600005C RID: 92 RVA: 0x000029E0 File Offset: 0x00000BE0
    // (set) Token: 0x0600005D RID: 93 RVA: 0x000029F8 File Offset: 0x00000BF8
    public int rows
    {
        get
        {
            return this.rowsCount;
        }
        set
        {
            this.rowsCount = value;
        }
    }

    // Token: 0x17000034 RID: 52
    // (get) Token: 0x0600005E RID: 94 RVA: 0x00002A04 File Offset: 0x00000C04
    // (set) Token: 0x0600005F RID: 95 RVA: 0x00002A1C File Offset: 0x00000C1C
    public int columns
    {
        get
        {
            return this.colsCount;
        }
        set
        {
            this.colsCount = value;
        }
    }

    // Token: 0x17000035 RID: 53
    // (get) Token: 0x06000060 RID: 96 RVA: 0x00002A28 File Offset: 0x00000C28
    // (set) Token: 0x06000061 RID: 97 RVA: 0x00002A40 File Offset: 0x00000C40
    public List<float> elements
    {
        get
        {
            return this.matrixData;
        }
        set
        {
            this.matrixData = value;
        }
    }

    // Token: 0x04000078 RID: 120
    private int rowsCount;

    // Token: 0x04000079 RID: 121
    private int colsCount;

    // Token: 0x0400007A RID: 122
    private List<float> matrixData;
}
