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
        //_agent.autoBraking = false;
        _waypoints = new List<Vector3>();
    }
	
	// Update is called once per frame
	void Update () {

        Debug.Log("Remaining distance: " + _agent.remainingDistance + "Current Waypoint: " + _currentWaypoint.x + ":" + _currentWaypoint.y + ":" + _currentWaypoint.z);

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                _waypoints.Add(hit.point);
                if (_waypoints.Count == 1)
                {
                    _currentWaypoint = hit.point;
                }
                Debug.Log("Waypoint added: " + hit.point.x + ":" + hit.point.y + ":" + hit.point.z);
            }
        }


        if (_waypoints.Count > 0)
        {
            if (_agent.remainingDistance <= 0.5f)
            {
                Debug.Log("Waypoint reached.");
                int thisIndex = _waypoints.FindIndex(w => w.x == _currentWaypoint.x && w.y == _currentWaypoint.y && w.z == _currentWaypoint.z);
                if (thisIndex > -1 && _waypoints.Count >= thisIndex)
                {
                    _currentWaypoint = _waypoints[thisIndex + 1];
                }
            }
            _agent.SetDestination(_currentWaypoint);
        }

    }

    bool IsWaypoint(Vector3 waypoint1, Vector3 waypoint2)
    {
        return (waypoint1.x == waypoint2.x && waypoint1.y == waypoint2.y && waypoint1.z == waypoint2.z);
    }
}
