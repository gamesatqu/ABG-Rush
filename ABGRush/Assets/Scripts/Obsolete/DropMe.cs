using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropMe : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image containerImage;
    public Image receivingImage;
    public DragAndDropManagerScript manager;//send all drop information to the manager.
    private Color normalColor;
    private GameObject self, curChild;//never have more than 1 child.
    public Color highlightColor = Color.yellow;


    void Awake()
    {
        self = gameObject;
    }

    public void OnEnable ()
    {
        if (containerImage != null)
            normalColor = containerImage.color;
    }
    
    //public void OnDrop(PointerEventData data)
    //{
    //    containerImage.color = normalColor;
        
    //    if (receivingImage == null)
    //        return;
        
    //    Sprite dropSprite = GetDropSprite (data);
    //    if (dropSprite != null)
    //        receivingImage.overrideSprite = dropSprite;
    //}

    public void OnPointerEnter(PointerEventData data)
    {
        if (containerImage == null)
            return;
        
        Sprite dropSprite = GetDropSprite (data);
        if (dropSprite != null)
            containerImage.color = highlightColor;
    }

    public void OnPointerExit(PointerEventData data)
    {
        if (containerImage == null)
            return;
        
        containerImage.color = normalColor;
    }
    
    private Sprite GetDropSprite(PointerEventData data)
    {
        var originalObj = data.pointerDrag;
        if (originalObj == null)
            return null;

        var srcImage = originalObj.GetComponent<Image>();
        if (srcImage == null)
            return null;
        //Debug.Log(originalObj.transform.GetChild(0).name);
        return srcImage.sprite;
    }


    //Ron's Versions

    public void OnDrop/*Object*/(PointerEventData data)
    {
        containerImage.color = normalColor;

        GameObject g = GetDropObject(data);
        //if dropped GO is not null
        if (g != null)
        {
            //create new instance of it
            GameObject n = Instantiate(g);

            //check for existing child.
            if (curChild != null)
            {
                //compare the new object to the child, if they are the same, destroy the new and do nothing to the child, else, destroy the child
                if (curChild == n)
                {
                    //destroy the new object.
                    Destroy(n);
                    //do nothing since the dropped object is the same.
                    return;
                }
                else if(curChild != n)
                {
                    //destroy the child
                    manager.DestroyChild(curChild);
                    curChild = null;

                }
                
            }

            //make this object the parent of the new instance.
            n.transform.SetParent(self.transform);
            curChild = n.gameObject;

            //send the object to the manager to make sure that no other copies exist.
            manager.DroppedObject(n);
            

            


        }
            
    }

    private GameObject GetDropObject(PointerEventData data)
    {
        var originalObj = data.pointerDrag;
        if (originalObj == null)
            return null;

        var obj = originalObj;
        if (obj == null)
            return null;
        //Debug.Log(originalObj.transform.GetChild(0).name);
        return obj;
    }
}
