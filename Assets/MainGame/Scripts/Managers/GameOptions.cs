using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class GameOptions : CustomBehaviour
{
    #region Fields
    //[Title("Gameplay Mechanics")]
    //public GameMechanics GameMechanic;
    //public List<GameMechanic> Mechanics;

    [Title("StartText")]
    public StartTextTypes StartTextType;

    [Space(10)]
    [Title("COUNTDOWN")]
    public bool UseCountdown;
    public Countdown Countdown;

    [Space(10)]
    [Title("Player Name Options")]
    public bool UsePlayerName;
    public TMP_InputField InputFieldName;

    [Space(10)]
    [Title("Player Movement Options")]
    public bool IsConstantlyMoving;
    [EnableIf("@this.IsConstantlyMoving")]
    public ConstantMoveAxis ConstantMoveAxis;

    [Space(10)]
    [Title("Choose Player Control Type")]
    [InfoBox("WARNING: TapToMove option requires baked NavMeshArea to work.", InfoMessageType.Warning)]
    public ControlType ControlType;

    private TMP_InputField mInputFieldName;
    #endregion

    #region Initialization
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);

        InitializeCustomOptions();
        //InitializeMechanic();
    }

    private void InitializeCustomOptions()
    {
        if (UseCountdown)
        {
            Countdown.Initialize(GameManager);
        }

        if (UsePlayerName)
        {
            mInputFieldName = Instantiate(InputFieldName, GameManager.UIManager.GetPanel(Panels.MainMenu).Transform);
            ((MainMenu)GameManager.UIManager.GetPanel(Panels.MainMenu)).SetNameInputFieldTo(mInputFieldName);
        }
    }

    //private void InitializeMechanic()
    //{
    //    Mechanics.ForEach(x => x.Deactivate());

    //    if(GameMechanic != GameMechanics.Other)
    //    {
    //        Mechanics[(int)GameMechanic].Initialize(this);
    //    }
    //}
    #endregion

    #region Methods
    //public bool CanUseTap()
    //{
    //    switch (GameMechanic)
    //    {
    //        case GameMechanics.Tap:
    //            return true;
    //        default:
    //            return false;
    //    }
    //}

    public void SetName()
    {
        if (UsePlayerName)
        {
            var temporaryName = InputFieldName.text;

            if (!string.IsNullOrEmpty(temporaryName))
            {
                GameManager.PlayerManager.UpdateNameData(temporaryName);
            }
        }
    }

    public void GetName()
    {
        if (UsePlayerName)
        {
            var fieldName = GameManager.PlayerManager.GetPlayerName();

            if(fieldName == TAGS.Player)
            {
                fieldName = null;
            }

            InputFieldName.SetTextWithoutNotify(fieldName);
        }
    }
    #endregion
}
