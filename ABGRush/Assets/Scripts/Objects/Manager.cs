using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Manager : MonoBehaviour {

    
    public GameObject[] prefabPatients;
    public Transform locationEntrance, locationExit;
   
	public GameplayUIScript gameplayUI;
	public float timerSpawn = 15f;
	public float timerSpawnRate = 3f;
	public float timerSpawnDelayMin = 25f;
	public static Manager _manager;

	public List<string> namesFirst, namesLast;
    private Triage triage;
   
    private List<WaitingChair> listWaitingChairs;
    private List<ExamRoom> listExamRooms;
	private int scoreCorrectDiagnoses, scoreCorrectInitialAssessment, scoreAngryPatients, scorePatientsSeen;
	private int currentPatients, patientPrefabsUsed;
    private float scoreSatisfaction;
	private float timerSpawnUsed;
    private Nurse nurse;
	private ABG abg;
	private Sink sink;


    public Nurse MyNurse
    {
        get { return nurse; }
    }

	
	void Start () {
        ManagerInitialize();
	}
	
	
	void Update () {

		if (timerSpawnUsed > 0)
		{
			timerSpawnUsed -= Time.deltaTime;
			if (timerSpawnUsed <= 0)
			{
				ManagerPatientSpawn();
				
			}
		}

		if (scoreSatisfaction > 0)
		{
			scoreSatisfaction -= Time.deltaTime / 6f;
			gameplayUI.satisfaction.SatisfactionUpdate(scoreSatisfaction);
			if (scoreSatisfaction <= 0)
			{
				Debug.Log("Update Gameover");
				GameOver();
			}
		}


	}

    #region Patient Leaving
    //If time permits, come back and revamp this. These two methods may be similar enough near the end of the project that they can easily be simplified and molded into a single method depending on what we have each of them do.

    /// <summary>
    /// Leave the practice / emergency facility angrily. 
    /// </summary>
    /// <param name="p">The Patient leaving</param>
    public void ManagerPatientStormOut(Patient p)
    {
		//play Leave Angry sound
		if (SoundManager._SoundManager)
		{
			SoundManager._SoundManager.PlaySound("PatientLeave");
		}

		//update the amount of patients.
		currentPatients--;
		//update the amount of patients that have been seen.
		scorePatientsSeen++;
		

		//spawn another patient if we are down to 0.
		if (currentPatients <= 0)
		{
			ManagerPatientSpawn();
		}

        //listPatients.Remove(p);

        //Perform some kind of animation

		//update the total amount of patients to leave angry
		scoreAngryPatients++;

        //decrease points based on level of satisfaction
		UpdateSatisfactionScore(-5);
		UpdatePatientsTreated();

		//inform abg that the diagnosis is no longer being used.
		//abg.PatientDiagnosisComplete(p.MyDiagnosis());

		//make the patient go to the exit
        p.PersonMove(locationExit.position,"Exit");
    }

    /// <summary>
    /// Leave the facility happily, and award points
    /// </summary>
    /// <param name="p"></param>
    public void ManagerPatientLeave(Patient p)
    {
		//update the amount of patients.
		currentPatients--;
		//update the amount of patients that have been seen.
		scorePatientsSeen++;		

		//spawn another patient if we are down to 0.
		if (currentPatients <= 0)
		{
			ManagerPatientSpawn();
		}

        //listPatients.Remove(p);
        //perform some kind of animation

		//Determine if the initial assessment of the patient was correct.
		bool rm = (p.InitialAssessmentGetRM() == p.MyDiagnosis().AnswerRespiratoryMetabolic);
		bool aa = (p.InitialAssessmentGetAA() == p.MyDiagnosis().AnswerAcidosisAlkalosis);

		//update score
		scoreCorrectDiagnoses++;
		if (rm && aa)
		{
			scoreCorrectInitialAssessment++;
		}

		gameplayUI.PatientFeedback().PatientFeedback(p, rm, aa);
		gameplayUI.PatientFeedback().gameObject.SetActive(true);
		Time.timeScale = 0;
        //gain/increase points based on level of satisfaction
		UpdateSatisfactionScore(6);
		UpdatePatientsTreated();

		//inform abg that the diagnosis is no longer being used.
		//abg.PatientDiagnosisComplete(p.MyDiagnosis());

        p.PersonMove(locationExit.position, "Exit");
    }

    #endregion


	#region Check Hotspots
	/// <summary>
    /// Determine if there is an empty examination room
    /// </summary>
    /// <returns>Empty room or null</returns>
    public ExamRoom ManagerEmptyExamRoom()
    {
        foreach (ExamRoom e in listExamRooms)
        {
            if (!e.PatientObjectOccupied())
            {
                return e;
            }
        }

        return null;
    }


    /// <summary>
    /// Determine if there is an empty Waiting Chair
    /// </summary>
    /// <returns>Empty chair or null</returns>
    public WaitingChair ManagerEmptyWaitingChair()
    {
        foreach (WaitingChair w in listWaitingChairs)
        {
            if (!w.PatientObjectOccupied())
            {
                return w;
            }
        }
        return null;
    }
	#endregion

	/// <summary>
    /// Set up the manager
    /// </summary>
    private void ManagerInitialize()
    {
		//Initialize the ABG class and prepare all the diagnoses
		abg = new ABG("NursingInterventions");
        listWaitingChairs = new List<WaitingChair>();
        listExamRooms = new List<ExamRoom>();
        //listPatients = new List<Patient>();

		//Populate the list of waiting chairs.
        GameObject[] wc = GameObject.FindGameObjectsWithTag("WaitingChair");
        foreach (GameObject w in wc)
        {
            listWaitingChairs.Add(w.GetComponent<WaitingChair>());
        }

		//Populate the list of exam rooms.
        GameObject[] er = GameObject.FindGameObjectsWithTag("ExamRoom");
        foreach (GameObject e in er)
        {
            listExamRooms.Add(e.GetComponent<ExamRoom>());
        }

		//Find the triage
        triage = GameObject.FindGameObjectWithTag("Triage").GetComponent<Triage>();

		//Find the nurse
        nurse = GameObject.FindGameObjectWithTag("Nurse").GetComponent<Nurse>();

		//Find the Sink
		sink = GameObject.FindGameObjectWithTag("Sink").GetComponent<Sink>();

		//prepare the patients.
		for (int i = 0; i < prefabPatients.Length; i++)
		{
			GameObject temp = prefabPatients[i];
			int rand = Random.Range(0, prefabPatients.Length);
			prefabPatients[i] = prefabPatients[rand];
			prefabPatients[rand] = temp;
		}

		//Reset the score
        scorePatientsSeen = 0;
		scoreAngryPatients = 0;
		scoreCorrectDiagnoses = 0;
		scoreCorrectInitialAssessment = 0;
		scoreSatisfaction = 100f;
		
		currentPatients = 0;
		//reset the spawn timer
		timerSpawnUsed = 0.1f;

		//set the manager
		_manager = this;

		gameplayUI.satisfaction.SatisfactionUpdate(scoreSatisfaction);
		UpdatePatientsTreated();
    }

    private void ManagerPatientSpawn()
    {
		//prepare a dob
		string dob = Random.Range(1, 13).ToString() + "/" + Random.Range(1, 29).ToString() + "/" + Random.Range(1940, 2000).ToString();
		GameObject newPatient = NewPatientPrefab();

        Patient p = (Instantiate(newPatient,locationEntrance.position,newPatient.transform.rotation) as GameObject).GetComponent<Patient>();

		p.PatientSetup(namesFirst[Random.Range(0, namesFirst.Count)] + " " + namesLast[Random.Range(0, namesLast.Count)], dob, abg.PatientDiagnosis());

        triage.TriagePatientAdd(p);
		currentPatients++;

		timerSpawn *= timerSpawnRate;
		timerSpawn = Mathf.Clamp(timerSpawn, timerSpawnDelayMin, 120f);
		//allow patient to spawn either 10 seconds sooner or 10 seconds after base time
		timerSpawnUsed = Random.Range(-3, 3);
		timerSpawnUsed += timerSpawn;
    }

	/// <summary>
	/// Called during any/all interactions involving the nurse and a patient.
	/// </summary>
	public void CleanHandsCheck()
	{
		bool clean = false;

		if (nurse)
		{
			clean = nurse.IsClean();
		}

		if (clean)
		{
			UpdateSatisfactionScore(2);
		}
		else
		{
			UpdateSatisfactionScore(-3);
		}
	}


	public GameplayUIScript GamePlayUI()
	{
		return gameplayUI;
	}

	public void UpdateSatisfactionScore(int s){
		scoreSatisfaction += s;
		//clamp the score so it cannot go above 100
		scoreSatisfaction = Mathf.Clamp(scoreSatisfaction, 0f, 100f);

		gameplayUI.satisfaction.SatisfactionModify(s);
		gameplayUI.satisfaction.SatisfactionUpdate(scoreSatisfaction);

		if (scoreSatisfaction <= 0)
		{
			Debug.Log("SatisfactionScore Gameover");
			GameOver();
		}
	}

	/// <summary>
	/// Inform the UI to display the correct amount of patients we have treated vs seen.
	/// </summary>
	private void UpdatePatientsTreated()
	{
		gameplayUI.satisfaction.UpdateTreatedPatients(scoreCorrectDiagnoses, scorePatientsSeen);
	}

	public Sink MySink()
	{
		return sink;
	}

	private void GameOver()
	{
		Debug.Log("GameOver Called");
		bool delayGO = false;

		if (gameplayUI.ExamRoomComputerUI().gameObject.activeInHierarchy)
		{
			delayGO = true;
		}
		

		gameplayUI.GameOverUI().GameOver(delayGO, scoreAngryPatients,scoreCorrectDiagnoses,scoreCorrectInitialAssessment);
		
		gameplayUI.GameOverUI().gameObject.SetActive(true);
		Debug.Log("Paused");
		Time.timeScale = 0;
		
	}

	private GameObject NewPatientPrefab()
	{
		if (patientPrefabsUsed == prefabPatients.Length)
		{
			//shuffle the patients
			for (int i = 0; i < prefabPatients.Length; i++)
			{
				GameObject temp = prefabPatients[i];
				int rand = Random.Range(i, prefabPatients.Length);
				prefabPatients[i] = prefabPatients[rand];
				prefabPatients[rand] = temp;
			}
			//reset the patients Used
			patientPrefabsUsed = 0;
		}

		GameObject p = prefabPatients[patientPrefabsUsed];
		patientPrefabsUsed++;

		return p;
	}
}
