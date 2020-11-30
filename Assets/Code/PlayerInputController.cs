
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerInputController : MonoBehaviour
{
    [SerializeField]
    private LayerMask groundMask;

    private float _zoomInput;
    private FocusTarget _focusInput;

    private PlayerCameraController pcc;
    private PlayerMovementController pmc;
    private bool _isRotating;

    private void Start()
    {
        pcc = GetComponent<PlayerCameraController>();
        pmc = GetComponent<PlayerMovementController>();
    }

    private void Update()
    {
        Camera cam = pcc.getCamera();
        
        // Handle changes to camera zoom
        _zoomInput = Input.GetAxis("Mouse ScrollWheel");
        if (_zoomInput != 0f)
        {
            pcc.setCurrentZoom(_zoomInput);
        }


        // Rotation is disengaged
        if (Input.GetMouseButtonUp(2))
        {
            _isRotating = false;
        }

        // Handle changes to camera rotation
        if (Input.GetMouseButtonDown(2))
        {
            _isRotating = true;
        }

        if (_isRotating)
        {
            pcc.setCameraOffset(Input.GetAxis("Mouse X"));
        }


        // Left click (Focus)
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

        // Right click (Movement)
        if(Input.GetMouseButtonDown(1))
        {
            // Get the point on the ground that was clicked on screen
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask))
            {
                // Update movementInput
                pmc.setDestination(hit.point);
            }
        }
    }

    /********************************************** 
    * 
    * Helper Functions for Private Variables
    * 
    **********************************************/
    public FocusTarget getFocus()
    {
        return _focusInput;
    }

    public enum FocusTarget
    {
        Unfocused,
        Possessible,
        Interactable,
        Enemy
    }
}