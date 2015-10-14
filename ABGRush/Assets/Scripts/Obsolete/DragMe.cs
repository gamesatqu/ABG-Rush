using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DragMe : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public bool dragOnSurfaces = true;
	
	private GameObject m_DraggingIcon;
	private RectTransform m_DraggingPlane;

    public void OnBeginDrag(PointerEventData eventData)
    {
        var canvas = FindInParents<Canvas>(gameObject);
        if (canvas == null)
            return;

        // We have clicked something that can be dragged.
        // What we want to do is create an icon for this.
        m_DraggingIcon = Instantiate(gameObject);
        ////m_DraggingIcon = new GameObject("icon");

        m_DraggingIcon.transform.SetParent(canvas.transform, false);
        m_DraggingIcon.transform.SetAsLastSibling();

        ////var image = m_DraggingIcon.AddComponent<Image>();
        //match the color of the previous object
        ////image.color = GetComponent<Image>().color;

        //Added by ron. Change this later
        ////Debug.Log("Image Type: " + image.type);
        ////Vector2 curSize = gameObject.GetComponent<RectTransform>().sizeDelta;
        ////Debug.Log(curSize);
        ////image.gameObject.GetComponent<RectTransform>().sizeDelta = curSize;
        ////Debug.Log(image.gameObject.GetComponent<RectTransform>().sizeDelta);

        //End Ron


        // The icon will be under the cursor.
        // We want it to be ignored by the event system.
        CanvasGroup group = m_DraggingIcon.AddComponent<CanvasGroup>();
        group.blocksRaycasts = false;

        ////image.sprite = GetComponent<Image>().sprite;
        //Debug.Log("Before setting native size " + image.gameObject.GetComponent<RectTransform>().sizeDelta);

        //image.SetNativeSize();
        //Debug.Log("After setting native size " + image.gameObject.GetComponent<RectTransform>().sizeDelta);

        if (dragOnSurfaces)
            m_DraggingPlane = transform as RectTransform;
        else
            m_DraggingPlane = canvas.transform as RectTransform;

        SetDraggedPosition(eventData);
    }

	public void OnDrag(PointerEventData data)
	{
		if (m_DraggingIcon != null)
			SetDraggedPosition(data);
	}

	private void SetDraggedPosition(PointerEventData data)
	{
		if (dragOnSurfaces && data.pointerEnter != null && data.pointerEnter.transform as RectTransform != null)
			m_DraggingPlane = data.pointerEnter.transform as RectTransform;
		
		var rt = m_DraggingIcon.GetComponent<RectTransform>();
		Vector3 globalMousePos;
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, data.position, data.pressEventCamera, out globalMousePos))
		{
			rt.position = globalMousePos;
			rt.rotation = m_DraggingPlane.rotation;
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (m_DraggingIcon != null)
			Destroy(m_DraggingIcon);
	}

	static public T FindInParents<T>(GameObject go) where T : Component
	{
		if (go == null) return null;
		var comp = go.GetComponent<T>();

		if (comp != null)
			return comp;
		
		Transform t = go.transform.parent;
		while (t != null && comp == null)
		{
			comp = t.gameObject.GetComponent<T>();
			t = t.parent;
		}
		return comp;
	}



    //Rons new version of Dragme begins here

    //private RectTransform parentRect;


    //public void OnBeginDrag(PointerEventData eventData)
    //{
    //    Debug.Log("Begin Drag");
    //    var canvas = FindInParents<Canvas>(gameObject);
    //    if (canvas == null)
    //        return;

    //    // We have clicked something that can be dragged.
    //    // What we want to do is create an icon for this.
    //    m_DraggingIcon = gameObject;//Instantiate(gameObject);
    //    ////m_DraggingIcon = new GameObject("icon");

    //    m_DraggingIcon.transform.SetParent(canvas.transform, false);
    //    m_DraggingIcon.transform.SetAsLastSibling();

    //    ////var image = m_DraggingIcon.AddComponent<Image>();
    //    //match the color of the previous object
    //    ////image.color = GetComponent<Image>().color;

    //    //Added by ron. Change this later
    //    ////Debug.Log("Image Type: " + image.type);
    //    ////Vector2 curSize = gameObject.GetComponent<RectTransform>().sizeDelta;
    //    ////Debug.Log(curSize);
    //    ////image.gameObject.GetComponent<RectTransform>().sizeDelta = curSize;
    //    ////Debug.Log(image.gameObject.GetComponent<RectTransform>().sizeDelta);

    //    //End Ron


    //    // The icon will be under the cursor.
    //    // We want it to be ignored by the event system.
    //    CanvasGroup group = m_DraggingIcon.AddComponent<CanvasGroup>();
    //    group.blocksRaycasts = false;

    //    ////image.sprite = GetComponent<Image>().sprite;
    //    //Debug.Log("Before setting native size " + image.gameObject.GetComponent<RectTransform>().sizeDelta);

    //    //image.SetNativeSize();
    //    //Debug.Log("After setting native size " + image.gameObject.GetComponent<RectTransform>().sizeDelta);

    //    if (dragOnSurfaces)
    //        m_DraggingPlane = transform as RectTransform;
    //    else
    //        m_DraggingPlane = canvas.transform as RectTransform;

    //    SetDraggedPosition(eventData);
    //}
    
}
