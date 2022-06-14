using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 5f;
    Vector3 forward, right;

    void Start()
    {

        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);

        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;


    }

    // Once per frame
    void Update()
    {

        // Call Move when Input happens
        if (Input.anyKey)
        {
            Move();
        }

    }

    void Move()
    {

        // Movement speed
        Vector3 rightMovement = right * moveSpeed * Input.GetAxis("Horizontal");
        Vector3 upMovement = forward * moveSpeed * Input.GetAxis("Vertical");

        // Calculate what is forward
        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

        // Setting of the new position
        Vector3 newPosition = transform.position;
        newPosition += rightMovement;
        newPosition += upMovement;

        // Smoothly move since addforce is a little more "aggressive"
        transform.forward = heading;
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime);

    }
}