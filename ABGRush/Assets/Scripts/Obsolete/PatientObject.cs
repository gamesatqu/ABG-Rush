using UnityEngine;
using System.Collections;

public class PatientObject : OfficeObject {


    public Vector2 locationPatient;
    private Patient patient;
    private UI_Patient ui;

    #region Patient

    public Patient MyPatient
    {
        get { return patient; }
    }

    public UI_Patient MyUI
    {
        get { return ui; }
        set { Debug.Log(value.name);
            ui = value;
            if (ui != null) { 
                ui.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Add a new patient
    /// </summary>
    /// <param name="p">Patient</param>
    public void PatientObjectPatientAdd(Patient p)
    {
        patient = p;
        p.PatientHotspot(this);
        p.PersonMove(locationPatient, tag, true, this);

		Debug.Log(gameObject + " has added patient " + p);
        OfficeObjectSetReadyState(true); //handled inside of the person class now.

		//Hotfix for now. Ron Come back and change this!!!
		if (CompareTag("ExamRoom"))
		{
			(this as ExamRoom).Computer().Highlight(true);
		}
    }

    /// <summary>
    /// Remove the current patient
    /// </summary>
    public void PatientObjectPatientRemove()
    {
        patient = null;
        OfficeObjectSetReadyState(false);

		//Hotfix for now. Ron Come back and change this!!!
		if (CompareTag("ExamRoom"))
		{
			(this as ExamRoom).Computer().Highlight(false);
			(this as ExamRoom).Computer().ResetBloodwork();
		}
    }

    /// <summary>
    /// Is this object occupied?
    /// </summary>
    /// <returns>True - occupied, false - free</returns>
    public bool PatientObjectOccupied()
    {
        return patient != null;
    }

    #endregion

    /// <summary>
    /// The location the patient should move to
    /// </summary>
    /// <returns></returns>
    public Vector2 PatientObjectLocationPatient()
    {
        return locationPatient;
    }


    /// <summary>
    /// Open up the UI for whatever the player has recently clicked on.
    /// </summary>
    public void PatientObjectOpenUI()
    {
        //set the patient information for the UI
        Debug.Log("Patient's name is " + patient.name);
        ui.MyPatient = patient;
        //turn the UI on.
        if (ui != null)
        {
            ui.gameObject.SetActive(true);
        }
        
    }
}
