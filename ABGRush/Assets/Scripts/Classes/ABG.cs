using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;
using System.Text;

public class ABG{

    ///*
    ///The purpose of this class is to:
    /// - Create Diagnoses at startup based on data read from file
    /// - Generate values for each diagnosis. 
    /// - Store all of the different Diagnoses
    /// - Return Random Diagnosis
    //*/

	//variables for xml
	private string fileLocation;//, fileName;
	private XmlReaderSettings xmlreaderSettings;

    private List<string> testStoriesL, testStoriesS, testMedications1, testMedications2, testMedications3, testMedications4, testSymptoms1, testSymptoms2, testSymptoms3, testSymptoms4, testConditions1, testConditions2, testConditions3, testConditions4;

    private List<Diagnosis> diagnoses, diagnosesInUse;
	private float valPHAcidMax = 7.24f, valPHAcidMin = 7.35f, valPHNeutral = 7.4f, valPHBasicMin = 7.45f, valPHBasicMax = 7.58f;
	private float valCO2AcidMax = 64f, valCO2AcidMin = 45f, valCO2Neutral = 40f, valCO2BasicMin = 35f, valCO2BasicMax = 20f;
	private float valHCO3AcidMax = 14f, valHCO3AcidMin = 22f, valHCO3Neutral = 24f, valHCO3BasicMin = 26f, valHCO3BasicMax = 42f;
    //private float valPHLowest = 7.24f, valPHNeutralLow = 7.35f, valPHNeutralHigh = 7.45f, valPHHighest = 7.58f;//PH Values
    //private float valCO2Lowest = 20f, valCO2NeutralLow = 35f, valCO2NeutralHigh = 45f, valCO2Highest = 64f;//CO2 Values
    //private float valHCO3Lowest = 14f, valHCO3NeutralLow = 22f, valHCO3NeutralHigh = 26f, valHCO3Highest = 42f;// HCO3 Values


	private int diagnosesUsed;

	/// <summary>
	/// Create a new ABG Class. 
	/// </summary>
	/// <param name="locationOfFile">Location of the xml file to be read from.</param>
	/// <param name="nameOfFile">Name of the xml file to be read from.</param>
    public ABG(string locationOfFile = "", string nameOfFile = "")
    {
		

        diagnoses = new List<Diagnosis>();
		diagnosesInUse = new List<Diagnosis>();

		if (locationOfFile != "" /*&& nameOfFile != ""*/)
		{
			fileLocation = locationOfFile;
			//fileName = nameOfFile;
			

			//prepare the settings for the xmlreader.
			xmlreaderSettings = new XmlReaderSettings();
			xmlreaderSettings.IgnoreComments = true;

			LoadDiagnosesFromXML();
			//Debug.Log("The total amount of interventions is: " + diagnoses.Count);

			//foreach (Diagnosis d in diagnoses)
			//{
			//	Debug.Log()
			//}
		}
		else
		{
			CreateDiagnoses();
		}


		//prepare / shuffle diagnoses
		//prepare the patients.
		for (int i = 0; i < diagnoses.Count; i++)
		{
			Diagnosis temp = diagnoses[i];
			int rand = Random.Range(0, diagnoses.Count);
			diagnoses[i] = diagnoses[rand];
			diagnoses[rand] = temp;
		}
		diagnosesUsed = 0;
    }
	
	


#region Diagnosis Generation and Preparation

