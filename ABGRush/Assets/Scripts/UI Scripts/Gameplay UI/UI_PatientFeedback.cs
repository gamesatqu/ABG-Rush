using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class UI_PatientFeedback : MonoBehaviour {

	public GameObject correctInitialAssessmentObject;// This object will be turned on/Off depending on whether or not the initial assessment was correct.
	public Sprite spriteCorrect, spriteIncorrect;
	public Image imagePatientID, imageInitial, imageDiagnosis;
	public Text textfieldName, textfieldInitialAssessment, textfieldDiagnosisFinal;
	public LanguageText languagetextSuccessLevel, languagetextTitle;

	public Color colorCorrect, colorWrong;
	public string hexColorCorrect = "#8EBD00", hexColorWrong = "#FF0000";

	private bool timebonus = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Close(){
		Time.timeScale = 1;
		if (timebonus)
		{
			if (Manager._manager)
			{
				Manager._manager.UpdateSatisfactionScore(4);
			}
		}
		gameObject.SetActive(false);
	}

	public void PatientFeedback(Patient p, bool initRMCorrect, bool initAACorrect)
	{

		string colorRM, colorAA;

		//inform the title to add the name of the patient to it's text.
		languagetextTitle.AddThisText(p.name);

		//textfieldName.text = p.name;

		
		imagePatientID.sprite = p.PatientPhotoID();

		if (initRMCorrect)
		{
			colorRM = hexColorCorrect;
			//textfieldInitialRM.color = colorCorrect;
		}
		else
		{
			colorRM = hexColorWrong;
			//textfieldInitialRM.color = colorWrong;
		}

		if (initAACorrect)
		{
			colorAA = hexColorCorrect;
			//textfieldInitialAA.color = colorCorrect;
		}
		else
		{
			colorAA = hexColorWrong;
			//textfieldInitialAA.color = colorWrong;
		}

		if (initRMCorrect && initAACorrect)
		{
			languagetextSuccessLevel.SwitchCurrentText(2);

			//display the correct sprite
			imageInitial.sprite = spriteCorrect;
			//display the correct object
			correctInitialAssessmentObject.SetActive(true);
			//set the bool for time bonus to true.
			timebonus = true;
		}
		else
		{
			languagetextSuccessLevel.SwitchCurrentText(1);
			//display the incorrect initial sprite
			imageInitial.sprite = spriteIncorrect;
			//deactivate the correct object
			correctInitialAssessmentObject.SetActive(false);
			//set the bool for time bonus to false
			timebonus = false;
		}

		//display the initial diagnosis text.
		textfieldInitialAssessment.text = "<color=" + colorRM +">" + p.InitialAssessmentGetRM() + "</color> " + "<color=" + colorAA + ">" + p.InitialAssessmentGetAA() + "</color>";

		//textfieldInitialRM.text = p.InitialAssessmentGetRM();
		//textfieldInitialAA.text = p.InitialAssessmentGetAA();

		//display the correct answer for the final diagnosis
		textfieldDiagnosisFinal.text ="<color=" + hexColorCorrect +">" + p.MyDiagnosis().AnswerRespiratoryMetabolic + " " + p.MyDiagnosis().AnswerAcidosisAlkalosis + " " + p.MyDiagnosis().AnswerCompensation + "</color>";

		//make sure that the final diagnosis sprite always displays correct.
		imageDiagnosis.sprite = spriteCorrect;
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
