using UnityEngine;
using Assets.scripts;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    public GameObject waypointMarker;

    private NavMeshAgent _agent;
    private List<Waypoint> _waypoints;
    private Vector3 _currentDestination;


    public Camera cam;


	// Use this for initialization
	void Start () {
        _agent = GetComponent<NavMeshAgent>();
        _waypoints = new List<Waypoint>();
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
                int thisIndex = _waypoints.FindIndex(w => IsPosition(w.Position, _currentDestination));
                if (thisIndex > -1 && _waypoints.Count - 1 > thisIndex)
                {
                    //set the next waypoint
                    _currentDestination = _waypoints[thisIndex + 1].Position;
                    //destroy the marker
                    Destroy(_waypoints[thisIndex].Marker);
                }
            }
            _agent.SetDestination(_currentDestination);
        }

        //adding waypoints on click
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            //check what we've clicked on
            if (Physics.Raycast(ray, out hit))
            {

                NavMeshHit nHit;
                NavMesh.FindClosestEdge(hit.point, out nHit, NavMesh.AllAreas);
                
                //see if destination is reachable
                NavMeshPath path = new NavMeshPath();
                if (NavMesh.CalculatePath(transform.position, hit.point, NavMesh.AllAreas, path))
                {
                    if (!Input.GetKey(KeyCode.LeftControl))
                    {
                        //if we're not holding control, we're clearing the current wayponits and setting new destination
                        ClearWaypoints();
                        _currentDestination = hit.point;
                    }

                    //create new marker and add it to the list
                    GameObject marker = (GameObject)Instantiate(waypointMarker, hit.point, new Quaternion());
                    _waypoints.Add(new Waypoint(hit.point, marker));

                    Debug.Log("Waypoint added: " + hit.point.x + ":" + hit.point.y + ":" + hit.point.z);
                }

            }
        }
    }

    bool IsPosition(Vector3 waypoint1, Vector3 waypoint2)
    {
        return (waypoint1.x == waypoint2.x && waypoint1.y == waypoint2.y && waypoint1.z == waypoint2.z);
    }

    void ClearWaypoints()
    {
        for (int i = 0; i < _waypoints.Count; i++)
        {
            Destroy(_waypoints[i].Marker);
        }
        _waypoints.Clear();
    }
}
