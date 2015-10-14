using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class SatisfactionBarScript : MonoBehaviour {

    public Slider satisfactionbar;
    public Text displayText, treatedPatientsText;
    public List<SatisfactionBoostTextScript> decrementers, incrementers;


	// Use this for initialization
	void Start () {
        foreach (SatisfactionBoostTextScript s in decrementers)
        {
            s.gameObject.SetActive(false);
        }
        foreach (SatisfactionBoostTextScript s in incrementers)
        {
            s.gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
		//if (Input.GetKeyUp(KeyCode.Space))
		//{
		//	SatisfactionModify(Random.Range(-5, 6));
		//}
	}

    public void SatisfactionUpdate(float value)
    {
        satisfactionbar.value = value;
		Debug.Log("Bar Value has changed to " + value);
		displayText.text = Mathf.CeilToInt(value).ToString();
    }

    /// <summary>
    /// Called when the player has done something that increases or decreases the time they have left
    /// </summary>
    /// <param name="value">Increase the bar by a positive number or decrease it by a negative number.</param>
    public void SatisfactionModify(int value)
    {
        if (value > 0)
        {
            //play an animation or visually display the boost // green
            foreach (SatisfactionBoostTextScript i in incrementers)
            {
                if (!i.gameObject.activeInHierarchy)
                {
                    i.Reactivate("+" + value.ToString());
                    break;
                }
            }
            
        }
        else if (value < 0)
        {
            //play an animation or visually display the decrease // red
            foreach (SatisfactionBoostTextScript d in decrementers)
            
            {
                if (!d.gameObject.activeInHierarchy)
                {
                    d.Reactivate(value.ToString());
                    break;
                }
            }
            
        }
    }

	/// <summary>
	/// Display the current number of treated vs untreated patients.
	/// </summary>
	/// <param name="treated">The number of patients that have been correctly treated.</param>
	/// <param name="seen">The number of patients that have not been treated or wrongly diagnosed.</param>
	public void UpdateTreatedPatients(int treated, int seen)
	{
		string display = "";
		//if the player has a perfect score, display a single digit.
		if (treated == seen)
		{
			display = treated.ToString();
		}
		//if the player does not have a perfect score, display a fraction.
		else if (seen > treated)
		{
			display = treated.ToString() + "/" + seen.ToString();
		}

		treatedPatientsText.text = display;
	}
}
