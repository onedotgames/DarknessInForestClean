using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : CustomBehaviour
{
    #region Collections
    public List<Level> Levels;
    public List<Interactable> CommonInteractables;
    #endregion

    public int ActivatedLevelNumber;
    public int CurrentLevelNumber;
    public Level TutorialLevel;
    public bool IsIncludeTutorial = false;
    private bool mCurrentLevelPassed = false;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);

        InitializeCustomOptions();

        Levels.ForEach(x => x.Initialize(gameManager));
        if (IsIncludeTutorial)
        {
            TutorialLevel.Initialize(gameManager);
        }
        

        ActivateCurrentLevel();
        SubscribeToEvents();
        if (CommonInteractables != null)
        {
            CommonInteractables.ForEach(x => x.Initialize(gameManager));
        }
    }

    private void InitializeCustomOptions()
    {
        CurrentLevelNumber = GameManager.PlayerManager.GetLevelNumber();
        ActivatedLevelNumber = CurrentLevelNumber;
    }

    private void SubscribeToEvents()
    {
        if (GameManager != null)
        {
            GameManager.OnReturnToMainMenu += OnReturnToMainMenu;
            GameManager.OnLevelCompleted += OnLevelCompleted;
            GameManager.OnTutorialCompleted += OnTutorialCompleted;
        }
    }

    private void ActivateCurrentLevel()
    {
        if (IsIncludeTutorial)
        {
            if (GameManager.PlayerManager.IsTutorialPassed())
            {
                CurrentLevelNumber = GameManager.PlayerManager.GetLevelNumber();

                if (IsLastLevelPlayed())
                {
                    if (mCurrentLevelPassed)
                    {
                        ActivatedLevelNumber = SelectRandomLevel();
                        mCurrentLevelPassed = false;
                    }
                    else
                    {
                        ActivatedLevelNumber = GameManager.PlayerManager.GetSelectedLevelNumber();
                    }
                }
                else
                {
                    ActivatedLevelNumber = CurrentLevelNumber - 1;
                }

                Levels[ActivatedLevelNumber].ActivateAndInitializeInteractables();
            }
            else
            {
                TutorialLevel.ActivateAndInitializeInteractables();
            }
        }
        else
        {
            CurrentLevelNumber = GameManager.PlayerManager.GetLevelNumber();

            if (IsLastLevelPlayed())
            {
                if (mCurrentLevelPassed)
                {
                    ActivatedLevelNumber = SelectRandomLevel();
                    mCurrentLevelPassed = false;
                }
                else
                {
                    ActivatedLevelNumber = GameManager.PlayerManager.GetSelectedLevelNumber();
                }
            }
            else
            {
                ActivatedLevelNumber = CurrentLevelNumber - 1;
            }

            Levels[ActivatedLevelNumber].ActivateAndInitializeInteractables();
        }
        
    }

    public Level GetActivatedLevel()
    {
        return Levels[ActivatedLevelNumber];
    }

    public bool IsLastLevelPlayed()
    {
        return Levels.Count <= CurrentLevelNumber - 1;
    }

    private int SelectRandomLevel()
    {
        var rndLevelId = (int)Random.Range(0, Levels.Count);
        GameManager.PlayerManager.UpdateSelectedLevelNumber(rndLevelId);
        return rndLevelId;
    }

    #region Events

    private void OnReturnToMainMenu()
    {
        ActivateCurrentLevel();
    }

    private void OnLevelCompleted()
    {
        mCurrentLevelPassed = true;
        GameManager.PlayerManager.UpdateLevelData();
        Levels[ActivatedLevelNumber].DeactivateInteractables();
    }

    private void OnTutorialCompleted()
    {
        TutorialLevel.DeactivateInteractables();
    }

    private void OnLevelFailed()
    {
        mCurrentLevelPassed = false;
        Levels[ActivatedLevelNumber].DeactivateInteractables();
    }

    private void OnDestroy()
    {
        if (GameManager != null)
        {
            GameManager.OnReturnToMainMenu -= OnReturnToMainMenu;
            GameManager.OnLevelCompleted -= OnLevelCompleted;
            GameManager.OnTutorialCompleted -= OnTutorialCompleted;
        }
    }

    #endregion
}
