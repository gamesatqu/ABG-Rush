using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DragAndDropManagerScript : MonoBehaviour
{

    /// <summary>
    /// This script will manage the drag and drop functionality of the Tic Tac Toe board. 
    /// No more than 3 objects should exist in the board at once. 
    /// There should never be any duplicates in the TTT board either. 
    /// 
    /// The public function will allow all drop scripts to send information here.
    /// This data will then determine if any objects in the TTT board need to be removed.
    /// </summary>


    private List<GameObject> droppedObjects;//this should never have more than 3

    // Use this for initialization
    void Start()
    {

        //create the list that will contain the dropped objects.
        droppedObjects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DroppedObject(GameObject d)
    {
        //scan the list for a GO that matches.
        //the objects being sent here are all cloned, so they will have the same name if they are indeed the same.

        List<GameObject> tobeDeleted = new List<GameObject>();

        if (droppedObjects.Count > 0)
        {
            foreach (GameObject o in droppedObjects)
            {
                //compare names.
                if (/*o.name == d.name*/ o.name.Contains(d.name.Substring(0, 3)))
                {
                    //if the names match
                    tobeDeleted.Add(o);

                    //destroy the old
                    //Destroy(o);

                    //get out of the forloop to prevent wrongly altering the list.
                    //break;
                }
            }

            if (tobeDeleted.Count > 0)
            {
                foreach (GameObject tbd in tobeDeleted)
                {
                    //remove from the list.
                    droppedObjects.Remove(tbd);
                    Destroy(tbd);

                }

            }
        }

        Destroy(d.GetComponent<DragMe>());

        //add the newly dropepd object to the list
        droppedObjects.Add(d);
    }

    /// <summary>
    /// destroy a specific child object.
    /// </summary>
    public void DestroyChild(GameObject c)
    {
        droppedObjects.Remove(c);
        Destroy(c);
    }

    public void ResetTable()
    {
        if (droppedObjects != null)
        {
            if (droppedObjects.Count > 0)
            {
                foreach (GameObject o in droppedObjects)
                {
                    Destroy(o);
                }
            }
            droppedObjects.Clear();
        }

    }
}
