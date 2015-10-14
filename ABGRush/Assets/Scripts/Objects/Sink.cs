using UnityEngine;
using System.Collections;

[RequireComponent (typeof (LanguageSprite))]

public class Sink : OfficeObject {

	private LanguageSprite langSprite;

	void Awake()
	{
		langSprite = GetComponent<LanguageSprite>();
	}

	// Use this for initialization
	void Start () {
        OfficeObjectInitialize();
		Highlight(true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseOver()
    {
		//check to see if the nurse is currently busy. If the nurse is not busy...
		if (!Manager.MyNurse.IsBusy())
		{
			//highlight this object.
			//Highlight(true);
			//Manager.ManagerMouseOver(true);
			if (Input.GetMouseButtonUp(0))
			{
				Manager.MyNurse.PersonMove(locationNurse, "Sink", false, this);

				//Play a sound
				if (SoundManager._SoundManager)
				{
					SoundManager._SoundManager.PlaySound("Click");
				}
			}
		}
        
    }

    void OnMouseExit()
    {
        //Manager.ManagerMouseOver(false);
		//stop being highlighted
		//Highlight(false);
    }

	/// <summary>
	/// Update the Clean Hands Poster
	/// </summary>
	/// <param name="clean">True = Clean, False = Dirty</param>
	public void CleanHandsPoster(bool clean)
	{
		//make sure we have access to renderer
		if (langSprite)
		{
			if (clean)
			{
				//the clean sprite should be in slot 0.
				langSprite.SwapSprite(0);
			}
			else
			{
				langSprite.SwapSprite(1);
			}
		}
	}
}
