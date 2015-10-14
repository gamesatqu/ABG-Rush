using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class DragAndDropSlot : MonoBehaviour, IDropHandler {

	/// <summary>
	/// This script was created with assistance from a youtube video tutorial by Richard Gubb. The video can be found at this link:
	/// https://www.youtube.com/watch?v=c47QYgsJrWc
	/// It's purpose is to provide functionality to the slots that drag and droppable objects can be placed into in the new version of the ABG tool.
	/// </summary>


	public GameObject item
	{
		get
		{
			if (transform.childCount > 0)
			{
				return transform.GetChild(0).gameObject;
			}
			return null;
		}
	}


	#region IDropHandler implementation

	public void OnDrop(PointerEventData eventData)
	{
		//If an object is released on this slot, check and see if this slot is empty.
		Debug.Log("OnDrop");
		Debug.Log("Item returns " + item);
		if (!item)
		{
			//if this object is empty, take in the new object.
			Debug.Log("Dropping the object.");
			DragHandler.itemBeingDragged.transform.SetParent(transform);
		}
	}

	#endregion
}
