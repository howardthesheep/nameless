using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class InputController : MonoBehaviour
{
    public LayerMask groundMask;

    private NavMeshAgent agent;
    private Camera cam;

    private void Start()
    {
        // Grab our camera and NavMeshAgent
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
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
            
        }
    }
}