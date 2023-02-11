using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerToLook : MonoBehaviour
{

    public LayerMask mask; 
    public bool isLooking;    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(transform.position, transform.forward, out var hit, Mathf.Infinity, mask))
        {
            isLooking = true;
    
            Debug.Log("Check npc");
        }else
        {
            isLooking = false;
        }
    }
}
