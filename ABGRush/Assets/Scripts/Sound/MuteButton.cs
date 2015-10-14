using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour {

	public Vector2 positionStandalone, positionWebplayer;


	void Start()
	{
		DontDestroyOnLoad(this.gameObject);
		if(Application.isWebPlayer){
			gameObject.GetComponent<RectTransform>().anchoredPosition = positionWebplayer;
		}
		else{
			gameObject.GetComponent<RectTransform>().anchoredPosition = positionStandalone;
		}
		
		gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
	}

	private SoundManager GetSM()
	{
		if (SoundManager._SoundManager)
		{
			Debug.Log("Found Sound Manager.");
			return SoundManager._SoundManager;
		}
		else
		{
			return null;
		}
	}

	public void Mute(Toggle toggle)
	{
		SoundManager sm = GetSM();
		if (sm)
		{
			//when this toggle is true, that means that sounds are on. So reverse what we get.
			sm.Mute(!toggle.isOn);
		}
	}
}
