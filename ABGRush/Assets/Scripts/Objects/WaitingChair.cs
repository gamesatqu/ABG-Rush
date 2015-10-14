using UnityEngine;
using System.Collections;

public class WaitingChair : PatientObject {

    public UI_WaitingChair uiWaitingchair;

	// Use this for initialization
	void Start () {
        tag = "WaitingChair";
        OfficeObjectInitialize();
        MyUI = uiWaitingchair;
	}
	
}
