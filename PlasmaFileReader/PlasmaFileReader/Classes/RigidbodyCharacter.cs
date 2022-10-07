using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Rewired;
using UnityEngine;
namespace PlasmaFileReader.Plasma.Classes
{
	// Token: 0x0200002F RID: 47
	public class RigidbodyCharacter : MonoBehaviour
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x0000710A File Offset: 0x0000530A
		public Camera camera
		{
			get
			{
				if (this._dummyCamera != null && this.cameraIsTaken)
				{
					return this._dummyCamera;
				}
				return this._camera;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x0000712F File Offset: 0x0000532F
		public Vector3 position
		{
			get
			{
				return this._rigidbody.transform.position;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00007141 File Offset: 0x00005341
		public Vector3 orientation
		{
			get
			{
				return new Vector3(this._cameraPitch, this._cameraYaw, 0f);
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00007159 File Offset: 0x00005359
		public Vector3 velocity
		{
			get
			{
				return this._rigidbody.velocity;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00007166 File Offset: 0x00005366
		public bool cameraIsTaken
		{
			get
			{
				return this._cameraMasterComponentInUse || this._dummyCameraEnabled;
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00007178 File Offset: 0x00005378
		private void Awake()
		{

		}

		// Token: 0x060000EE RID: 238 RVA: 0x0000727B File Offset: 0x0000547B
		private void OnDestroy()
		{
			this._movementSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0000728A File Offset: 0x0000548A
		public void ResetInput()
		{
			this._flyingForce = 0f;
			this._inputVector = Vector3.zero;
			this._crouchPressed = false;
			this._jetpackEnabled = false;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000072B0 File Offset: 0x000054B0
		public void SetInitialPositionAndOrientation(Vector3 p, float pitch, float yaw)
		{
			this._rigidbody.transform.position = p;
			this.UpdateLookAngle(pitch, yaw);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x000072CC File Offset: 0x000054CC
		public void UpdateLookAngle(float pitch, float yaw)
		{
			this._cameraYaw = yaw;
			this._cameraPitch = pitch;
			if (this._isDocked)
			{
				this.container.localRotation = Quaternion.Euler(0f, yaw, 0f);
				this._camera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
				this.mount.transform.localRotation = Quaternion.Euler(pitch, this._activeDockingStation.dockingPoint.rotation.eulerAngles.y + this._cameraYaw, 0f);
				return;
			}
			this.mount.transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00007388 File Offset: 0x00005588
		public void Tick()
		{

		}

		// Token: 0x060000F3 RID: 243 RVA: 0x000076DC File Offset: 0x000058DC
		public bool UpdateMovement(Player input)
		{
			return true;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00007AF3 File Offset: 0x00005CF3
		private float InputDistanceFromTerrain(Player input)
		{
			if (input.GetButton("Crouch"))
			{
				return this.crouchingDistanceFromGround;
			}
			return this.baseDistanceFromGround;
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00007B10 File Offset: 0x00005D10
		private float DistanceFromTerrain()
		{
			return 0;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00007BF0 File Offset: 0x00005DF0
		private void SetMode(RigidbodyCharacter.State state)
		{
			this._state = state;
			switch (this._state)
			{
				case RigidbodyCharacter.State.Normal:
					this._rigidbody.useGravity = false;
					this.secondaryCollider.SetActive(true);
					return;
				case RigidbodyCharacter.State.Jumping:
					this._rigidbody.useGravity = true;
					this.secondaryCollider.SetActive(false);
					return;
				case RigidbodyCharacter.State.Flying:
					this._rigidbody.useGravity = false;
					this.secondaryCollider.SetActive(false);
					return;
				case RigidbodyCharacter.State.Ghost:
					this._rigidbody.useGravity = false;
					this.secondaryCollider.SetActive(false);
					return;
				default:
					return;
			}
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00007C85 File Offset: 0x00005E85
		public void EnterGhostMode()
		{
			this._gravityGun.EndGravityGun(false);
			this._primaryCollider.gameObject.layer = LayerMask.NameToLayer("Water");
			this.SetMode(RigidbodyCharacter.State.Ghost);
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00007CB4 File Offset: 0x00005EB4
		public void ExitGhostMode()
		{
			this._primaryCollider.gameObject.layer = LayerMask.NameToLayer("CharacterController");
			this.SetMode(RigidbodyCharacter.State.Normal);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00007E5E File Offset: 0x0000605E
		public void Teleport(Vector3 p)
		{
			this.MoveCharacterTo(p);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00007E68 File Offset: 0x00006068
		public void PopUpForUnmount()
		{
			Vector3 position = this.position;
			position.y += 1f;
			this.MoveCharacterTo(position);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00007E94 File Offset: 0x00006094
		public void Undock()
		{
			Vector3 pos = this.container.parent.position + new Vector3(0f, 1.2f, 0f);
			this._rigidbody.isKinematic = false;
			this.container.SetParent(this.interpolatedObject.transform, false);
			this.container.localRotation = Quaternion.identity;
			this._camera.transform.localRotation = Quaternion.identity;
			this._primaryCollider.enabled = true;
			this.secondaryCollider.SetActive(true);
			this._isDocked = false;
			this.MoveCharacterTo(pos);
			this.interpolatedObject.OverrideTransforms();
			this.interpolatedObject.Interpolate(0f);
			this._movementSoundInstance.setPaused(false);
			this._activeDockingStation = null;
			this.SetToTrigger(true);
			//Controllers.audioController.Play2DSound(this.undockingSound);
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00007F80 File Offset: 0x00006180
		private void SetToTrigger(bool value)
		{
			this._primaryCollider.isTrigger = value;
			this.secondaryCollider.GetComponent<Collider>().isTrigger = value;
			this._collidersAreTrigger = value;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00007FA6 File Offset: 0x000061A6
		private void MoveCharacterTo(Vector3 pos)
		{
			this._rigidbody.transform.position = pos;
			this._pidController.Reset();
			this._rigidbody.velocity = Vector3.zero;
			this._rigidbody.angularVelocity = Vector3.zero;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00007FE4 File Offset: 0x000061E4
		public void TriggerPenetration()
		{
			this._triggerPenetration = true;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00007FF0 File Offset: 0x000061F0
		public void TakeCamera(Transform t, Transform dummyAnchor = null)
		{
			if (t != null)
			{
				this._camera.transform.SetParent(t, false);
				this._camera.transform.localPosition = Vector3.zero;
				this._camera.transform.localRotation = Quaternion.identity;
				this._cameraMasterComponentInUse = true;
			}
			if (this._dummyCamera != null)
			{
				if (dummyAnchor != null)
				{
					this._dummyCamera.transform.SetParent(dummyAnchor, false);
				}
				else
				{
					this._dummyCamera.transform.SetParent(this.container, false);
				}
				this._dummyCameraEnabled = true;
				this._dummyCamera.transform.localPosition = Vector3.zero;
				this._dummyCamera.transform.localRotation = Quaternion.identity;
			}
			this.characterShadowGameObject.SetActive(false);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x000080C8 File Offset: 0x000062C8
		public void ReleaseCamera(bool useDummyCamera = false)
		{
			if (this._dummyCamera != null)
			{
				this._dummyCamera.transform.SetParent(this._camera.transform, false);
				this._dummyCamera.transform.localPosition = Vector3.zero;
				this._dummyCamera.transform.localRotation = Quaternion.identity;
				this._dummyCameraEnabled = false;
			}
			if (!useDummyCamera)
			{
				this._camera.transform.SetParent(this.container, false);
				this._camera.transform.localPosition = Vector3.zero;
				this._camera.transform.localRotation = Quaternion.identity;
				this._cameraMasterComponentInUse = false;
			}
			if (!this._dummyCameraEnabled && !this._cameraMasterComponentInUse)
			{
				this.characterShadowGameObject.SetActive(true);
			}
		}

		// Token: 0x06000109 RID: 265 RVA: 0x000081B7 File Offset: 0x000063B7
		public void StartGravityGun(Camera theCamera, Rigidbody body)
		{
			this._gravityGun.StartGravityGun(theCamera, body);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000081C6 File Offset: 0x000063C6
		public void StartGravityGunSound()
		{
			this._gravityGun.StartSound();
		}

		// Token: 0x0600010B RID: 267 RVA: 0x000081D3 File Offset: 0x000063D3
		public void StopGravityGunSound()
		{
			this._gravityGun.StopSound();
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000081E0 File Offset: 0x000063E0
		public void UpdateGravityGunEmitterPosition(Vector3 p)
		{
			this._gravityGun.UpdateGravityGunEmitterPosition(p);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x000081EE File Offset: 0x000063EE
		public void UpdateGravityGunTargetPosition(Quaternion cameraViewOffset, float cameraDistance)
		{
			this._gravityGun.UpdateGravityGunTargetPosition(cameraViewOffset, cameraDistance);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x000081FD File Offset: 0x000063FD
		public void EndGravityGun(bool throwObject = false)
		{
			this._gravityGun.EndGravityGun(throwObject);
			this._gravityGunOnDevice = null;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00008212 File Offset: 0x00006412
		public void DeviceBeingTeleported(Device device)
		{
			this._gravityGun.DeviceBeingTeleported(device);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00008220 File Offset: 0x00006420
		public void StoreInterpolationState()
		{
			if (!this._isDocked)
			{
				this.interpolatedObject.StoreState();
			}
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00008235 File Offset: 0x00006435
		public void Interpolate(float delta)
		{
			if (!this._isDocked)
			{
				//this.interpolatedObject.Interpolate(delta);
			}
		}

		// Token: 0x04000115 RID: 277
		public float movementSpeed;

		// Token: 0x04000116 RID: 278
		public float jumpForce;

		// Token: 0x04000117 RID: 279
		public float continuousJumpForce;

		// Token: 0x04000118 RID: 280
		public float verticalFlyingSpeed;

		// Token: 0x04000119 RID: 281
		public float horizontalFlyingSpeed;

		// Token: 0x0400011A RID: 282
		public float runningMultiplier;

		// Token: 0x0400011B RID: 283
		public GameObject mount;

		// Token: 0x0400011C RID: 284
		public GameObject gravityGunGameObject;

		// Token: 0x0400011D RID: 285
		public GameObject characterShadowGameObject;

		// Token: 0x0400011E RID: 286
		public dynamic interpolatedObject;

		// Token: 0x0400011F RID: 287
		public Transform container;

		// Token: 0x04000120 RID: 288
		public float dockingDuration;

		// Token: 0x04000121 RID: 289
		public float baseDistanceFromGround;

		// Token: 0x04000122 RID: 290
		public float crouchingDistanceFromGround;

		// Token: 0x04000123 RID: 291
		public float drag;

		// Token: 0x04000124 RID: 292
		public float pidRatio;

		// Token: 0x04000125 RID: 293
		public float raycastRadius;

		// Token: 0x04000126 RID: 294
		public GameObject secondaryCollider;

		// Token: 0x04000127 RID: 295
		public Transform deviceMountPoint;

		// Token: 0x04000128 RID: 296
		public float movementAccelerationStart;

		// Token: 0x04000129 RID: 297
		public float movementAccelerationFactor;

		// Token: 0x0400012A RID: 298
		[EventRef]
		public string movementSound;

		// Token: 0x0400012B RID: 299
		[EventRef]
		public string dockingSound;

		// Token: 0x0400012C RID: 300
		[EventRef]
		public string undockingSound;

		// Token: 0x0400012D RID: 301
		private Rigidbody _rigidbody;

		// Token: 0x0400012E RID: 302
		private Camera _camera;

		// Token: 0x0400012F RID: 303
		private Camera _dummyCamera;

		// Token: 0x04000130 RID: 304
		private float _cameraPitch;

		// Token: 0x04000131 RID: 305
		private float _cameraYaw;

		// Token: 0x04000132 RID: 306
		private RigidbodyCharacter.State _state;

		// Token: 0x04000133 RID: 307
		private bool _running;

		// Token: 0x04000134 RID: 308
		private List<ComponentHandler> _insideStructures;

		// Token: 0x04000135 RID: 309
		private bool _isDocked;

		// Token: 0x04000136 RID: 310
		private float _desiredDistanceFromGround;

		// Token: 0x04000137 RID: 311
		private bool _jetpackEnabled;

		// Token: 0x04000138 RID: 312
		private dynamic _pidController;

		// Token: 0x04000139 RID: 313
		private float _flyingForce;

		// Token: 0x0400013A RID: 314
		private bool _needsToJump;

		// Token: 0x0400013B RID: 315
		private Vector3 _inputVector;

		// Token: 0x0400013C RID: 316
		private bool _stayInJump;

		// Token: 0x0400013D RID: 317
		private dynamic _gravityGun;

		// Token: 0x0400013E RID: 318
		private Device _gravityGunOnDevice;

		// Token: 0x0400013F RID: 319
		private bool _crouchPressed;

		// Token: 0x04000140 RID: 320
		private Collider _primaryCollider;

		// Token: 0x04000141 RID: 321
		private ComponentHandler _activeDockingStation;

		// Token: 0x04000142 RID: 322
		private float _movementAccumulator;

		// Token: 0x04000143 RID: 323
		private float _maxSpeed;

		// Token: 0x04000144 RID: 324
		private EventInstance _movementSoundInstance;

		// Token: 0x04000145 RID: 325
		private bool _collidersAreTrigger;

		// Token: 0x04000146 RID: 326
		private bool _triggerPenetration;

		// Token: 0x04000147 RID: 327
		private bool _cameraMasterComponentInUse;

		// Token: 0x04000148 RID: 328
		private bool _dummyCameraEnabled;

		// Token: 0x020002D4 RID: 724
		private enum State
		{
			// Token: 0x04001944 RID: 6468
			Normal,
			// Token: 0x04001945 RID: 6469
			Jumping,
			// Token: 0x04001946 RID: 6470
			Flying,
			// Token: 0x04001947 RID: 6471
			Ghost
		}
	}
}
