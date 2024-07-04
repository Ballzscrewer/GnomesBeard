using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class cameraSledovanie : MonoBehaviour
{
    [SerializeField] private GameObject _chasedObject;
    [SerializeField] private float _maxRotationSpeed;
    [SerializeField] private float _minXZDist;
    [SerializeField] private float _defaultY;
    [SerializeField] private float _pkmRotationSpeed = 1f;
    private float _defaultXAngle;
    private ButtonsChecker _buttonsChecker;
    private Vector3 _fishingRod = new Vector3();
    private bool _pkmState = false;

    private Vector3 _movementFishingRod;
    private Vector3 _rotationFishingRod;
    private float _movementSpeed = 0f;
    private float _rotationSpeed = 0f;

    private Camera _camera;
    private float _mouseNewX = 0f;
    private float _mouseLastX = 0f;

    private float tmp;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        if(_chasedObject != null)
        {
            _buttonsChecker = _chasedObject.GetComponent<ButtonsChecker>();
            //первичный вкид УДИЛИЩА
            _fishingRod = _chasedObject.transform.position - transform.position;
        }
    }

    private void OnEnable()
    {
        if (_buttonsChecker != null)
        {
            _buttonsChecker.PKMBehaviour += PKMReaction;
            _buttonsChecker.MouseBehavior += MouseReaction;
        }
    }

    private void PKMReaction(bool pkmState)
    {
        _pkmState = pkmState;
        if (_pkmState)
        {
            float y = _fishingRod.y;
            _fishingRod.y = 0;
            _fishingRod = _fishingRod.normalized * _minXZDist;
            _fishingRod.y = -_defaultY;
        }
    }

    private void MouseReaction(Vector3 mousePosition)
    {
        _mouseLastX = _mouseNewX;
        _mouseNewX = _camera.ScreenToViewportPoint(mousePosition).x;
    }

    void Update()
    {
        if (_chasedObject != null)
        {
                                        //легендарная Филипповская УДИЛЬНАЯ СИСТЕМА
            
            _rotationFishingRod = _fishingRod;
            _rotationFishingRod.y = 0;

            _defaultXAngle = Mathf.Atan2(_defaultY, _minXZDist) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.LookRotation(_rotationFishingRod) * Quaternion.Euler(_defaultXAngle, 0, 0);
            float angleDiff = Quaternion.Angle(transform.rotation, targetRotation);

            if (!_pkmState)
            {
                _movementFishingRod = _fishingRod;
                _movementFishingRod.y = 0;

                //параметры движения
                _movementFishingRod -= _movementFishingRod.normalized * _minXZDist;
                _movementFishingRod.y = _fishingRod.y + _defaultY;

                //вычисляем скорости
                _movementSpeed = _movementFishingRod.magnitude * Time.deltaTime;
                _rotationSpeed = _maxRotationSpeed * (angleDiff / 180f) * Time.deltaTime;

                //повторный ЗАБРОС
                _fishingRod = _chasedObject.transform.position - transform.position;

            }
            else
            {
                _movementFishingRod = (_chasedObject.transform.position - _fishingRod) - transform.position;

                _movementSpeed = 1;
                _rotationSpeed = angleDiff;

                //повторное положение УДА по окружности
                float obletAngle = (_mouseNewX - _mouseLastX) * _pkmRotationSpeed;
                float newX = _fishingRod.x * Mathf.Cos(obletAngle) - _fishingRod.z * Mathf.Sin(obletAngle);
                float newZ = _fishingRod.x * Mathf.Sin(obletAngle) + _fishingRod.z * Mathf.Cos(obletAngle);
                _fishingRod.x = newX;
                _fishingRod.z = newZ;
            }

            transform.Translate(_movementSpeed * _movementFishingRod, Space.World);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed);
        } 
    }
}
