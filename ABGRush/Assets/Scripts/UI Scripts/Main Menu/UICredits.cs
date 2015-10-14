using UnityEngine;
using System.Collections;

public class UICredits : MonoBehaviour {

	public void Close()
	{
		if (SoundManager._SoundManager)
		{
			SoundManager._SoundManager.PlaySound("Click");
		}

		gameObject.SetActive(false);
	}
}