    /// <summary>
    /// Set the answer values for a diagnosis
    /// </summary>
    /// <param name="d">The Diagnosis</param>
    /// <returns>The same Diagnosis with answers and values</returns>
    public Diagnosis DiagnosisAnswerValues(Diagnosis d)
    {
        
        
        
        //if the diagnosis does not have set answers, give it some.
        if (d.AnswerRespiratoryMetabolic == "")
        {
            int r = Random.Range(0, 12);
			//create strings to hold the answer values.
			string rm = "", aa = "", c = "";

            switch (r)
            {
                case (0): rm = "Respiratory"; aa = "Acidosis"; c = "Uncompensated"; break;
                case (1): rm = "Respiratory"; aa = "Acidosis"; c = "Partial Compensation"; break;
                case (2): rm = "Respiratory"; aa = "Acidosis"; c = "Compensated"; break;
                case (3): rm = "Respiratory"; aa = "Alkalosis"; c = "Uncompensated"; break;
                case (4): rm = "Respiratory"; aa = "Alkalosis"; c = "Partial Compensation"; break;
                case (5): rm = "Respiratory"; aa = "Alkalosis"; c = "Compensated"; break;
                case (6): rm = "Metabolic"; aa = "Acidosis"; c = "Uncompensated"; break;
                case (7): rm = "Metabolic"; aa = "Acidosis"; c = "Partial Compensation"; break;
                case (8): rm = "Metabolic"; aa = "Acidosis"; c = "Compensated"; break;
                case (9): rm = "Metabolic"; aa = "Alkalosis"; c = "Uncompensated"; break;
                case (10): rm = "Metabolic"; aa = "Alkalosis"; c = "Partial Compensation"; break;
                case (11): rm = "Metabolic"; aa = "Alkalosis"; c = "Compensated"; break;
            }

			// Don't need to translate here, or ever because the answer is returned in it's translated form.

			////if language manager is available
			//if (LanguageManager.LanguageManager)
			//{
			//	LanguageManager lm = LanguageManager.LanguageManager;
			//	//translate to current language
			//	rm = lm.DirectTranslation("ABG", rm);
			//	aa = lm.DirectTranslation("ABG", aa);
			//	c = lm.DirectTranslation("ABG", c);
			//}

			d.AnswerRespiratoryMetabolic = rm;
			d.AnswerAcidosisAlkalosis = aa;
			d.AnswerCompensation = c;
			

            //go through the gauntlet again but this time, pick up the number values.
            return DiagnosisAnswerValues(d);
        }
        else
        {
			string rm = d.AnswerRespiratoryMetabolic, aa = d.AnswerAcidosisAlkalosis, comp = d.AnswerCompensation;
			//prepare our comparison values.
			string r = "Respiratory", m = "Metabolic", alk = "Alkalosis", aci = "Acidosis", uc = "Uncompensated", pc = "Partial Compensation", c = "Compensated";

			//translate the answers if possible
			if (LanguageManager._LanguageManager)
			{
				LanguageManager lm = LanguageManager._LanguageManager;
				r = lm.DirectTranslation("ABG", r);
				m = lm.DirectTranslation("ABG", m);
				alk = lm.DirectTranslation("ABG", alk);
				aci = lm.DirectTranslation("ABG", aci);
				uc = lm.DirectTranslation("ABG", uc);
				pc = lm.DirectTranslation("ABG", pc.Replace(" ",""));//due to xml syntax, this is not spelled the same and the whitespace needs to be removed.
				c = lm.DirectTranslation("ABG", c);
			}

            //since we have set answers, set the number values
            if (rm == r && aa == aci && comp == uc)
            {
				//Debug.Log("Resp Aci UC");
                d.PH = GenerateDiagnosisValues("PH", -2); d.CO2 = GenerateDiagnosisValues("CO2", -2); d.HCO3 = GenerateDiagnosisValues("HCO3", 0);
            }
            else if (rm == r && aa == aci && comp == pc)
            {
				Debug.Log("Resp Aci PC");
                d.PH = GenerateDiagnosisValues("PH", -2); d.CO2 = GenerateDiagnosisValues("CO2", -2); d.HCO3 = GenerateDiagnosisValues("HCO3", 2);
            }
            else if (rm == r && aa == aci && comp == c)
            {
				//Debug.Log("Resp Aci Comp");
                d.PH = GenerateDiagnosisValues("PH", -1); d.CO2 = GenerateDiagnosisValues("CO2", -2); d.HCO3 = GenerateDiagnosisValues("HCO3", 2);
            }
            else if (rm == r && aa == alk && comp == uc)
            {
                d.PH = GenerateDiagnosisValues("PH", 2); d.CO2 = GenerateDiagnosisValues("CO2", 2); d.HCO3 = GenerateDiagnosisValues("HCO3", 0);
            }
            else if (rm == r && aa == alk && comp == pc)
            {
                d.PH = GenerateDiagnosisValues("PH", 2); d.CO2 = GenerateDiagnosisValues("CO2", 2); d.HCO3 = GenerateDiagnosisValues("HCO3", -2);
            }
            else if (rm == r && aa == alk && comp == c)
            {
                d.PH = GenerateDiagnosisValues("PH", 1); d.CO2 = GenerateDiagnosisValues("CO2", 2); d.HCO3 = GenerateDiagnosisValues("HCO3", -2);
            }
            else if (rm == m && aa == aci && comp == uc)
            {
                d.PH = GenerateDiagnosisValues("PH", -2); d.CO2 = GenerateDiagnosisValues("CO2", 0); d.HCO3 = GenerateDiagnosisValues("HCO3", -2);
            }
            else if (rm == m && aa == aci && comp == pc)
            {
                d.PH = GenerateDiagnosisValues("PH", -2); d.CO2 = GenerateDiagnosisValues("CO2", 2); d.HCO3 = GenerateDiagnosisValues("HCO3", -2);
            }
            else if (rm == m && aa == aci && comp == c)
            {
                d.PH = GenerateDiagnosisValues("PH", -1); d.CO2 = GenerateDiagnosisValues("CO2", 2); d.HCO3 = GenerateDiagnosisValues("HCO3", -2);
            }
            else if (rm == m && aa == alk && comp == uc)
            {
                d.PH = GenerateDiagnosisValues("PH", 2); d.CO2 = GenerateDiagnosisValues("CO2", 0); d.HCO3 = GenerateDiagnosisValues("HCO3", 2);
            }
            else if (rm == m && aa == alk && comp == pc)
            {
                d.PH = GenerateDiagnosisValues("PH", 2); d.CO2 = GenerateDiagnosisValues("CO2", -2); d.HCO3 = GenerateDiagnosisValues("HCO3", 2);
            }
            else if (rm == m && aa == alk && comp == c)
            {
                d.PH = GenerateDiagnosisValues("PH", 1); d.CO2 = GenerateDiagnosisValues("CO2", -2); d.HCO3 = GenerateDiagnosisValues("HCO3", 2);
            }

            return d;
        }

        

        //return d;
    }



