using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000046 RID: 70
public class RenderQueueController : Controller
{
    // Token: 0x06000208 RID: 520 RVA: 0x0000EB64 File Offset: 0x0000CD64
    public override void Init()
    {
        /*
        this._queuedOperations = new List<RenderQueueController.Operation>();
        RenderQueueController.temporaryTexture2D = new Texture2D(Controllers.assetController.textureWidth, Controllers.assetController.textureHeight, Controllers.assetController.userTextureFormat, new UnityEngine.Experimental.Rendering.TextureCreationFlags());
        RenderQueueController.temporaryTexture2D.wrapMode = TextureWrapMode.Clamp;
        RenderQueueController.totalPixels = Controllers.assetController.textureWidth * Controllers.assetController.textureHeight;
        this._blitMaterial = UnityEngine.Object.Instantiate<Material>(this.blitMaterial);
        RenderQueueController.drawLineMaterial = new Material(this.drawLineShader);
        RenderQueueController.fillColorMaterial = new Material(this.fillColorShader);
        RenderQueueController.drawRectMaterial = new Material(this.drawRectShader);
        RenderQueueController.drawCircleMaterial = new Material(this.drawCircleShader);
        RenderQueueController.fillCircleMaterial = new Material(this.fillCircleShader);
        */
    }

    // Token: 0x06000209 RID: 521 RVA: 0x0000EC2C File Offset: 0x0000CE2C
    public void ScheduleClearTexture(Color destinationColor, bool noColor, int destinationTextureIndex, string info)
    {
        RenderQueueController.Operation operation = new RenderQueueController.Operation();
        operation.type = RenderQueueController.Operation.Types.ClearTexture;
        operation.destinationColor = destinationColor;
        operation.noColor = noColor;
        operation.destinationIndex = destinationTextureIndex;
        operation.info = info;
        this._queuedOperations.Add(operation);
        string str = "Scheduled operation: ";
        RenderQueueController.Operation operation2 = operation;
        //this.Log(str + ((operation2 != null) ? operation2.ToString() : null));
    }

    // Token: 0x0600020A RID: 522 RVA: 0x0000EC90 File Offset: 0x0000CE90
    public void ScheduleApplyTexture(Color32[] pixels, int destinationTextureIndex, string info)
    {
        RenderQueueController.Operation operation = new RenderQueueController.Operation();
        operation.type = RenderQueueController.Operation.Types.ApplyTexture;
        operation.pixels = (Color32[])pixels.Clone();
        operation.destinationIndex = destinationTextureIndex;
        operation.info = info;
        this._queuedOperations.Add(operation);
        string str = "Scheduled operation: ";
        RenderQueueController.Operation operation2 = operation;
        //this.Log(str + ((operation2 != null) ? operation2.ToString() : null));
    }

    // Token: 0x0600020B RID: 523 RVA: 0x0000ECF4 File Offset: 0x0000CEF4
    public void ScheduleCameraRender(Camera theCamera, int renderTextureIndex, string info)
    {
        RenderQueueController.Operation operation = new RenderQueueController.Operation();
        operation.type = RenderQueueController.Operation.Types.CameraRender;
        operation.camera = theCamera;
        operation.sourceIndex = renderTextureIndex;
        operation.info = info;
        this._queuedOperations.Add(operation);
        string str = "Scheduled operation: ";
        RenderQueueController.Operation operation2 = operation;
        //this.Log(str + ((operation2 != null) ? operation2.ToString() : null));
    }

    // Token: 0x0600020C RID: 524 RVA: 0x0000ED4C File Offset: 0x0000CF4C
    public void ScheduleCopyTexture(int sourceTextureIndex, int destinationTextureIndex, string info)
    {
        RenderQueueController.Operation operation = new RenderQueueController.Operation();
        operation.type = RenderQueueController.Operation.Types.CopyTexture;
        operation.sourceIndex = sourceTextureIndex;
        operation.destinationIndex = destinationTextureIndex;
        operation.info = info;
        this._queuedOperations.Add(operation);
        string str = "Scheduled operation: ";
        RenderQueueController.Operation operation2 = operation;
        //this.Log(str + ((operation2 != null) ? operation2.ToString() : null));
    }

    // Token: 0x0600020D RID: 525 RVA: 0x0000EDA4 File Offset: 0x0000CFA4
    public void ScheduleAdvancedCopyTexture(int sourceTextureIndex, int destinationTextureIndex, int sourceX, int sourceY, int sourceWidth, int sourceHeight, int destinationWidth, int destinationHeight, int destinationX, int destinationY, Color destinationColor, string info)
    {
        RenderQueueController.Operation operation = new RenderQueueController.Operation();
        operation.type = RenderQueueController.Operation.Types.AdvancedCopyTexture;
        operation.sourceIndex = sourceTextureIndex;
        operation.destinationIndex = destinationTextureIndex;
        operation.sourceX = sourceX;
        operation.sourceY = sourceY;
        operation.sourceWidth = sourceWidth;
        operation.sourceHeight = sourceHeight;
        operation.destinationX = destinationX;
        operation.destinationY = destinationY;
        operation.destinationWidth = destinationWidth;
        operation.destinationHeight = destinationHeight;
        operation.destinationColor = destinationColor;
        operation.blitMaterial = this._blitMaterial;
        operation.info = info;
        this._queuedOperations.Add(operation);
        string str = "Scheduled operation: ";
        RenderQueueController.Operation operation2 = operation;
        //this.Log(str + ((operation2 != null) ? operation2.ToString() : null));
    }

