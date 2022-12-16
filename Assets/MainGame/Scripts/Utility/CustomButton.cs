using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : Button
{
    #region Fields
    private Text mText;

    public Text Text 
    { 
        get 
        { 
            if(mText == null)
            {
                mText = base.GetComponent<Text>();
            }

            return mText;
        }
    }

    private Text mChildText;

    public Text ChildText
    {
        get
        {
            if (mChildText == null)
            {
                mChildText = base.GetComponentInChildren<Text>();
            }

            return mChildText;
        }
    }

    private Image mImage;

    public Image Image
    {
        get
        {
            if (mImage == null)
            {
                mImage = base.GetComponent<Image>();
            }

            return mImage;
        }
    }

    private Image mChildImage;

    public Image ChildImage
    {
        get
        {
            if (mChildImage == null)
            {
                mChildImage = base.GetComponentInChildren<Image>();
            }

            return mChildImage;
        }
    }

    private RectTransform mRectTransform;

    public RectTransform RectTransform
    {
        get
        {
            if (mRectTransform == null)
            {
                mRectTransform = base.GetComponent<RectTransform>();
            }

            return mRectTransform;
        }
    }

    public bool DontPlayAnimation { get; set; }

    protected UIManager UIManager { get; set; }

    private event Action mOnClick;
    private event Action<int> mOnClickWeapon;

    private event Action<PointerEventData> mOnClickWithData;

    private event Action<PointerEventData> mOnPointerDown;

    private event Action<PointerEventData> mOnPointerUp;

    private Vector3 mStartScale;
    #endregion

    #region Methods
    public virtual void Initialize(UIManager uiManager, Action onClick, bool active = true)
    {
        gameObject.SetActive(active);
        UIManager = uiManager;
        mOnClick = onClick;

        mStartScale = transform.localScale;

        if (transform.localScale.x< 0.1f)
        {
            mStartScale = Vector3.one;
        }
    }

    internal void InitializeWeapon(UIManager uIManager, Action<int> invokeAction, bool v)
    {
        gameObject.SetActive(v);
        UIManager = uIManager;
        mOnClickWeapon = invokeAction;

        mStartScale = transform.localScale;

        if (transform.localScale.x < 0.1f)
        {
            mStartScale = Vector3.one;
        }
    }

    public virtual void InitializeSkill(UIManager uiManager, Action onClick, bool active = true)
    {
        gameObject.SetActive(active);
        UIManager = uiManager;
        mOnClick = onClick;

        mStartScale = transform.localScale;

        if (transform.localScale.x < 0.1f)
        {
            mStartScale = Vector3.one;
        }
    }

    public virtual void Initialize(UIManager uiManager, Action onClick, bool noAnimation, bool active = true)
{
    Initialize(uiManager, onClick, active);
    DontPlayAnimation = noAnimation;
}

public virtual void Initialize(UIManager uiManager, Action<PointerEventData> onClickWithData, Action<PointerEventData> onPointerDown, Action<PointerEventData> onPointerUp, bool active = true)
{
    gameObject.SetActive(active);
    UIManager = uiManager;
    mOnClickWithData = onClickWithData;
    mOnPointerDown = onPointerDown;
    mOnPointerUp = onPointerUp;

    mStartScale = transform.localScale;

    if (transform.localScale.x < 0.1f)
    {
        mStartScale = Vector3.one;
    }
}

public virtual void SetInteraction(bool interaction)
{
    SetInteractionWithoutColor(interaction);
    image.color = interaction ? colors.normalColor : colors.disabledColor;
}

public virtual void SetInteractionWithoutColor(bool interaction)
{
    interactable = interaction;
}

public override bool IsInteractable()
{
    return interactable;
}

#endregion

#region Events

public override void OnPointerClick(PointerEventData eventData)
{
    if (!interactable)
    {
        return;
    }

    if (eventData != null)
    {
        base.OnPointerClick(eventData);

        if (mOnClickWithData != null)
        {
            mOnClickWithData(eventData);
        }
    }

    if (mOnClick != null)
    {
        mOnClick();
    }

    //UIManager.GameManager.SoundManager.PlayClickSound (ClickSounds.Button);
    //Debug.Log("CustomButton.OnPointerClick");
}

public override void OnPointerDown(PointerEventData eventData)
{
    if (!interactable)
    {
        return;
    }

    if (!DontPlayAnimation && UIManager != null)
    {
        transform.localScale = new Vector3(mStartScale.x - .1f, mStartScale.y - .1f, mStartScale.z - .1f);
    }

    base.OnPointerDown(eventData);

    if (mOnPointerDown != null)
    {
        mOnPointerDown(eventData);
    }

    //Debug.Log("CustomButton.OnPointerDown");
}

public override void OnPointerUp(PointerEventData eventData)
{
    if (!interactable)
    {
        return;
    }

    base.OnPointerUp(eventData);

    if (!DontPlayAnimation && UIManager != null)
    {
        transform.localScale = mStartScale;
    }

    if (mOnPointerUp != null)
    {
        mOnPointerUp(eventData);
    }

    //Debug.Log("CustomButton.OnPointerUp");
}

    #endregion
}
