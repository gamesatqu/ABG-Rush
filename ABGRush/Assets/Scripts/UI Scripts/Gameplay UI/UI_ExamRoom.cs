using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Control an array/list of UI objects that are updated and used accordingly based on the status of the patient.
/// </summary>

public class UI_ExamRoom : UI_Patient {




	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnEnable()
	{
		//tell the nurse that they are busy.
		if (MyManager != null)
		{
			MyManager.MyNurse.IsBusy(1);
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
