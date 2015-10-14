using UnityEngine;
using System.Collections;

public class OfficeObject : MonoBehaviour {

    public Vector2 locationNurse;
	[HideInInspector]
    public Animator anim;
	private int hashHighlight = Animator.StringToHash("Highlight");
    //private int hashReady = Animator.StringToHash("Ready");
    //private int hashMouseOver = Animator.StringToHash("MouseOver");
    //not sure if stateready is needed at the moment, but ill keep it for now
    private bool stateReady, stateMouseOver;//am I idle, or am I ready. Am I currently being moused over?
    private Manager manager;


    /// <summary>
    /// Return the manager
    /// </summary>
    public Manager Manager
    {
        get { return manager; }
    }

    public void OfficeObjectInitialize()
    {
        anim = GetComponent<Animator>();
        if(GameObject.Find("Manager")) manager = GameObject.Find("Manager").GetComponent<Manager>();
    }

    

    public Vector2 OfficeObjectLocationNurse()
    {
        return locationNurse;
    }

    public bool OfficeObjectMousedOver()
    {
        return stateMouseOver;
    }

    public bool OfficeObjectReady()
    {
        return stateReady;
    }

	/// <summary>
	/// Highlight this officeobject
	/// </summary>
	/// <param name="on"> True = Hightlight, False = Turn Off Highlight</param>
	public void Highlight(bool on)
	{
		if (anim)
		{
			anim.SetBool(hashHighlight, on);
		}
	}

	/// <summary>
	/// Set the object as ready or idle
	/// </summary>
	/// <param name="ready">True = ready, false = idle</param>
	public void OfficeObjectSetReadyState(bool ready)
	{
		stateReady = ready;
		//if (anim != null)
		//{
		//	anim.SetBool(hashReady, ready);
		//}

	}

	///// <summary>
	///// Inform the OfficeObject that it's patient is currently being moused over.
	///// </summary>
	//public void OfficeObjectMouseEnter()
	//{
	//	stateMouseOver = true;
	//	if (anim != null)
	//	{
	//		anim.SetBool(hashMouseOver, true);
	//	}
        
	//}

//	/// <summary>
//	/// Inform the OfficeObject that it's patient is no longer being moused over.
//	/// </summary>
//	public void OfficeObjectMouseExit()
//	{
//		stateMouseOver = false;
//		if (anim != null)
//		{
//			anim.SetBool(hashMouseOver, false);
//		}
//	}
}