    /// <summary>
    /// Generate number values for each of the variables.
    /// </summary>
    /// <param name="value">name of the value ie. PH, CO2, HCO3</param>
    /// <param name="LowMedHigh">-2 for acid, 0 for neutral, 2 for basic, -1 for Lower Neutral, 1 = Higher Neutral</param>
    /// <returns></returns>
    private float GenerateDiagnosisValues(string value, int acidneutralbasic)
    {
		//The variables that will be used for our check.
		float acidMax = 0f, acidMin = 0f, neutral = 0f, basicMin = 0f, basicMax = 0f;
		float randomVariance = 0f;	
		float finalValue = -90f;
		
		
        if (value == "PH")
        {
			Debug.Log("Generating a Diagnosis Value for: " + value + " in range: " + acidneutralbasic);
			acidMax = valPHAcidMax;//Should be 7.24
			acidMin = valPHAcidMin;//should be 7.35
			neutral = valPHNeutral;//should be 7.40
			basicMin = valPHBasicMin;//should be 7.45
			basicMax = valPHBasicMax;//should be 7.58
			randomVariance = .01f;
			
        }
        else if (value == "CO2")
        {
			acidMax = valCO2AcidMax;//64
			acidMin = valCO2AcidMin;//45
			neutral = valCO2Neutral;//40
			basicMin = valCO2BasicMin;//35
			basicMax = valCO2BasicMax;//20
			randomVariance = 1f;

			
        }
        else if (value == "HCO3")
        {
			acidMax = valHCO3AcidMax;//14
			acidMin = valHCO3AcidMin;//22
			neutral = valHCO3Neutral;//24
			basicMin = valHCO3BasicMin;//26
			basicMax = valHCO3BasicMax;//42
			randomVariance = 1f;

			
        }

		//Acidic Range
		if (acidneutralbasic == -2)
		{
			Debug.Log("-2 Range is: " + (acidMax + randomVariance) + " , " + (acidMin - randomVariance));
			if (acidMax > acidMin)
			{
				//CO2
				finalValue = Random.Range(acidMin + randomVariance, acidMax -randomVariance);
			}
			else
			{
				finalValue = Random.Range(acidMax + randomVariance, acidMin - randomVariance);
			}
			
		}
			//Neutral Acidic Range
		else if (acidneutralbasic == -1)
		{
			if (acidMin > neutral)
			{
				finalValue = Random.Range(neutral + randomVariance, acidMin - randomVariance);
			}
			else
			{
				finalValue = Random.Range(acidMin + randomVariance, neutral - randomVariance);
			}

			//Debug.Log("Neutral Acidic Range for " + value + " is: " + finalValue);
			
		}
			//Neutral Range
		else if (acidneutralbasic == 0)
		{
			if (acidMin > basicMin)
			{
				//this should occur for CO2
				finalValue = Random.Range(basicMin +randomVariance,acidMin - randomVariance);
			}
			else
			{
				finalValue = Random.Range(acidMin + randomVariance, basicMin - randomVariance);
			}
			
		}
			//Neutral Basic Range
		else if (acidneutralbasic == 1)
		{
			if (neutral > basicMin)
			{
				//this should occur for CO2
				finalValue = Random.Range(basicMin + randomVariance, neutral -randomVariance);
			}
			else
			{
				finalValue = Random.Range(neutral + randomVariance, basicMin - randomVariance);
			}
			//Debug.Log("Neutral Basic Range for " + value + " is: " + finalValue);
			
		}
			//Basic Range
		else if (acidneutralbasic == 2)
		{
			if (basicMin > basicMax)
			{
				finalValue = Random.Range(basicMax + randomVariance, basicMin - randomVariance);
			}
			else
			{
				finalValue = Random.Range(basicMin + randomVariance, basicMax - randomVariance);
			}
			
		}

		return finalValue;
    }


