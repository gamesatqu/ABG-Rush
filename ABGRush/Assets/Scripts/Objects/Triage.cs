using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Triage : PatientObject {

    //public UITriage uiTriage;//Change this later to be accessed from the gameplay UI script instead of a public variable.
    public Transform[] patientStandingLocation;
    public float setTimeGreeting = 10, setTimeInitialDelay = 2;

    private Animator myAnim;
    private int hashTalking = Animator.StringToHash("Talking");
    private List<Patient> patients;
    private List<Vector2> patientLocations;
    private float timeGreeting, timeInitialDelay;//the amount of time spent greeting the patient. // The amount of time to wait before speaking to patient
    private bool newPatient;//Determine if I am speaking to a new patient. 
    private int stageGreetingTotal, stageGreeting; // what stage of the greeting process am I in? There should be 5 in all. R,P,R,P,R

	// Use this for initialization
	void Start () {
        
        TriageInitialize();
        OfficeObjectInitialize();
        
        //MyUI = uiTriage;

	}

    void TriageInitialize()
    {
        tag = "Triage";
        //initialize the lists
        patients = new List<Patient>();
        patientLocations = new List<Vector2>();
        Debug.Log("Patients: " + patients);
        Debug.Log("Patient Locations: " + patientLocations);
        //populate the patient location list
        foreach (Transform t in patientStandingLocation)
        {
            patientLocations.Add(t.position);
        }

        myAnim = GetComponent<Animator>();

        newPatient = false;
        stageGreetingTotal = 3;
        stageGreeting = 0;

        timeGreeting = setTimeGreeting / stageGreetingTotal;
        timeInitialDelay = setTimeInitialDelay;
    }

    /// <summary>
    /// Called by Manager after spawning a new patient.
    /// </summary>
    /// <param name="p">The New Patient</param>
    public void TriagePatientAdd(Patient p)
    {
        //p.PersonMove(triage.locationPatient, "Triage");

        //Add patient to patient list
        
        //Debug.Log(patients);
        patients.Add(p);
        //Debug.Log(patients.Count);
        //tell patient to move to next open location

        p.PersonMove(patientLocations[patients.Count -1],tag,true);
        //tell patient to wait, and not tick down timer
        p.PatientToggleCountdown(true);
        //determine if I only have 1 patient
        if (patients.Count == 1)
        {
            newPatient = true;
            timeInitialDelay = setTimeInitialDelay;
        }
    }


	
	// Update is called once per frame
	void Update () {
        if (patients.Count > 0)
        {
            TriageGreeting();
        }
        
	}

    private void TriageGreeting()
    {
        if (newPatient)
        {
            timeInitialDelay -= Time.deltaTime;
            if (timeInitialDelay <= 0)
            {
                newPatient = false;
                stageGreeting = 1;
                //turn receptionist talking animation on.
                myAnim.SetBool(hashTalking, true);
            }
        }
        else
        {
            timeGreeting -= Time.deltaTime;
            if (timeGreeting <= 0)
            {
                //the process should go R,P,R,P,R. The Initial R is handled above.
                switch (stageGreeting % 2)
                {
                    case 1: patients[0].PatientAnimation("Talking", false, true); myAnim.SetBool(hashTalking, false); break;
                    case 0: patients[0].PatientAnimation("Talking", false, false); myAnim.SetBool(hashTalking, true); break;
					//case 3: patients[0].PatientAnimation("Talking", false, true); myAnim.SetBool(hashTalking, false); break;
					//case 4: patients[0].PatientAnimation("Talking", false, false); myAnim.SetBool(hashTalking, true); break;
                }
                stageGreeting++;
                if (stageGreeting <= stageGreetingTotal)
                {
                    timeGreeting = setTimeGreeting / stageGreetingTotal;
                }
                else
                {
					//tell the patient to stop talking
					patients[0].PatientAnimation("Talking", false, false);

                    //send the patient to an open waiting chair.
                    WaitingChair wc = Manager.ManagerEmptyWaitingChair();
                    if (wc)
                    {
                        //grab a reference to the patient
                        Patient p = patients[0];
                        //remove the patient from the queue
                        patients.RemoveAt(0);
                        //add the patient to the waiting chair
                        wc.PatientObjectPatientAdd(p);
                        //the waiting chair should tell the patient to move.

                        //patients[0].PersonMove(wc.locationPatient, wc.tag, true, wc);

                        //make each patient move up in the queue
                        TriageUpdateLine();
                        //set greeting time and initial time
                        timeGreeting = setTimeGreeting / stageGreetingTotal;
                        timeInitialDelay = setTimeInitialDelay;
                        if (patients.Count > 0)
                        {
                            //set new patient = true
                            newPatient = true;
                        }
                        //set stage greeting to 0
                        stageGreeting = 0;
                        //turn nurse speech bubble off
                        myAnim.SetBool(hashTalking, false);
                    }
                    else
                    {
                        //what if there are no empty waiting chairs?
                    }
                }
            }
        }
    }

    private void TriageUpdateLine()
    {
        for (int i = 0; i < patients.Count; i++)
        {
            patients[i].PersonMove(patientLocations[i],tag,true);
        }
    }

    //void OnMouseOver()
    //{
    //    if (OfficeObjectReady() && OfficeObjectMousedOver())
    //    {
    //        if (Input.GetMouseButtonDown(0))
    //        {
    //            //tell the nurse to move to location
    //            Manager.MyNurse.PersonMove(locationNurse,tag,true,this);
    //        }

    //    }
    //}

    
}
