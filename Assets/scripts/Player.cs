using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    private NavMeshAgent _agent;
    private List<Vector3> _waypoints;
    private Vector3 _currentWaypoint;


    public Camera cam;


	// Use this for initialization
	void Start () {
        _agent = GetComponent<NavMeshAgent>();
        _waypoints = new List<Vector3>();
    }
	
	// Update is called once per frame
	void Update () {

        //Debug.Log("Remaining distance: [" + _agent.remainingDistance + "] Current Waypoint: [" + _currentWaypoint.ToString() + "] Waypoints: [" + _waypoints.Count.ToString() + "]");

        //if we've got some waypoints, we need to move!
        if (_waypoints.Count > 0)
        {
            if (_agent.remainingDistance <= 0.5f)
            {
                //you have reached your destination
                Debug.Log("Waypoint reached.");
                int thisIndex = _waypoints.FindIndex(w => IsWayPoint(w, _currentWaypoint));
                if (thisIndex > -1 && _waypoints.Count - 1 > thisIndex)
                {
                    //set the next waypoint
                    _currentWaypoint = _waypoints[thisIndex + 1];
                }
            }
            _agent.SetDestination(_currentWaypoint);
        }

        //adding waypoints on click
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (!Input.GetKey(KeyCode.LeftControl))
                {
                    _waypoints.Clear();
                    _currentWaypoint = hit.point;
                }
                _waypoints.Add(hit.point);
                Debug.Log("Waypoint added: " + hit.point.x + ":" + hit.point.y + ":" + hit.point.z);
            }
        }
    }

    bool IsWayPoint(Vector3 waypoint1, Vector3 waypoint2)
    {
        return (waypoint1.x == waypoint2.x && waypoint1.y == waypoint2.y && waypoint1.z == waypoint2.z);
    }
}
