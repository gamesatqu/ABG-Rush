using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ABGToolSliderScript : MonoBehaviour {

    /// <summary>
    /// Change the color of the slider based on the values
    /// Possibly update the TTT board if necessary
    /// Only update when a value changes
    /// Reset when the player wants to reset
    /// </summary> 

    public Text myCurValue;//this will display my current value to the player.
    public Color col_Acid, col_Base, col_Neutral;
    public bool inverseCO2;
    private Image handleImage;
    private Slider mySlider;
    private float minVal, maxVal, range, temp;

    void Awake()
    {
        mySlider = GetComponent<Slider>();
        handleImage = mySlider.handleRect.gameObject.GetComponent<Image>();
    }

	// Use this for initialization
	void Start () {
        
        minVal = mySlider.minValue;
        maxVal = mySlider.maxValue;
        
        range = maxVal - minVal;

        Reset();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Reset()
    {
        handleImage.color = col_Neutral;
        mySlider.value = (minVal + maxVal) / 2f;
        myCurValue.text = mySlider.value.ToString("F2");
    }

    public void UpdateSlider()
    {
        temp = (mySlider.value - minVal) / range;
        if (inverseCO2)
        {
            temp = 1f - temp;
        }
        handleImage.color = Color.Lerp(col_Acid, col_Base, temp);
        myCurValue.text = mySlider.value.ToString("F2");
    }

    void OnEnabled()
    {
        Reset();
    }
}