    /// <summary>
    /// Read in data from file to create a list of diagnoses.
    /// </summary>
    private void CreateDiagnoses()
    {
		Debug.Log("ABG is creating Diagnoses");
        //Come back and change this to read in from a file later.


        //Initialize Test Sample lists.

		testStoriesL = new List<string>(); testStoriesS = new List<string>();
		testSymptoms1 = new List<string>(); testSymptoms2 = new List<string>(); testSymptoms3 = new List<string>(); testSymptoms4 = new List<string>();
		testConditions1 = new List<string>(); testConditions2 = new List<string>(); testConditions3 = new List<string>(); testConditions4 = new List<string>();
		testMedications1 = new List<string>(); testMedications2 = new List<string>(); testMedications3 = new List<string>(); testMedications4 = new List<string>();

		//populate the test sample lists.
		//long stories
		testStoriesL.Add("I fell off a ladder yesterday afternoon while trimming my hedges and have been taking a narcotic pain reliever since shortly after the accident."); testStoriesL.Add("I woke up in the middle of the night with stomach cramps and feeling nauseous after eating at a delicatessen yesterday. I vomited lots of green fluid 3 times and I feel weak and sick to my stomach."); testStoriesL.Add("I was on my way to an important meeting for work when I realized I forgot several essential documents. I became anxious and started hyperventilating. Then I became dizzy and felt tingling in my fingertips."); testStoriesL.Add("My child has been very difficult to feed and has had very frequent runny poops for the past 3 days. My child also seems to be breathing fast.");


		//short stories
		testStoriesS.Add("I fell off a ladder yesterday afternoon while trimming my hedges."); testStoriesS.Add("I woke up in the middle of the night with stomach cramps"); testStoriesS.Add("I became anxious on the way to work and started hyperventilating!!"); testStoriesS.Add("My child has been difficult to feed and has had runny poops for days.");

		//symptoms
        testSymptoms1.Add("Severe Pain"); testSymptoms1.Add("Cyanotic Fingers");
        testSymptoms2.Add("Stomach Cramps"); testSymptoms2.Add("Weak & Fainty");
		testSymptoms3.Add("Hyperventilation"); testSymptoms3.Add("Dizziness"); testSymptoms3.Add("Palpitations"); testSymptoms3.Add("Tingling in arms");
		testSymptoms4.Add("Tachypnea"); testSymptoms4.Add("Depressed anterior fontanel");

		//conditions
        testConditions1.Add("Asthma"); testConditions1.Add("Lung Disease");
        testConditions2.Add("Stomach Polyps"); testConditions2.Add("Peptic Ulcer Disease");

		//medications
        testMedications1.Add("Alleve"); testMedications1.Add("Zyrtec");
        testMedications2.Add("Pepto-Bismol"); testMedications2.Add("Tums");

		//generate the diagnoses
        Diagnosis d = new Diagnosis("Respiratory", "Acidosis", "Uncompensated", testStoriesL[0], testStoriesS[0], testMedications1, testSymptoms1, testConditions1);
        Diagnosis e = new Diagnosis("Metabolic", "Alkalosis", "Compensated", testStoriesL[1], testStoriesS[1], testMedications2, testSymptoms2, testConditions2);
        
		Diagnosis f = new Diagnosis("Respiratory", "Alkalosis", "Uncompensated", testStoriesL[2], testStoriesS[2], testMedications3, testSymptoms3, testMedications3);
		Diagnosis g = new Diagnosis("Metabolic", "Acidosis", "Compensated", testStoriesL[3], testStoriesS[3], testMedications4, testSymptoms4, testConditions4);

		//add the new diagnoses to the list.
		diagnoses.Add(d); diagnoses.Add(e); diagnoses.Add(f); diagnoses.Add(g);

        //get the values for the diagnoses
        for (int i = 0; i < diagnoses.Count; i++ )
        {
            diagnoses[i] = DiagnosisAnswerValues(diagnoses[i]);
        }
    }

