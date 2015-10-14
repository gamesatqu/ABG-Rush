using UnityEngine;
using System.Collections;

public class Nurse : Person {

	private bool clean = false, waitingAtExamRoom;//Are hands clean. //Is the nurse waiting at the exam room for the patient?
	private Animator anim;
	private int hashTalking = Animator.StringToHash("Talking");
	private int hashFaceFront = Animator.StringToHash("FaceFront");
	private int hashFaceRight = Animator.StringToHash("FaceRight");
	private int hashFaceBack = Animator.StringToHash("FaceBack");
	private int hashWalk = Animator.StringToHash("Walk");
	private int busy;//Busy is an int and not a bool because it's possible that more than one update to the busy state to occur within a short period of time. So using an integer and checking for 0 will be most efficient.
	private OfficeObject currentOfficeObject;
	private Patient currentPatient;



	#region Initialization
	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	// Use this for initialization
	void Start () {
        PersonInitialize();
		busy = 0;
        tag = "Nurse";
		waitingAtExamRoom = false;
		//make sure that the nurse's hands start off dirty.
		IsClean(-1);
	}

	#endregion

	// Update is called once per frame
	void Update () {
		
        PersonUpdate();
	}

	#region Actions

	/// <summary>
	/// A copy of the Function found in the Patient Class. 
	/// </summary>
	/// <param name="animation">Talking, FaceFront, FaceRight, FaceBack, Walk</param>
	/// <param name="isTrigger">Is the animation a trigger</param>
	/// <param name="tru">On or Off</param>
	public void NurseAnimation(string animation, bool isTrigger, bool tru)
	{
		if (anim)
		{
			if (isTrigger)
			{
				Debug.Log("Performing animation: " + animation);
				if (animation == "FaceFront")
				{
					anim.SetTrigger(hashFaceFront);
				}
				else if (animation == "FaceRight")
				{
					anim.SetTrigger(hashFaceRight);
				}
				else if (animation == "FaceBack")
				{
					anim.SetTrigger(hashFaceBack);
				}
				else if (animation == "Walk")
				{
					anim.SetTrigger(hashWalk);
				}
			}
			else
			{
				anim.SetBool(animation, tru);
			}
		}
		
	}

	/// <summary>
	/// Called when the nurse needs to perform an action of some kind. Make the nurse wash their hands.
	/// <param name="action">Name of the Action: Wash Hands, Vitals, ExamRoomSetup</param>
	/// </summary>
	public void NursePerformAction(string action, OfficeObject o = null, Patient p = null)
	{
		if (action == "Wash Hands")
		{
			currentOfficeObject = o;
			StartCoroutine("WashHands");
			
		}
		//else if (action == "Vitals")
		//{
		//	currentOfficeObject = o;
		//	currentPatient = p;
		//	StartCoroutine("CheckVitals");
		//}
		else if (action == "ExamRoomSetup")
		{
			currentOfficeObject = o;
			currentPatient = p;
			StartCoroutine("ExamRoomSetup");
		}
	}

