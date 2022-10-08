using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x02000008 RID: 8
public class BuildReplayComponent : MonoBehaviour
{
	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000030 RID: 48 RVA: 0x00002E4A File Offset: 0x0000104A
	// (set) Token: 0x06000031 RID: 49 RVA: 0x00002E52 File Offset: 0x00001052
	public Vector3 fromPosition { get; set; }

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x06000032 RID: 50 RVA: 0x00002E5B File Offset: 0x0000105B
	// (set) Token: 0x06000033 RID: 51 RVA: 0x00002E63 File Offset: 0x00001063
	public Quaternion fromRotation { get; set; }

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x06000034 RID: 52 RVA: 0x00002E6C File Offset: 0x0000106C
	// (set) Token: 0x06000035 RID: 53 RVA: 0x00002E74 File Offset: 0x00001074
	public Vector3 toPosition { get; set; }

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x06000036 RID: 54 RVA: 0x00002E7D File Offset: 0x0000107D
	// (set) Token: 0x06000037 RID: 55 RVA: 0x00002E85 File Offset: 0x00001085
	public Quaternion toRotation { get; set; }

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x06000038 RID: 56 RVA: 0x00002E8E File Offset: 0x0000108E
	// (set) Token: 0x06000039 RID: 57 RVA: 0x00002E96 File Offset: 0x00001096
	public ComponentHandler component { get; set; }

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x0600003A RID: 58 RVA: 0x00002E9F File Offset: 0x0000109F
	// (set) Token: 0x0600003B RID: 59 RVA: 0x00002EA7 File Offset: 0x000010A7
	public int level { get; set; }

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x0600003C RID: 60 RVA: 0x00002EB0 File Offset: 0x000010B0
	public List<ComponentHandler> mergedComponents
	{
		get
		{
			return this._mergedComponents;
		}
	}

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x0600003D RID: 61 RVA: 0x00002EB8 File Offset: 0x000010B8
	// (set) Token: 0x0600003E RID: 62 RVA: 0x00002EC0 File Offset: 0x000010C0
	public int guid { get; set; }

	// Token: 0x0600003F RID: 63 RVA: 0x00002EC9 File Offset: 0x000010C9
	private void Awake()
	{
		//this._canvas = Require.ComponentInParent<BuildReplayCanvas>(base.gameObject);
		this._children = new List<BuildReplayComponent>();
		this._mergedComponents = new List<ComponentHandler>();
	}

	// Token: 0x06000043 RID: 67 RVA: 0x000030C9 File Offset: 0x000012C9
	public void AddChild(BuildReplayComponent buildReplayComponent)
	{
		this._children.Add(buildReplayComponent);
	}

	// Token: 0x06000044 RID: 68 RVA: 0x000030D7 File Offset: 0x000012D7
	public void MergeChildIntoSelf(BuildReplayComponent buildReplayComponent)
	{
		this._children.Remove(buildReplayComponent);
		this._mergedComponents.Add(buildReplayComponent.component);
	}

	// Token: 0x06000046 RID: 70 RVA: 0x00003168 File Offset: 0x00001368
	public void SetToggleOpenSketch(bool value)
	{
		this._openSketch = value;
	}

	// Token: 0x06000047 RID: 71 RVA: 0x00003171 File Offset: 0x00001371
	public void SetCameraFrom(Vector3 p, Quaternion r)
	{
		this.fromPosition = p;
		this.fromRotation = r;
	}

	// Token: 0x06000048 RID: 72 RVA: 0x00003181 File Offset: 0x00001381
	public void SetCameraTo(Vector3 p, Quaternion r)
	{
		this.toPosition = p;
		this.toRotation = r;
	}

	// Token: 0x0600004A RID: 74 RVA: 0x000031BD File Offset: 0x000013BD
	public void OnSketchClose()
	{
		this._waitingForSketchClose = false;
	}

	// Token: 0x0600004B RID: 75 RVA: 0x000031C8 File Offset: 0x000013C8
	public void SetMeshVisible(bool value)
	{
		this.component.SetRenderGroupVisible(value);
		foreach (ComponentHandler componentHandler in this._mergedComponents)
		{
			if (componentHandler != null)
			{
				componentHandler.SetRenderGroupVisible(value);
			}
		}
	}

	// Token: 0x0600004D RID: 77 RVA: 0x000032A4 File Offset: 0x000014A4
	public void PopulateSerializedItem(BuildReplayCanvas.SerializedComponent serializedComponent)
	{
		serializedComponent.uniqueId = this.guid;
		serializedComponent.componentGuid = this.component.guid;
		if (this._parent != null)
		{
			serializedComponent.parentUniqueId = this._parent.guid;
		}
		serializedComponent.fromPosition = this.fromPosition;
		serializedComponent.fromRotation = this.fromRotation;
		serializedComponent.toPosition = this.toPosition;
		serializedComponent.toRotation = this.toRotation;
		serializedComponent.childReferenceFrame = this._childReferenceFrame;
		serializedComponent.parentReferenceFrame = this._parentReferenceFrame;
		List<int> list = new List<int>();
		foreach (BuildReplayComponent buildReplayComponent in this._children)
		{
			list.Add(buildReplayComponent.guid);
		}
		serializedComponent.childrenIndices = list.ToArray();
		List<int> list2 = new List<int>();
		foreach (ComponentHandler componentHandler in this._mergedComponents)
		{
			if (componentHandler != null)
			{
				list2.Add(componentHandler.guid);
			}
		}
		serializedComponent.mergedComponentIds = list2.ToArray();
		serializedComponent.openSketch = this._openSketch;
	}

	// Token: 0x04000033 RID: 51
	private BuildReplayComponent _parent;

	// Token: 0x04000034 RID: 52
	private List<BuildReplayComponent> _children;

	// Token: 0x04000035 RID: 53
	private List<ComponentHandler> _mergedComponents;

	// Token: 0x04000036 RID: 54
	private BuildReplayCanvas _canvas;

	// Token: 0x04000037 RID: 55
	private SubComponentHandler _parentSubComponent;

	// Token: 0x04000038 RID: 56
	public TMP_Text title;

	// Token: 0x04000039 RID: 57
	private QTransform _parentReferenceFrame;

	// Token: 0x0400003A RID: 58
	private QTransform _childReferenceFrame;

	// Token: 0x0400003B RID: 59
	private int _componentGuid;

	// Token: 0x0400003C RID: 60
	private bool _waitingForSketchClose;

	// Token: 0x0400003D RID: 61
	private bool _openSketch;
}
