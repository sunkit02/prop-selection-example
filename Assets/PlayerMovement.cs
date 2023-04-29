using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Transform playerTransform;
    public float movementSpeed = 10f;
    public float rotationSpeed = 10f;
    public Vector3 cameraOffset = new Vector3(0, 2f, -5f);
    public Camera playerCamera;
    public float selectionRange = 2f;

    public const float COOL_DOWN = 3f;
    public float currentCoolDown = 0f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = playerTransform.position;
        transform.rotation = playerTransform.rotation;
        playerCamera = transform.GetChild(0).GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        PlayerRotate();
        CheckForProp();
        if (Input.GetKeyDown(KeyCode.Space)) ChangeProp();

        if (currentCoolDown > 0) currentCoolDown -= Time.deltaTime;
    }

    void PlayerMove()
    {
        Vector3 movement = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            movement.z += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movement.z -= 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movement.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movement.x += 1;
        }

        if (movement != Vector3.zero)
        {
            playerTransform.position += playerTransform.rotation * (movement * movementSpeed * Time.deltaTime);
            transform.position = playerTransform.position;
            playerTransform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }

        // Move and rotate camera 
        playerCamera.transform.position = playerTransform.position + (playerTransform.rotation * cameraOffset);
    }
    public void PlayerRotate()
    {
        Quaternion rotation = Quaternion.Euler(0, 0, 0);
        if (Input.GetKey(KeyCode.Q))
        {
            rotation *= Quaternion.Euler(0, -10 * rotationSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotation *= Quaternion.Euler(0, 10 * rotationSpeed * Time.deltaTime, 0);
        }
        playerTransform.rotation *= rotation ;
        transform.rotation = playerTransform.rotation;
    }

    void CheckForProp()
    {
        if (currentCoolDown <= 0) {
            Ray ray = new Ray(playerTransform.position, playerTransform.forward);
            RaycastHit hit;

            // Check if the ray hits an object within the selection range
            if (Physics.Raycast(ray, out hit, selectionRange))
            {
                // Check if the object has a specific tag or component
                if (hit.collider.CompareTag("Prop"))
                {
                    hit.collider.gameObject.GetComponent<PropControl>().DisplayMarker();
                }
            }
        }
    }

    void ChangeProp()
    {
        if (currentCoolDown <= 0)
        {
            // Cast a ray from the player's position
            Ray ray = new Ray(playerTransform.position, playerTransform.forward);
            RaycastHit hit;

            // Check if the ray hits an object within the selection range
            if (Physics.Raycast(ray, out hit, selectionRange))
            {
                // Check if the object has a specific tag or component
                if (hit.collider.CompareTag("Prop"))
                {
                    Debug.Log("Found it!!!!!");
                    var newProp = Instantiate(hit.collider.gameObject);
                    newProp.transform.position = playerTransform.position;
                    newProp.transform.rotation = playerTransform.rotation;
                    newProp.GetComponent<PropControl>().marker = null;

                    Destroy(playerTransform.gameObject);

                    playerTransform = newProp.transform;

                    currentCoolDown = COOL_DOWN;
                }
            }
        }
    }
}
