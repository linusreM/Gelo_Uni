using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour {
    public move mover;
    public MoveToClickPoint moveClick;
    public MoveCamera moveCam;
    public CustomCommand commandInput;
    public OrderPanel order;


    public void ClickToMove()
    {
        moveClick.activateFlag = !moveClick.activateFlag;
    }

    public void MoveCamera()
    {
        moveCam.activateFlag = !moveCam.activateFlag;
    }
    public void Command()
    {
        commandInput.activateFlag = !commandInput.activateFlag;
    }
    public void MappingOn()
    {
        mover.mapping = !mover.mapping;
    }
    public void ShowOrderPanel()
    {
        order.activateFlag = !order.activateFlag;
    }

}
