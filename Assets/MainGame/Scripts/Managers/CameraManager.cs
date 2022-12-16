using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : CustomBehaviour
{
    #region Fields
    public List<CameraOptions> CameraOptions;
    public int ActiveCameraOption;
    public Transform PlayerTransform;
    public bool ShouldFollowOnX = false;

    private Transform mCameraTransform;
    private Vector3 mUpdatedCameraPosition;
    private Vector3 mCurrentOffset;
    private Vector3 mInitialOffset;
    private float mOffsetX;
    private bool mCanFollow = false;

    public List<BackGround> BackGrounds;
    #endregion

    #region Methods
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);

        SubscribeEvents();
        InitializeOptions();
    }

    private void SubscribeEvents()
    {
        if(GameManager != null)
        {
            GameManager.OnStartGame += StartGame;
            GameManager.OnReturnToMainMenu += ReturnToMainMenu;
        }
    }
    private void UnSubscribeEvents()
    {
        if (GameManager != null)
        {
            GameManager.OnStartGame -= StartGame;
            GameManager.OnReturnToMainMenu -= ReturnToMainMenu;
        }
    }

    private void InitializeOptions()
    {
        mCameraTransform = MainCamera.transform;

        mCameraTransform.position = CameraOptions[ActiveCameraOption].Position;
        mCameraTransform.eulerAngles = CameraOptions[ActiveCameraOption].Rotation;

        mCurrentOffset = mCameraTransform.position - PlayerTransform.position;
        mInitialOffset = mCurrentOffset;

        mOffsetX = mCameraTransform.position.x - PlayerTransform.position.x;

        mUpdatedCameraPosition = new Vector3(mOffsetX, mCurrentOffset.y, mCurrentOffset.z + PlayerTransform.position.z);
    }

    private void LateUpdate()
    {
        PlayerFollow();
    }

    private void PlayerFollow()
    {
        if (!mCanFollow)
        {
            return;
        }
        else
        {
            if (ShouldFollowOnX)
            {
                mUpdatedCameraPosition = new Vector3(mOffsetX + PlayerTransform.position.x, mCurrentOffset.y + PlayerTransform.position.y, mCurrentOffset.z + PlayerTransform.position.z);
                mCameraTransform.position = mUpdatedCameraPosition;
            }
            else
            {
                mUpdatedCameraPosition = new Vector3(mOffsetX, mCurrentOffset.y, mCurrentOffset.z + PlayerTransform.position.z);
                mCameraTransform.position = mUpdatedCameraPosition;
            }
            
        }
    }

    private void ResetOffset()
    {
        mCurrentOffset = mInitialOffset;
    }
    #endregion

    #region Event Methods
    private void StartGame()
    {
        mCanFollow = true;
    }

    private void ReturnToMainMenu()
    {
        ResetOffset();
        
    }


    private void OnDestroy()
    {
        UnSubscribeEvents();
    }
    #endregion
}

[System.Serializable]
public struct CameraOptions
{
    public Vector3 Position;
    public Vector3 Rotation;
}
