using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class UI_Patient : MonoBehaviour {


    private Patient patient;
    private Manager manager;

    /// <summary>
    /// Return the current Patient. Set the current patient. 
    /// </summary>
    public Patient MyPatient
    {
        get { return patient; }
        set { patient = value; }
    }

    public Manager MyManager
    {

        get
        {
            if (manager == null)
            {
				manager = Manager._manager;
            } 
            return manager;
        }
    }

    void Start()
    {
        if (GameObject.Find("Manager"))
        {
            manager = GameObject.Find("Manager").GetComponent<Manager>();
        }
    }



    public void Close(bool continuePatient = false)
    {
        MyPatient.PatientToggleCountdown(false);

		//make sure nurse's hands are dirty since they are no longer continuing with us. 
		if (!continuePatient)
		{
			MyManager.MyNurse.IsClean(-1);
		}
		

        gameObject.SetActive(false);
    }


    #region UI Button Functions


    /// <summary>
    /// Called by button press to send a patient home
    /// </summary>
    public void SendAway()
    {
        MyPatient.PatientLeave();
        Close();
    }

    public void SendWaitingRoom()
    {
        //verify that there is an open waiting chair
        WaitingChair wc = MyManager.ManagerEmptyWaitingChair();
        if (wc != null)
        {
            //remove my patient from it's current hotspot
            MyPatient.PatientHotspotGet().PatientObjectPatientRemove();

            //add patient to it's new hotspot
            wc.PatientObjectPatientAdd(MyPatient);

            //move the patient to the proper location of it's new hotspot
            MyPatient.PersonMove(wc.PatientObjectLocationPatient(), wc.tag);


            Debug.Log("Patient has been given to: " + wc.name);
			//inform the nurse to keep their hands in their current state if clean.
            Close(true);
        }

    }



    /// <summary>
    /// Send the patient to an open exam room. Only called by a button click
    /// </summary>
    public void SendExamRoom()
    {
        //verify that there is an open room
        ExamRoom e = MyManager.ManagerEmptyExamRoom();
        if (e != null)
        {
            //remove my patient from it's current hotspot
            MyPatient.PatientHotspotGet().PatientObjectPatientRemove();

            //add the patient to it's new hotspot
            e.PatientObjectPatientAdd(MyPatient);

			//make the patient update the player's score.
			//MyPatient.PatientPatienceScore();

            //make the patient move to the proper location of it's new hotspot
            MyPatient.PersonMove(e.PatientObjectLocationPatient(), e.tag,true,e);

			//make the nurse move to the proper location of the exam room's computer.
			manager.MyNurse.DelayedPersonMove(.75f,e.Computer().OfficeObjectLocationNurse(),e.Computer().tag, false, e.Computer());
            
            Debug.Log("Patient has been given to: " + e.name);



			//inform the nurse to keep their hands in their current state if clean.
            Close(true);
        }
    }


    public void Pacify()
    {
        if (MyPatient.PatientPacifyAmountLeft() > 0)
        {
            MyPatient.PatientPacify();
            Debug.Log(MyPatient.name + " has been pacified");
            Close(false);
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


    #endregion
}