    // Token: 0x0600020E RID: 526 RVA: 0x0000EE50 File Offset: 0x0000D050
    public void ScheduleDrawLine(int sourceX, int sourceY, int destinationX, int destinationY, Color destinationColor, int destinationTextureIndex, string info)
    {
        RenderQueueController.Operation operation = new RenderQueueController.Operation();
        operation.type = RenderQueueController.Operation.Types.DrawLine;
        operation.sourceX = sourceX;
        operation.sourceY = sourceY;
        operation.destinationX = destinationX;
        operation.destinationY = destinationY;
        operation.destinationColor = destinationColor;
        operation.destinationIndex = destinationTextureIndex;
        operation.blitMaterial = this._blitMaterial;
        operation.info = info;
        this._queuedOperations.Add(operation);
        string str = "Scheduled operation: ";
        RenderQueueController.Operation operation2 = operation;
        //this.Log(str + ((operation2 != null) ? operation2.ToString() : null));
    }

    // Token: 0x0600020F RID: 527 RVA: 0x0000EED4 File Offset: 0x0000D0D4
    public void ScheduleDrawRect(int sourceX, int sourceY, int destinationX, int destinationY, Color destinationColor, int destinationTextureIndex, string info)
    {
        RenderQueueController.Operation operation = new RenderQueueController.Operation();
        operation.type = RenderQueueController.Operation.Types.DrawRect;
        operation.sourceX = sourceX;
        operation.sourceY = sourceY;
        operation.destinationX = destinationX;
        operation.destinationY = destinationY;
        operation.destinationColor = destinationColor;
        operation.destinationIndex = destinationTextureIndex;
        operation.blitMaterial = this._blitMaterial;
        operation.info = info;
        this._queuedOperations.Add(operation);
        string str = "Scheduled operation: ";
        RenderQueueController.Operation operation2 = operation;
        //this.Log(str + ((operation2 != null) ? operation2.ToString() : null));
    }

    // Token: 0x06000210 RID: 528 RVA: 0x0000EF58 File Offset: 0x0000D158
    public void ScheduleDrawCircle(int sourceX, int sourceY, int radius, Color destinationColor, int destinationTextureIndex, string info)
    {
        RenderQueueController.Operation operation = new RenderQueueController.Operation();
        operation.type = RenderQueueController.Operation.Types.DrawCircle;
        operation.sourceX = sourceX;
        operation.sourceY = sourceY;
        operation.radius = radius;
        operation.destinationColor = destinationColor;
        operation.destinationIndex = destinationTextureIndex;
        operation.blitMaterial = this._blitMaterial;
        operation.info = info;
        this._queuedOperations.Add(operation);
        string str = "Scheduled operation: ";
        RenderQueueController.Operation operation2 = operation;
        //this.Log(str + ((operation2 != null) ? operation2.ToString() : null));
    }

    // Token: 0x06000211 RID: 529 RVA: 0x0000EFD4 File Offset: 0x0000D1D4
    public void ScheduleFillRect(int sourceX, int sourceY, int destinationX, int destinationY, Color destinationColor, int destinationTextureIndex, string info)
    {
        RenderQueueController.Operation operation = new RenderQueueController.Operation();
        operation.type = RenderQueueController.Operation.Types.FillRect;
        operation.sourceX = sourceX;
        operation.sourceY = sourceY;
        operation.destinationX = destinationX;
        operation.destinationY = destinationY;
        operation.destinationColor = destinationColor;
        operation.destinationIndex = destinationTextureIndex;
        operation.blitMaterial = this._blitMaterial;
        operation.info = info;
        this._queuedOperations.Add(operation);
        string str = "Scheduled operation: ";
        RenderQueueController.Operation operation2 = operation;
        //this.Log(str + ((operation2 != null) ? operation2.ToString() : null));
    }

    // Token: 0x06000212 RID: 530 RVA: 0x0000F058 File Offset: 0x0000D258
    public void ScheduleFillCircle(int sourceX, int sourceY, int radius, Color destinationColor, int destinationTextureIndex, string info)
    {
        RenderQueueController.Operation operation = new RenderQueueController.Operation();
        operation.type = RenderQueueController.Operation.Types.FillCircle;
        operation.sourceX = sourceX;
        operation.sourceY = sourceY;
        operation.radius = radius;
        operation.destinationColor = destinationColor;
        operation.destinationIndex = destinationTextureIndex;
        operation.blitMaterial = this._blitMaterial;
        operation.info = info;
        this._queuedOperations.Add(operation);
        string str = "Scheduled operation: ";
        RenderQueueController.Operation operation2 = operation;
        //this.Log(str + ((operation2 != null) ? operation2.ToString() : null));
    }

