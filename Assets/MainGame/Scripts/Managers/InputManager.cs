using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class InputManager : CustomBehaviour
{
    #region Actions
    public event Action OnTouched;
    public event Action OnTouchEnd;
    public event Action OnSwiped;
    public float LastAngle;
    #endregion

    #region Fields
    public bool Interactable = false;
    public float MinimumSwipeDistanceInViewportPoint;
    public LayerMask MouseRayLayer;

    public bool IsUIOverride { get; private set; }

    private Vector2 mPointerDownLocation;
    private Vector2 mPointerUpLocation;
    public Vector3 MouseDownLocation;

    private RaycastHit mHit;
    private Ray mRay;
    #endregion

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);

        EventSubscriptions();
    }

    private void EventSubscriptions()
    {
        GameManager.OnStartGame += OnStartGame;
        GameManager.OnRestartGame += OnRestartGame;
        GameManager.OnResumeGame += OnResumeGame;
        GameManager.OnReturnToMainMenu += OnReturnToMainMenu;
        GameManager.OnCountdownFinished += OnCountdownFinished;
    }

    private void Update()
    {
        UpdateUIOverride();
        UpdateInput();
    }

    private void UpdateInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(mRay, out mHit, Mathf.Infinity, MouseRayLayer))
            {
                MouseDownLocation = mHit.point;
            }
        }
    }

    private void UpdateUIOverride()
    {
        IsUIOverride = EventSystem.current.IsPointerOverGameObject();
    }

    #region Events
    public void Touched()
    {
        if(GameManager.GameOptions.StartTextType == StartTextTypes.TapToPlay && Interactable && OnTouched != null)
        {
            OnTouched();
        }
    }

    public void TouchEnd()
    {
        if(OnTouchEnd != null)
        {
            OnTouchEnd();
        }
    }

    public void Touched(PointerEventData eventData)
    {
        mPointerDownLocation = MainCamera.ScreenToViewportPoint(eventData.pressPosition);
    }

    public void TouchEnd(PointerEventData eventData)
    {
        mPointerUpLocation = MainCamera.ScreenToViewportPoint(eventData.position);
        float distance = Vector2.Distance(mPointerDownLocation, mPointerUpLocation);

        ControlSwipe(distance);
    }

    private void ControlSwipe(float distance)
    {
        if (GameManager.GameOptions.StartTextType != StartTextTypes.SwipeToPlay) return;

        if(distance >= MinimumSwipeDistanceInViewportPoint)
        {
            if(OnSwiped != null)
            {
                OnSwiped();
            }
        }
    }

    private void OnStartGame()
    {
        if (GameManager.GameOptions.UseCountdown) return;

        Interactable = true;
    }

    private void OnRestartGame()
    {
        Interactable = true;
    }

    private void OnResumeGame()
    {
        Interactable = true;
    }

    private void OnReturnToMainMenu()
    {
        Interactable = false;
    }

    private void OnCountdownFinished()
    {
        Interactable = true;
    }

    private void OnDestroy()
    {
        if(GameManager != null)
        {
            GameManager.OnStartGame -= OnStartGame;
            GameManager.OnRestartGame -= OnRestartGame;
            GameManager.OnResumeGame -= OnResumeGame;
            GameManager.OnReturnToMainMenu -= OnReturnToMainMenu;
            GameManager.OnCountdownFinished -= OnCountdownFinished;
        }        
    }
    #endregion
}