	/// <summary>
	/// Converse with the patient for a period of time, and then open UI
	/// </summary>
	/// <returns></returns>
	private IEnumerator ExamRoomSetup()
	{
		//This is virtually the same as check vitals except for 1-2 differences. If there is no need to keep them differentiated at the end, change them.
		//set busy to true so no actions can be taken/made
		IsBusy(1);

		//Make sure that the nurse is facing the computer/patient.
		(this as Nurse).NurseAnimation("FaceBack", true, false);

		//tell the patient to stop losing patience
		currentPatient.PatientToggleCountdown(true);

		//check if hands are clean. Gain/Lose points based on result.
		//RON COME BACK AND UPDATE THIS

		for (int i = 0; i < 1; i++)
		{
			//make sure the patient's speech bubble is off.
			currentPatient.PatientAnimation("Talking", false, false);

			//Nurse says something to patient
			NurseAnimation("Talking", false, true);

			//wait for time
			yield return new WaitForSeconds(.75f);

			//make sure the nurse's speech bubble is off
			NurseAnimation("Talking", false, false);

			//patient responds to nurse
			currentPatient.PatientAnimation("Talking", false, true);

			//wait for time
			yield return new WaitForSeconds(.75f);
		}

		//turn both animations off just in case.
		currentPatient.PatientAnimation("Talking", false, false);
		NurseAnimation("Talking", false, false);

		//set hands to dirty
		IsClean(-1);

		//update the patient's status
		currentPatient.PatientStatusUpdate("Assessment");//changed from VitalsComplete //changed from vitals

		//Open the ExamRoom Computer Interface
		if (MyManager)
		{
			MyManager.GamePlayUI().ExamRoomComputerUI().SetPatient(currentPatient);
			MyManager.GamePlayUI().ExamRoomComputerUI().gameObject.SetActive(true);
		}
		
		//remove a busy counter
		IsBusy(-1);

		currentOfficeObject = null;
		currentPatient = null;
		waitingAtExamRoom = false;

	}


	/// <summary>
	/// Converse with the patient for a period of time, and then open UI
	/// </summary>
	/// <returns></returns>
	//private IEnumerator CheckVitals()
	//{
	//	//set busy to true so no actions can be taken/made
	//	IsBusy(1);

	//	//make the patient update the player's score.
	//	currentPatient.PatientPatienceScore();

	//	//Make sure that the nurse is facing the computer/patient.
	//	(this as Nurse).NurseAnimation("FaceBack", true, false);

	//	//tell the patient to stop losing patience
	//	currentPatient.PatientToggleCountdown(true);

	//	//check if hands are clean. Gain/Lose points based on result.
	//	if (MyManager)
	//	{

	//		if (clean)
	//		{
	//			MyManager.UpdateSatisfactionScore(1);
	//		}
	//		else
	//		{
	//			MyManager.UpdateSatisfactionScore(-2);
	//		}
	//	}
		
	//	//make sure the patient is no longer highlighted
	//	currentPatient.PatientAnimation("Highlight", false, false);

	//	for (int i = 0; i < 2; i++)
	//	{
	//		//make sure the patient's speech bubble is off.
	//		currentPatient.PatientAnimation("Talking", false, false);

	//		//Nurse says something to patient
	//		NurseAnimation("Talking", false, true);
			
	//		//wait for time
	//		yield return new WaitForSeconds(.75f);
			
	//		//make sure the nurse's speech bubble is off
	//		NurseAnimation("Talking", false, false);
			
	//		//patient responds to nurse
	//		currentPatient.PatientAnimation("Talking", false, true);
			
	//		//wait for time
	//		yield return new WaitForSeconds(.75f);
	//	}
		
	//	//turn both animations off just in case.
	//	currentPatient.PatientAnimation("Talking", false, false);
	//	NurseAnimation("Talking", false, false);

	//	//set hands to dirty
	//	IsClean(-1);
		
	//	//update the patient's status
	//	currentPatient.PatientStatusUpdate("VitalsComplete");

	//	//Move over to Exam Room Computer, and open the popup.
	//	PersonMove((currentOfficeObject as ExamRoom).Computer().OfficeObjectLocationNurse(),"ExamRoomComputer",false, (currentOfficeObject as ExamRoom).Computer());
		
	//	//remove a busy counter
	//	IsBusy(-1);

	//	currentOfficeObject = null;
	//	currentPatient = null;
		
	//}


