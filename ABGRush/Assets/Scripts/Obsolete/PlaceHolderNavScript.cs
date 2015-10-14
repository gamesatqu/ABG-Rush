using UnityEngine;
using System.Collections;

public class PlaceHolderNavScript : MonoBehaviour {

    void Awake()
    {
        DontDestroyOnLoad(gameObject.transform.parent);
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GoToPracticeScene()
    {
        if (Application.loadedLevel != 1)
        {
            Application.LoadLevel(1);
        }
    }

    public void GoToHelpScene()
    {
        if (Application.loadedLevel != 2)
        {
            Application.LoadLevel(2);
        }
    }
}
