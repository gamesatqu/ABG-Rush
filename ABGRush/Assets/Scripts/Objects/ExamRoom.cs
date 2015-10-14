using UnityEngine;
using System.Collections;

public class ExamRoom : PatientObject {

    //public UI_ExamRoom uiExamRoom;
	public ExamRoomComputer computer;

	// Use this for initialization
	void Start () {
        tag = "ExamRoom";
        OfficeObjectInitialize();
		computer.MyExamRoom(this);
        //MyUI = uiExamRoom;
	}

	/// <summary>
	/// Return the ExamRoom Computer attached to this room.
	/// </summary>
	/// <returns></returns>
	public ExamRoomComputer Computer()
	{
		return computer;
	}

}
