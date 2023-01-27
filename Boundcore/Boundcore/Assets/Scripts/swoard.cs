using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swoard : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        print("hit");

        if(other.tag == "enemy")
        {
            print("hit2");
            Destroy(other.gameObject);
            
        }


    }
}
