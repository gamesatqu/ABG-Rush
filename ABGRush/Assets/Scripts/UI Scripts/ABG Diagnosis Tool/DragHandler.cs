using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	/// <summary>
	/// This script was created with assistance from a youtube video tutorial by Richard Gubb. The video can be found at this link:
	/// https://www.youtube.com/watch?v=c47QYgsJrWc
	/// It's purpose is to provide functionality to the Drag and Droppable objects in the new version of the ABG tool.
	/// </summary>

	public static GameObject itemBeingDragged;
	public Transform foreground;
	Vector3 startPosition;
	Transform startParent;


	#region IBeginDragHandler implementation
	
	public void OnBeginDrag(PointerEventData eventData)
	{
		itemBeingDragged = gameObject;
		startPosition = transform.position;
		startParent = transform.parent;
		GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	#endregion

	#region IDragHandler implementation

	public void OnDrag(PointerEventData eventData)
	{
		transform.position = Input.mousePosition;
		transform.SetParent(foreground);
	}

	#endregion

	#region IEndDragHandler implementation
	
	public void OnEndDrag(PointerEventData eventData)
	{
		Debug.Log("OnEndDrag");
		itemBeingDragged = null;
		GetComponent<CanvasGroup>().blocksRaycasts = true;
		if (transform.parent == startParent || transform.parent == foreground)
		{
			transform.position = startPosition;
			transform.parent = startParent;
		}
	}

	#endregion
}
