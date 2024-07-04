using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsChecker : MonoBehaviour
{
    private bool _shiftFlag = false;
    private bool _pkmFlag = false;

    public event Action<bool> ShiftBehavior;
    public event Action<Vector3> MouseBehavior;
    public event Action<bool> PKMBehaviour;

    void Update()
    {
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ^ _shiftFlag)
        {
            _shiftFlag = !_shiftFlag;
            ShiftBehavior?.Invoke(_shiftFlag);
        }

        if((_pkmFlag ^ Input.GetMouseButton(1)) == true)
        {
            _pkmFlag = !_pkmFlag;
            PKMBehaviour?.Invoke(_pkmFlag);
        }

        MouseBehavior?.Invoke(Input.mousePosition);
        
    }
}
