using UnityEngine;
using System.Collections;

public class UIQuit : MonoBehaviour {


	public void Quit()
	{
		if (SoundManager._SoundManager)
		{
			SoundManager._SoundManager.PlaySound("Click");
		}

		Time.timeScale = 1f;
		Application.LoadLevel(1);
	}

	public void Cancel()
	{
		if (SoundManager._SoundManager)
		{
			SoundManager._SoundManager.PlaySound("Click");
		}

		Time.timeScale = 1f;
		gameObject.SetActive(false);
	}
}

