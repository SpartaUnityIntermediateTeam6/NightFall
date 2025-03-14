using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Reference
    private TPSCharacterController controller;
    private PlayerStats stats;

    void Awake()
    {
        //Refernce Initialize
        controller = GetComponent<TPSCharacterController>();
        stats = GetComponent<PlayerStats>();
    }

    void Update()
    {

    }
}
