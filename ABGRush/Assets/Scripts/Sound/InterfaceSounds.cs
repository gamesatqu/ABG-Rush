using UnityEngine;
using System.Collections;

public class InterfaceSounds : MonoBehaviour {

	public void SoundClick()
	{
		//Play a sound
		if (SoundManager._SoundManager)
		{
			SoundManager._SoundManager.PlaySound("Click");
		}
	}
}
