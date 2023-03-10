using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VariableJoystick : Joystick
{
    public float MoveThreshold { get { return moveThreshold; } set { moveThreshold = Mathf.Abs(value); } }

    [SerializeField] private float moveThreshold = 1;
    [SerializeField] private JoystickType joystickType = JoystickType.Fixed;

    private Vector2 fixedPosition = Vector2.zero;

    public void SetMode(JoystickType joystickType)
    {
        this.joystickType = joystickType;
        if(joystickType == JoystickType.Fixed)
        {
            background.gameObject.SetActive(true);
            background.anchoredPosition = fixedPosition;
        }
        else
        {
            background.gameObject.SetActive(false);
        }
    }

    protected override void Start()
    {
        base.Start();
        fixedPosition = background.anchoredPosition;
        SetMode(joystickType);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if(joystickType != JoystickType.Fixed)
        {
            //background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            Vector2 localPoint = Vector2.zero;
            Vector3 worldPoint = Vector2.zero;
            Ray ray = RectTransformUtility.ScreenPointToRay(Camera.main, eventData.position);
            worldPoint = ray.GetPoint(0);
            localPoint = baseRect.InverseTransformPoint(worldPoint);
            //if (RectTransformUtility.ScreenPointToWorldPointInRectangle(baseRect, eventData.position, Camera.main, out var worldPoint))
            //{
            //    localPoint = baseRect.InverseTransformPoint(worldPoint);
            //}
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, eventData.position, Camera.main, out localPoint))
            {
                Vector2 pivotOffset = baseRect.pivot * baseRect.sizeDelta;
                background.anchoredPosition =  localPoint - (background.anchorMax * baseRect.sizeDelta) + pivotOffset;
            }
            background.gameObject.SetActive(true);
        }
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if(joystickType != JoystickType.Fixed)
            background.gameObject.SetActive(false);

        base.OnPointerUp(eventData);
    }

    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (joystickType == JoystickType.Dynamic && magnitude > moveThreshold)
        {
            Vector2 difference = normalised * (magnitude - moveThreshold) * radius;
            background.anchoredPosition += difference;
        }
        base.HandleInput(magnitude, normalised, radius, cam);
    }
}

public enum JoystickType { Fixed, Floating, Dynamic }