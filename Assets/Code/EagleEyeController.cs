﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class EagleEyeController : MonoBehaviour
{
    public LayerMask groundMask;
    public Vector3 cameraOffset;
    public Animator animator;
    public float zoomSpeed = 4f;
    public float rotationSpeed = 4f;
    public float minZoom = 2f;
    public float maxZoom = 6f;

    NavMeshAgent agent;
    Camera cam;

    private float currentRotation;
    private float currentZoom = 4f;

    // Start is called before the first frame update
    void Start()
    {
        // Init our camera, nav agent, and animator
        cam = Camera.main;
        cameraOffset = new Vector3(0f, -2f, 1.3f);
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        /**************************
        * 
        * Animation Handling
        * 
        **************************/

        animator.SetFloat("Speed", agent.velocity.magnitude);

        /**************************
        * 
        * Mouse Event Handling
        * 
        **************************/

        // Handle changes to camera zoom
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        // If user left clicks (Focus)
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                // If we hit enemy, attack
                // If we hit interactable, etc.
                // if we hit possessible, etc.
            }
        }

        // If user right clicks (Movement)
        if(Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Get the point on the ground that was clicked on screen
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask))
            {
                // Move player towards where they clicked on the ground layer and remove focus
                agent.SetDestination(hit.point);
            }
        }
        
        // If user middle clicks (Camera rotation)
        if (Input.GetMouseButtonDown(2))
        {
            print("Middle Click!");
            currentRotation += Input.GetAxis("Horizontal") * rotationSpeed;
        }
    }
    
    void LateUpdate()
    {
        // Adjust the camera position, ensure the player is still the focus
        cam.transform.position = agent.transform.position - cameraOffset * currentZoom;
        cam.transform.Rotate(currentRotation, 0.0f, 0.0f, Space.Self); //TODO: fix this bc it doesn't work
        cam.transform.LookAt(agent.transform);
    }

}
