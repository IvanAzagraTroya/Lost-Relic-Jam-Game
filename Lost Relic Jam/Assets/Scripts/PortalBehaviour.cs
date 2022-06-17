using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBehaviour : MonoBehaviour
{
    // The private ones are declared to take themselves from the components, the only one that has to be declared 
    // public is the other portal u want to link.
    [Header("Declarative Components")]
    public Transform destinationPos;

    [Header("Autodefined Objects")]
    private Transform originPos;
    private BoxCollider portal;
    private Vector3 newPos;
    private GameObject player;

    void Start() {

        player = GameObject.Find("Player");
        portal = GetComponentInChildren(typeof(BoxCollider)) as BoxCollider;
        originPos = GetComponent<Transform>();
        newPos = destinationPos.position;
    }

    void OnTriggerEnter(Collider other) {
        Debug.Log("Algo ha entrado en el trigger del portal");
        if(other.tag == "Player") {
            Debug.Log("The player has entered the portal");
            player.transform.position = newPos;
        }
    }
}
