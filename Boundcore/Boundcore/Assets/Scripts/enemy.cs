using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemy : MonoBehaviour
{
    public float health;

    public bool hitDealay;

    public EnemyAiTutorial Es;

    public Slider healthbar;

    public Animator anim;

    public bool isDeath;

    public GameObject Healthbar;

    public UnityEngine.AI.NavMeshAgent agent;

  

    public EnemyAiTutorial script;
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        print("hit");

        if (other.tag == "sword")
        {
            if(hitDealay )
                print("hit2");
                hitDealay = true;
                StartCoroutine(getHit());
        }


    }

    private void Start()
    {
        script.enabled = true;

        anim.GetComponent<Animator>();

        isDeath = false;

    }

    void Update()
    {
        if(health < 1)
        {
            anim.SetBool("death", true);
            StartCoroutine(death());
            isDeath = false;
        }

        healthbar.value = health;
    }

    private IEnumerator getHit()
    {
        hitDealay = false;
        health = health - 1;
        yield return new WaitForSeconds(4f);
        hitDealay = true;
       

    }

    private IEnumerator death()
    {
        agent.enabled = false;
        Healthbar.SetActive(false);
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
        

    }
}
