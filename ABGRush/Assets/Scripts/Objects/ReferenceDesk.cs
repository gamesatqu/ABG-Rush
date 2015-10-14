using UnityEngine;
using System.Collections;

public class ReferenceDesk : OfficeObject {

	public GameObject referenceDeskUI;
	//private Animator anim;

	void Awake()
	{
		//anim = GetComponent<Animator>();
	}

	// Use this for initialization
	void Start () {
		//if (anim)
		//{
		//	anim.SetBool("Highlight", true);
		//}

		tag = "ReferenceDesk";
		if (referenceDeskUI)
		{
			referenceDeskUI.SetActive(false);
		}
		OfficeObjectInitialize();
		Highlight(true);
	}

	void OnMouseOver()
	{
		//check to see if the nurse is currently busy. If the nurse is not busy...
		if (!Manager.MyNurse.IsBusy())
		{
			
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

	/// <summary>
	/// Open up the Reference Desk Computer
	/// </summary>
	public void OpenReferenceDesk()
	{
		//pause the game
		Time.timeScale = 0f;
		referenceDeskUI.SetActive(true);
	}
}
