using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// This class is used to allow UI Textfields and their text to change based on the language being used.
/// </summary>
public class LanguageText : MonoBehaviour {

	public string xmlTextSection = " ";
	public int xmlTextID = -9;//the specific id we can find this text at.
	public int xmlTextID2 = -9;//The specific id we can find the second text at. Request Bloodwork button text and Feedback Text will utilize this.

	public bool directTranslation;
	public string translateText;

	public bool referenceTopic = false;//is this a reference topic?
	public string xmlReferenceTopicSectionTitle;
	public string xmlReferenceTopicSectionContent;
	public int xmlReferenceTopicID;//used by the UIReferenceDesk Script.


	private Text textField;
	private int curXmlTextID = -9;
	private bool overridden = false;

	private string concatenateText = "";//this is the string that will be added onto our text.
	private string editorText = "";

	void Awake()
	{
		textField = GetComponent<Text>();
		editorText = textField.text;
		//concatenateText = "";
	}

	void OnEnable()
	{
		UpdateText();
	}

	/// <summary>
	/// Switch the current text to use.
	/// </summary>
	/// <param name="number">1 = Initial, 2 = Secondary</param>
	public void SwitchCurrentText(int number)
	{
		if (xmlTextID >= 0 && xmlTextID2 != xmlTextID && xmlTextID2 >= 0)
		{
			if (number == 1)
			{
				curXmlTextID = xmlTextID;
			}
			else if (number == 2)
			{
				curXmlTextID = xmlTextID2;
			}
			//make sure the text field gets updated
			UpdateText();
		}
		
	}

	/// <summary>
	/// Update the text field.
	/// </summary>
	private void UpdateText()
	{
		string t = editorText;

		//verify we have access to the text field.
		if (textField)
		{

			//make sure we have access to language manager.
			if (LanguageManager._LanguageManager)
			{
				//see if we need to do a direct translation
				if (directTranslation)
				{
					t = LanguageManager._LanguageManager.DirectTranslation(xmlTextSection, translateText);
				}
				else
				{
					//make sure we have a current id to use.
					if (curXmlTextID < 0)
					{
						curXmlTextID = xmlTextID;
					}

					//set our text.
					t = LanguageManager._LanguageManager.TextTranslation(xmlTextSection, curXmlTextID);
				}
				
			}

			if (concatenateText != "")
			{
				textField.text = t + " " + concatenateText;
			}
			else
			{
				textField.text = t;
			}
		}
	}


	/// <summary>
	/// Should this text remain what is is?
	/// </summary>
	/// <param name="overrideThisText">True = don't reset, false = allow reset</param>
	public void AddThisText(string addThisText = "")
	{
		Debug.Log("Add this Text " + addThisText);
		concatenateText = addThisText;
		UpdateText();
	}

	
}
