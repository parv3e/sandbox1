using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Entity))]
public class AI : MonoBehaviour {

    public bool isHostile = true;
    public float enemyScanRange = 20f;
    public float enemyDropTargetRange = 40f;
    public float enemyMinRange = 0.5f;
    public float enemyMaxRange = 35f;
    public float meleeAttackRange = 1f;
    public float fleeHitPointRatio = 0.1f;

    private Entity _entity;
    private GameObject _currentTargetGameObject;
    private Entity _currentTargetEntity;
    private NavMeshAgent _nmAgent;
    private Animator _animator;
    
    // Use this for initialization
	void Start () {
        _entity = GetComponent<Entity>();
        _nmAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        //frame variables
        float f_distanceToTarget = 0;

        //1. TARGET MANAGEMENT
        //drop the target if to far away
        if (_currentTargetGameObject && Vector3.Distance(gameObject.transform.position, _currentTargetGameObject.transform.position) > enemyDropTargetRange)
        {
            SetTarget(null);
        }

        //scan for target if i'm hostile
        if (isHostile)
        {
            //and i haven't got a target, HACK: this is separate, as we might want to switch targets later
            if (_currentTargetGameObject == null)
            {
                Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, enemyScanRange);
                // loop through scan results
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    Collider thisCollider = hitColliders[i];
                    //we're considering only entities
                    if (thisCollider.gameObject.GetComponent<Entity>())
                    {
                        //HACK: for now we're only considering player
                        if (thisCollider.gameObject.tag == "Player")
                        {
                            SetTarget(thisCollider.gameObject);
                        }
                    }
                }
            }
        }
        if (_currentTargetGameObject)
        {
            f_distanceToTarget = Vector3.Distance(gameObject.transform.position, _currentTargetGameObject.transform.position);
        }


        //2. ATTACK

        //3. CAST SPELLS

        //4. MOVE
        //if i'm mobile, if i'm hostile and i've got the target, do i need to get closer?
        bool f_isMoving = false;
        if (_nmAgent && _currentTargetGameObject && isHostile && f_distanceToTarget > enemyMinRange)
        {
            _nmAgent.SetDestination(_currentTargetGameObject.transform.position);
            f_isMoving = true;
        }
        //let the animator know if we're moving
        if (_animator)
            _animator.SetBool("move", f_isMoving);
        
	}

    private void SetTarget(GameObject target)
    {
        _currentTargetGameObject = target;
        _currentTargetEntity = target.GetComponent<Entity>();
    }

    void OnAnimatorMove()
    {
        // Update position to agent position
        transform.position = _nmAgent.nextPosition;
    }
}
