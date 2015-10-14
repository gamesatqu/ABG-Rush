using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class DiagnosisTool : MonoBehaviour {

	
	public Color colWrong, colCorrect, colNormal;
	public Text dragndropPH, dragndropCO2, dragndropHCO3, ansRM, ansAA, ansC;
	public Button btnSubmit;
	public Image imageRM, imageAA, imageC;
	public float answerTimer;
	public Transform tttPH, tttCO2, tttHCO3;

	private Transform startParentPH, startParentCO2, startParentHCO3;
	private UI_ExamRoomComputer ercUI;
	private bool practice, answerSubmitted, answerCorrect;
	private ABG abg;
	private Diagnosis diagnosis;
	private string defaultRM, defaultAA, defaultC;
	private float answerTimerUsed;


	///The Following variables are for the Help Panel of the Diagnosis Tool.
	public GameObject panelHelp;
	public Slider sliderPH, sliderCO2, sliderHCO3;
	public Text helpPH, helpCO2, helpHCO3;



	/// <summary>
	/// Prepare the Diagnosis Tool
	/// </summary>
	/// <param name="gamemode">True = Gameplay, False = Practice</param>
	/// <param name="ui">The UI Exam Computer</param>
	public void Initialize(bool gamemode, UI_ExamRoomComputer ui = null )
	{
		if(gamemode){
			//make sure the help panel is off.
			panelHelp.SetActive(false);
			if (ui) { ercUI = ui; practice = false; }
		}
		else
		{
			//create an instance of ABG, or gain a reference of ABG
			abg = new ABG();
			//set practice to true
			practice = true;
			//turn on the help panel.
			panelHelp.SetActive(true);
		}

		//Set the original parents of the drag and drop objects
		if (tttPH && tttHCO3 && tttCO2)
		{
			startParentPH = tttPH.parent;
			startParentCO2 = tttCO2.parent;
			startParentHCO3 = tttHCO3.parent;
		}

		//get the original text for the answer fields.
		if (ansRM && ansAA && ansC)
		{
			defaultRM = ansRM.text;
			defaultAA = ansAA.text;
			defaultC = ansC.text;
		}
		//get the original color for the answer fields.
		if (imageAA && imageC && imageRM)
		{
			colNormal = imageRM.color;
		}

		
		
	}

	/// <summary>
	/// Reset Positions of ABG Variables. Create new Diagnosis if practice mode.
	/// </summary>
	public void Reset(Diagnosis d = null)
	{
		//Since the drag/drop slots handle positioning/scaling and everything else, just tell the objects who their parent/slot is.
		tttPH.SetParent(startParentPH);
		tttCO2.SetParent(startParentCO2);
		tttHCO3.SetParent(startParentHCO3);

		//If a diagnosis was provided, use it
		if (d != null)
		{
			diagnosis = d;
		}
		//otherwise, create our own.
		else 
		{
			//we must be in practice mode
			diagnosis = abg.RandomDiagnosis();
			Debug.Log("Created random diagnosis");
		}

		//set the text inside of the drag and drop objects.
		dragndropPH.text = "pH\n" + diagnosis.PH.ToString("F2");
		dragndropCO2.text = "CO2\n" + Mathf.RoundToInt(diagnosis.CO2).ToString();//ToString("F2");
		dragndropHCO3.text = "HCO3\n" + Mathf.RoundToInt(diagnosis.HCO3).ToString();//("F2")*/;

		//reset the color of each button to it's original color/status
		imageRM.color = colNormal;
		imageAA.color = colNormal;
		imageC.color = colNormal;


		// reset the value of each answer text to it's original text
		ansAA.text = defaultAA;
		ansRM.text = defaultRM;
		ansC.text = defaultC;


		//make sure submit button is clickable.
		btnSubmit.interactable = true;

		//reset timer
		answerTimerUsed = 0;

		//Set answersubmitted and correct to false;
		answerCorrect = false;
		answerSubmitted = false;

		//if we are in practice mode, make sure help panel is enabled, and the sliders are reset
		if (practice)
		{
			if (panelHelp)
			{
				//make sure the help panel is on.
				panelHelp.SetActive(true);
				//reset the value of each slider.
				sliderCO2.value = 40;
				sliderHCO3.value = 24;
				sliderPH.value = 7.4f;
			}
		}
	}


	


	private void Update()
	{
		//determine if we are waiting for the submit button to become clickable again.
		//if (gameObject.activeInHierarchy && !btnSubmit.interactable)
		//{
		//	answerTimerUsed += Time.deltaTime;

		//	//if time is up, turn the button back on.
		//	if (answerTimerUsed >= answerTimer)
		//	{
		//		btnSubmit.interactable = true;
		//	}
		//}

		if (gameObject.activeInHierarchy && answerSubmitted)
		{
			answerTimerUsed += Time.deltaTime;
			if (answerTimerUsed >= answerTimer)
			{
				answerSubmitted = false;
				if (!answerCorrect)
				{
					btnSubmit.interactable = true;
				}
				else
				{
					//play Correct Diagnosis sound
					if (SoundManager._SoundManager)
					{
						SoundManager._SoundManager.PlaySound("PatientDiagnosed");
					}

					if (practice)
					{
						//reset the tool
						Reset();
					}
					else
					{
						//inform the manager and the ui
						ercUI.FinishDiagnosis();
					}
				}
			}
		}
	}


	#region Functions/Methods Used by UI Buttons

	/// <summary>
	/// Called by the dropdown answer buttons
	/// </summary>
	/// <param name="t"></param>
	public void SwapText(Text t)
	{
		string s = t.text;

		string r = "Respiratory", m = "Metabolic", aci = "Acidosis", alk = "Alkalosis", uc = "Uncompensated", pc = "Partial Compensation", c = "Compensated";
		//translate if possible
		if (LanguageManager._LanguageManager)
		{
			LanguageManager lm = LanguageManager._LanguageManager;
			r = lm.DirectTranslation("ABG", r);
			m = lm.DirectTranslation("ABG", m);
			aci = lm.DirectTranslation("ABG", aci);
			alk = lm.DirectTranslation("ABG", alk);
			uc = lm.DirectTranslation("ABG", uc);
			pc = lm.DirectTranslation("ABG", pc.Replace(" ", ""));
			c = lm.DirectTranslation("ABG", c);
		}

		//swap the text of the RM answer field.
		if (s == r || s == m)
		{
			ansRM.text = s;
		}
		//swap the text of the AA answer field.
		else if (s == aci || s == alk)
		{
			ansAA.text = s;
		}
		//swap the text of the C answer field.
		else if (s == uc || s == pc|| s == c)
		{
			ansC.text = s;
		}
	}



	/// <summary>
	/// Called from the Submit UI Button. Determine if selected answers are correct
	/// </summary>
	public void Submit()
	{


		bool a = false, b = false, c = false;

		if (ansAA.text == diagnosis.AnswerAcidosisAlkalosis)
		{
			a = true;
			imageAA.color = colCorrect;
		}
		else
		{
			imageAA.color = colWrong;
		}
		if (ansRM.text == diagnosis.AnswerRespiratoryMetabolic)
		{
			b = true;
			imageRM.color = colCorrect;
		}
		else
		{
			imageRM.color = colWrong;
		}
		if (ansC.text == diagnosis.AnswerCompensation)
		{
			c = true;
			imageC.color = colCorrect;
		}
		else
		{
			imageC.color = colWrong;
		}


		//Set the timer and values
		
		answerCorrect = (a && b && c);
		if (!practice)
		{
			btnSubmit.interactable = false;

			if (!answerCorrect)
			{
				Manager._manager.UpdateSatisfactionScore(-25);
			}
			else
			{
				Manager._manager.UpdateSatisfactionScore(+10);
			}
			//Make the player wait for a period of time.
			answerSubmitted = true;
			answerTimerUsed = 0;
		}

		else
		{
			//provide immediate feedback since the game is paused, we cannot wait.
			//The player will reset the problem when they are ready
		}

	}

	/// <summary>
	/// Called when a slider's value has been updated.
	/// </summary>
	public void UpdateSliderText()
	{
		//update the value being displayed by each slider.
		helpPH.text = sliderPH.value.ToString("F2");
		helpCO2.text = sliderCO2.value.ToString();
		helpHCO3.text = sliderHCO3.value.ToString();
	}

	/// <summary>
	/// Called from the Reset Button
	/// </summary>
	public void ResetTool()
	{
		Reset();
	}

	public void SoundClick()
	{
		//Play a sound
		if (SoundManager._SoundManager)
		{
			SoundManager._SoundManager.PlaySound("Click");
		}
	}
	#endregion
}