	private void LoadDiagnosesFromXML()
	{
		Debug.Log("LoadDiagnosesFromXML");
		if (fileLocation != "")
		{
			//create a reader that uses our settings defined earlier.
			TextAsset xml = Resources.Load(fileLocation) as TextAsset;

			//XmlReader xmlreader = XmlReader.Create(fileLocation, xmlreaderSettings);

			//create a new instance of an xml document
			XmlDocument xmlDoc = new XmlDocument();
			//xmlDoc.Load(xmlreader);
			xmlDoc.LoadXml(xml.text);

			//Debug.Log(xmlDoc.ChildNodes[1].InnerText);

			//parse through all the different interventions.
			XmlNodeList interventions = xmlDoc.GetElementsByTagName("Intervention");

			//determine how many languages there are.
			//int totalLanguages = interventions[0].ChildNodes.Count;

			#region Nursing Intervention
			foreach (XmlNode intervention in interventions)
			{
				
				//create the variables
				//RespiratoryMetabolic, AcidosisAlkalosis, Compensation, Greeting
				string rm = "" , aa = "", c = "";
				string greetingEnglish = "", greetingSpanish = "";
				
				//History, and Signs & Symptoms
				List<string> historyEnglish = new List<string>();
				List<string> historySpanish = new List<string>();
				List<string> signsSymptomsEnglish = new List<string>();
				List<string> signsSymptomsSpanish = new List<string>();

				//Get the Diagnosis /Intervention/English/Diagnosis
				XmlNode xn = intervention.FirstChild.FirstChild;
				
				//Respiratory Or Metabolic, Acidosis or Alkalosis, Compensation
				rm = xn.FirstChild.InnerText;
				aa = xn.ChildNodes[1].InnerText;
				c = xn.LastChild.InnerText;

				//get the greeting, signs and symptoms, and history for each language.
				#region Language
				foreach (XmlNode langauge in intervention)
				{
					//Keep track of what language we are currently in.
					string lang = langauge.Name;
					//Debug.Log(lang);
					//the greeting for this language
					string greeting = "";
					//The history and signs and symptoms for this language
					List<string> hist = new List<string>();
					List<string> ss = new List<string>();

					#region Sections
					//now find the information for each section in this language.
					foreach (XmlNode section in langauge)
					{
						//Greeting
						if (section.Name == "Greeting")
						{
							greeting = section.InnerText;
						}

						//Signs and Symptoms
						else if (section.Name == "SignsAndSymptoms")
						{
							//comb through the list of elements and add each value
							foreach (XmlNode element in section)
							{
								if (element.NodeType != XmlNodeType.Comment && element.InnerText != "" && element.NodeType != XmlNodeType.Whitespace)
								{
									ss.Add(element.InnerText);
								}
								
							}
						}

						else if (section.Name == "History")
						{
							foreach (XmlNode element in section)
							{

								if (element.NodeType != XmlNodeType.Comment && element.InnerText != "" && element.NodeType != XmlNodeType.Whitespace)
								{
									hist.Add(element.InnerText);
								}
								
							}
						}
					}
					//now save the retrieved values to their proper variables.
					if (lang == "English")
					{
						greetingEnglish = greeting;
						historyEnglish = hist;
						signsSymptomsEnglish = ss;
					}
					else if (lang == "Spanish")
					{
						greetingSpanish = greeting;
						historySpanish = hist;
						signsSymptomsSpanish = ss;
					}
					#endregion
				}
				#endregion

				#region Create the Nursing Intervention And add to list
				Diagnosis d = new Diagnosis(rm, aa, c, greetingEnglish, greetingSpanish, historyEnglish, historySpanish, signsSymptomsEnglish, signsSymptomsSpanish);
				diagnoses.Add(d);
				#endregion

			}
			#endregion

		}
		
	}


#endregion


