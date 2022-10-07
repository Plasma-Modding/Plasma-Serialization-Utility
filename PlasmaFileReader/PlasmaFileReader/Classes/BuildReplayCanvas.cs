using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PlasmaFileReader.Plasma.Classes
{
	// Token: 0x02000007 RID: 7
	public class BuildReplayCanvas : MonoBehaviour
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000019 RID: 25 RVA: 0x00002559 File Offset: 0x00000759
		public Transform dummyCamera
		{
			get
			{
				return this._dummyTransform.transform;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002566 File Offset: 0x00000766
		// (set) Token: 0x0600001B RID: 27 RVA: 0x0000256E File Offset: 0x0000076E
		public Device operatingDevice { get; set; }

		// Token: 0x0600001F RID: 31 RVA: 0x000025BD File Offset: 0x000007BD
		public void ToggleMakeSolid(bool value)
		{
			this._makeSolid = value;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002B62 File Offset: 0x00000D62
		public void OpenSketch(ComponentHandler componentHandler)
		{
			Controllers.worldController.BuildReplayOpenSketch(componentHandler);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002B6F File Offset: 0x00000D6F
		public void OnSketchClose()
		{
			if (this._current != null)
			{
				this._current.OnSketchClose();
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002B8C File Offset: 0x00000D8C
		public BuildReplayCanvas.SerializedData Save()
		{
			BuildReplayCanvas.SerializedData serializedData = new BuildReplayCanvas.SerializedData();
			List<BuildReplayCanvas.SerializedComponent> list = new List<BuildReplayCanvas.SerializedComponent>();
			if (this._components.Count > 0)
			{
				serializedData.deviceGuid = this._components[0].component.device.guid;
				for (int i = 0; i < this._components.Count; i++)
				{
					BuildReplayCanvas.SerializedComponent serializedComponent = new BuildReplayCanvas.SerializedComponent();
					this._components[i].PopulateSerializedItem(serializedComponent);
					list.Add(serializedComponent);
				}
			}
			serializedData.components = list.ToArray();
			serializedData.makeSolid = this._makeSolid;
			serializedData.timerCamera = this.cameraTime;
			serializedData.timerWait = this.waitTime;
			serializedData.timerStep = this.stepChange;
			serializedData.timerDelay = this.delayTime;
			serializedData.overrideKeylightRotation = this._overrideKeylightRotation;
			serializedData.keyLightRotation = this._keylightRotation;
			return serializedData;
		}

		// Token: 0x04000021 RID: 33
		public GameObject componentEventPrefab;

		// Token: 0x04000022 RID: 34
		public Transform content;

		// Token: 0x04000023 RID: 35
		public float stepChange = 0.05f;

		// Token: 0x04000024 RID: 36
		public float cameraTime = 0.5f;

		// Token: 0x04000025 RID: 37
		public float waitTime = 0.2f;

		// Token: 0x04000026 RID: 38
		public float delayTime = 1f;

		// Token: 0x04000027 RID: 39
		private List<BuildReplayComponent> _components;

		// Token: 0x04000028 RID: 40
		private int _cameraMoveIndex;

		// Token: 0x04000029 RID: 41
		private GameObject _dummyTransform;

		// Token: 0x0400002A RID: 42
		private bool _hasSpawned;

		// Token: 0x0400002B RID: 43
		private Camera _camera;

		// Token: 0x0400002C RID: 44
		private bool _cameraSetToggle;

		// Token: 0x0400002D RID: 45
		private bool _makeSolid;

		// Token: 0x0400002E RID: 46
		private int _uniqueGuid;

		// Token: 0x0400002F RID: 47
		private bool _overrideKeylightRotation;

		// Token: 0x04000030 RID: 48
		private Vector3 _keylightRotation;

		// Token: 0x04000031 RID: 49
		private BuildReplayComponent _current;

		// Token: 0x020002CB RID: 715
		public class SerializedData
		{
			// Token: 0x04001901 RID: 6401
			public int deviceGuid;

			// Token: 0x04001902 RID: 6402
			public BuildReplayCanvas.SerializedComponent[] components;

			// Token: 0x04001903 RID: 6403
			public float timerWait;

			// Token: 0x04001904 RID: 6404
			public float timerCamera;

			// Token: 0x04001905 RID: 6405
			public float timerStep;

			// Token: 0x04001906 RID: 6406
			public float timerDelay;

			// Token: 0x04001907 RID: 6407
			public bool makeSolid;

			// Token: 0x04001908 RID: 6408
			public bool overrideKeylightRotation;

			// Token: 0x04001909 RID: 6409
			public Vector3 keyLightRotation;
		}

		// Token: 0x020002CC RID: 716
		public class SerializedComponent
		{
			// Token: 0x06001E34 RID: 7732 RVA: 0x000953CD File Offset: 0x000935CD
			public SerializedComponent()
			{
				this.parentUniqueId = -1;
			}

			// Token: 0x0400190A RID: 6410
			public int uniqueId;

			// Token: 0x0400190B RID: 6411
			public int parentUniqueId;

			// Token: 0x0400190C RID: 6412
			public int[] childrenIndices;

			// Token: 0x0400190D RID: 6413
			public int componentGuid;

			// Token: 0x0400190E RID: 6414
			public Vector3 fromPosition;

			// Token: 0x0400190F RID: 6415
			public Vector3 toPosition;

			// Token: 0x04001910 RID: 6416
			public Quaternion fromRotation;

			// Token: 0x04001911 RID: 6417
			public Quaternion toRotation;

			// Token: 0x04001912 RID: 6418
			public QTransform childReferenceFrame;

			// Token: 0x04001913 RID: 6419
			public QTransform parentReferenceFrame;

			// Token: 0x04001914 RID: 6420
			public int[] mergedComponentIds;

			// Token: 0x04001915 RID: 6421
			public bool openSketch;
		}
	}
}
