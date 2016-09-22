using UnityEngine;
using System.Collections;

public class MoveTo : MonoBehaviour {

    public Transform goal;

    private NavMeshAgent _agent;

    // Use this for initialization
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update () {
        _agent.destination = goal.position;
    }
}
