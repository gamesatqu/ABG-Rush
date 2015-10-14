using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]

public class SoundPop : MonoBehaviour {

	private AudioSource aSource;
	private float waitTime, volume;
	private bool isOneShot, playing;


	void Awake()
	{
		aSource = GetComponent<AudioSource>();
	}

	void start()
	{
		isOneShot = false;
		playing = false;
	}

	void Update()
	{
		if (playing)
		{
			waitTime -= Time.deltaTime;
			if (waitTime <= 0 && isOneShot)
			{
				Destroy(gameObject);
			}
		}
	}

	public void PlaySound(AudioClip ac, float vol = .5f, bool oneshot = true)
	{
		aSource.clip = ac;
		volume = vol;
		aSource.volume = vol;

		if (oneshot)
		{
			isOneShot = true; 
			waitTime = ac.length;
			aSource.PlayOneShot(ac);
			playing = true;
		}
		else
		{
			aSource.Play();
		}
	}

	/// <summary>
	/// Mute this sound
	/// </summary>
	/// <param name="muted">True = Mute, False = Un-Mute</param>
	public void MuteSound(bool muted)
	{
		if (muted)
		{
			aSource.volume = 0;
		}
		else
		{
			aSource.volume = volume;
		}
	}
}