    // Token: 0x06000213 RID: 531 RVA: 0x0000F0D4 File Offset: 0x0000D2D4
    public void ScheduleDrawPolygon(float[] points, bool contiguous, Color destinationColor, int destinationTextureIndex, string info)
    {
        RenderQueueController.Operation operation = new RenderQueueController.Operation();
        operation.type = RenderQueueController.Operation.Types.DrawPolygon;
        operation.points = points;
        operation.contiguous = contiguous;
        operation.destinationColor = destinationColor;
        operation.destinationIndex = destinationTextureIndex;
        operation.blitMaterial = this._blitMaterial;
        operation.info = info;
        this._queuedOperations.Add(operation);
        string str = "Scheduled operation: ";
        RenderQueueController.Operation operation2 = operation;
        //this.Log(str + ((operation2 != null) ? operation2.ToString() : null));
    }

    // Token: 0x06000214 RID: 532 RVA: 0x0000F148 File Offset: 0x0000D348
    public void ScheduleDrawText(int destinationX, int destinationY, string text, Color destinationColor, int destinationTextureIndex, string info)
    {
        RenderQueueController.Operation operation = new RenderQueueController.Operation();
        operation.type = RenderQueueController.Operation.Types.DrawText;
        operation.destinationX = destinationX;
        operation.destinationY = destinationY;
        operation.text = text;
        operation.destinationColor = destinationColor;
        operation.destinationIndex = destinationTextureIndex;
        operation.fontAtlas = this.fontAtlas;
        operation.fontAtlasColumns = this.fontAtlasColumns;
        operation.fontAtlasRows = this.fontAtlasRows;
        operation.fontAtlasGlyphWidth = this.fontAtlasGlyphWidth;
        operation.fontAtlasGlyphHeight = this.fontAtlasGlyphHeight;
        operation.blitMaterial = this._blitMaterial;
        operation.info = info;
        this._queuedOperations.Add(operation);
        string str = "Scheduled operation: ";
        RenderQueueController.Operation operation2 = operation;
        //this.Log(str + ((operation2 != null) ? operation2.ToString() : null));
    }

    // Token: 0x06000215 RID: 533 RVA: 0x0000F204 File Offset: 0x0000D404
    public void ScheduleExportImage(int sourceTextureIndex, string assetName, string info)
    {
        if (Time.time - this._lastImageExportTime > 10f)
        {
            RenderQueueController.Operation operation = new RenderQueueController.Operation();
            operation.type = RenderQueueController.Operation.Types.ExportImage;
            operation.sourceIndex = sourceTextureIndex;
            operation.text = assetName;
            operation.info = info;
            this._queuedOperations.Add(operation);
            string str = "Scheduled operation: ";
            RenderQueueController.Operation operation2 = operation;
            //this.Log(str + ((operation2 != null) ? operation2.ToString() : null));
            this._lastImageExportTime = Time.time;
        }
    }

    // Token: 0x06000216 RID: 534 RVA: 0x0000F27C File Offset: 0x0000D47C
    public void ProcessOperations()
    {

    }


    // Token: 0x04000255 RID: 597
    public Material blitMaterial;

    // Token: 0x04000256 RID: 598
    public Texture2D fontAtlas;

    // Token: 0x04000257 RID: 599
    public int fontAtlasRows;

    // Token: 0x04000258 RID: 600
    public int fontAtlasColumns;

    // Token: 0x04000259 RID: 601
    public int fontAtlasGlyphWidth;

    // Token: 0x0400025A RID: 602
    public int fontAtlasGlyphHeight;

    // Token: 0x0400025B RID: 603
    public Shader drawLineShader;

    // Token: 0x0400025C RID: 604
    public Shader fillColorShader;

    // Token: 0x0400025D RID: 605
    public Shader drawRectShader;

    // Token: 0x0400025E RID: 606
    public Shader drawCircleShader;

    // Token: 0x0400025F RID: 607
    public Shader fillCircleShader;

    // Token: 0x04000260 RID: 608
    private List<RenderQueueController.Operation> _queuedOperations;

    // Token: 0x04000261 RID: 609
    private Material _blitMaterial;

    // Token: 0x04000262 RID: 610
    private float _lastImageExportTime;

    // Token: 0x04000263 RID: 611
    private const float _imageExportInterval = 10f;

    // Token: 0x04000264 RID: 612
    private static Texture2D temporaryTexture2D;

    // Token: 0x04000265 RID: 613
    private static int totalPixels;

    // Token: 0x04000266 RID: 614
    private static Material drawLineMaterial;

    // Token: 0x04000267 RID: 615
    private static Material fillColorMaterial;

