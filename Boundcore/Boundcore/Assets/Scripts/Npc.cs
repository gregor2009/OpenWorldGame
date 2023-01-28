
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
    public bool inWaveRange;
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
        }        
    }

    public void Walking()
    { 
        anim.SetBool("isWaving", false);
        //Set Walk Point
        randomZ = Random.Range(-walkPointRange, walkPointRange);
        randomX = Random.Range(-walkPointRange, walkPointRange);
        
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        //Walk
        anim.SetBool("isWalking", true);
        agent.SetDestination(walkPoint);

        // Start Waiting
        StartCoroutine(Waiting());
      
    }



    public IEnumerator Waiting()
    {
        if(!inWaveRange)
        {
         //Wait
            anim.SetBool("isWaving", false);
            waitTime = Random.Range(3f, 20f);
            yield return new WaitForSeconds(waitTime);
            agent.enabled = true;
            Walking();

        }
       
       
    }

    void OnTriggerEnter(Collider other)
    {   
        // Check for Player
        if (other.gameObject.tag == "Player")
        {
            agent.enabled = false;
            inWaveRange = true; 
            anim.SetBool("isWaving", true);
            anim.SetBool("isWalking", false);
         
           
 
        }
        if(other.gameObject.tag != "Player")
        {
            print("Waving stopp");
            agent.enabled = true;
            StartCoroutine(Waiting());
            
        }
    }



   
}
