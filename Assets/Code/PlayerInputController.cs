using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerInputController : MonoBehaviour
{
    public LayerMask groundMask;

    private NavMeshAgent agent;
    private PlayerCameraController CameraController;   
    private float _zoomSpeed = 4.0f, _currentZoom;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        CameraController = GetComponent<PlayerCameraController>();
    }

    private void Update()
    {
        // Handle changes to camera zoom
        Camera cam = CameraController.getCamera();
        _currentZoom -= Input.GetAxis("Mouse ScrollWheel") * _zoomSpeed;
        CameraController.setCurrentZoom(_currentZoom);
        
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
            Input.GetAxis("Horizontal");
        }
    }
}