    // Token: 0x04000268 RID: 616
    private static Material drawRectMaterial;

    // Token: 0x04000269 RID: 617
    private static Material drawCircleMaterial;

    // Token: 0x0400026A RID: 618
    private static Material fillCircleMaterial;

    // Token: 0x020002E5 RID: 741
    public class Operation
    {
        // Token: 0x06001E75 RID: 7797 RVA: 0x00095FD4 File Offset: 0x000941D4
        public bool CheckSanity()
        {
            switch (this.type)
            {
                case RenderQueueController.Operation.Types.CameraRender:
                    return this.camera != null && Controllers.assetController.DoesTextureExist(this.sourceIndex);
                case RenderQueueController.Operation.Types.CopyTexture:
                case RenderQueueController.Operation.Types.AdvancedCopyTexture:
                    return Controllers.assetController.DoesTextureExist(this.sourceIndex) && Controllers.assetController.DoesTextureExist(this.destinationIndex) && this.destinationIndex != 0;
                case RenderQueueController.Operation.Types.DrawLine:
                case RenderQueueController.Operation.Types.DrawRect:
                case RenderQueueController.Operation.Types.DrawCircle:
                case RenderQueueController.Operation.Types.FillRect:
                case RenderQueueController.Operation.Types.FillCircle:
                case RenderQueueController.Operation.Types.ClearTexture:
                    return Controllers.assetController.DoesTextureExist(this.destinationIndex) && this.destinationIndex != 0;
                case RenderQueueController.Operation.Types.DrawPolygon:
                    return Controllers.assetController.DoesTextureExist(this.destinationIndex) && this.destinationIndex != 0 && this.points.Length >= 4 && this.points.Length % (this.contiguous ? 2 : 4) == 0;
                case RenderQueueController.Operation.Types.ApplyTexture:
                    return Controllers.assetController.DoesTextureExist(this.destinationIndex) && this.destinationIndex != 0 && this.pixels.Length == RenderQueueController.totalPixels;
                case RenderQueueController.Operation.Types.DrawText:
                    return Controllers.assetController.DoesTextureExist(this.destinationIndex) && this.destinationIndex != 0 && !string.IsNullOrEmpty(this.text);
                case RenderQueueController.Operation.Types.ExportImage:
                    return Controllers.assetController.DoesTextureExist(this.sourceIndex) && !Controllers.assetController.IsUserTexture(this.sourceIndex);
                default:
                    return false;
            }
        }

        // Token: 0x06001E76 RID: 7798 RVA: 0x00096158 File Offset: 0x00094358
        public void Execute()
        {
            switch (this.type)
            {
                case RenderQueueController.Operation.Types.CameraRender:
                    this.camera.targetTexture = (RenderTexture)Controllers.assetController.GetTexture(this.sourceIndex);
                    this.camera.Render();
                    return;
                case RenderQueueController.Operation.Types.CopyTexture:
                    if (this.sourceIndex != this.destinationIndex)
                    {
                        Texture texture = Controllers.assetController.GetTexture(this.sourceIndex);
                        RenderTexture dynamicTexture = Controllers.assetController.GetDynamicTexture(this.destinationIndex);
                        Graphics.CopyTexture(texture, dynamicTexture);
                        return;
                    }
                    break;
                case RenderQueueController.Operation.Types.AdvancedCopyTexture:
                    if (this.sourceIndex != this.destinationIndex)
                    {
                        Texture texture2 = Controllers.assetController.GetTexture(this.sourceIndex);
                        RenderTexture dynamicTexture2 = Controllers.assetController.GetDynamicTexture(this.destinationIndex);
                        this.BlitSpecial(texture2, dynamicTexture2, this.sourceX, this.sourceY, this.sourceWidth, this.sourceHeight, this.destinationX, this.destinationY, this.destinationWidth, this.destinationHeight, this.destinationColor);
                        return;
                    }
                    break;
                case RenderQueueController.Operation.Types.DrawLine:
                case RenderQueueController.Operation.Types.DrawRect:
                case RenderQueueController.Operation.Types.DrawCircle:
                case RenderQueueController.Operation.Types.FillRect:
                case RenderQueueController.Operation.Types.FillCircle:
                case RenderQueueController.Operation.Types.DrawPolygon:
                    this.DrawShape(this.type);
                    return;
                case RenderQueueController.Operation.Types.ClearTexture:
                    {
                        RenderTexture dynamicTexture3 = Controllers.assetController.GetDynamicTexture(this.destinationIndex);
                        Color backgroundColor = this.destinationColor;
                        backgroundColor.a = (float)(this.noColor ? 0 : 1);
                        RenderTexture.active = dynamicTexture3;
                        GL.Clear(false, true, backgroundColor);
                        return;
                    }
                case RenderQueueController.Operation.Types.ApplyTexture:
                    {
                        RenderQueueController.temporaryTexture2D.SetPixels32(this.pixels);
                        RenderQueueController.temporaryTexture2D.Apply();
                        RenderTexture dynamicTexture4 = Controllers.assetController.GetDynamicTexture(this.destinationIndex);
                        Graphics.Blit(RenderQueueController.temporaryTexture2D, dynamicTexture4);
                        return;
                    }
                case RenderQueueController.Operation.Types.DrawText:
                    break;
                case RenderQueueController.Operation.Types.ExportImage:
                    {
                        Texture2D texture2D;
                        if (Controllers.assetController.IsUserTexture(this.sourceIndex))
                        {
                            texture2D = Controllers.assetController.GetUserTexture(this.sourceIndex);
                        }
                        else
                        {
                            RenderTexture.active = Controllers.assetController.GetDynamicTexture(this.sourceIndex);
                            RenderQueueController.temporaryTexture2D.ReadPixels(new Rect(0f, 0f, (float)RenderQueueController.temporaryTexture2D.width, (float)RenderQueueController.temporaryTexture2D.height), 0, 0);
                            texture2D = RenderQueueController.temporaryTexture2D;
                        }
                        //Texture2D texture2D2 = new Texture2D(texture2D.width, texture2D.height, Controllers.assetController.userTextureFormat, new UnityEngine.Experimental.Rendering.TextureCreationFlags());
                        //Graphics.CopyTexture(texture2D, texture2D2);
                        //TextureScale.Bilinear(texture2D2, Controllers.assetController.thumbnailWidth, Controllers.assetController.thumbnailHeight);
                        //GlobalControllers.userAssetsController.ImportImage(this.text, "<FROM RUNTIME>", texture2D.GetRawTextureData(), texture2D2.GetRawTextureData());
                        break;
                    }
                default:
                    return;
            }
        }

