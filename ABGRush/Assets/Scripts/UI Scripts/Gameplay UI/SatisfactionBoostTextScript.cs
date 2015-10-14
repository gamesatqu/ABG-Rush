using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class SatisfactionBoostTextScript : MonoBehaviour {

    private Animator anim;
    private Text myText;//the textbox we use to display our value
    private int hashActivated = Animator.StringToHash("Activated"), hashPositive = Animator.StringToHash("Positive");
    public bool positive;

    void Awake()
    {
        anim = GetComponent<Animator>();
        myText = GetComponent<Text>();
        if (myText == null)
        {
            Debug.LogWarning("Could not find a text component");
        }
        
    }


	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// called from the animation
    /// </summary>
    public void Deactivate()
    {
        anim.SetBool(hashActivated, false);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// called by the satisfaction bar when it increases or decreases it's value.
    /// </summary>
    /// <param name="val">the amount that was added or taken away</param>
    public void Reactivate(string val)
    {
        //set text
        myText.text = val;
        //become active again
        gameObject.SetActive(true);
        anim.SetBool(hashPositive, positive);
        anim.SetBool(hashActivated, true);
        
    }
}
