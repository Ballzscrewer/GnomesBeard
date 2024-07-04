using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _inspectorSpeed;
    [SerializeField] private float _speedMultiplier;
    [SerializeField] private LayerMask _groundCheckLayers;
    [SerializeField] private GameObject _camera;

    private float _speed;
    private Vector3 _characterVelocity;
    private CharacterController _characterController;
    private float _groundCheckDistance;
    private bool _isGrounded;

    private bool _movingFlag = false;

    //кем то используется не удалять
    public Vector3 movingDirection = new Vector3(0, 0, 0);

    private ButtonsChecker _buttonsChecker;

    public event Action<bool> MovingBehavior;

    private bool _pkmState;

    private Vector2 _input = new Vector2();

    private void GroundCheck()
    {
        _isGrounded = false;
        Vector3 bottomSphere = transform.position - Vector3.up * _characterController.radius;
        if (Physics.SphereCast(
            bottomSphere, _characterController.radius,
            Vector3.down, out RaycastHit hit, _groundCheckDistance,
        _groundCheckLayers, QueryTriggerInteraction.Ignore))
        {
            _isGrounded = true;
            if (hit.distance > _characterController.skinWidth)
            {
                _characterController.Move(Vector3.down * hit.distance);
            }
        }
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _groundCheckDistance = _characterController.skinWidth + Physics.defaultContactOffset;
        _buttonsChecker = GetComponent<ButtonsChecker>();

        //забиваем дефолтную скорость
        _speed = _inspectorSpeed;
    }

    private void OnEnable()
    {
        _buttonsChecker.ShiftBehavior += SpeedControl;
        _buttonsChecker.PKMBehaviour += PKMReaction;
    }

    private void SpeedControl(bool shifted)
    {
        if(shifted) _speed = _inspectorSpeed * _speedMultiplier;
        else _speed = _inspectorSpeed;
    }

    private void PKMReaction(bool pkmState)
    {
        _pkmState = pkmState;
    }

    private void Update()
    {
        GroundCheck();

        //определение вектора движения
        Vector2 buttonsInput = Vector2.ClampMagnitude(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), 1f);

        //определение вектора движения относительсно камеры
        Vector2 directionVector = new Vector2(
                transform.position.x - _camera.transform.position.x,
                transform.position.z - _camera.transform.position.z
                );
        float angle = Vector2.SignedAngle(Vector2.up, directionVector);
        _input = Quaternion.Euler(0, 0, angle) * buttonsInput;

        //кем то используется не удалять
        movingDirection.Set(_input.x, 0, _input.y);

        //рассылка на движение
        if ((_input.magnitude > 0.1f) ^ _movingFlag)
        {
            _movingFlag = !_movingFlag;
            MovingBehavior?.Invoke(_movingFlag);
        }

        //двигаем
        _characterVelocity = new Vector3(_input.x * _speed, _characterVelocity.y, _input.y * _speed);

        if (_isGrounded) _characterVelocity.y = 0f;
        else _characterVelocity.y += Physics.gravity.y * Time.deltaTime;

        _characterController.Move(_characterVelocity * Time.deltaTime);
    }
}