        // Token: 0x06001E77 RID: 7799 RVA: 0x000963DC File Offset: 0x000945DC
        private void DrawShape(RenderQueueController.Operation.Types operation)
        {
            RenderTexture.active = Controllers.assetController.GetDynamicTexture(this.destinationIndex);
            switch (operation)
            {
                case RenderQueueController.Operation.Types.DrawLine:
                    this.DrawLine();
                    return;
                case RenderQueueController.Operation.Types.DrawRect:
                    this.DrawRect();
                    return;
                case RenderQueueController.Operation.Types.DrawCircle:
                    this.DrawCircle();
                    return;
                case RenderQueueController.Operation.Types.FillRect:
                    this.FillRect();
                    return;
                case RenderQueueController.Operation.Types.FillCircle:
                    this.FillCircle();
                    return;
                case RenderQueueController.Operation.Types.DrawPolygon:
                    this.DrawPolygon();
                    return;
                default:
                    //Plasma.LogWarning("Draw Operation not supported: " + operation.ToString());
                    return;
            }
        }

        // Token: 0x06001E78 RID: 7800 RVA: 0x00096468 File Offset: 0x00094668
        private void BlitSpecial(Texture sourceTexture, RenderTexture destinationTexture, int sX, int sY, int sWidth, int sHeight, int dX, int dY, int dWidth, int dHeight, Color dColor)
        {
            Vector4 zero = Vector4.zero;
            zero.x = 1f / (float)sourceTexture.width * (float)sX;
            zero.y = 1f / (float)sourceTexture.height * (float)sY;
            zero.z = 1f / (float)sourceTexture.width * (float)sWidth;
            zero.w = 1f / (float)sourceTexture.height * (float)sHeight;
            Vector4 value = Vector3.zero;
            value.x = 1f / (float)destinationTexture.width * (float)dX;
            value.y = 1f / (float)destinationTexture.height * (float)dY;
            value.z = 1f / (float)destinationTexture.width * (float)dWidth;
            value.w = 1f / (float)destinationTexture.height * (float)dHeight;
            //this.blitMaterial.SetVector(GraphicsControllers.vfxController.p_CutXYWH, zero);
            //this.blitMaterial.SetVector(GraphicsControllers.vfxController.p_DestXYWH, value);
            //this.blitMaterial.SetTexture(GraphicsControllers.vfxController.p_SourceTex, sourceTexture);
            //this.blitMaterial.SetColor(GraphicsControllers.vfxController.p_DestColor, dColor);
            Graphics.Blit(sourceTexture, destinationTexture, this.blitMaterial);
        }