	/// <summary>
	/// //wait period of time, set hands clean = true
	/// </summary>
	/// <returns></returns>
	private IEnumerator WashHands()
	{
		IsBusy(1);
		if (currentOfficeObject)
		{
			//un-highlight the sink
			currentOfficeObject.Highlight(false);
		}
		if (anim)
		{
			//turn to face the sink
			(this as Nurse).NurseAnimation("FaceBack", true, false);
			
		}

		//Play hand washing sound
		if (SoundManager._SoundManager)
		{
			SoundManager._SoundManager.PlaySound("WashHands");
		}

		//wait for time
		yield return new WaitForSeconds(.5f);
		//set clean hands to true.
		IsClean(1);
		if (anim)
		{
			//return to normal animation
			(this as Nurse).NurseAnimation("FaceFront", true, false);
		}

		if (currentOfficeObject)
		{
			//highlight the sink again.
			currentOfficeObject.Highlight(true);
		}
		//remove a counter from busy
		IsBusy(-1);

	}

	/// <summary>
	/// Set the Nurse and Patient that have arrived at the Exam Room.
	/// </summary>
	/// <param name="p">Patient or Nurse</param>
	/// <param name="o">ExamRoom</param>
	public void NurseExamRoomArrival(Person p, OfficeObject o = null)
	{
		if (p.tag == "Patient")
		{
			//Debug.Log("ExamRoomArrival : Patient " + p.gameObject + " for the Nurse " + gameObject);
			currentPatient = (p as Patient);
			if (o) { currentOfficeObject = o; }
		}
		else if (p.tag == "Nurse")
		{
			//Debug.Log("ExamRoomArrival : Nurse " + gameObject);
			waitingAtExamRoom = true;
			if (o) { currentOfficeObject = o; }
		}

		if (currentPatient && waitingAtExamRoom)
		{
			//Debug.Log("ExamRoomArrival: Both Conditions have been met");
			NursePerformAction("ExamRoomSetup", currentOfficeObject, currentPatient);
		}
	}
	#endregion


	#region Statuses
	/// <summary>
	/// Set the whether or not the nurse is busy, and return the status.
	/// </summary>
	/// <param name="b"> 0 = Just Checking. -1 = Set Busy to false. 1 = Set Busy to true.</param>
	/// <returns> True - Yes, False - No.</returns>
	public bool IsBusy(int b = 0)
	{
		//*To further explain the reason that busy is an integer:
		/// Movement can determine whether or not the Nurse is busy
		/// Interfaces/popups can also determine whether or not the nurse is busy.
		/// 
		/// If busy was a bool:
		/// (1) Opening an Interface would set busy to true.
		/// While this interface is open, the player may click a button that moves the nurse to another location.
		/// (2) This movement would then set the nurse to busy.
		/// (3) After the interface is finished executing all of it's statements, it will close, and tell the nurse it's no longer busy.
		/// In this situation, the nurse would be moving, but no longer be busy. Since it's no longer busy, the player could attempt to take other actions as well and these actions would be allowed since the nurse is indeed not busy.
		/// (4) However, the nurse should remain busy until they have stopped moving/arrived at their destination at which point, busy will again be set to true.
		/// 
		/// This above situation essentially does:
		/// (1) Busy = true.
		/// (2) Busy = true.
		/// (3) Busy = false
		/// (4) Busy = false.
		/// 
		/// However, this situation should, and now does yield:
		/// (1) Busy = true.
		/// (2) Busy = true.
		/// (3) Busy = true.
		/// (4) Busy = false.
		//*
	
		if (b == 0)
		{
			//do nothing, 
		}
		else if (b < 0)
		{
			busy--;
		}
		else if (b > 0)
		{
			busy++; ;
		}

		return !(busy == 0);
	}

	/// <summary>
	/// Return whether or not the Nurse's hands are clean.
	/// </summary>
	/// <param name="c">1 = clean, -1 = dirty, 0 = Just Checking</param>
	/// <returns>True for clean, false for dirty.</returns>
	public bool IsClean(int c = 0)
	{
		if (c > 0)
		{
			clean = true;
		}
		else if (c < 0)
		{
			clean = false;
		}
		if (MyManager)
		{
			MyManager.MySink().CleanHandsPoster(clean);
		}
		return clean;
	}

	#endregion
}
