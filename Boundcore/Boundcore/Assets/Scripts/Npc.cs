
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

//Angry
    public float playerNear;
    public bool angrySightRange;
    public bool go;

//Trade
    public GameObject TradeScreen; 
    public GameObject PressE;
    public bool CanExit;

    // Start is called before the first frame update
    void Start()
    {
        CanExit = false;
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
        angrySightRange = Physics.CheckSphere(transform.position, playerNear, whatIsPlayer);

        if(playerInSightRange)
        {
            if(Input.GetButtonDown("e") && !CanExit)
            {
                TradeScreen.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                StartCoroutine(canExit());
            }

            if(CanExit && Input.GetButtonDown("e"))
            {
                CanExit = false;
                Cursor.lockState = CursorLockMode.Locked;
                TradeScreen.SetActive(false);
            }

            PressE.SetActive(true);
        }

        if(playerInSightRange && !angrySightRange)
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
            PressE.SetActive(false);
            CanExit = false;
            Cursor.lockState = CursorLockMode.Locked;
            TradeScreen.SetActive(false);
            agent.enabled = true;
            anim.SetBool("isWaving", false);

            if(Go)
            {
                Go = false;
                StartCoroutine(Waiting());
            }
            
        } 

        if(angrySightRange)
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

        if(!angrySightRange)
        {
            agent.enabled = true;
            anim.SetBool("isAngry", false);

            if(go)
            {
                go = false;
                StartCoroutine(Waiting());
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



    public IEnumerator Waiting()
    {
        randomZ = Random.Range(-walkPointRange, walkPointRange);
        randomX = Random.Range(-walkPointRange, walkPointRange); 
        walkPoint = new Vector3(transform.position.x + randomX, 0.0832333f , transform.position.z + randomZ);

        anim.SetBool("isWalking", false);
        waitTime = Random.Range(3f, 10f);
        yield return new WaitForSeconds(waitTime);
        Walking();
    }

    public IEnumerator canExit()
    {
        yield return new WaitForSeconds(0.1f);
        CanExit = true;
    }




   
}
