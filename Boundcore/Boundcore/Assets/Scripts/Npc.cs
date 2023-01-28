
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

//Waiting
    public float waitTime;

//Waving
    public bool playerInSightRange;
    public float sightRange;
    public LayerMask whatIsPlayer;
    public Transform target;
    public float LookSpeed;
    public bool Go;

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
             // Start Waiting
             StartCoroutine(Waiting());
        }     

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        if(playerInSightRange)
        {
            Go = true;
            agent.enabled = false;
            anim.SetBool("isWalking", false);
            anim.SetBool("isWaving", true);
            
            Vector3 direction = target.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, LookSpeed * Time.deltaTime);
        }  

        if(!playerInSightRange)
        {
            agent.enabled = true;
            anim.SetBool("isWaving", false);

            if(Go)
            {
                Go = false;
                Walking();
            }
            
        } 
    }

    public void Walking()
    { 
        
        //Set Walk Point
        randomZ = Random.Range(-walkPointRange, walkPointRange);
        randomX = Random.Range(-walkPointRange, walkPointRange);
        
        walkPoint = new Vector3(transform.position.x + randomX, 0.0832333f , transform.position.z + randomZ);

        //Walk
        if(!playerInSightRange)
        {
            anim.SetBool("isWalking", true);
        }
        agent.SetDestination(walkPoint);
      

       
      
    }



    public IEnumerator Waiting()
    {
        anim.SetBool("isWalking", false);
        waitTime = Random.Range(3f, 10f);
        yield return new WaitForSeconds(waitTime);
        Walking();
    }




   
}
