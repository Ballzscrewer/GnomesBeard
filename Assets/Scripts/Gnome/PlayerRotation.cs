using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerRotation : MonoBehaviour
{
    private CharacterController _characterController;
    private PlayerController _playerController;
    private ButtonsChecker _buttonsChecker;
    private MouseByCamera _mouseByCamera;
    private bool _shiftFlag;
    private bool _pkmState;
    private Vector3 _direction = new Vector3(0, 0, 0);
    [SerializeField] private float _rotationSpeed;
    private float _rollingAngle;
    private Quaternion _targetRotation = new Quaternion();

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerController = GetComponent<PlayerController>();
        _mouseByCamera = GetComponent<MouseByCamera>();
        _buttonsChecker = GetComponent<ButtonsChecker>();
    }

    private void OnEnable()
    {
        _buttonsChecker.ShiftBehavior += ShiftReaction;
        _buttonsChecker.PKMBehaviour += PKMReaction;
        _mouseByCamera.GroundMouseCoordsBehavior += mouseRotation;
    }

    private void ShiftReaction(bool isShifted)
    {
        _shiftFlag = isShifted;
        //фикс убираем крена когда перс перешел на ходьбу а пкм то зажат
        if (!isShifted)
        {
            _direction = _playerController.movingDirection;
            _targetRotation = Quaternion.LookRotation(_direction);
        }
    }
    private void PKMReaction(bool pkmState)
    {
        _pkmState = pkmState;
    }

    private void mouseRotation(Vector3 mousePosition)
    {
        if (!_shiftFlag)
        {
            if (!_pkmState)
            {
                //поворот относительно мыши
                _direction = (mousePosition - transform.position).normalized;
                _direction.y = 0;
                _targetRotation = Quaternion.LookRotation(_direction);
            }
        }
        else
        {
            //поворот беговой
            _direction = _playerController.movingDirection;
            _targetRotation = Quaternion.LookRotation(_direction);
            //крен
            _targetRotation *= Quaternion.Euler(0, 0, -Vector3.Cross(transform.forward, _direction).y * 100f);
        }

        //поворот Ќ≈ѕќ—–≈ƒ—“¬≈ЌЌџ…
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);
    } 
}