        // Token: 0x06001E79 RID: 7801 RVA: 0x000965AC File Offset: 0x000947AC
        private void DrawLine()
        {
            int textureWidth = Controllers.assetController.textureWidth;
            int textureHeight = Controllers.assetController.textureHeight;
            GL.PushMatrix();
            RenderQueueController.drawLineMaterial.SetColor("_Color", this.destinationColor);
            RenderQueueController.drawLineMaterial.SetVector("_Position1", new Vector2((float)this.sourceX, (float)this.sourceY));
            RenderQueueController.drawLineMaterial.SetVector("_Position2", new Vector2((float)this.destinationX, (float)this.destinationY));
            RenderQueueController.drawLineMaterial.SetPass(0);
            GL.LoadOrtho();
            GL.LoadPixelMatrix(0f, (float)textureWidth, 0f, (float)textureHeight);
            GL.Begin(7);
            GL.TexCoord2(0f, (float)textureHeight);
            GL.Vertex3(0f, (float)textureHeight, 0f);
            GL.TexCoord2((float)textureWidth, (float)textureHeight);
            GL.Vertex3((float)textureWidth, (float)textureHeight, 0f);
            GL.TexCoord2((float)textureWidth, 0f);
            GL.Vertex3((float)textureWidth, 0f, 0f);
            GL.TexCoord2(0f, 0f);
            GL.Vertex3(0f, 0f, 0f);
            GL.End();
            GL.PopMatrix();
        }

        // Token: 0x06001E7A RID: 7802 RVA: 0x000966E0 File Offset: 0x000948E0
        private void DrawRect()
        {
            int textureWidth = Controllers.assetController.textureWidth;
            int textureHeight = Controllers.assetController.textureHeight;
            GL.PushMatrix();
            RenderQueueController.drawRectMaterial.SetColor("_Color", this.destinationColor);
            RenderQueueController.drawRectMaterial.SetVector("_Position1", new Vector2((float)this.sourceX, (float)this.sourceY));
            RenderQueueController.drawRectMaterial.SetVector("_Position2", new Vector2((float)this.destinationX, (float)this.destinationY));
            RenderQueueController.drawRectMaterial.SetPass(0);
            GL.LoadOrtho();
            GL.LoadPixelMatrix(0f, (float)textureWidth, 0f, (float)textureHeight);
            GL.Begin(7);
            GL.TexCoord2(0f, (float)textureHeight);
            GL.Vertex3(0f, (float)textureHeight, 0f);
            GL.TexCoord2((float)textureWidth, (float)textureHeight);
            GL.Vertex3((float)textureWidth, (float)textureHeight, 0f);
            GL.TexCoord2((float)textureWidth, 0f);
            GL.Vertex3((float)textureWidth, 0f, 0f);
            GL.TexCoord2(0f, 0f);
            GL.Vertex3(0f, 0f, 0f);
            GL.End();
            GL.PopMatrix();
        }

        // Token: 0x06001E7B RID: 7803 RVA: 0x00096814 File Offset: 0x00094A14
        private void DrawCircle()
        {
            int textureWidth = Controllers.assetController.textureWidth;
            int textureHeight = Controllers.assetController.textureHeight;
            GL.PushMatrix();
            RenderQueueController.drawCircleMaterial.SetColor("_Color", this.destinationColor);
            RenderQueueController.drawCircleMaterial.SetVector("_Center", new Vector2((float)this.sourceX, (float)this.sourceY));
            RenderQueueController.drawCircleMaterial.SetFloat("_Radius", (float)this.radius);
            RenderQueueController.drawCircleMaterial.SetPass(0);
            GL.LoadOrtho();
            GL.LoadPixelMatrix(0f, (float)textureWidth, 0f, (float)textureHeight);
            GL.Begin(7);
            GL.TexCoord2(0f, (float)textureHeight);
            GL.Vertex3(0f, (float)textureHeight, 0f);
            GL.TexCoord2((float)textureWidth, (float)textureHeight);
            GL.Vertex3((float)textureWidth, (float)textureHeight, 0f);
            GL.TexCoord2((float)textureWidth, 0f);
            GL.Vertex3((float)textureWidth, 0f, 0f);
            GL.TexCoord2(0f, 0f);
            GL.Vertex3(0f, 0f, 0f);
            GL.End();
            GL.PopMatrix();
        }

        // Token: 0x06001E7C RID: 7804 RVA: 0x00096938 File Offset: 0x00094B38
        private void FillRect()
        {
            int textureWidth = Controllers.assetController.textureWidth;
            int textureHeight = Controllers.assetController.textureHeight;
            Vector2Int vector2Int = new Vector2Int(this.sourceX, this.sourceY);
            Vector2Int vector2Int2 = new Vector2Int(this.destinationX, this.destinationY);
            int num = Mathf.Clamp(Mathf.Min(vector2Int.x, vector2Int2.x), 0, textureWidth - 1);
            int num2 = Mathf.Clamp(Mathf.Min(vector2Int.y, vector2Int2.y), 0, textureHeight - 1);
            int num3 = Mathf.Clamp(Mathf.Max(vector2Int.x, vector2Int2.x), 0, textureWidth - 1);
            int num4 = Mathf.Clamp(Mathf.Max(vector2Int.y, vector2Int2.y), 0, textureHeight - 1);
            GL.PushMatrix();
            RenderQueueController.fillColorMaterial.SetColor("_Color", this.destinationColor);
            RenderQueueController.fillColorMaterial.SetPass(0);
            GL.LoadOrtho();
            GL.LoadPixelMatrix(0f, (float)textureWidth, 0f, (float)textureHeight);
            GL.Begin(7);
            GL.Vertex3((float)num, (float)num4, 0f);
            GL.Vertex3((float)num3, (float)num4, 0f);
            GL.Vertex3((float)num3, (float)num2, 0f);
            GL.Vertex3((float)num, (float)num2, 0f);
            GL.End();
            GL.PopMatrix();
        }

