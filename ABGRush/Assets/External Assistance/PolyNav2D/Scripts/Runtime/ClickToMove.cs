using UnityEngine;
using System.Collections.Generic;

//example
[RequireComponent(typeof(PolyNavAgent))]
public class ClickToMove : MonoBehaviour{
	
	private PolyNavAgent _agent;
	public PolyNavAgent agent{
		get
		{
			if (!_agent)
				_agent = GetComponent<PolyNavAgent>();
			return _agent;			
		}
	}

    private float Z
    {
        //set the z position so that this object will be drawn on the screen in the correct order.
        set { transform.position = new Vector3(transform.position.x, transform.position.y, value); }
    }


	void Update() {
        //set the z position so that the object will be drawn properly on the screen.
        Z = transform.position.y;
        //Debug.Log(transform.position.y);
        
        if (Input.GetMouseButton(0))
            agent.SetDestination(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        
	}

    
}