    /// <summary>
    /// Return a Diagnosis with Random Values for Practice mode.
    /// </summary>
    /// <returns>Diagnosis with Random Answer Values.</returns>
    public Diagnosis RandomDiagnosis()
    {
        //create new diagnosis
        Diagnosis d = new Diagnosis();
        //set values for answers
        DiagnosisAnswerValues(d);
        return d;
    }

	/// <summary>
	/// Return a random diagnosis for game mode.
	/// </summary>
	/// <returns></returns>
	public Diagnosis PatientDiagnosis()
	{
		//return a random diagnosis for a patient.
		if (diagnoses.Count > 0)
		{
			if (diagnosesUsed == diagnoses.Count)
			{
				//shuffle the diagnoses
				for (int i = 0; i < diagnoses.Count; i++)
				{
					Diagnosis temp = diagnoses[i];
					int rand = Random.Range(i, diagnoses.Count);
					diagnoses[i] = diagnoses[rand];
					diagnoses[rand] = temp;
				}
				//reset the diagnoses Used
				diagnosesUsed = 0;
			}

			Diagnosis d = diagnoses[diagnosesUsed];
			diagnosesUsed++;
			//diagnoses.Remove(d);
			//diagnosesInUse.Add(d);
			//randomize the values
			return DiagnosisAnswerValues(d);

		}
		else
		{
			return RandomDiagnosis();
		}
	}

	/// <summary>
	/// Inform ABG that this Diagnosis is finished.
	/// </summary>
	/// <param name="d"></param>
	public void PatientDiagnosisComplete(Diagnosis d)
	{
		//remove the diagnosis because it's no longer in use by a patient.
		//diagnosesInUse.Remove(d);
		//add the diagnosis back to the available diagnosis list.
		//diagnoses.Add(d);
	}

}