        // Token: 0x06001E7D RID: 7805 RVA: 0x00096A84 File Offset: 0x00094C84
        private void FillCircle()
        {
            int textureWidth = Controllers.assetController.textureWidth;
            int textureHeight = Controllers.assetController.textureHeight;
            GL.PushMatrix();
            RenderQueueController.fillCircleMaterial.SetColor("_Color", this.destinationColor);
            RenderQueueController.fillCircleMaterial.SetVector("_Center", new Vector2((float)this.sourceX, (float)this.sourceY));
            RenderQueueController.fillCircleMaterial.SetFloat("_Radius", (float)this.radius);
            RenderQueueController.fillCircleMaterial.SetPass(0);
            GL.LoadOrtho();
            GL.LoadPixelMatrix(0f, (float)textureWidth, 0f, (float)textureHeight);
            GL.Begin(7);
            GL.TexCoord2(0f, (float)textureHeight);
            GL.Vertex3(0f, (float)textureHeight, 0f);
            GL.TexCoord2((float)textureWidth, (float)textureHeight);
            GL.Vertex3((float)textureWidth, (float)textureHeight, 0f);
            GL.TexCoord2((float)textureWidth, 0f);
            GL.Vertex3((float)textureWidth, 0f, 0f);
            GL.TexCoord2(0f, 0f);
            GL.Vertex3(0f, 0f, 0f);
            GL.End();
            GL.PopMatrix();
        }

        // Token: 0x06001E7E RID: 7806 RVA: 0x00096BA8 File Offset: 0x00094DA8
        private void DrawPolygon()
        {
            int textureWidth = Controllers.assetController.textureWidth;
            int textureHeight = Controllers.assetController.textureHeight;
            int num = this.contiguous ? (this.points.Length - 2) : this.points.Length;
            int num2 = this.contiguous ? 2 : 4;
            RenderQueueController.drawLineMaterial.SetColor("_Color", this.destinationColor);
            GL.PushMatrix();
            GL.LoadOrtho();
            GL.LoadPixelMatrix(0f, (float)textureWidth, 0f, (float)textureHeight);
            for (int i = 0; i < num; i += num2)
            {
                RenderQueueController.drawLineMaterial.SetVector("_Position1", new Vector2(this.points[i], this.points[i + 1]));
                RenderQueueController.drawLineMaterial.SetVector("_Position2", new Vector2(this.points[i + 2], this.points[i + 3]));
                RenderQueueController.drawLineMaterial.SetPass(0);
                GL.Begin(7);
                GL.TexCoord2(0f, (float)textureHeight);
                GL.Vertex3(0f, (float)textureHeight, 0f);
                GL.TexCoord2((float)textureWidth, (float)textureHeight);
                GL.Vertex3((float)textureWidth, (float)textureHeight, 0f);
                GL.TexCoord2((float)textureWidth, 0f);
                GL.Vertex3((float)textureWidth, 0f, 0f);
                GL.TexCoord2(0f, 0f);
                GL.Vertex3(0f, 0f, 0f);
                GL.End();
            }
            GL.PopMatrix();
        }

        // Token: 0x06001E7F RID: 7807 RVA: 0x00096D2C File Offset: 0x00094F2C
        public override string ToString()
        {
            switch (this.type)
            {
                case RenderQueueController.Operation.Types.CameraRender:
                case RenderQueueController.Operation.Types.ClearTexture:
                case RenderQueueController.Operation.Types.ApplyTexture:
                    return string.Concat(new string[]
                    {
                    this.type.ToString(),
                    " [index: ",
                    this.destinationIndex.ToString(),
                    "] ... ",
                    this.info
                    });
                case RenderQueueController.Operation.Types.CopyTexture:
                    return string.Concat(new string[]
                    {
                    this.type.ToString(),
                    " [from index: ",
                    this.sourceIndex.ToString(),
                    " to index: ",
                    this.destinationIndex.ToString(),
                    "] ... ",
                    this.info
                    });
                case RenderQueueController.Operation.Types.AdvancedCopyTexture:
                    return string.Concat(new string[]
                    {
                    this.type.ToString(),
                    " [from index: ",
                    this.sourceIndex.ToString(),
                    " to index: ",
                    this.destinationIndex.ToString(),
                    " at (",
                    this.sourceX.ToString(),
                    ", ",
                    this.sourceY.ToString(),
                    ") with size (",
                    this.sourceWidth.ToString(),
                    ", ",
                    this.sourceHeight.ToString(),
                    ")] ... ",
                    this.info
                    });
                case RenderQueueController.Operation.Types.DrawLine:
                case RenderQueueController.Operation.Types.DrawRect:
                case RenderQueueController.Operation.Types.FillRect:
                    return string.Concat(new string[]
                    {
                    this.type.ToString(),
                    " [from (",
                    this.sourceX.ToString(),
                    ", ",
                    this.sourceY.ToString(),
                    ") to (",
                    this.destinationX.ToString(),
                    ", ",
                    this.destinationY.ToString(),
                    ")] ... ",
                    this.info
                    });
                case RenderQueueController.Operation.Types.DrawCircle:
                case RenderQueueController.Operation.Types.FillCircle:
                    return string.Concat(new string[]
                    {
                    this.type.ToString(),
                    " [center (",
                    this.sourceX.ToString(),
                    ", ",
                    this.sourceY.ToString(),
                    ") radius (",
                    this.radius.ToString(),
                    ")] ... ",
                    this.info
                    });
                case RenderQueueController.Operation.Types.ExportImage:
                    return string.Concat(new string[]
                    {
                    this.type.ToString(),
                    "[index: ",
                    this.sourceIndex.ToString(),
                    " exported as '",
                    this.text,
                    "']"
                    });
            }
            return "";
        }

