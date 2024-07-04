using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseByCamera : MonoBehaviour
{
    [SerializeField] private Camera _characterCamera;
    [SerializeField] private GameObject _sharik;
    [SerializeField] private LayerMask _groundCheckLayer;
    private ButtonsChecker _buttonsChecker;
    private Vector3 _mousePosition;

    public event Action<Vector3> GroundMouseCoordsBehavior;

    private void MouseReaction(Vector3 mousePosition)
    {
        _mousePosition = mousePosition;
    }

    private void Awake()
    {
        _buttonsChecker = GetComponent<ButtonsChecker>();
    }

    private void OnEnable()
    {
        _buttonsChecker.MouseBehavior += MouseReaction;
    }

    void Update()
    {
        Ray cameraMouseRay = _characterCamera.ScreenPointToRay(_mousePosition);
        if(Physics.Raycast(cameraMouseRay, out RaycastHit hit, Mathf.Infinity, _groundCheckLayer))
        {
            GroundMouseCoordsBehavior(hit.point);
        }
        
    }
}
