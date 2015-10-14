using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_ExamRoomComputer : MonoBehaviour {

	private Patient patient;//the patient I am currently displaying information about.

	public Button buttonClose, buttonDiagnose, buttonPatientHistory, buttonBloodwork, buttonSubmitassessment;
	public Text textfieldname, textfieldDOB, textfieldBloodwork,/*textfieldHistory, textfieldSymptoms, textfieldConditions, textfieldMedications, textfieldPH, textfieldHCO3, textfieldCO2,*/ textfieldInitialRM, textfieldInitialAA, textfieldassessmentAnswer;

	//The following are text fields for patient information and patient signs and symptoms.
	public List<Text> patientHistory, patientSignsSymptoms;

	public Image imagePatient;
	public GameObject screenPatientHistory, screenDiagnosis, panelassessment1, panelassessment2;
	public DiagnosisTool diagnosisTool;

	private Manager manager;
	private bool init = false;
	private string defaultRM, defaultAA;

	private List<Text> patientABGValues, patientStoryInformation;
	

	/// <summary>
	/// Set the patient that the UI will access for data
	/// </summary>
	/// <param name="p"></param>
	public void SetPatient(Patient p)
	{
		patient = p;
		Debug.Log("ExamRoomComputerUI Patient has been set.");
		
	}

	void OnEnable()
	{

		if (!init)
		{
			Initialize();
		}

		if (manager)
		{
			//inform the nurse that they are currently busy and should not be able to perform actions outside of this UI.
			manager.MyNurse.IsBusy(1);
		}

		//play computer sound
		if (SoundManager._SoundManager)
		{
			SoundManager._SoundManager.PlaySound("UseComputer");
		}


		Debug.Log("ExamRoomComputer " + patient);
		if (patient)
		{
			//tell the patient to stop counting down.
			patient.PatientToggleCountdown(true);

			//Turn text fields off
			//ToggleTexts("ABG Values", false);
			//ToggleTexts("Story Information", false);


			//Name
			textfieldname.text = patient.name;

			//DOB
			textfieldDOB.text = patient.DateOfBirth(); //Random.Range(1960, 2001).ToString();
			
			//Picture
			imagePatient.sprite = patient.PatientPhotoID();

			//setup patient history information ***RON COME BACK AND UNCOMMENT THIS***
			DisplayHistoryAndSignsSymptoms();

			//RON COME BACK AND CHANGE THIS

			//set the values of the PH, HCO3, CO2, Medications, Symptoms, and Conditions
			Diagnosis d = patient.MyDiagnosis();
			/*if (textfieldCO2 && textfieldHCO3 && textfieldPH)
			{
				textfieldPH.text = "PH : " + d.PH.ToString("F2");
				textfieldHCO3.text = "HCO3 : " + d.HCO3.ToString("F2");
				textfieldCO2.text = "CO2 : " + d.CO2.ToString("F2");
			}

			if (textfieldSymptoms && textfieldConditions && textfieldMedications)
			{
				textfieldSymptoms.text = d.Symptoms();
				textfieldConditions.text = d.Conditions();
				textfieldMedications.text = d.Medications();
			}*/
			
			//display the initial patient history screen, and make sure diagnosis screen is off.
			PatientHistoryDiagnosisTabSwitch(true);
			buttonDiagnose.interactable = false;


			//determine what needs to be displayed.
			string status = patient.Status();
			
			if (status == "ExamRoom" || status == "Vitals")
			{
				//display the basic information
				//textfieldHistory.text = d.Story("S");

				//set status of buttons

				//ABG Tool/DIagnosis should be disabled/Invisible
				if (buttonDiagnose)
				{
					//since the player is not currently able to diagnose the patient, disable this button
					buttonDiagnose.interactable = false;
					//make sure that the diagnose button is not currently in it's focused animation.
					buttonDiagnose.GetComponent<Animator>().SetBool("Focused", false);
				}

				if (buttonPatientHistory)
				{
					//Since the player cannot diagnose, they are unable to change tabs/screens. Therefore, this button should also be disabled.
					buttonPatientHistory.interactable = false;
					//Make sure that the patient history button is in it's focused animation.
					buttonPatientHistory.GetComponent<Animator>().SetBool("Focused", true);
				}
				
				//Bloodwork button should also be disabled/invis
				if (buttonBloodwork)
				{
					//We cannot currently request bloodwork, so this button should also be disabled.
					buttonBloodwork.interactable = false;
					buttonBloodwork.gameObject.SetActive(false);
				}
			}
			else if (status == "VitalsComplete" || status == "Bloodwork" || status == "Diagnosis" || status == "Assessment")
			{
				//display all the information
				
				//History
				//textfieldHistory.text = d.Story("L");
				
				//Extra Story Information
				//ToggleTexts("Story Information", true);

				if (status == "VitalsComplete")
				{
					//the patient is now able to get their bloodwork done.
					if (buttonBloodwork)
					{
						//display the bloodwork button
						buttonBloodwork.gameObject.SetActive(true);
						//make the button interactable
						buttonBloodwork.interactable = true;
					}
					
				}

				if (status == "Assessment")
				{
					//determine if the player has already made an assessment
					string aa = patient.InitialAssessmentGetAA(), rm = patient.InitialAssessmentGetRM();
					
					//create comparisons
					string aci = "Acidosis", alk = "Alkalosis", r = "Respiratory", m = "Metabolic";
					if (LanguageManager._LanguageManager)
					{
						LanguageManager lm = LanguageManager._LanguageManager;
						aci = lm.DirectTranslation("ABG", aci);
						alk = lm.DirectTranslation("ABG", alk);
						r = lm.DirectTranslation("ABG", r);
						m = lm.DirectTranslation("ABG", m);
					}

					if ((aa == alk || aa == aci) && (rm == r || rm == m))
					{
						//turn the first assessment panel off
						panelassessment1.SetActive(false);
						//setup the answer the player provided.
						textfieldassessmentAnswer.text = rm + " " + aa;
						//turn the second panel on.
						panelassessment2.SetActive(true);
						//turn off the submit assessment button
						if (buttonSubmitassessment)
						{
							buttonSubmitassessment.gameObject.SetActive(false);
						}
						//display the bloodwork button
						if (buttonBloodwork)
						{
							buttonBloodwork.interactable = true;
							textfieldBloodwork.gameObject.GetComponent<LanguageText>().SwitchCurrentText(1);
							buttonBloodwork.gameObject.SetActive(true);
						}
					}
					else
					{
						//make sure the second panel is off.
						panelassessment2.SetActive(false);

						//make sure the initial panel has it's values and buttons prepared.
						textfieldInitialAA.text = defaultAA;
						textfieldInitialRM.text = defaultRM;

						//make sure that the bloodwork button is not being displayed
						if (buttonBloodwork)
						{
							buttonBloodwork.interactable = false;
							buttonBloodwork.gameObject.SetActive(false);
						}

						//turn the initial panel on.
						panelassessment1.SetActive(true);

						//make sure the submit button is active and interactable
						if (buttonSubmitassessment)
						{
							buttonSubmitassessment.interactable = true;
							buttonSubmitassessment.gameObject.SetActive(true);
						}
					}
				}

				if (status == "BloodworkWaiting")
				{
					//The patient is already getting their bloodwork done. Make sure that the bloodwork button is either disabled, or gone. Maybe place some kind of animation/visual to show the percentage /time remaining
					if (buttonBloodwork)
					{
						//stop showing the bloodwork button
						//buttonBloodwork.gameObject.SetActive(true);
						
						//make sure the button is not interactable
						buttonBloodwork.interactable = false;
					}
				}

				if (status == "Diagnosis")
				{
					if (diagnosisTool)
					{
						//supply the diagnosis tool with the required information
						diagnosisTool.Reset(patient.MyDiagnosis());
					}

					if (buttonDiagnose)
					{
						buttonDiagnose.interactable = true;
					}

					if (buttonBloodwork)
					{
						//stop displaying bloodwork since we have the results now.
						buttonBloodwork.interactable = false;
						buttonBloodwork.gameObject.SetActive(false);

					}
					//Display the bloodwork results / ABG Values
					ToggleTexts("ABG Values", true);
				}
			}
		}
	}

	void OnDisable()
	{
		if (manager)
		{
			//inform the nurse that they are busy
			manager.MyNurse.IsBusy(-1);
		}

		//make sure that the diagnose button is uninteractable
		buttonDiagnose.interactable = false;

		//inform the patient to resume counting down
		patient.PatientToggleCountdown(false);

		//turn the extra text fields off
		ToggleTexts("ABG Values", false);
		ToggleTexts("Story Information", false);
	}

	#region Private Methods

	/// <summary>
	/// Toggle the Text components to On or Off
	/// </summary>
	/// <param name="w">ABG Values or Story Information</param>
	/// <param name="on">On enables, False Disables</param>
	private void ToggleTexts(string w, bool on)
	{
		if (w == "ABG Values")
		{
			foreach (Text t in patientABGValues)
			{
				t.enabled = on;
			}
		}
		else if (w == "Story Information")
		{
			foreach (Text t in patientStoryInformation)
			{
				t.enabled = on;
			}
		}
	}


	private void Initialize()
	{
		//Initialize both lists.
		patientABGValues = new List<Text>();
		patientStoryInformation = new List<Text>();

		//populate the lists.
		//patientABGValues.Add(textfieldPH); patientABGValues.Add(textfieldCO2); patientABGValues.Add(textfieldHCO3);
		//patientStoryInformation.Add(textfieldSymptoms); patientStoryInformation.Add(textfieldConditions); patientStoryInformation.Add(textfieldMedications);

		//clear the placeholder values inside.
		foreach (Text t in patientABGValues)
		{
			t.text = " ";
		}
		foreach (Text t in patientStoryInformation)
		{
			t.text = " ";
		}

		//turn off buttons that may not be used immediately
		if (buttonBloodwork)
		{
			buttonBloodwork.interactable = false;
		}
		//gain access to the manager
		if (GameObject.Find("Manager"))
		{
			manager = GameObject.Find("Manager").GetComponent<Manager>();
		}

		//Prepare assestment field
		if (textfieldInitialAA && textfieldInitialRM)
		{
			defaultAA = textfieldInitialAA.text;
			defaultRM = textfieldInitialRM.text;
		}
		else
		{
			Debug.LogWarning(name + "'s UIExamRoomComputer does not have access to the assessment fields.");
		}

		//initialize the diagnosis tool
		diagnosisTool.Initialize(true, this);

		init = true;

	}

	/// <summary>
	/// Populate the patient information area as well as the Signs and Symptoms area.
	/// </summary>
	private void DisplayHistoryAndSignsSymptoms()
	{
		//make sure we have a patient
		if (patient)
		{
			//grab the diagnosis
			Diagnosis d = patient.MyDiagnosis();

			//gain access to the history and signs and symptoms of that patient
			List<string> h = d.History(), ss = d.SignsAndSymptoms();

			//patient history bullet points
			for (int i = 0; i < patientHistory.Count; i++)
			{
				//verify that there is something in this location
				if (h.Count > i)
				{
					//place the value in the text field.
					patientHistory[i].text = "- " + h[i];
					//make sure this textfield is on.
					patientHistory[i].gameObject.SetActive(true);
				}
				else
				{
					//become inactive since there is no value to go inside.
					patientHistory[i].gameObject.SetActive(false);
				}
			}

			//signs and symptoms bullet points
			for (int i = 0; i < patientSignsSymptoms.Count; i++)
			{
				//verify that there is something in this location
				if (ss.Count > i)
				{
					//place the value in the text field.
					patientSignsSymptoms[i].text = "- " + ss[i];
					//make sure this textfield is on.
					patientSignsSymptoms[i].gameObject.SetActive(true);
				}
				else
				{
					//become inactive since there is no value to go inside.
					patientSignsSymptoms[i].gameObject.SetActive(false);
				}
			}
		}
		
	}


	#endregion


	#region Button Functions

	/// <summary>
	/// Change the Tabs and screens being shown
	/// </summary>
	/// <param name="pHistory">True = Patient History, False = Diagnosis</param>
	public void PatientHistoryDiagnosisTabSwitch(bool pHistory)
	{
		//turn the diagnosis screen off.
		screenDiagnosis.SetActive(!pHistory);

		//turn the patient history screen on.
		screenPatientHistory.SetActive(pHistory);

		//change the focus animations for the tab buttons
		buttonDiagnose.interactable = pHistory;
		buttonDiagnose.GetComponent<Animator>().SetBool("Focused", !pHistory);

		buttonPatientHistory.interactable = !pHistory;
		buttonPatientHistory.GetComponent<Animator>().SetBool("Focused", pHistory);

	}

	public void RequestBloodwork()
	{
		//make the button un-interactable
		buttonBloodwork.interactable = false;
		//textfieldBloodwork.text = "REQUESTED BLOOD WORK";
		textfieldBloodwork.gameObject.GetComponent<LanguageText>().SwitchCurrentText(1);
		//Change the status of the patient to bloodwork

		//Inform the patient's computer to send the bloodwork and begin counting down.
		if (patient)
		{
			(patient.PatientHotspotGet() as ExamRoom).Computer().SendBloodwork();
		}
		
		//inform the patient to stop/begin counting down. Not sure which at the moment, or if they should just be halted.

		//Close this window
		Close();
	}

	public void InitialassessmentTextSwap(Text t)
	{
		string s = t.text;
		//make sure we have access to both of these text fields.
		if (textfieldInitialAA && textfieldInitialRM)
		{
			//check for language
			if (LanguageManager._LanguageManager)
			{
				LanguageManager lm = LanguageManager._LanguageManager;

				//swap the text inside of the text field
				if (s == lm.DirectTranslation("ABG", "Respiratory") || s == lm.DirectTranslation("ABG", "Metabolic"))
				{
					textfieldInitialRM.text = s;
				}
				else if (s == lm.DirectTranslation("ABG", "Acidosis") || s == lm.DirectTranslation("ABG", "Alkalosis"))
				{
					textfieldInitialAA.text = s;
				}
			}
			else
			{
				//default to english since we are probably testing.

				//swap the text inside of the text field
				if (s == "Respiratory" || s == "Metabolic")
				{
					textfieldInitialRM.text = s;
				}
				else if (s == "Acidosis" || s == "Alkalosis")
				{
					textfieldInitialAA.text = s;
				}
			}
			
		}
		else
		{
			Debug.LogWarning(name + "'s UIExamRoomComputer does not have access to the assessment text fields");
		}
	}

	public void InitialassessmentSubmit()
	{
		//take in the values submitted.
		string aa = textfieldInitialAA.text, rm = textfieldInitialRM.text;

		//create comparison values.
		string r = "Respiratory", m = "Metabolic", aci = "Acidosis", alk = "Alkalosis";
		//translate comparison values if possible.
		if (LanguageManager._LanguageManager)
		{
			LanguageManager lm = LanguageManager._LanguageManager;
			r = lm.DirectTranslation("ABG", r);
			m = lm.DirectTranslation("ABG", m);
			aci = lm.DirectTranslation("ABG", aci);
			alk = lm.DirectTranslation("ABG", alk);
		}

		//make sure that the submitted values are indeed proper values.
		if ((aa == alk || aa == aci) && (rm == r || rm == m))
		{
			//turn off the panel for the first part.
			panelassessment1.SetActive(false);

			//turn off the assessment submit button
			if (buttonSubmitassessment)
			{
				buttonSubmitassessment.gameObject.SetActive(false);
			}

			//set the text of the second panel
			textfieldassessmentAnswer.text = rm + " " + aa;

			//give these values to the patient.
			if (patient)
			{
				patient.InitialAssessmentSet(rm, aa);
			}
			
			//display this secondary panel
			panelassessment2.SetActive(true);

			//turn off the submit assessment button
			if (buttonSubmitassessment)
			{
				buttonSubmitassessment.gameObject.SetActive(false);
			}
			
			//display bloodwork button.
			if (buttonBloodwork)
			{
				buttonBloodwork.gameObject.SetActive(true);
				buttonBloodwork.interactable = true;
			}

		}

	}

	/// <summary>
	/// Close the interface
	/// </summary>
	public void Close()
	{
		//make sure the nurse's hands are now dirty.
		manager.MyNurse.IsClean(-1);
		gameObject.SetActive(false);

	}

	#endregion


	

	/// <summary>
	/// Tell the Patient to leave.
	/// Close Self/Turn Self off.
	/// </summary>
	public void FinishDiagnosis()
	{
		Debug.Log("FinishDiagnosis");
		manager.MyNurse.IsClean(-1);
		gameObject.SetActive(false);
		
		patient.PatientStatusUpdate("DiagnosisComplete");

		
		//patient.Patient_Leave(); // this is now handled inside the patient.

		//close since the diagnosis is complete
		//StartCoroutine("Deactivate", 1f);
		//gameObject.SetActive(false);
	}

	private IEnumerator Deactivate(float delay)
	{
		yield return new WaitForSeconds(delay);
		if (gameObject.activeInHierarchy)
		{
			gameObject.SetActive(false);
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
