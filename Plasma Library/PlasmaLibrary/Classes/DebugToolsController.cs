using UnityEngine;
// Token: 0x02000038 RID: 56
public class DebugToolsController : Controller
{
    // Token: 0x06000174 RID: 372 RVA: 0x000097B6 File Offset: 0x000079B6
    public override void Init()
    {
        this._imageDebuggerCanvas = GameObject.FindGameObjectWithTag("ImageDebuggerCanvas");
        this._imageDebuggerCanvas.SetActive(false);
    }

    // Token: 0x0400017C RID: 380
    public KeyCode imageDebuggerKey;

    // Token: 0x0400017D RID: 381
    private GameObject _imageDebuggerCanvas;
}
