using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Triage : UI_Patient {

    //should be 3 buttons, 4 at the most
    //0 - Send to Exam Room, 1 - Waiting Room, 2 - Leave, 3 - Exit/Cancel
    public Button[] buttonChoices;
    public Text textStory;

    //Add in Name
    //Add in DOB
    
	
	// Update is called once per frame
	void Update () {
        if (MyManager != null)
        {
            buttonChoices[0].interactable = (MyManager.ManagerEmptyExamRoom() != null);
            buttonChoices[1].interactable = (MyManager.ManagerEmptyWaitingChair() != null);
        }
	}

    void OnEnable()
    {
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
        if (MyPatient != null) { MyPatient.PatientToggleCountdown(true); }
        
    }

}
