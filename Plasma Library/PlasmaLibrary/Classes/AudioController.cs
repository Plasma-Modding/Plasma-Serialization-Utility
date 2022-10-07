using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x02000036 RID: 54
public class AudioController : Controller
{
	// Token: 0x06000152 RID: 338 RVA: 0x00009277 File Offset: 0x00007477
	public override void Init()
	{
		this._masterBus = RuntimeManager.GetBus("Bus:/");
		this.SetMasterVolume(PlayerPrefs.GetFloat("MasterVolume", 0.75f));
	}

	// Token: 0x06000153 RID: 339 RVA: 0x0000929E File Offset: 0x0000749E
	public void SetMasterVolume(float volume)
	{
		this._masterBus.setVolume(volume);
		PlayerPrefs.SetFloat("MasterVolume", volume);
	}

	// Token: 0x06000154 RID: 340 RVA: 0x000092B8 File Offset: 0x000074B8
	public float GetMasterVolume()
	{
		float result;
		this._masterBus.getVolume(out result);
		return result;
	}

	// Token: 0x06000155 RID: 341 RVA: 0x000092D4 File Offset: 0x000074D4
	public void SetMasterMute(bool value)
	{
		if (value && !this._muteSnapshot.isValid())
		{
			this._muteSnapshot = RuntimeManager.CreateInstance(this.muteSnapshot);
			this._muteSnapshot.start();
			return;
		}
		if (!value && this._muteSnapshot.isValid())
		{
			this._muteSnapshot.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			this._muteSnapshot.release();
		}
	}

	// Token: 0x06000156 RID: 342 RVA: 0x00009338 File Offset: 0x00007538
	public void EnableSnapshot(string snapshot)
	{
		this.DisableSnapshot();
		this._currentSnapshot = RuntimeManager.CreateInstance(snapshot);
		this._currentSnapshot.start();
	}

	// Token: 0x06000157 RID: 343 RVA: 0x00009358 File Offset: 0x00007558
	public void DisableSnapshot()
	{
		if (this._currentSnapshot.isValid())
		{
			this._currentSnapshot.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			this._currentSnapshot.release();
		}
	}

	// Token: 0x06000158 RID: 344 RVA: 0x00009380 File Offset: 0x00007580
	public void StopAllSounds()
	{
		this._masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
	}

	// Token: 0x06000159 RID: 345 RVA: 0x00009390 File Offset: 0x00007590
	public EventInstance Play2DSound(string soundEvent)
	{
		EventInstance result = RuntimeManager.CreateInstance(soundEvent);
		result.start();
		result.release();
		return result;
	}

	// Token: 0x0600015A RID: 346 RVA: 0x000093B8 File Offset: 0x000075B8
	public EventInstance Play3DSound(string soundEvent, Vector3 position)
	{
		EventInstance result = RuntimeManager.CreateInstance(soundEvent);
		result.set3DAttributes(position.To3DAttributes());
		result.start();
		result.release();
		return result;
	}

	// Token: 0x0600015B RID: 347 RVA: 0x000093EB File Offset: 0x000075EB
	public void PlayUIHeavyClickSound()
	{
		this.Play2DSound(this.UIHeavyClickSound);
	}

	// Token: 0x0600015C RID: 348 RVA: 0x000093FA File Offset: 0x000075FA
	public void PlayUIHeavyHighlightSound()
	{
		this.Play2DSound(this.UIHeavyHighlightSound);
	}

	// Token: 0x0600015D RID: 349 RVA: 0x00009409 File Offset: 0x00007609
	public void PlayUILightClickSound()
	{
		this.Play2DSound(this.UILightClickSound);
	}

	// Token: 0x0600015E RID: 350 RVA: 0x00009418 File Offset: 0x00007618
	public void PlayUILightHighlightSound()
	{
		this.Play2DSound(this.UILightHighlightSound);
	}

	// Token: 0x0600015F RID: 351 RVA: 0x00009427 File Offset: 0x00007627
	public void PlayUISpecialWindowOpenSound()
	{
		this.Play2DSound(this.UISpecialWindowOpenSound);
	}

	// Token: 0x06000160 RID: 352 RVA: 0x00009436 File Offset: 0x00007636
	public void PlayUISpecialWindowCloseSound()
	{
		this.Play2DSound(this.UISpecialWindowCloseSound);
	}

	// Token: 0x0400016D RID: 365
	[EventRef]
	public string UIHeavyClickSound;

	// Token: 0x0400016E RID: 366
	[EventRef]
	public string UIHeavyHighlightSound;

	// Token: 0x0400016F RID: 367
	[EventRef]
	public string UILightClickSound;

	// Token: 0x04000170 RID: 368
	[EventRef]
	public string UILightHighlightSound;

	// Token: 0x04000171 RID: 369
	[EventRef]
	public string UISpecialWindowOpenSound;

	// Token: 0x04000172 RID: 370
	[EventRef]
	public string UISpecialWindowCloseSound;

	// Token: 0x04000173 RID: 371
	[EventRef]
	public string muteSnapshot;

	// Token: 0x04000174 RID: 372
	public const string soundLibraryPath = "event:/Components/Behaviour/Speaker/3d/";

	// Token: 0x04000175 RID: 373
	public const string soundLibraryPreviewPath = "event:/Components/Behaviour/Speaker/Preview/";

	// Token: 0x04000176 RID: 374
	private Bus _masterBus;

	// Token: 0x04000177 RID: 375
	private EventInstance _muteSnapshot;

	// Token: 0x04000178 RID: 376
	private EventInstance _currentSnapshot;

	// Token: 0x04000179 RID: 377
	private const string _masterBusPath = "Bus:/";

	// Token: 0x0400017A RID: 378
	private const float _defaultMasterVolume = 0.75f;
}
