using UnityEngine;
using System.Collections;
using UnityEngine.UI;


/// <summary>
/// This class is used to allow UI sprites to change based on the language being used.
/// </summary>
public class LanguageUIImage : MonoBehaviour {

	public Sprite spriteEnglish = null, spriteSpanish = null;//the sprites that will be used for each language
	public bool referenceTopic;
	
	private Image image;// the image component attached to this object
	void Awake()
	{
		image = GetComponent<Image>();
		if (image == null)
		{
			Debug.LogWarning("Could not find a sprite renderer");
		}
	}





	void OnEnable()
	{
		if (!image)
		{
			image = GetComponent<Image>();
		}
		if (!referenceTopic)
		{
			UpdateImage();
		}
		
	}

	private void UpdateImage()
	{
		//make sure we have access to language manager
		if (LanguageManager._LanguageManager)
		{
			if (image && spriteEnglish && spriteSpanish)
			{
				string lang = LanguageManager._LanguageManager.Language();
				Sprite s = null;

				switch (lang)
				{
					case "English": s = spriteEnglish; break;
					case "Spanish": s = spriteSpanish; break;
				}

				if (s != null)
				{
					image.sprite = s;
				}
			}
		}
	}

	/// <summary>
	/// The Image that will be displayed
	/// </summary>
	/// <returns>Image for the current language</returns>
	public Sprite CurrentImage()
	{
		Sprite s = null;
		//make sure we have access to language manager
		if (LanguageManager._LanguageManager)
		{
			if (spriteEnglish && spriteSpanish)
			{
				string lang = LanguageManager._LanguageManager.Language();
				Debug.Log(lang);

				switch (lang)
				{
					case "English": s = spriteEnglish; break;
					case "Spanish": s = spriteSpanish; break;
				}

				
			}
		}
		if (s != null)
		{
			return s;
		}
		else
		{
			return spriteEnglish;
		}
	}
}
