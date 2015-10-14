using UnityEngine;
using System.Collections;


[RequireComponent(typeof(PolyNavAgent))]
[RequireComponent(typeof(SpriteRenderer))]

/// <summary>
/// The purpose of this class is to be the parent of both the nurse and patients. This will contain functions or variables that both will use.
/// </summary>
public class Person : MonoBehaviour {

    private PolyNavAgent agent;
    private string destinationName;
    private Manager manager;
    private bool moving, patientObject;
    private OfficeObject officeObject;
    private SpriteRenderer sr;

	/// <summary>
	/// The Variables used for when the movement of a person needs to be delayed.
	/// </summary>
	private Vector2 delayedMoveM;
	private string delayedDName;
	private bool delayedPObject = true;
	private OfficeObject delayedOfficeobject = null;



    public Manager MyManager
    {
        get {
			if (!manager) {
				manager = Manager._manager; 
			}
			return manager; 
		}
    }


    /// <summary>
    /// Prepare all variables and components
    /// </summary>
    public void PersonInitialize()
    {
        //Debug.Log("Initializing Person");
        agent = GetComponent<PolyNavAgent>();
        sr = GetComponent<SpriteRenderer>();
		//if (CompareTag("Patient"))
		//{
		//	sr.color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
		//}
        destinationName = "";
		manager = Manager._manager;
        moving = false;
    }

	#region Movement

	public void DelayedPersonMove(float time, Vector2 m, string dName, bool pObject = true, OfficeObject officeobject = null)
	{
		//set the value of each variable.
		delayedMoveM = m;
		delayedDName = dName;
		delayedPObject = pObject;
		delayedOfficeobject = officeobject;
		//Perform the Delay.
		StartCoroutine("DelayedMovement", time);
	}

	private IEnumerator DelayedMovement(float t)
	{
		//wait for the specific period of time.
		yield return new WaitForSeconds(t);
		//Execute movement.
		PersonMove(delayedMoveM, delayedDName, delayedPObject, delayedOfficeobject);
	}

	/// <summary>
    /// Move to specified location. Called by Hotspot or Manager
    /// </summary>
    /// <param name="m">Location to move to</param>
    /// <param name="dName">Tag of destination</param>
    /// <param name="pObject">Is this a Patient Object?</param>
    /// <param name="officeobject">The class</param>
    public void PersonMove(Vector2 m, string dName, bool pObject = true, OfficeObject officeobject = null)
    {
		//if I am a nurse, set myself to busy
		if (gameObject.CompareTag("Nurse"))
		{
			(this as Nurse).IsBusy(-1);
			if (Vector2.Distance(m, transform.position) > Mathf.Abs(0.1f))
			{
				(this as Nurse).NurseAnimation("Walk", true, false);
			}
		}
		else if (gameObject.CompareTag("Patient"))
		{
			//Debug.Log("Setting Patient Walking to True");

			(this as Patient).PatientAnimation("Highlight", false, false);
			(this as Patient).PatientAnimation("Walking", false, true);
		}

		//make sure that I have a reference to my polynav agent.
        if (agent == null)
        {
            agent = GetComponent<PolyNavAgent>();
        }
		

        destinationName = dName;
		//Debug.Log("OfficeObject = " + officeobject);
        if (officeobject) { patientObject = pObject; officeObject = officeobject; }

		//make sure that the agent is enabled, and then set the agent's destination.
		agent.enabled = true;
		
        moving = true;

		agent.SetDestination(m, PersonMovementStatus);
    }

