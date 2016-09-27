using UnityEngine;
using Assets.scripts;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    public GameObject waypointMarker;

    private NavMeshAgent _agent;
    private Animator _animator;
    private List<Waypoint> _waypoints;
    private Vector3 _currentDestination;
    private Vector2 _velocity;
    private Vector2 _smoothDeltaPosition;


    public Camera cam;


	// Use this for initialization
	void Start () {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>() ;
        _waypoints = new List<Waypoint>();


        _agent.updatePosition = false;
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

                } else {
                    _animator.SetBool("move", false);
                }
            } else
            {
                Vector3 worldDeltaPosition = _agent.nextPosition - transform.position;

                // Map 'worldDeltaPosition' to local space
                float dx = Vector3.Dot(transform.right, worldDeltaPosition);
                float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
                Vector2 deltaPosition = new Vector2(dx, dy);

                // Low-pass filter the deltaMove
                float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
                _smoothDeltaPosition = Vector2.Lerp(_smoothDeltaPosition, deltaPosition, smooth);

                // Update velocity if time advances
                if (Time.deltaTime > 1e-5f)
                    _velocity = _smoothDeltaPosition / Time.deltaTime;

                //bool shouldMove = _velocity.magnitude > 0.5f && _agent.remainingDistance > _agent.radius;

                // Update animation parameters
                _animator.SetBool("move", true);
                _animator.SetFloat("velx", _velocity.x);
                _animator.SetFloat("vely", _velocity.y);

                //GetComponent<LookAt>().lookAtTargetPosition = agent.steeringTarget + transform.forward;



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

    void OnAnimatorMove()
    {
        // Update position to agent position
        transform.position = _agent.nextPosition;
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
