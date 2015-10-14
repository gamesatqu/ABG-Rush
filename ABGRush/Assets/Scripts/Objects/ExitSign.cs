using UnityEngine;
using System.Collections;

public class ExitSign : MonoBehaviour {

	public UIQuit quitUI;
	public Color colorHighlighted;
	public SpriteRenderer exitSign;

	private Color colorNormal;

	void Start()
	{
		colorNormal = exitSign.color;
	}

	void OnMouseEnter()
	{
		if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1))
		{
			exitSign.color = colorHighlighted;
		}
		
	}

	void OnMouseExit()
	{
		exitSign.color = colorNormal;
	}

	void OnMouseDown()
	{
		if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1))
		{
			Time.timeScale = 0f;
			quitUI.gameObject.SetActive(true);
		}
		
	}
}

