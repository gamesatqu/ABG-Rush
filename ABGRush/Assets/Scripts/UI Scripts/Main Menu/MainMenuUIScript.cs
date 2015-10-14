using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuUIScript : MonoBehaviour {

	public GameObject goQuit;

	// Use this for initialization
	void Start () {
		goQuit.SetActive(!Application.isWebPlayer);
		
	}
	
    public void Practice()
    {
        Application.LoadLevel("Practice");

    }

    public void Play()
    {
        Application.LoadLevel("Play");
    }

	public void QuitOut()
	{
		Application.Quit();
	}

	public void SwitchLanguage(string language)
	{
		if (LanguageManager._LanguageManager)
		{
			LanguageManager._LanguageManager.SetLanguage(language);
			Application.LoadLevel(1);
		}
	}

	public void ToggleLanguage()
	{
		LanguageManager lm = LanguageManager._LanguageManager;
		if (lm)
		{
			if (lm.Language() == "English")
			{
				lm.SetLanguage("Spanish");
			}
			else if (lm.Language() == "Spanish")
			{
				lm.SetLanguage("English");
			}

			Application.LoadLevel(1);
		}
	}


	public void SoundClick()
	{
		//Play a sound
		if (SoundManager._SoundManager)
		{
			SoundManager._SoundManager.PlaySound("Click");
		}
	}
}
