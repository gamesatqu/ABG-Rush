using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public static SoundManager _SoundManager;
	public SoundPop prefabSoundPop;
	public MuteButton prefabMuteButton;

	[HideInInspector]
	public bool muted;
	public AudioClip acClick, acGameOver, acPatientDiagnosed, acPatientLeave, acUseComputer, acBloodworkComplete, acWashHands;

	private MuteButton mutebutton;

	void OnLevelWasLoaded(int level)
	{
		if (level > 0 && !mutebutton)
		{
			GameObject c = GameObject.Find("Canvas");

			mutebutton = Instantiate(prefabMuteButton);
			mutebutton.transform.SetParent(c.transform);
		}
	}

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(gameObject);
		mutebutton = null;
		
		
		_SoundManager = this;
	}
	


	/// <summary>
	/// Play a sound
	/// </summary>
	/// <param name="sndName">Click, GameOver, PatientDiagnosed, PatientLeave, UseComputer, BloodworkComplete, WashHands</param>
	public void PlaySound(string sndName)
	{
		AudioClip ac = null;
		float volume = 1f;

		switch (sndName)
		{
			case "Click": ac = acClick; break;
			case "GameOver": ac = acGameOver; break;
			case "PatientDiagnosed": ac = acPatientDiagnosed; break;
			case "PatientLeave": ac = acPatientLeave; break;
			case "UseComputer": ac = acUseComputer; break;
			case "BloodworkComplete": ac = acBloodworkComplete; break;
			case "WashHands": ac = acWashHands; break;
		}

		SoundPop pop = Instantiate(prefabSoundPop) as SoundPop;
		pop.transform.SetParent(transform);

		if (muted)
		{
			volume = 0f;
		}
		pop.PlaySound(ac, volume, true);
	}


	/// <summary>
	/// Mute the Game Sounds
	/// </summary>
	/// <param name="mute">true = mute, false = un-mute</param>
	public void Mute(bool mute)
	{
		muted = mute;
	}
}
