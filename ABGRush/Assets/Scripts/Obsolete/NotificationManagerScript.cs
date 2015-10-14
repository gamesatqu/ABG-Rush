using UnityEngine;
using System.Collections;

public class NotificationManagerScript : MonoBehaviour {

    public Transform notificationsBackground;
    public GameObject[] notiPrefab;

    private Animator anim;

    private int hashActiveNotifications = Animator.StringToHash("ActiveNotifications");
    private int activeNotifications;

    private bool closed = true;//determine if the notification system is open or closed.


    void Awake()
    {
        anim = GetComponent<Animator>();
    }

	// Use this for initialization
	void Start () {
	    //at the moment, call Reset at the beginning. This may change once a manager is added.
        Reset();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.N))
        {
            //NewNotification();
        }
	}

    public void Reset()
    {
        activeNotifications = 0;
        anim.SetInteger(hashActiveNotifications, activeNotifications);
    }

    public void NewNotification()
    {
        Debug.Log("Childcount of notifications background = " + notificationsBackground.childCount);
        GameObject n = Instantiate(notiPrefab[Random.Range(0, notiPrefab.Length)]) as GameObject;
        //GameObject d = notificationsBackground.GetChild(notificationsBackground.childCount - 1).gameObject;
        //Destroy( d);
        Destroy(notificationsBackground.GetChild(notificationsBackground.childCount - 1).gameObject);

        //place on top of the notification stack
        n.transform.SetParent(notificationsBackground);
        n.transform.SetAsFirstSibling();
        
        //update the active number
        activeNotifications++;
        activeNotifications = Mathf.Clamp(activeNotifications, 0, 3);
        anim.SetInteger(hashActiveNotifications, activeNotifications);
    }

    /// <summary>
    /// called by public script
    /// </summary>
    /// <param name="n">The notification to be removed</param>
    public void RemoveNotification(GameObject n)
    {
        //update the active number
        activeNotifications--;
        anim.SetInteger(hashActiveNotifications, activeNotifications);

        //delete this notification
        Destroy(n);        

        //add a dummy notification to help/assist with keeping size.
        GameObject dummy = Instantiate(notiPrefab[Random.Range(0, notiPrefab.Length)]) as GameObject;
        dummy.transform.SetParent(notificationsBackground);

        
    }

    /// <summary>
    /// called when the player clicks on the notification bar.
    /// </summary>
    public void CloseOpenNotifications()
    {
        //come back and add more to this later.
        anim.SetInteger(hashActiveNotifications, 0);
    }
}
