using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ExamRoomComputer : OfficeObject {

	public float bloodworkTimeAlloted;
	public Transform bloodworkBar;

	private ExamRoom er;
	
	private float bloodworkTimeUsed;
	private bool bloodworkSent;
	private int hashNoBloodWork = Animator.StringToHash("NoBloodWork");
	private int hashBloodWorkInProgress = Animator.StringToHash("BloodWorkInProgress");
	private int hashBloodWorkFinished = Animator.StringToHash("BloodWorkFinished");

	// Use this for initialization
	void Start () {
		
		
        OfficeObjectInitialize();
		InitializeExamRoomComputer();
	}

	private void InitializeExamRoomComputer()
	{
		tag = "ExamRoomComputer";
		bloodworkTimeUsed = 0;
		bloodworkSent = false;
		//tell the animator to stop displaying bloodwork.
		ResetBloodwork();
	}

	// Update is called once per frame
	void Update () {
		if (bloodworkSent)
		{
			bloodworkTimeUsed += Time.deltaTime;
			bloodworkBar.localScale = new Vector2(bloodworkTimeUsed / bloodworkTimeAlloted, bloodworkBar.localScale.y);

			if (bloodworkTimeUsed >= bloodworkTimeAlloted)
			{
				//play Bloodwork Completed
				if (SoundManager._SoundManager)
				{
					SoundManager._SoundManager.PlaySound("BloodworkComplete");
				}

				//reset bloodwork sent
				bloodworkSent = false;

				anim.SetTrigger(hashBloodWorkFinished);

				//inform the patient of status change/update.
				
				MyExamRoom().MyPatient.PatientStatusUpdate("Diagnosis");
			}
		}
	}

    void OnMouseOver()
    {
		//verify that nurse is not currently busy. Verify that I have a reference to a patient
		if (!Manager.MyNurse.IsBusy() && er.MyPatient)
		{
			////Highlight this object.
			//Highlight(true);

			//Manager.ManagerMouseOver(true);
			if (Input.GetMouseButtonUp(0))
			{
				Manager.MyNurse.PersonMove(locationNurse, tag, false, this);

				//Play a sound
				if (SoundManager._SoundManager)
				{
					SoundManager._SoundManager.PlaySound("Click");
				}
			}
		}
        
    }

    void OnMouseExit()
    {
        ////Manager.ManagerMouseOver(false);
		//Highlight(false);
    }

	/// <summary>
	/// Return the Exam Room I am part of.
	/// <param name="e"> Exam Room to set or nothing to retrieve.</param>
	/// </summary>
	public ExamRoom MyExamRoom(ExamRoom e = null)
	{
		if (e)
		{
			er = e;
		}
		return er;
	}

	/// <summary>
	/// Begin the countdown for bloodwork
	/// </summary>
	public void SendBloodwork()
	{
		anim.SetTrigger(hashBloodWorkInProgress);
		//Debug.Log("Bloodwork In Progress");

		bloodworkTimeUsed = 0;
		bloodworkSent = true;
		
		MyExamRoom().MyPatient.PatientStatusUpdate("BloodworkWaiting");
	}

	/// <summary>
	/// Called when the patient leaves the exam room. Resets the status of bloodwork.
	/// </summary>
	public void ResetBloodwork()
	{
		bloodworkTimeUsed = 0;
		bloodworkSent = false;
		
		anim.SetTrigger(hashNoBloodWork);
	}
}
