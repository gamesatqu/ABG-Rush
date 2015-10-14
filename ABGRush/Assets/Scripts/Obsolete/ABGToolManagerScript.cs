using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class ABGToolManagerScript : MonoBehaviour {

    /// Manage and control all components of the Tool
    /// 
    public bool practiceMode;

    public Color col_Wrong, col_Right, col_Reset;//colors that the answer boxes will show.
    public Text value_TextpH, value_TextCO2, value_TextHCO3, textplayer_RespiratoryMetabolic, textplayer_AcidosisAlkalosis, textplayer_Compensation, textanswer_RespiratoryMetabolic, textanswer_AcidosisAlkalosis, textanswer_Compensation;
    public Image imganswer_RespiratoryMetabolic, imganswer_AcidosisAlkalosis, imganswer_Compensation;// each of the images will change their color 
    public DragAndDropManagerScript tic_tac_toe_Table;
    public GameObject helpPanel;
    public List<ABGToolSliderScript> helpSliders;

    private string resetText_RespiratoryMetabolic, resetText_AcidosisAlkalosis, resetText_Compensation;

    private List<Text> textAnswers;//all of the answer texts
    private List<Image> imageAnswers;//all of the answer images
    private bool ready = false;
    

	// Use this for initialization
	void Start () {
        
        Setup();

        //if practice mode, create a new problem
        if (practiceMode)
        {
            Reset();
        }
	}


    /// <summary>
    /// make sure all required components are referenced and linked.
    /// </summary>
    void Setup()
    {
        //get the Reset strings for the player answer boxes.
        resetText_RespiratoryMetabolic = textplayer_RespiratoryMetabolic.text;
        resetText_AcidosisAlkalosis = textplayer_AcidosisAlkalosis.text;
        resetText_Compensation = textplayer_Compensation.text;

        //create the lists of text answers and images and sliders.
        textAnswers = new List<Text>();
        textAnswers.Add(textanswer_RespiratoryMetabolic); textAnswers.Add(textanswer_AcidosisAlkalosis); textAnswers.Add(textanswer_Compensation);

        imageAnswers = new List<Image>();
        imageAnswers.Add(imganswer_RespiratoryMetabolic); imageAnswers.Add(imganswer_AcidosisAlkalosis); imageAnswers.Add(imganswer_Compensation);

        //helpSliders = new List<ABGToolSliderScript>();

        //turn the help screen off. Change this later for animation purposes.
        helpPanel.SetActive(false);

        ready = true;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Choose_RespiratoryMetabolic(Text t)
    {
        //set the text of the player's answer.
        textplayer_RespiratoryMetabolic.text = t.text;
    }

    public void Choose_AcidosisAlkalosis(Text t)
    {
        //set the text of the player's answer.
        textplayer_AcidosisAlkalosis.text = t.text;
    }

    public void Choose_Compensation(Text t)
    {
        //set the text of the player's answer.
        textplayer_Compensation.text = t.text;
    }

    public void Reset()
    {
        if (!ready)
        {
            Setup();
        }

        //reset the text
        textplayer_RespiratoryMetabolic.text = resetText_RespiratoryMetabolic;
        textplayer_AcidosisAlkalosis.text = resetText_AcidosisAlkalosis;
        textplayer_Compensation.text = resetText_Compensation;

        //reset the answer colors to clear
        foreach (Image i in imageAnswers)
        {
            i.color = col_Reset;
        }
        //reset the answer text to empty
        foreach (Text t in textAnswers)
        {
            t.text = "";
        }

        //reset the table
        tic_tac_toe_Table.ResetTable();

        //get new data for next problem
        if (practiceMode)
        {
            //use the functions inside of this script
            //randomAlive();
            //checkCondition();

            GenerateDiagnosis();

            Debug.Log("Answers for each are currently set to: \n " + answerRespMet + "\n" + answerAcidAlk + "\n" + answerCompensation);

            //set the data for the values of ph, co2, and hco3.
            value_TextCO2.text = co2.ToString("F2");
            value_TextHCO3.text = hco3.ToString("F2");
            value_TextpH.text = ph.ToString("F2");
        }
        

        //reset the sliders
        foreach (ABGToolSliderScript s in helpSliders)
        {
            s.Reset();
        }
    }



    /// <summary>
    /// Return the answer the player provided
    /// called by an outside script
    /// </summary>
    /// <returns></returns>
    public string CheckAnswer_RespiratoryMetabolic()
    {
        return textplayer_RespiratoryMetabolic.text;
    }

    public string CheckAnswer_AcidosisAlkalosis()
    {
        return textplayer_AcidosisAlkalosis.text;
    }

    public string CheckAnswer_Compensation()
    {
        return textplayer_Compensation.text;
    }


    /// <summary>
    /// Allow other script to set the answer
    /// </summary>
    /// <param name="correct"></param>
    /// <param name="ans"></param>
    public void SetAnswer_RespiratoryMetabolic(bool correct, string ans)
    {
        textanswer_RespiratoryMetabolic.text = ans;
        Debug.Log("The answer for RespMet was " + ans);
        if (correct)
        {
            imganswer_RespiratoryMetabolic.color = col_Right;
        }
        else
        {
            imganswer_RespiratoryMetabolic.color = col_Wrong;
        }
    }

    public void SetAnswer_AcidosisAlkalosis(bool correct, string ans)
    {
        textanswer_AcidosisAlkalosis.text = ans;
        Debug.Log("The answer for AcidAlk was " + ans);
        if (correct)
        {
            imganswer_AcidosisAlkalosis.color = col_Right;
        }
        else
        {
            imganswer_AcidosisAlkalosis.color = col_Wrong;
        }
    }

    public void SetAnswer_Compensation(bool correct, string ans)
    {
        Debug.Log("The answer for Compensation was " + ans);
        textanswer_Compensation.text = ans;
        if (correct)
        {
            imganswer_Compensation.color = col_Right;
        }
        else
        {
            imganswer_Compensation.color = col_Wrong;
        }
    }

    /// <summary>
    /// Called by the submit button
    /// take in all the answers provided by the player and send them to the ABG logic controller/manager
    /// if in practice mode, send the information to functions in this script
    /// 
    /// </summary>
    public void SubmitDiagnosis()
    {

        if (practiceMode)
        {
            SelfDiagnose();
        }
    }

    public void Help(Toggle t)
    {
        helpPanel.SetActive(t.isOn);
        if (t)
        {
            foreach (ABGToolSliderScript s in helpSliders)
            {
                s.Reset();
            }
        }
    }


    ///Summary
    ///Everything below is for the practice mode. 
    ///if practice mode is on, then the following things will occur.
    ///The following is ripped from the old boardLogic C# script and should probably be modified/updated at some point.
    ///

    private string answerRespMet, answerAcidAlk, answerCompensation;
    private float ph, co2, hco3;

  //  private void randomAlive() //randoms a living condition
  //  {
  //      ph = Random.Range(7.25f, 7.55f);
  //      co2 = Random.Range(25, 55);
  //      hco3 = Random.Range(18, 30);
  //  }

  //  void checkCondition()
  //  {
        
        
  //      //Respiratory Acidosis
  //      if (ph < 7.35 && (hco3 <= 28 && hco3 >= 22) && co2 > 45)
  //      {
  //          answerRespMet = "Respiratory";
  //          answerAcidAlk = "Acidosis";
  //          answerCompensation = "Uncompensated";
  //          //rightAnswer = "Uncompensated" + answerRespMet + answerAcidAlk;
  //      }
  //      else if (ph < 7.35 && hco3 > 26 && co2 > 45)
  //      {
  //          answerRespMet = "Respiratory";
  //          answerAcidAlk = "Acidosis";
  //          answerCompensation = "Partial Compensation";
  //          //rightAnswer = "Partial Compensation" + answerRespMet + answerAcidAlk;
  //      }
  //      else if ((ph <= 7.45) && (ph >= 7.35) && hco3 > 26 && co2 > 45)
  //      {
  //          answerRespMet = "Respiratory";
  //          answerAcidAlk = "Acidosis";
  //          answerCompensation = "Compensated";
  //          //rightAnswer = "Compensated" + answerRespMet + answerAcidAlk;
  //      }

  /////Respiratory Alkalosis

  //      else if (ph > 7.45 && (hco3 <= 28 && hco3 >= 22) && co2 < 35)
  //      {
  //          answerRespMet = "Respiratory";
  //          answerAcidAlk = "Alkalosis";
  //          answerCompensation = "Uncompensated";
  //          //rightAnswer = "Uncompensated" + answerRespMet + answerAcidAlk;
  //      }
  //      else if (ph > 7.45 && hco3 < 22 && co2 < 35)
  //      {
  //          answerRespMet = "Respiratory";
  //          answerAcidAlk = "Alkalosis";
  //          answerCompensation = "Partial Compensation";
  //          //rightAnswer = "Partial Compensation" + answerRespMet + answerAcidAlk;
  //      }
  //      else if ((ph <= 7.45) && (ph >= 7.35) && hco3 < 22 && co2 < 35)
  //      {
  //          answerRespMet = "Respiratory";
  //          answerAcidAlk = "Alkalosis";
  //          answerCompensation = "Compensated";
  //          //rightAnswer = "Compensated" + answerRespMet + answerAcidAlk;
  //      }
  //      //METABOLIC Acidosis

  //      else if (ph < 7.35 && hco3 < 22 && (co2 >= 35 && co2 <= 45))
  //      {
  //          answerRespMet = "Metabolic";
  //          answerAcidAlk = "Acidosis";
  //          answerCompensation = "Uncompensated";
  //          //rightAnswer = "Uncompensated" + answerRespMet + answerAcidAlk;
  //      }
  //      else if (ph < 7.35 && hco3 < 22 && co2 < 35)
  //      {
  //          answerRespMet = "Metabolic";
  //          answerAcidAlk = "Acidosis";
  //          answerCompensation = "Partial Compensation";
  //          //rightAnswer = "Partial Compensation" + answerRespMet + answerAcidAlk;
  //      }
  //      else if ((ph <= 7.45) && (ph >= 7.35) && hco3 < 22 && co2 < 35)
  //      {
  //          answerRespMet = "Metabolic";
  //          answerAcidAlk = "Acidosis";
  //          answerCompensation = "Compensated";
  //          //rightAnswer = "Compensated" + answerRespMet + answerAcidAlk;
  //      }

  ////METABOLIC Alkalosis
  //      else if (ph > 7.45 && hco3 > 26 && (co2 >= 35 && co2 <= 45))
  //      {
  //          answerRespMet = "Metabolic";
  //          answerAcidAlk = "Alkalosis";
  //          answerCompensation = "Uncompensated";
  //          //rightAnswer = "Uncompensated" + answerRespMet + answerAcidAlk;
  //      }

  //      else if (ph > 7.45 && hco3 > 26 && co2 > 45)
  //      {
  //          answerRespMet = "Metabolic";
  //          answerAcidAlk = "Alkalosis";
  //          answerCompensation = "Partial Compensation";
  //          //rightAnswer = "Partial Compensation" + answerRespMet + answerAcidAlk;
  //      }

  //      else if ((ph <= 7.45) && (ph >= 7.35) && hco3 > 26 && co2 > 45)
  //      {
  //          answerRespMet = "Metabolic";
  //          answerAcidAlk = "Alkalosis";
  //          answerCompensation = "Compensated";
  //          //rightAnswer = "Compensated" + answerRespMet + answerAcidAlk;
  //      }

  //      else
  //      {//impossible value
  //          randomAlive();
            
  //          //answerRespMet = "Is";
  //          // answerAcidAlk = "Impossible";
  //          //answerCompensation = "This";

  //      }


  //  }

    /// <summary>
    /// Used for practice mode
    /// Compare the players answers to the real answers
    /// </summary>
    private void SelfDiagnose()
    {
        
        SetAnswer_RespiratoryMetabolic( answerRespMet == (CheckAnswer_RespiratoryMetabolic()), answerRespMet);
        SetAnswer_AcidosisAlkalosis(answerAcidAlk == (CheckAnswer_AcidosisAlkalosis()), answerAcidAlk);
        SetAnswer_Compensation(answerCompensation == (CheckAnswer_Compensation()), answerCompensation);
    }




    /// <summary>
    /// The next several functions are used to create a diagnosis for the player to answer
    ///
    /// </summary>

    private float lowestPHValue = 7.24f, highestPHValue = 7.58f, lowNeutralPHValue = 7.35f, highNeutralPHValue = 7.45f, lowestCO2Value = 20f, lowNeutralCO2Value = 35f, highNeutralCO2Value = 45f, highestCO2Value = 64f, lowestHCO3Value = 14f, lowNeutralHCO3Value = 22f, highNeutralHCO3Value = 26f, highestHCO3Value = 42f;

    /// <summary>
    /// Generate a diagnosis. Either random, or a selected one.
    /// </summary>
    /// <param name="d">The diagnosis wanted. Providing no param returns a random diagnosis</param>
    private void GenerateDiagnosis(int d = -1)
    {
        if (d == -1)
        {
            d = Random.Range(0, 12);
        }

        switch (d)
        {
            //set the answer strings, and set the values.
            //respiratory acidosis
            case 0: answerRespMet = "Respiratory"; answerAcidAlk = "Acidosis"; answerCompensation = "Uncompensated"; ph = GenDiagnosisValues("PH",0); co2 = GenDiagnosisValues("CO2",2); hco3 = GenDiagnosisValues("HCO3",1); break;
            case 1: answerRespMet = "Respiratory"; answerAcidAlk = "Acidosis"; answerCompensation = "Partial Compensation"; ph = GenDiagnosisValues("PH", 0); co2 = GenDiagnosisValues("CO2", 2); hco3 = GenDiagnosisValues("HCO3", 2); break;
            case 2: answerRespMet = "Respiratory"; answerAcidAlk = "Acidosis"; answerCompensation = "Compensated"; ph = GenDiagnosisValues("PH", 1); co2 = GenDiagnosisValues("CO2", 2); hco3 = GenDiagnosisValues("HCO3", 2); break;
            //respiratory alkalosis
            case 3: answerRespMet = "Respiratory"; answerAcidAlk = "Alkalosis"; answerCompensation = "Uncompensated"; ph = GenDiagnosisValues("PH", 2); co2 = GenDiagnosisValues("CO2", 0); hco3 = GenDiagnosisValues("HCO3", 1); break;
            case 4: answerRespMet = "Respiratory"; answerAcidAlk = "Alkalosis"; answerCompensation = "Partial Compensation"; ph = GenDiagnosisValues("PH", 2); co2 = GenDiagnosisValues("CO2", 0); hco3 = GenDiagnosisValues("HCO3", 0); break;
            case 5: answerRespMet = "Respiratory"; answerAcidAlk = "Alkalosis"; answerCompensation = "Compensated"; ph = GenDiagnosisValues("PH", 1); co2 = GenDiagnosisValues("CO2", 0); hco3 = GenDiagnosisValues("HCO3", 0); break;
            //metabolic acidosis
            case 6: answerRespMet = "Metabolic"; answerAcidAlk = "Acidosis"; answerCompensation = "Uncompensated"; ph = GenDiagnosisValues("PH", 0); co2 = GenDiagnosisValues("CO2", 1); hco3 = GenDiagnosisValues("HCO3", 0); break;
            case 7: answerRespMet = "Metabolic"; answerAcidAlk = "Acidosis"; answerCompensation = "Partial Compensation"; ph = GenDiagnosisValues("PH", 0); co2 = GenDiagnosisValues("CO2", 0); hco3 = GenDiagnosisValues("HCO3", 0); break;
            case 8: answerRespMet = "Metabolic"; answerAcidAlk = "Acidosis"; answerCompensation = "Compensated"; ph = GenDiagnosisValues("PH", 1); co2 = GenDiagnosisValues("CO2", 0); hco3 = GenDiagnosisValues("HCO3", 0); break;
            //metabolic alkalosis
            case 9: answerRespMet = "Metabolic"; answerAcidAlk = "Alkalosis"; answerCompensation = "Uncompensated"; ph = GenDiagnosisValues("PH", 2); co2 = GenDiagnosisValues("CO2", 1); hco3 = GenDiagnosisValues("HCO3", 2); break;
            case 10: answerRespMet = "Metabolic"; answerAcidAlk = "Alkalosis"; answerCompensation = "Partial Compensation"; ph = GenDiagnosisValues("PH", 2); co2 = GenDiagnosisValues("CO2", 2); hco3 = GenDiagnosisValues("HCO3", 2); break;
            case 11: answerRespMet = "Metabolic"; answerAcidAlk = "Alkalosis"; answerCompensation = "Compensated"; ph = GenDiagnosisValues("PH", 1); co2 = GenDiagnosisValues("CO2", 2); hco3 = GenDiagnosisValues("HCO3", 2); break;
        }
    }


    /// <summary>
    /// Generate number values for each of the variables.
    /// </summary>
    /// <param name="value">name of the value ie. PH, CO2, HCO3</param>
    /// <param name="LowMedHigh">0 for acid, 1 for neutral, 2 for basic</param>
    /// <returns></returns>
    private float GenDiagnosisValues(string value, int LowMedHigh)
    {
        float lowest = 0f, low = 0f, high = 0f, highest = 0f;
        if (value == "PH")
        {
            lowest = lowestPHValue; low = lowNeutralPHValue; high = highNeutralPHValue; highest = highestPHValue;
        }
        else if (value == "CO2")
        {
            lowest = lowestCO2Value; low = lowNeutralCO2Value; high = highNeutralCO2Value; highest = highestCO2Value;
        }
        else if (value == "HCO3")
        {
            lowest = lowestHCO3Value; low = lowNeutralHCO3Value; high = highNeutralHCO3Value; highest = highestHCO3Value;
        }


        if (LowMedHigh == 0)
        {
            return Random.Range(lowest, low);
        }
        else if (LowMedHigh == 1)
        {
            return Random.Range(low, high);
        }
        else if (LowMedHigh == 2)
        {
            return Random.Range(high, highest);
        }

        Debug.LogWarning("Gen CO2 Did not receive a valid Param");
        return -50f;
    }
}
