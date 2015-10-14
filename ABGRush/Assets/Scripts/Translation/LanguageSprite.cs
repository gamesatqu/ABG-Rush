using UnityEngine;
using System.Collections;


/// <summary>
/// This class is used to allow gameobject sprites to change based on the language being used.
/// </summary>
public class LanguageSprite : MonoBehaviour {

	//public Sprite spriteEnglish = null, spriteSpanish = null;//the sprites that will be used for each language
	//public bool multipleSprites;
	public SpriteRenderer externalSpriteRenderer;
	public Sprite[] spritesEnglish, spritesSpanish;

	private Sprite[] currentLanguageSprites;//the sprites for the language
	private SpriteRenderer sr; // the sprite renderer attached to this object.
	private int currentSprite = 0;//the current sprite

	void Awake()
	{
		if (externalSpriteRenderer)
		{
			sr = externalSpriteRenderer;
		}
		else
		{
			sr = GetComponent<SpriteRenderer>();
		}
		
		if (sr == null)
		{
			Debug.LogWarning("Could not find a sprite renderer");
		}
		currentSprite = 0;

	}


	void OnEnable()
	{
		UpdateLanguageSprites();
		SwapSprite(currentSprite);

	}

	/// <summary>
	/// Determine which sprites will be used based on the language.
	/// </summary>
	private void UpdateLanguageSprites()
	{
		if (spritesEnglish.Length > 0 && spritesSpanish.Length > 0)
		{
			string lang = "English";

			//make sure we have access to language manager
			if (LanguageManager._LanguageManager)
			{
				//Since we do, get the current language from the manager.
				lang = LanguageManager._LanguageManager.Language();
			}

			//set the current sprites based on the language.
			switch (lang)
			{
				case "English": currentLanguageSprites = spritesEnglish; break;
				case "Spanish": currentLanguageSprites = spritesSpanish; break;
			}
		}
	}

	/// <summary>
	/// Swap the sprite being used.
	/// </summary>
	/// <param name="spriteNumber">0,1,2...</param>
	public void SwapSprite(int spriteNumber)
	{
		//make sure I update the language sprite
		UpdateLanguageSprites();

		//make sure that the number is valid.
		if (spriteNumber >= 0 && spriteNumber < (Mathf.Min(spritesEnglish.Length, spritesSpanish.Length)))
		{
			currentSprite = spriteNumber;
		}

		//set the sprite
		if (sr)
		{
			sr.sprite = currentLanguageSprites[currentSprite];
		}
	}

}
