// Token: 0x0200000C RID: 12
using System.IO;

public static class PathExtension
{
    // Token: 0x06000033 RID: 51 RVA: 0x00003334 File Offset: 0x00001534
    public static string Combine(params string[] path)
    {
        if (path.Length == 0)
        {
            return "";
        }
        string text = path[0];
        for (int i = 1; i < path.Length; i++)
        {
            text = Path.Combine(text, path[i]);
        }
        return text;
    }
}