        // Token: 0x06001E80 RID: 7808 RVA: 0x00097034 File Offset: 0x00095234
        public bool IsEqualTo(RenderQueueController.Operation other)
        {
            if (other == null)
            {
                return false;
            }
            if (this.type != other.type)
            {
                return false;
            }
            RenderQueueController.Operation.Types types = this.type;
            if (types != RenderQueueController.Operation.Types.CameraRender)
            {
                return types - RenderQueueController.Operation.Types.CopyTexture <= 1 && this.sourceIndex == other.sourceIndex && this.destinationIndex == other.destinationIndex;
            }
            return this.camera == other.camera && this.sourceIndex == other.sourceIndex;
        }

        // Token: 0x0400198C RID: 6540
        public RenderQueueController.Operation.Types type;

        // Token: 0x0400198D RID: 6541
        public int sourceIndex;

        // Token: 0x0400198E RID: 6542
        public int destinationIndex;

        // Token: 0x0400198F RID: 6543
        public int sourceX;

        // Token: 0x04001990 RID: 6544
        public int sourceY;

        // Token: 0x04001991 RID: 6545
        public int sourceWidth;

        // Token: 0x04001992 RID: 6546
        public int sourceHeight;

        // Token: 0x04001993 RID: 6547
        public int destinationX;

        // Token: 0x04001994 RID: 6548
        public int destinationY;

        // Token: 0x04001995 RID: 6549
        public int destinationWidth;

        // Token: 0x04001996 RID: 6550
        public int destinationHeight;

        // Token: 0x04001997 RID: 6551
        public int radius;

        // Token: 0x04001998 RID: 6552
        public float[] points;

        // Token: 0x04001999 RID: 6553
        public Color32[] pixels;

        // Token: 0x0400199A RID: 6554
        public bool contiguous;

        // Token: 0x0400199B RID: 6555
        public Color destinationColor;

        // Token: 0x0400199C RID: 6556
        public string text;

        // Token: 0x0400199D RID: 6557
        public Texture2D fontAtlas;

        // Token: 0x0400199E RID: 6558
        public int fontAtlasRows;

        // Token: 0x0400199F RID: 6559
        public int fontAtlasColumns;

        // Token: 0x040019A0 RID: 6560
        public int fontAtlasGlyphWidth;

        // Token: 0x040019A1 RID: 6561
        public int fontAtlasGlyphHeight;

        // Token: 0x040019A2 RID: 6562
        public bool noColor;

        // Token: 0x040019A3 RID: 6563
        public Material blitMaterial;

        // Token: 0x040019A4 RID: 6564
        public string info;

        // Token: 0x040019A5 RID: 6565
        public Camera camera;

        // Token: 0x020004B9 RID: 1209
        public enum Types
        {
            // Token: 0x040020C3 RID: 8387
            CameraRender,
            // Token: 0x040020C4 RID: 8388
            CopyTexture,
            // Token: 0x040020C5 RID: 8389
            AdvancedCopyTexture,
            // Token: 0x040020C6 RID: 8390
            DrawLine,
            // Token: 0x040020C7 RID: 8391
            DrawRect,
            // Token: 0x040020C8 RID: 8392
            DrawCircle,
            // Token: 0x040020C9 RID: 8393
            FillRect,
            // Token: 0x040020CA RID: 8394
            FillCircle,
            // Token: 0x040020CB RID: 8395
            DrawPolygon,
            // Token: 0x040020CC RID: 8396
            ClearTexture,
            // Token: 0x040020CD RID: 8397
            ApplyTexture,
            // Token: 0x040020CE RID: 8398
            DrawText,
            // Token: 0x040020CF RID: 8399
            ExportImage
        }
    }
}
