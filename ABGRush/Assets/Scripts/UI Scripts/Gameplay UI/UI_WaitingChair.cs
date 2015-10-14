using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_WaitingChair : UI_Patient
{

    //should be 2 buttons, 3 at the most
    //0 - Send to Exam Room, 1 - Pacifiy, 2 - Exit/Cancel
    public Button[] buttonChoices;
    public Text textname, textStory;
	public Image imagePatientID;

    //Add in Name
    //Add in DOB


    // Update is called once per frame
    void Update()
    {
        if (MyManager != null)
        {
            buttonChoices[0].interactable = (MyManager.ManagerEmptyExamRoom() != null);
            buttonChoices[1].interactable = (MyPatient.PatientPacifyAmountLeft() > 0);
        }
    }

    void OnEnable()
    {
		//tell the nurse that they are busy.
		if (MyManager != null)
		{
			MyManager.MyNurse.IsBusy(1);
		}

        if (buttonChoices.Length < 1)
        {
            Debug.LogWarning(name + " is missing buttons in it's UITriage Script");
        }
        if (textStory == null)
        {
            Debug.LogWarning(name + " is missing the textfield to display it's story");
        }
        else
        {
            //textStory.text = MyPatient.
        }
        if (MyPatient != null) {
			//tell the patient to stop counting down.
			MyPatient.PatientToggleCountdown(true);

			//display the patients name
			textname.text = MyPatient.name;
			
			//display portions of the history from the patient.
			textStory.text = MyPatient.MyDiagnosis().Greeting();

			//Image
			imagePatientID.sprite = MyPatient.PatientPhotoID();
		}
        
    }

	void OnDisable()
	{
		//inform the nurse they are no longer busy.
		if (MyManager != null)
		{
			MyManager.MyNurse.IsBusy(-1);
		}
	}

 
}
