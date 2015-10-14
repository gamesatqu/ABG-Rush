using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Patient : Person {

	public SpriteRenderer highlightRenderer = null;
	public Sprite photoID;

    private float timerTriage, timerWaitingRoom, timerExamRoom, timerVitals, timerBloodwork, timerDiagnosis, timerDelayPacification, timerCurrent, timerCurrentSet;
    private int pacifyAmountLeft;//the amount will change if a patient is interacted with, but no action is taken. This will reduce the current timer by the pacification delay.
    private string patientName, status;//The current status of the patient: (Triage, Waiting, Exam Room, Vitals, etc...)
	private string assessmentRM, assessmentAA;//the initial assessment provided by the player.
	private string dateOfBirth;
	private Diagnosis diagnosis;
    private PatientObject hotspot;
    private bool timerHalted;
    private Collider2D collider;

	//Animator/animation stuff
	private Animator anim;
	private List<int> animationPositions;
	private int hashTalking = Animator.StringToHash("Talking");
	private int hashWalking = Animator.StringToHash("Walking");
	private int hashSitting = Animator.StringToHash("Sitting");
	private int hashTurned = Animator.StringToHash("Turned");
	private int hashPatience = Animator.StringToHash("Patience");
	private int hashHighlight = Animator.StringToHash("Highlight");
	private int hashWaiting = Animator.StringToHash("Waiting");
 

	// Use this for initialization
	void Start () {
        //PersonInitialize();
        //PatientInitalize();
        //tag = "Patient";
	}
	
	// Update is called once per frame
	void Update () {
        PersonUpdate(highlightRenderer);
        if (!Moving() /*and not currently in UI*/ && !timerHalted)
        {
            PatientPatienceCountdown();
        }
	}

    /// <summary>
    /// Inform the patient of it's new Status
    /// </summary>
    /// <param name="location">Triage, WaitingRoom, ExamRoom, Vitals, Bloodwork, Diagnosis, Leave</param>
    public void PatientStatusUpdate(string stat)
    {
        switch (stat)
        {
			case "Triage": PatientToggleCountdown(true); collider.enabled = false; PatientAnimation("Turned", false, true); break;
			case "WaitingChair": timerCurrent = timerWaitingRoom; timerCurrentSet = timerWaitingRoom; PatientToggleCountdown(false); collider.enabled = true; PatientAnimation("Waiting", false, true); PatientAnimation("Highlight", false, true); break;
			case "ExamRoom": timerCurrent = timerExamRoom; timerCurrentSet = timerExamRoom; collider.enabled = false; PatientAnimation("Sitting", false, true); break;
			case "Vitals": timerCurrent = timerVitals; timerCurrentSet = timerVitals; collider.enabled = true; PatientAnimation("Sitting", false, true); PatientAnimation("Highlight", false, true); break;
			case "VitalsComplete": timerCurrent = timerVitals; timerCurrentSet = timerVitals; PatientToggleCountdown(true); collider.enabled = false; PatientAnimation("Sitting", false, true); break;//countdown will re-enable after a decision is made in UI
			case "Assessment": timerCurrent = timerVitals; timerCurrentSet = timerVitals; PatientToggleCountdown(false); collider.enabled = true; PatientAnimation("Sitting", false, true); break;

			//case "Bloodwork": timerCurrent = timerBloodwork; collider.enabled = false; break;
			case "BloodworkWaiting": timerCurrent = timerBloodwork; timerCurrentSet = timerBloodwork; collider.enabled = false; PatientAnimation("Sitting", false, true); PatientAnimation("Highlight", false, false); break;
			case "Diagnosis": timerCurrent = timerDiagnosis; timerCurrentSet = timerDiagnosis; collider.enabled = true; /*PatientAnimation("Sitting", false, true);*/ PatientAnimation("Highlight", false, true); break;
			case "DiagnosisComplete": timerCurrent = 999f; collider.enabled = false; PatientAnimation("Highlight", false, false); PatientLeave(); break;
            case "Exit": Destroy(gameObject); break;
        }
		status = stat;
    }

    /// <summary>
    /// Set this patient's current Hotspot
    /// </summary>
    /// <param name="po"></param>
    public void PatientHotspot(PatientObject po)
    {
        hotspot = po;
    }

    /// <summary>
    /// Return this patient's hotspot
    /// </summary>
    public PatientObject PatientHotspotGet()
    {
        return hotspot;
    }


    /// <summary>
    /// Initialize Patient variables
    /// </summary>
    private void PatientInitalize(){
        //This may be changed later depending on when/where we would like to generate all the patient's information. 
        //It could be done here either immediately on startup or after the manager has given input, and each patient can be self inclusive
        //This information could be created/generated entirely through the manager as well, and this patient just holds the data.

        timerTriage = 10f;//how long I will wait at the triage before leaving
        timerWaitingRoom = 30f;//how long I will wait in the waiting room before leaving
        timerExamRoom = 30f;//how long I will wait in the exam room before leaving
		timerVitals = 30f;
		timerBloodwork = 500f;//this is currently a state in which the player has no control over the patient, so this can be as long as necessary.
		timerDiagnosis = 60f;
        timerCurrent = 10f;
        timerDelayPacification = 20f;
        pacifyAmountLeft = 2;
        anim = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        if (collider)
        {
            //turn collider off
            collider.enabled = false;
        }

		if (anim)
		{
			anim.SetBool(hashHighlight, false);
			anim.SetBool(hashSitting, false);
			anim.SetBool(hashTalking, false);
			anim.SetBool(hashWaiting, false);
			anim.SetBool(hashWalking, false);
			anim.SetFloat(hashPatience, 1f);

			animationPositions = new List<int>();
			animationPositions.Add(hashSitting); animationPositions.Add(hashWalking); animationPositions.Add(hashWaiting); animationPositions.Add(hashTurned);
		}
    }

    /// <summary>
    /// Pacifiy the patient, and add more time to the timer
    /// </summary>
    public void PatientPacify()
    {
        if (pacifyAmountLeft > 0)
        {
            timerCurrent += timerDelayPacification;
            pacifyAmountLeft--;
        }
    }

    /// <summary>
    /// Return the amount of times a patient can be pacified.
    /// </summary>
    /// <returns></returns>
    public int PatientPacifyAmountLeft()
    {
        return pacifyAmountLeft;
    }


    /// <summary>
    /// Called each update to tick down the patience timers.
    /// </summary>
    private void PatientPatienceCountdown()
    {
        if (timerCurrent > 0)
        {
            timerCurrent -= Time.deltaTime;
			if (anim)
			{
				anim.SetFloat(hashPatience, timerCurrent / timerCurrentSet);
			}
            if (timerCurrent <= 0)
            {
                PatientStormOut();
            }
        }
        
    }

    /// <summary>
    /// Called to stop the patient's current countdown clock.
    /// </summary>
    /// <param name="halt">True - Stop, False - Resume/Start</param>
    public void PatientToggleCountdown(bool halt)
    {
        timerHalted = halt;
    }


    /// <summary>
    /// Leave angrily
    /// </summary>
    private void PatientStormOut()
    {
        //inform current hotspot
        hotspot.PatientObjectPatientRemove();
        //inform manager
		if (MyManager)
		{
			MyManager.ManagerPatientStormOut(this);
		}
        //storm out
        //manager should inform me of where the exit is.
    }

    /// <summary>
    /// Leave after either finishing treatment or being told I cannot be helped here.
    /// </summary>
    public void PatientLeave()
    {
		Debug.Log("PatientLeave");
        //inform current hotspot
        hotspot.PatientObjectPatientRemove();
        //inform manager
		if (MyManager)
		{
			MyManager.ManagerPatientLeave(this);
        
		}
        //leave
    }

	//void OnMouseEnter()
	//{
	//	////if I am on a hotspot, and the nurse is not busy.
	//	//if (hotspot && !Manager.MyNurse.IsBusy())
	//	//{
	//	//	//Manager.ManagerMouseOver(true);
	//	//	//hotspot.OfficeObjectMouseEnter();
	//	//}
	//}

	//void OnMouseExit()
	//{
	//	if (hotspot)
	//	{
	//		//Manager.ManagerMouseOver(false);
	//		//hotspot.OfficeObjectMouseExit();
	//	}
        
        
	//}

    void OnMouseOver()
    {
		if (MyManager)
		{
			if (hotspot && !MyManager.MyNurse.IsBusy())
			{
				if (hotspot.OfficeObjectReady()/* && hotspot.OfficeObjectMousedOver()*/ && hotspot.tag != "Triage" && !Moving())
				{
					if (Input.GetMouseButtonUp(0))
					{
						bool snd = false;
						//bool nextStep = false;
						if (status == "ExamRoom" || status == "Vitals")
						{
							//tell the nurse to move to the proper location
							//exam room should no longer be a status since it's automated now, but it currently remains since this is not final.

							//MyManager.MyNurse.PersonMove(hotspot.OfficeObjectLocationNurse(), "ExamRoom", true, hotspot);
							//PatientToggleCountdown(true);
							//nextStep = true;
						}
						else if (status == "BloodworkWaiting" || status == "Diagnosis" || status == "VitalsComplete" || status == "Assessment")
						{
							//tell the nurse to move to the exam room computer
							MyManager.MyNurse.PersonMove((hotspot as ExamRoom).Computer().OfficeObjectLocationNurse(), "ExamRoomComputer", false, (hotspot as ExamRoom).Computer());
							PatientToggleCountdown(true);

							snd = true;
							//nextStep = true;
						}
						else if (status == "WaitingChair")
						{
							MyManager.MyNurse.PersonMove(hotspot.OfficeObjectLocationNurse(), hotspot.tag, true, hotspot);
							PatientToggleCountdown(true);
							snd = true;
							//nextStep = true;
						}
						//if (nextStep && !timerHalted)
						//{
						//	if (timerCurrent / timerCurrentSet > .6) { Manager.UpdateSatisfactionScore(2); }
						//	else if (timerCurrent / timerCurrentSet < .3) { Manager.UpdateSatisfactionScore(-1); }
						//}

						if (SoundManager._SoundManager && snd)
						{
							SoundManager._SoundManager.PlaySound("Click");
						}
					}

				}
			}
		}
        
        
    }

	/// <summary>
	/// Update the score based on how long you waited.
	/// </summary>
	public void PatientPatienceScore()
	{
		if (status != "ExamRoom" && status != "Triage")
		{
			if (MyManager)
			{
				if (timerCurrent / timerCurrentSet > .6)
				{ 
					MyManager.UpdateSatisfactionScore(2);
				}
				else if (timerCurrent / timerCurrentSet < .3)
				{
					MyManager.UpdateSatisfactionScore(-1);
				}
			}
			
		}
	}

    /// <summary>
    /// Called outside of the class.
    /// </summary>
    /// <param name="animationName">Name of the Animation: Talking</param>
    /// <param name="trigger">Is this animation a trigger?</param>
    /// <param name="on">On or Off</param>
    public void PatientAnimation(string animationName, bool trigger, bool on)
    {
		//if (trigger)
		//{
		//	anim.SetTrigger(animationName);
		//}
		//else
		//{
		//	anim.SetBool(animationName, tru);
		//}
		if (anim)
		{
			Debug.Log("Setting " + animationName + " to " + on);
			if (animationName == "Waiting" || animationName == "Sitting" || animationName == "Walking" || animationName == "Turned")
			{
				anim.SetBool(animationName, on);
				foreach (int i in animationPositions)
				{
					if (Animator.StringToHash(animationName) != i)
					{
						anim.SetBool(i, false);
					}
				}
				
				
			}
			if (animationName == "Talking")
			{
				anim.SetBool(hashTalking, on);
			}
			
			else if (animationName == "Highlight")
			{
				anim.SetBool(hashHighlight, on);
			}
		}
		
    }

	/// <summary>
	/// The status of the patient.
	/// </summary>
	/// <returns>Triage, WaitingChair, ExamRoom, Vitals, Bloodwork, Diagnosis, Exit</returns>
	public string Status()
	{
		return status;
	}

	/// <summary>
	/// Return the sprite that Identifies this patient.
	/// </summary>
	/// <returns></returns>
	public Sprite PatientPhotoID()
	{
		return photoID;
	}

	/// <summary>
	/// Return the Diagnosis
	/// </summary>
	/// <returns></returns>
	public Diagnosis MyDiagnosis()
	{
		return diagnosis;
	}


	public void PatientSetup(string n, string dob, Diagnosis d){
		PersonInitialize();
		PatientInitalize();
		tag = "Patient";

		name = n;
		patientName = n;

		dateOfBirth = dob;

		diagnosis = d;
		assessmentAA = " ";
		assessmentRM = " ";

		Debug.Log(name + "'s Diagnosis is: " + d.AnswerRespiratoryMetabolic + " " + d.AnswerAcidosisAlkalosis + " " + d.AnswerCompensation);
		//Debug.Log(name + "'s Story is: " + d.Story("S"));
	}

	public void InitialAssessmentSet(string RM, string AA)
	{
		assessmentRM = RM;
		assessmentAA = AA;
	}

	public string InitialAssessmentGetRM()
	{
		return assessmentRM;
	}

	public string InitialAssessmentGetAA()
	{
		return assessmentAA;
	}

	public string DateOfBirth()
	{
		return dateOfBirth;
	}
}