    /// <summary>
    /// Called after a movement action has been taken
    /// </summary>
    /// <param name="moved">True - Reached Destination | False - Did not reach Destination</param>
    private void PersonMovementStatus(bool moved)
    {
		Debug.Log(name + " has arrived at destination " + destinationName);
        //inform self that my status has changed, so if I'm a nurse, I should now be either waiting at triage, waiting at waiting room, or waiting in patient room
        if (moved && gameObject.CompareTag("Patient"))
        {
			Patient p = (this as Patient);
            p.PatientStatusUpdate(destinationName);
            if (p.PatientHotspotGet())
            {
                //p.PatientHotspotGet().OfficeObjectSetReadyState(true);
            }

			if (p.Status() == "ExamRoom")
			{
				manager.MyNurse.NurseExamRoomArrival(p, officeObject);
			
			}
                        
        }

        else if (gameObject.CompareTag("Nurse"))
        {
			string status = " ";
			//if we are dealing with a patientobject
			if (patientObject)
			{
				status = (officeObject as PatientObject).MyPatient.Status();

				if (destinationName == "WaitingChair")
				{
					//Turn to the right
					(this as Nurse).NurseAnimation("FaceRight",true,false);
					Debug.Log("Turning to the right");

					//Check for clean hands.
					MyManager.CleanHandsCheck();
					
					//open the UI of the Waiting Chair
					(officeObject as PatientObject).PatientObjectOpenUI();
					
					//Debug.Log("Opened the UI for " + officeObject.name);
				}
				else if (destinationName == "ExamRoom")
				{
					//if the patient is currently waiting to get their vitals checked.
					if (status == "Vitals")
					{
						//the patient is waiting for their vitals to be checked.
						//perform vital sign check, another conversation between nurse and patient, and then popup.
						(this as Nurse).NursePerformAction("Vitals", officeObject, (officeObject as PatientObject).MyPatient);

					}
					//if the patient is currently waiting to get a diagnosis
					else if (status == "Diagnosis")
					{
						//the patient is waiting to be diagnosed.
						//RON COME BACK AND CHANGE THIS
						//do nothing.. or move to the computer
					}
					//else if (status == "ExamRoom")
					//{
						//Moved down below
					//}
				}
			}
			else
			{
				Debug.Log("DestinationName : " + destinationName);
				if (destinationName == "ExamRoomComputer")
				{
					//Make sure that the nurse faces the computer.
					(this as Nurse).NurseAnimation("FaceBack", true, false);

					//get the patient
					//Debug.Log("OfficeObject = " + officeObject);
					Patient p = (officeObject as ExamRoomComputer).MyExamRoom().MyPatient;
					//Debug.Log("The patient the nurse is seeing is: " + p + " and their status is " + p.Status());

					//If the status is exam room or waiting chair, the patient is on their way and the nurse must setup and prepare the computer.
					if (p.Status() == "ExamRoom" || p.Status() == "WaitingChair")
					{
						//same /similar process as vitals
						//(this as Nurse).NursePerformAction("ExamRoomSetup", officeObject, p);
						//Debug.Log("Nurse setting up arrival");
						(this as Nurse).NurseExamRoomArrival(this, officeObject);
						
					}
					else
					{
						//Check for clean hands.
						MyManager.CleanHandsCheck();

						//Open up the Interface for the computer.
						manager.GamePlayUI().ExamRoomComputerUI().SetPatient((officeObject as ExamRoomComputer).MyExamRoom().MyPatient);
						manager.GamePlayUI().ExamRoomComputerUI().gameObject.SetActive(true);
					}

					//open up the examroom computer with the proper information
					//The method should take in the patient, and open up the interface based on the patient's status.

					//if the status of the patient is VitalsComplete, display 100% of information and pose the question to get bloodwork.
					//if the status of the patient is vitals, display only the story, name and dob.
					//if the status of the patient is bloodwork, display 100% of the information.
					//if the status of the patient is diagnosis, display the abg tool
				}
				else if (destinationName == "Sink")
				{
					(this as Nurse).NursePerformAction("Wash Hands",officeObject);
				}
				else if (destinationName == "ReferenceDesk")
				{
					//Make sure that the nurse is facing the reference desk
					(this as Nurse).NurseAnimation("FaceFront", true, false);

					//open up the reference desk
					(officeObject as ReferenceDesk).OpenReferenceDesk();
					//pause the game?
				}
			}
            patientObject = false;
            officeObject = null;
            agent.enabled = false;

			(this as Nurse).IsBusy(1);
        }

        moving = false;
    }

    public bool Moving()
    {
        return moving;
    }

	#endregion

	public void PersonUpdate(SpriteRenderer r = null)
    {
        //if (moving)
        //{
        //Debug.Log("Person Update from " + name);
		int so = -1 * (Mathf.CeilToInt(transform.position.y * 100f));
            sr.sortingOrder = so;
			if (r)
			{
				r.sortingOrder = so -1;

			}
        //}
    }
}
