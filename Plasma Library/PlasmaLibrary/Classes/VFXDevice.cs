using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plasma.Classes
{
	// Token: 0x02000194 RID: 404
	public class VFXDevice : MonoBehaviour
	{
		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000F02 RID: 3842 RVA: 0x0004D160 File Offset: 0x0004B360
		public IEnumerable<MeshRenderer> allMeshRenderers
		{
			get
			{
				return this._cachedMeshRenderers;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000F03 RID: 3843 RVA: 0x0004D168 File Offset: 0x0004B368
		public Bounds bounds
		{
			get
			{
				return this._cachedBounds;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000F04 RID: 3844 RVA: 0x0004D170 File Offset: 0x0004B370
		// (set) Token: 0x06000F05 RID: 3845 RVA: 0x0004D178 File Offset: 0x0004B378
		public VFXDevice.Commands command { get; set; }

		// Token: 0x06000F06 RID: 3846 RVA: 0x0004D181 File Offset: 0x0004B381
		private void Awake()
		{
			//this._device = Require.ComponentInParent<Device>(this);
			this._cachedChildren = new List<VFXComponent>();
			this._cachedMeshRenderers = new List<MeshRenderer>();
			this.SetupFSM();
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x0004D1AB File Offset: 0x0004B3AB
		private void Start()
		{
			//Controllers.easyFSMController.StartStateMachineInstance(this._stateMachine);
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x0004D1C0 File Offset: 0x0004B3C0
		private void SetupFSM()
		{
			EasyState easyState = new EasyState("Idle");
			EasyState easyState2 = new EasyState("Wireframe");
			easyState2.AddInstantTask(delegate
			{
				this._forceRefreshBounds = false;
			}, EasyState.Stages.OnUpdate);
			EasyState easyState3 = new EasyState("Solid");
			easyState3.AddInstantTask(delegate
			{
				this._forceRefreshBounds = false;
			}, EasyState.Stages.OnUpdate);
			EasyState easyState4 = new EasyState("Transparent");
			EasyState easyState5 = new EasyState("To Wireframe");
			easyState5.AddInstantTask(delegate
			{
				this.command = VFXDevice.Commands.None;
				this.UpdatePositionAndRadius(false, true);
			}, EasyState.Stages.OnUpdate);
			EasyState easyState6 = new EasyState("To Wireframe Animated");
			easyState6.AddInstantTask(delegate
			{
				this.command = VFXDevice.Commands.None;
				this._forceRefreshBounds = true;
				this.AnimateToWireframe();
			}, EasyState.Stages.OnUpdate);
			easyState6.AddCoroutineTask(new Func<IEnumerator>(this.UpdateBoundsAndSphere), EasyState.Stages.OnUpdate);
			EasyState easyState7 = new EasyState("To Solid");
			easyState7.AddInstantTask(delegate
			{
				this.command = VFXDevice.Commands.None;
				this.UpdatePositionAndRadius(true, true);
			}, EasyState.Stages.OnUpdate);
			EasyState easyState8 = new EasyState("To Solid Animated");
			easyState8.AddInstantTask(delegate
			{
				this.command = VFXDevice.Commands.None;
				this._forceRefreshBounds = true;
				this.AnimateToSolid();
			}, EasyState.Stages.OnUpdate);
			easyState8.AddCoroutineTask(new Func<IEnumerator>(this.UpdateBoundsAndSphere), EasyState.Stages.OnUpdate);
			EasyState easyState9 = new EasyState("Update Wireframe");
			easyState9.AddInstantTask(delegate
			{
				this.command = VFXDevice.Commands.None;
				this._forceRefreshBounds = true;
			}, EasyState.Stages.OnUpdate);
			EasyState easyState10 = new EasyState("Update Solid");
			easyState10.AddInstantTask(delegate
			{
				this.command = VFXDevice.Commands.None;
				this._forceRefreshBounds = true;
				this.UpdatePositionAndRadius(true, false);
			}, EasyState.Stages.OnUpdate);
			EasyState easyState11 = new EasyState("To Transparent");
			easyState11.AddInstantTask(delegate
			{
				this.command = VFXDevice.Commands.None;
				this.MakeTransparent(true);
			}, EasyState.Stages.OnUpdate);
			EasyState easyState12 = new EasyState("Exit Transparent");
			easyState12.AddInstantTask(delegate
			{
				this.MakeTransparent(false);
			}, EasyState.Stages.OnUpdate);
			EasyState easyState13 = new EasyState("Turn Off Transparency and Update");
			easyState13.AddInstantTask(delegate
			{
				this.command = VFXDevice.Commands.None;
				this._forceRefreshBounds = true;
				this.UpdatePositionAndRadius(false, false);
				this.MakeTransparent(false);
			}, EasyState.Stages.OnUpdate);
			EasyState easyState14 = new EasyState("Turn On Transparency and Update");
			easyState14.AddInstantTask(delegate
			{
				this.command = VFXDevice.Commands.None;
				this._forceRefreshBounds = true;
				this.UpdatePositionAndRadius(false, false);
				this.MakeTransparent(true);
			}, EasyState.Stages.OnUpdate);
			EasyTransition easyTransition = new EasyTransition(easyState13);
			easyTransition.AddCondition(() => this.command == VFXDevice.Commands.GoToWireframe);
			easyState.AddTransition(easyTransition);
			EasyTransition easyTransition2 = new EasyTransition(easyState6);
			easyTransition2.AddCondition(() => this.command == VFXDevice.Commands.GoToWireframeAnimated);
			easyState.AddTransition(easyTransition2);
			EasyTransition easyTransition3 = new EasyTransition(easyState7);
			easyTransition3.AddCondition(() => this.command == VFXDevice.Commands.GoToSolid);
			easyState.AddTransition(easyTransition3);
			EasyTransition easyTransition4 = new EasyTransition(easyState8);
			easyTransition4.AddCondition(() => this.command == VFXDevice.Commands.GoToSolidAnimated);
			easyState.AddTransition(easyTransition4);
			EasyTransition easyTransition5 = new EasyTransition(easyState14);
			easyTransition5.AddCondition(() => this.command == VFXDevice.Commands.GoToTransparent);
			easyState.AddTransition(easyTransition5);
			EasyTransition easyTransition6 = new EasyTransition(easyState8);
			easyTransition6.AddCondition(() => this.command == VFXDevice.Commands.GoToSolidAnimated);
			easyState2.AddTransition(easyTransition6);
			EasyTransition easyTransition7 = new EasyTransition(easyState7);
			easyTransition7.AddCondition(() => this.command == VFXDevice.Commands.GoToSolid);
			easyState2.AddTransition(easyTransition7);
			EasyTransition easyTransition8 = new EasyTransition(easyState9);
			easyTransition8.AddCondition(() => this.command == VFXDevice.Commands.Update);
			easyState2.AddTransition(easyTransition8);
			EasyTransition easyTransition9 = new EasyTransition(easyState11);
			easyTransition9.AddCondition(() => this.command == VFXDevice.Commands.GoToTransparent);
			easyState2.AddTransition(easyTransition9);
			EasyTransition easyTransition10 = new EasyTransition(easyState13);
			easyTransition10.AddCondition(() => this.command == VFXDevice.Commands.TurnOffTransparencyAndUpdate);
			easyState2.AddTransition(easyTransition10);
			EasyTransition easyTransition11 = new EasyTransition(easyState6);
			easyTransition11.AddCondition(() => this.command == VFXDevice.Commands.GoToWireframeAnimated);
			easyState3.AddTransition(easyTransition11);
			EasyTransition easyTransition12 = new EasyTransition(easyState5);
			easyTransition12.AddCondition(() => this.command == VFXDevice.Commands.GoToWireframe);
			easyState3.AddTransition(easyTransition12);
			EasyTransition easyTransition13 = new EasyTransition(easyState10);
			easyTransition13.AddCondition(() => this.command == VFXDevice.Commands.Update);
			easyState3.AddTransition(easyTransition13);
			EasyTransition transition = new EasyTransition(easyState2);
			easyState5.AddTransition(transition);
			EasyTransition easyTransition14 = new EasyTransition(easyState2);
			easyTransition14.AddCondition(() => this._toWireframeAnimatedDone);
			easyState6.AddTransition(easyTransition14);
			EasyTransition easyTransition15 = new EasyTransition(easyState8);
			easyTransition15.AddCondition(() => this.command == VFXDevice.Commands.GoToSolidAnimated);
			easyState6.AddTransition(easyTransition15);
			EasyTransition easyTransition16 = new EasyTransition(easyState7);
			easyTransition16.AddCondition(() => this.command == VFXDevice.Commands.GoToSolid);
			easyState6.AddTransition(easyTransition16);
			EasyTransition transition2 = new EasyTransition(easyState3);
			easyState7.AddTransition(transition2);
			EasyTransition easyTransition17 = new EasyTransition(easyState3);
			easyTransition17.AddCondition(() => this._toSolidAnimatedDone);
			easyState8.AddTransition(easyTransition17);
			EasyTransition easyTransition18 = new EasyTransition(easyState6);
			easyTransition18.AddCondition(() => this.command == VFXDevice.Commands.GoToWireframeAnimated);
			easyState8.AddTransition(easyTransition18);
			EasyTransition easyTransition19 = new EasyTransition(easyState5);
			easyTransition19.AddCondition(() => this.command == VFXDevice.Commands.GoToWireframe);
			easyState8.AddTransition(easyTransition19);
			EasyTransition transition3 = new EasyTransition(easyState2);
			easyState9.AddTransition(transition3);
			EasyTransition transition4 = new EasyTransition(easyState3);
			easyState10.AddTransition(transition4);
			EasyTransition transition5 = new EasyTransition(easyState4);
			easyState11.AddTransition(transition5);
			EasyTransition easyTransition20 = new EasyTransition(easyState12);
			easyTransition20.AddCondition(() => this.command == VFXDevice.Commands.GoToWireframe);
			easyState4.AddTransition(easyTransition20);
			EasyTransition easyTransition21 = new EasyTransition(easyState5);
			easyTransition21.AddCondition(() => this.command == VFXDevice.Commands.GoToWireframe || this.command == VFXDevice.Commands.Update);
			easyState12.AddTransition(easyTransition21);
			EasyTransition easyTransition22 = new EasyTransition(easyState11);
			easyTransition22.AddCondition(() => this.command == VFXDevice.Commands.GoToTransparent);
			easyState12.AddTransition(easyTransition22);
			EasyTransition transition6 = new EasyTransition(easyState2);
			easyState13.AddTransition(transition6);
			EasyTransition transition7 = new EasyTransition(easyState4);
			easyState14.AddTransition(transition7);
			//Controllers.easyFSMController.CreateStateMachineInstance(EasyFSMEnum.VFXDevice, out this._stateMachine).AddState(easyState).AddState(easyState2).AddState(easyState3).AddState(easyState5).AddState(easyState6).AddState(easyState7).AddState(easyState8).AddState(easyState9).AddState(easyState10).AddState(easyState11).AddState(easyState4).AddState(easyState12).AddState(easyState13).AddState(easyState14).SetStartState(easyState);
			this.command = VFXDevice.Commands.None;
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x0004D799 File Offset: 0x0004B999
		private IEnumerator UpdateBoundsAndSphere()
		{
			for (; ; )
			{
				this.RefreshBounds(false);
				Vector3 center = this._cachedBounds.center;
				foreach (VFXComponent vfxcomponent in this._cachedChildren)
				{
					//vfxcomponent.SetSpherePosition(center);
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x0004D7A8 File Offset: 0x0004B9A8
		private void RefreshCache()
		{
			if (this._device.dirtyHierarchy)
			{
				this._device.dirtyHierarchy = false;
				this._cachedChildren.Clear();
				this._cachedMeshRenderers.Clear();
				this._cachedChildren.AddRange(this._device.rootComponent.GetVFXChildren(true, true, true));
				foreach (VFXComponent vfxcomponent in this._cachedChildren)
				{
					this._cachedMeshRenderers.AddRange(vfxcomponent.meshRenderers);
				}
			}
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x0004D858 File Offset: 0x0004BA58
		private void RefreshBounds(bool force = false)
		{
			if (this._device.dirtyBounds || this._forceRefreshBounds || force)
			{
				this._device.dirtyBounds = false;
				//this._cachedBounds = VFXController.CalculateObjectBounds(this._cachedMeshRenderers);
			}
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x0004D88F File Offset: 0x0004BA8F
		public void ForceRefreshBounds()
		{
			this.RefreshCache();
			this.RefreshBounds(true);
		}

		// Token: 0x06000F0D RID: 3853 RVA: 0x0004D8A0 File Offset: 0x0004BAA0
		private void AnimateToWireframe()
		{

		}

		// Token: 0x06000F0E RID: 3854 RVA: 0x0004D97C File Offset: 0x0004BB7C
		private void UpdatePositionAndRadius(bool isSolid, bool updateSpecialMaterials)
		{
			
		}

		// Token: 0x06000F0F RID: 3855 RVA: 0x0004DA08 File Offset: 0x0004BC08
		public void CompleteTransition()
		{
			foreach (VFXComponent vfxcomponent in this._cachedChildren)
			{
				//vfxcomponent.CompleteTransition();
			}
		}

		// Token: 0x06000F10 RID: 3856 RVA: 0x0004DA58 File Offset: 0x0004BC58
		public void ForceUpdateWireframe()
		{
			this._forceRefreshBounds = true;
			this.UpdatePositionAndRadius(false, false);
			this._forceRefreshBounds = false;
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x0004DA70 File Offset: 0x0004BC70
		private void AnimateToSolid()
		{

		}

		// Token: 0x06000F12 RID: 3858 RVA: 0x0004DB3C File Offset: 0x0004BD3C
		public void MakeTransparent(bool value)
		{
			foreach (VFXComponent vfxcomponent in this._cachedChildren)
			{
				//vfxcomponent.SetTransparent(value);
			}
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x0004DB90 File Offset: 0x0004BD90
		public void SetWireframeColorGrabbing(bool grabbing)
		{
			foreach (VFXComponent vfxcomponent in this._cachedChildren)
			{
				//vfxcomponent.SetWireframeColorGrabbing(grabbing);
			}
		}

		// Token: 0x06000F14 RID: 3860 RVA: 0x0004DBE4 File Offset: 0x0004BDE4
		public void ShowFeedbackEffect()
		{
			foreach (VFXComponent vfxcomponent in this._cachedChildren)
			{
				//vfxcomponent.RunFeedbackFX();
			}
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x0004DC34 File Offset: 0x0004BE34
		public void CleanUp()
		{
			//Controllers.easyFSMController.DeleteStateMachineInstance(this._stateMachine);
		}

		// Token: 0x06000F16 RID: 3862 RVA: 0x0004DC48 File Offset: 0x0004BE48
		private void OnDrawGizmos()
		{
			//Gizmos.color = Color.blue;
			//Gizmos.DrawWireCube(this._cachedBounds.center, this._cachedBounds.size);
			//Gizmos.DrawWireSphere(this._cachedBounds.center, this._cachedBounds.CalculateSphereRadius() * 0.5f);
		}

		// Token: 0x04000CB1 RID: 3249
		private Device _device;

		// Token: 0x04000CB2 RID: 3250
		private List<VFXComponent> _cachedChildren;

		// Token: 0x04000CB3 RID: 3251
		private List<MeshRenderer> _cachedMeshRenderers;

		// Token: 0x04000CB4 RID: 3252
		private Bounds _cachedBounds;

		// Token: 0x04000CB5 RID: 3253
		private int _stateMachine;

		// Token: 0x04000CB6 RID: 3254
		private float _currentRadius;

		// Token: 0x04000CB7 RID: 3255
		private dynamic _transitionTween;

		// Token: 0x04000CB8 RID: 3256
		private bool _toWireframeAnimatedDone;

		// Token: 0x04000CB9 RID: 3257
		private bool _toSolidAnimatedDone;

		// Token: 0x04000CBA RID: 3258
		private bool _forceRefreshBounds;

		// Token: 0x020003CA RID: 970
		public enum Commands
		{
			// Token: 0x04001D55 RID: 7509
			None,
			// Token: 0x04001D56 RID: 7510
			GoToWireframe,
			// Token: 0x04001D57 RID: 7511
			GoToWireframeAnimated,
			// Token: 0x04001D58 RID: 7512
			GoToSolid,
			// Token: 0x04001D59 RID: 7513
			GoToSolidAnimated,
			// Token: 0x04001D5A RID: 7514
			GoToTransparent,
			// Token: 0x04001D5B RID: 7515
			Update,
			// Token: 0x04001D5C RID: 7516
			TurnOffTransparencyAndUpdate
		}
	}
}
