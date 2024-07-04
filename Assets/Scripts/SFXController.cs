using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SFXController : MonoBehaviour
{
    private ButtonsChecker _buttonsChecker;
    private PlayerController _playerController;

    void Awake()
    {
        _buttonsChecker = GetComponent<ButtonsChecker>();
        _playerController = GetComponent<PlayerController>();
        OnAwaking();
    }

    private void OnEnable()
    {
        _buttonsChecker.ShiftBehavior += ShiftReaction;
        _playerController.MovingBehavior += MovingReaction;
        OnOnEnabling();
    }

    protected abstract void OnAwaking();

    protected abstract void OnOnEnabling();

    protected abstract void ShiftReaction(bool shifted);

    protected abstract void MovingReaction(bool isMoving);
}
