
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Npc : MonoBehaviour
{

    public float randomX;
    public float randomZ;
    public Transform player;

    
    public UnityEngine.AI.NavMeshAgent agent;
    public Animator anim;
//Walking
    public Vector3 walkPoint;
    public Vector3 distanceToWalkPoint;
    public bool walkPointSet;
    public float walkPointRange;
    public float high;

//Waiting
    public float waitTime;

//Waving
    public bool playerInSightRange;
    public float sightRange;
    public LayerMask whatIsPlayer;
    public Transform target;
    public float LookSpeed;
    public bool Go;

//Angry
    public float playerNear;
    public bool angrySightRange;
    public bool go;

//Run away
    public CheckPlayerToLook checkPlayerToLook;
    public bool runAway;

//decision
    public float decision;
//attack
   
    public bool boxing;

//death
    public bool isDeath;

    // Start is called before the first frame update
    void Start()
    {

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();
        Walking();
        boxing = false;
        runAway = false;
        isDeath = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isDeath)
        {
            agent.enabled = false;
        }

        if(transform.position == walkPoint && !boxing && !isDeath)
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning",false);
            agent.speed = 1.5f;
             // Start Waiting
             StartCoroutine(Waiting());
             runAway = false;
        }     

        if(boxing && !isDeath)
        {
            agent.SetDestination(player.position);
        }

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        angrySightRange = Physics.CheckSphere(transform.position, playerNear, whatIsPlayer);

        if(playerInSightRange && !angrySightRange && !checkPlayerToLook.isLooking && !Input.GetKey("mouse 1") && !runAway && !boxing && !isDeath)
        {
            Go = true;
            agent.enabled = false;
            anim.SetBool("isWalking", false);
            anim.SetBool("isWaving", true);
            
            Vector3 direction = target.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, LookSpeed * Time.deltaTime);
        }  

        if(!playerInSightRange && !runAway && !boxing && !isDeath)
        {
            agent.enabled = true;
            anim.SetBool("isWaving", false);

            if(Go)
            {
                Go = false;
                StartCoroutine(Waiting());
            }
            
        } 

        if(angrySightRange && !runAway && !boxing && !isDeath)
        {
            go = true;
            agent.enabled = false;

            anim.SetBool("isWalking", false);
            anim.SetBool("isWaving", false);
            anim.SetBool("isAngry", true);
                
            Vector3 direction = target.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, LookSpeed * Time.deltaTime);

        }

        if(!angrySightRange && !runAway && !boxing && !isDeath)
        {
            agent.enabled = true;
            anim.SetBool("isAngry", false);

            if(go)
            {
                go = false;
                StartCoroutine(Waiting());
            }

        }

        if(!angrySightRange && boxing && !isDeath)
        {
            anim.SetBool("isBoxing", false);
            anim.SetBool("isRunning", true);
            agent.enabled = true;
        }

        if(angrySightRange && boxing && !isDeath)
        {
            anim.SetBool("isBoxing", true);
            anim.SetBool("isRunning", false);
            agent.enabled = false;

        }

//Run away
        if(playerInSightRange && checkPlayerToLook.isLooking && Input.GetKey("mouse 1") && !runAway && !boxing && !isDeath)
        {
            decision = Random.Range(1f, 2f);

            if(decision < 1.5f)
            {
                RunAway();
                decision = 0f;
            }

            if(decision > 1.5f)
            {
                Attack();
                print("attack");
                decision = 0f;
            }
            
        }
        
    }

    public void Walking()
    { 
        
        //Set Walk Point
        anim.SetBool("isWalking", false);

        //Walk

        agent.SetDestination(walkPoint);
        if(!playerInSightRange)
        {
            anim.SetBool("isWalking", true);
        }
    }

    public void RunAway()
    {
        //change speed
        agent.speed = 7f;
    
        //set a point
        randomZ = Random.Range(15f, 25f);
        randomX = Random.Range(15f, 25f); 
        walkPoint = new Vector3(transform.position.x + randomX, high , transform.position.z + randomZ);

        //run to point
        
        anim.SetBool("isWalking", false);
        anim.SetBool("isAngry", false);
        anim.SetBool("isWaving", false);
        anim.SetBool("isRunning", true);

        agent.SetDestination(walkPoint);
        runAway = true;

       
    }

    public void Attack()
    {
        agent.speed = 5;
        print("attack");
        
        Vector3 direction = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, LookSpeed * Time.deltaTime);    
        anim.SetBool("isWalking", false);
        anim.SetBool("isAngry", false);
        anim.SetBool("isWaving", false);
        anim.SetBool("isRunning", false);
        //anim.SetBool("isBoxing", true);
        

        boxing = true;
    }



    public IEnumerator Waiting()
    {
        if(!checkPlayerToLook.isLooking && !Input.GetKey("mouse 1"))
        {
            randomZ = Random.Range(30, walkPointRange);
            randomX = Random.Range(30, walkPointRange); 
            walkPoint = new Vector3(transform.position.x + randomX, high , transform.position.z + randomZ);

            anim.SetBool("isWalking", false);
            waitTime = Random.Range(3f, 10f);
            yield return new WaitForSeconds(waitTime);
            Walking();
        }

    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "sword")
        {
            isDeath = true;
            anim.SetBool("isDeath", true);
        }
    }



   
}
