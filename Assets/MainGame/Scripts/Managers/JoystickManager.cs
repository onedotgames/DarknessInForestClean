using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickManager : CustomBehaviour
{
    public GameObject Joystick;
    public VariableJoystick variableJoystick;
    public GameObject TopRightFocus;
    public GameObject TopLeftFocus;
    public GameObject BottomRightFocus;
    public GameObject BottomLeftFocus;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
    }

    private void Update()
    {
        if (GetHorizontal() > 0)
        {
            if (GetVertical() > 0)
            {
                if (!TopRightFocus.activeInHierarchy)
                {
                    TopRightFocus.SetActive(true);
                    TopLeftFocus.SetActive(false);
                    BottomRightFocus.SetActive(false);
                    BottomLeftFocus.SetActive(false);
                }
            }
            else
            {
                if (!BottomRightFocus.activeInHierarchy)
                {
                    TopRightFocus.SetActive(false);
                    TopLeftFocus.SetActive(false);
                    BottomRightFocus.SetActive(true);
                    BottomLeftFocus.SetActive(false);
                }
            }
        }
        else
        {
            if (GetVertical() > 0)
            {
                if (!TopLeftFocus.activeInHierarchy)
                {
                    TopRightFocus.SetActive(false);
                    TopLeftFocus.SetActive(true);
                    BottomRightFocus.SetActive(false);
                    BottomLeftFocus.SetActive(false);
                }
            }
            else
            {
                if (!BottomLeftFocus.activeInHierarchy)
                {
                    TopRightFocus.SetActive(false);
                    TopLeftFocus.SetActive(false);
                    BottomRightFocus.SetActive(false);
                    BottomLeftFocus.SetActive(true);
                }
            }
        }
    }

    public float GetHorizontal()
    {
        return variableJoystick.Horizontal;
    }

    public float GetVertical()
    {
        return variableJoystick.Vertical;
    }
}
