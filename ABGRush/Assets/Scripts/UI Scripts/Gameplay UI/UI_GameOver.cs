using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_GameOver : MonoBehaviour {

	public Text textfieldAngryPatients, textfieldCorrectDiagnoses, textfieldCorrectAssessments;

	private float startTime;
	private bool front = false;

	public void GameOver(bool delay, int angrypatients, int correctdiagnosis, int correctassessments)
	{
		if (textfieldAngryPatients && textfieldCorrectAssessments && textfieldCorrectDiagnoses)
		{
			textfieldCorrectDiagnoses.text = correctdiagnosis.ToString();
			textfieldCorrectAssessments.text = correctassessments.ToString();
			textfieldAngryPatients.text = angrypatients.ToString();
		}

		//make sure that this is the lowest child on the hierarchy so it will be seen.
		//transform.SetParent(transform.parent);

		if (delay)
		{
			startTime = Time.realtimeSinceStartup;
			front = false;
		}
		else
		{
			ComeToFront();
		}
		
		
	}

	/// <summary>
	/// Become the last sibling in parent transform.
	/// </summary>
	private void ComeToFront()
	{
		if (SoundManager._SoundManager)
		{
			SoundManager._SoundManager.PlaySound("GameOver");
		}
		//make sure that this is the lowest child on the hierarchy so it will be seen.
		transform.SetAsLastSibling();
		front = true;
		//Time.timeScale = 0;
	}

	void Update()
	{
		if (Time.realtimeSinceStartup - startTime >= 3f && !front)
		{
			ComeToFront();
			
		}
	}


	public void MainMenu()
	{
		Time.timeScale = 1;
		Application.LoadLevel(1);
	}

	public void SoundClick()
	{
		//Play a sound
		if (SoundManager._SoundManager)
		{
			SoundManager._SoundManager.PlaySound("Click");
		}
	}
}
