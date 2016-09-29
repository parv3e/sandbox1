using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {


    //characteristics
    public bool isMobile = true;
    public bool isVulnerable = true;

    //attributes
    public float strength;
    public float dexterity;
    public float vitality;
    public float inteligence;

    //skills
    public float melee;

    //current status
    private float _hitPoints;
    private float _mana;
    private float _stamina;

    //calculated
    public float MaxHitPoints { get { return strength + (vitality * strength / 100f); } }
    public float MaxStamina { get { return (strength / 4f) + dexterity + (vitality * dexterity / 100f); } }
    public float MaxMana { get { return inteligence * 1.25f; } }

    //INTERNAL STUFF
    private float _speedModifier;
    
    // Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {



	}
}
