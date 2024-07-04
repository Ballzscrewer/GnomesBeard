using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator _animator;
    private PlayerController _playerController;
    private ButtonsChecker _buttonsChecker;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerController = GetComponent<PlayerController>();
        _buttonsChecker = GetComponent<ButtonsChecker>();
    }

    private void OnEnable()
    {
        _playerController.MovingBehavior += SetWalking;
        _buttonsChecker.ShiftBehavior += SetRunning;
    }

    private void SetWalking(bool isWalking)
    {
        _animator.SetBool("isWalking", isWalking);
    }

    private void SetRunning(bool isRunning)
    {
        _animator.SetBool("isRunning", isRunning);
    }

}
