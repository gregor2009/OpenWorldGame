
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Npc : MonoBehaviour
{

    public float randomX;
    public float randomZ;
    

    
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

    // Start is called before the first frame update
    void Start()
    {

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();
        Walking();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position == walkPoint)
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning",false);
            agent.speed = 1.5f;
             // Start Waiting
             StartCoroutine(Waiting());
             runAway = false;
        }     

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        angrySightRange = Physics.CheckSphere(transform.position, playerNear, whatIsPlayer);

        if(playerInSightRange && !angrySightRange && !checkPlayerToLook.isLooking && !Input.GetKey("mouse 1") && !runAway)
        {
            Go = true;
            agent.enabled = false;
            anim.SetBool("isWalking", false);
            anim.SetBool("isWaving", true);
            
            Vector3 direction = target.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, LookSpeed * Time.deltaTime);
        }  

        if(!playerInSightRange && !runAway )
        {
            agent.enabled = true;
            anim.SetBool("isWaving", false);

            if(Go)
            {
                Go = false;
                StartCoroutine(Waiting());
            }
            
        } 

        if(angrySightRange && !checkPlayerToLook.isLooking  && !Input.GetKey("mouse 1") && !runAway)
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

        if(!angrySightRange && !runAway)
        {
            agent.enabled = true;
            anim.SetBool("isAngry", false);

            if(go)
            {
                go = false;
                StartCoroutine(Waiting());
            }

        }
//Run away
        if(playerInSightRange && checkPlayerToLook.isLooking && Input.GetKey("mouse 1") )
        {
            //change speed
            agent.speed = 4;

            //set a point
            randomZ = Random.Range(10, 15);
            randomX = Random.Range(10, 15); 
            walkPoint = new Vector3(transform.position.x + randomX, high , transform.position.z + randomZ);

            //run to point
            agent.SetDestination(walkPoint);
            anim.SetBool("isWalking", false);
            anim.SetBool("isAngry", false);
            anim.SetBool("isWaving", false);
            anim.SetBool("isRunning", true);

            runAway = true;
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



    public IEnumerator Waiting()
    {
        if(!checkPlayerToLook.isLooking && !Input.GetKey("mouse 1"))
        {
            randomZ = Random.Range(-walkPointRange, walkPointRange);
            randomX = Random.Range(-walkPointRange, walkPointRange); 
            walkPoint = new Vector3(transform.position.x + randomX, high , transform.position.z + randomZ);

            anim.SetBool("isWalking", false);
            waitTime = Random.Range(3f, 10f);
            yield return new WaitForSeconds(waitTime);
            Walking();
        }

    }




   
}
