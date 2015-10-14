using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class UIReferenceDesk : MonoBehaviour {

	public GameObject tabReference, tabABGPractice;
	public Button buttonReference, buttonABGPractice;
	public Scrollbar scrollbarContent;
	public Text textfieldTitle, textfieldContent;//the text fields.
	public Image imageContent;
	public DiagnosisTool diagnosistool;

	private LayoutElement layoutelementContent;//the layout element component of the content text field.

	void Awake()
	{
		if (textfieldContent)
		{
			layoutelementContent = textfieldContent.GetComponent<LayoutElement>();
		}
	}


	

	public void DisplayTopicInformation(LanguageText topic)
	{
		//make sure that the languagetext is indeed a topic.
		if (topic.referenceTopic)
		{
			//make sure we turn the image off and the textfields on.
			imageContent.gameObject.SetActive(false);
			textfieldContent.gameObject.SetActive(true);
			textfieldTitle.gameObject.SetActive(true);

			string title = "Unable to find a title for " + topic.name;//the title of this topic
			string content = "The same principle applies to a metabolic imbalance but in that case the bicarbonate informs us that a metabolic condition exists. Therefore, the CO2 would be compensating/partially compensating for HCO3-.The chart below may be helpful in seeing the number values associated with each imbalance but the numbers are only one part of the picture. The signs and symptoms a patient reports or exhibits are essential for understanding the patient’s condition and deciding upon the medical and nursing care management";  //"Unable to find content for " + topic.name;//the content

			LanguageManager lm = GetLanguageManager();
			if (lm)
			{
				title = lm.TextTranslation(topic.xmlReferenceTopicSectionTitle,topic.xmlReferenceTopicID);
				content = lm.TextTranslation(topic.xmlReferenceTopicSectionContent, topic.xmlReferenceTopicID);
				
				

				
			}
			//2.25 x number of characters.
			layoutelementContent.minHeight = content.Length * 0.5f;
			
			textfieldTitle.text = title;
			textfieldContent.text = content;

			scrollbarContent.value = 1;
			Debug.Log(scrollbarContent.value);
		}
	}

	public void DisplayTopicImage(LanguageUIImage topic)
	{
		Debug.Log("DisplayTopicImage");
		if (topic.referenceTopic)
		{
			//turn the text fields off and turn the image on.
			textfieldTitle.gameObject.SetActive(false);
			textfieldContent.gameObject.SetActive(false);

			//get the correct image to be displayed
			Debug.Log("Image Name: " + topic.CurrentImage().name);
			imageContent.sprite = topic.CurrentImage();
			imageContent.gameObject.SetActive(true);
		}
	}

	private LanguageManager GetLanguageManager(){
		if (LanguageManager._LanguageManager)
		{
			return LanguageManager._LanguageManager;
		}
		else
		{
			return null;
		}
	}

	/// <summary>
	/// Swap the Tab being Displayed
	/// </summary>
	/// <param name="tab">Reference or ABGPractice</param>
	public void TabSwap(string tab)
	{
		bool swapped = false;
		if (tab == "Reference")
		{
			tabABGPractice.SetActive(false);
			tabReference.SetActive(true);
			swapped = true;
			
		}
		else if (tab == "ABGPractice")
		{
			tabABGPractice.SetActive(true);
			tabReference.SetActive(false);
			swapped = true;
		}

		if (swapped && buttonABGPractice && buttonReference)
		{
			buttonABGPractice.interactable = !(tab == "ABGPractice");
			buttonABGPractice.GetComponent<Animator>().SetBool("Focused", tab == "ABGPractice");


			buttonReference.interactable = !(tab == "Reference");
			buttonReference.GetComponent<Animator>().SetBool("Focused", tab == "Reference");
		}
	}

	void OnEnable()
	{
		if (Application.loadedLevelName == "Practice")
		{
			//do nothing.
		}
		else
		{
			//pause the game.
			Time.timeScale = 0;

			//play computer sound
			if (SoundManager._SoundManager)
			{
				SoundManager._SoundManager.PlaySound("UseComputer");
			}
		}

		TabSwap("Reference");
		diagnosistool.Initialize(false);
		diagnosistool.Reset();
	}

	void OnDisable()
	{
		if (Application.loadedLevelName == "Practice")
		{
			//go to main menu
			Application.LoadLevel(1);
		}
		else
		{
			Time.timeScale = 1;
		}
	}

	public void Close()
	{
		//Dirty the Nurse's hands.
		if (Application.loadedLevelName == "Play")
		{
			if (Manager._manager)
			{
				Manager._manager.MyNurse.IsClean(-1);
			}
		}


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
