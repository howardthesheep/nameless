using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class EagleEyeController : MonoBehaviour
{
    public GameObject player;
    public Vector3 cameraOffset;
    public Animator animator;
    public float zoomSpeed = 4f;
    public float rotationSpeed = 4f;
    public float minZoom = 2f;
    public float maxZoom = 6f;

    
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
        
    }
    
    void LateUpdate()
    {
        // Adjust the camera position, ensure the player is still the focus
        cam.transform.RotateAround(player.transform.position, cam.transform.right, currentRotation * Time.deltaTime);
        cam.transform.position = agent.transform.position - cameraOffset * currentZoom;
        cam.transform.LookAt(agent.transform);
        currentRotation = 0;
    }

}
