using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickController : MonoBehaviour
{
    public static Vector3 direct;

    private Vector3 screen;

    private Vector3 mousePosition => Input.mousePosition - screen / 2;

    private Vector3 startPoint;
    private Vector3 currentPoint;

    public RectTransform joyStickBG;
    public RectTransform joystickControl; //Using RectTransform to anchored the postion of the joystick
    public float magnitude;

    public GameObject joystickPanel;


    private void Awake()
    {
        screen.x = Screen.width;
        screen.y = Screen.height;
        direct = Vector3.zero;
        joystickPanel.SetActive(false);
    }

    private void Update()
    {
        MovingButton();
    }
     
    private void MovingButton()
    {
        //Check if the left mousebutton is pressed
        if (Input.GetMouseButtonDown(0))
        {
            startPoint = mousePosition;
            joyStickBG.anchoredPosition = startPoint; //MousePosition = anchored background joystick
            joystickPanel.SetActive(true);
        }
        //Check if the left mousebutton is being held
        if (Input.GetMouseButton(0))
        {
            currentPoint = mousePosition;

            joystickControl.anchoredPosition = Vector3.ClampMagnitude((currentPoint - startPoint), magnitude) + startPoint; //MousePosition = anchored control joystick

            direct = (currentPoint - startPoint).normalized;

            direct.z = direct.y;
            direct.y = 0;
        }
        if (Input.GetMouseButtonUp(0))
        {
            joystickPanel.SetActive(false);
            direct = Vector3.zero;
        }
    }


    private void OnDisable()
    {
        direct = Vector3.zero;
    }
}

