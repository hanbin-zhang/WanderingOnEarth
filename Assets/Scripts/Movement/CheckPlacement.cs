using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlacement : MonoBehaviour
{

    BuidingSystem buidingSystem;

    void Start()
    {
        buidingSystem = GameObject.Find("Player").GetComponent<BuidingSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            //buidingSystem.canPlace = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            //buidingSystem.canPlace = true;
        }
    }
}
