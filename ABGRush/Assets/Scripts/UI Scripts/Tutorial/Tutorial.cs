using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

	public GameObject btnLeft, btnRight;//the nav button game objects.
	public Text textfieldTutorialContent, textfieldBtnLeft, textfieldBtnRight;//the text fields for the nav buttons.
	public Image imageTutorial;
	public Sprite[] spritesTutorial;

	//the following are ID's that will be used to locate the content within the xml doc.
	public int xmlTutorialTextStartID, xmlButtonIDNext, xmlButtonIDBack, xmlButtonIDClose;

	public string xmlSection = "Tutorial";
	
	private int currentSlide = 0, totalSlides = 0;//getting the content for current slide will be current+start ID.
	private bool initialized = false;
	private LanguageManager lm = null;

	private void Init()
	{
		currentSlide = 0;
		totalSlides = spritesTutorial.Length;
		if (LanguageManager._LanguageManager)
		{
			lm = LanguageManager._LanguageManager;
		}
		else
		{
			lm = null;
		}

		initialized = true;

	}

	void OnEnable()
	{
		if (!initialized)
		{
			Init();
		}

		//move to first slide.
		Time.timeScale = 0f;
		currentSlide = 0;
		UpdateTutorialContent();
	}

	void UpdateTutorialContent()
	{
		string leftText = "", rightText = "";
		int rightID = -10, leftID = -10;

		#region Preparation

		//first slide
		if (currentSlide == 0)
		{
			//set the text of the next/right button to next
			rightText = "Next";
			rightID = xmlButtonIDNext;
		}
		//last slide
		else if (currentSlide == totalSlides - 1)
		{
			//right text
			rightText = "Close";
			rightID = xmlButtonIDClose;

			//left text
			leftText = "Back";
			leftID = xmlButtonIDBack;
		}
		//middle slides
		else if(currentSlide >= 0 && currentSlide < totalSlides)
		{
			//right text
			rightText = "Next";
			rightID = xmlButtonIDNext;

			//left text
			leftText = "Back";
			leftID = xmlButtonIDBack;
		}

		#endregion

		//now set everything.
		#region Nav Buttons
		if (rightText == "" || rightID == -10)
		{
			//since right has not been updated, it should be turned off.
			btnRight.SetActive(false);
		}
		else
		{
			//translate the text if possible.
			if (lm)
			{
				rightText = lm.TextTranslation(xmlSection, rightID);
			}
			//set the text.
			textfieldBtnRight.text = rightText;
			//make sure the button is on.
			btnRight.SetActive(true);
		}

		if (leftText == "" || leftID == -10)
		{
			//since left has not been updated, it should be turned off.
			btnLeft.SetActive(false);
		}
		else
		{
			//translate the text if possible.
			if (lm)
			{
				leftText = lm.TextTranslation(xmlSection, leftID);
			}
			//set the text.
			textfieldBtnLeft.text = leftText;
			//make sure the button is on.
			btnLeft.SetActive(true);
		}

		#endregion

		#region Content

		//Image
		imageTutorial.sprite = spritesTutorial[currentSlide];

		if (lm)
		{
			textfieldTutorialContent.text = lm.TextTranslation(xmlSection, currentSlide + xmlTutorialTextStartID);
		}
		else
		{
			textfieldTutorialContent.text = "The text for the Tutorial could not be found";
		}

		#endregion
	}

	public void NavUpdate(string direction)
	{
		if (direction == "Left")
		{
			if (currentSlide > 0)
			{
				currentSlide--;
			}
		}
		else if (direction == "Right")
		{
			if (currentSlide < totalSlides -1)
			{
				currentSlide++;
			}
			else
			{
				//if this is the last slide, then the right button must say close now.
				Close();
			}
		}

		//update the content
		UpdateTutorialContent();
	}

	public void Close()
	{
		Time.timeScale = 1f;
		gameObject.SetActive(false);
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
