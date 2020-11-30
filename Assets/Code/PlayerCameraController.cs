
using System;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCameraController : MonoBehaviour
{
    private const float CameraArcRadius = 4.5f;
    private const float RotationSpeed = 5f;
    private const float ZoomSpeed = 1f;
    private const float MinZoom = 3f;
    private const float MaxZoom = 1f;
    
    [SerializeField]
    private Vector3 cameraOffset;
    private Camera _cam;
    private NavMeshAgent _agent;
    private float _currentZoom = MaxZoom;
    

    private void Start()
    {
        _cam = Camera.main;
        _agent = GetComponent<NavMeshAgent>();
        cameraOffset = new Vector3(0.0f, -CameraArcRadius, CameraArcRadius);
    }

    private void Update()
    {
        Transform cameraTransform = _cam.transform;
        Transform agentTransform = _agent.transform;
        
        cameraTransform.position = agentTransform.position - cameraOffset * _currentZoom;
        cameraTransform.LookAt(agentTransform);
    }
    
    /********************************************** 
    * 
    * Helper Functions for Private Variables
    * 
    **********************************************/

    public Vector3 getCameraOffset()
    {
        return cameraOffset;
    }
    
    public void setCameraOffset(float angle)
    {
        Quaternion camRotateAngle = Quaternion.AngleAxis(angle * RotationSpeed, Vector3.up);
        cameraOffset = camRotateAngle * getCameraOffset();
    }
    public Camera getCamera()
    {
        return _cam;
    }

    public void setCurrentZoom(float zoom)
    {
        _currentZoom -= zoom * ZoomSpeed;
        _currentZoom = Mathf.Clamp(_currentZoom, MaxZoom, MinZoom);
    }
}
