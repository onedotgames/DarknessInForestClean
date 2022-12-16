using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : CustomBehaviour
{
    #region Fields
    public AudioSource ClickAudioSource;
    public AudioClip[] ClickAudioClips;

    public AudioSource GameStateAudioSource;
    public AudioClip[] GameStateAudioClips;

    public AudioSource PlayerInteractionAudioSource;
    public AudioClip[] PlayerInteractionAudioClips;

    public bool IsSoundOn { get; set; }
    #endregion

    #region Methods
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);

        InitializeSoundState();
    }

    public void InitializeSoundState()
    {
        IsSoundOn = PlayerPrefs.GetInt(ConstantDatas.SOUND_STATE, 1) == 1;
    }

    public void ToggleSound()
    {
        IsSoundOn = !IsSoundOn;
        SaveSoundStateOnChange();
    }

    public void SaveSoundStateOnChange() 
    {
        PlayerPrefs.SetInt(ConstantDatas.SOUND_STATE, IsSoundOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void PlayClickSound(ClickSounds clickSound)
    {
        if (!IsSoundOn) return;

        ClickAudioSource.clip = ClickAudioClips[(int)clickSound];
        ClickAudioSource.Play();
    }

    public void PlayGameStateSound(GameStateSounds sound)
    {
        if (!IsSoundOn) return;
        GameStateAudioSource.clip = GameStateAudioClips[(int)sound];
        GameStateAudioSource.Play();
    }

    public void PlayPlayerInteractionSound(PlayerInteractionSounds sound)
    {
        if (!IsSoundOn) return;
        GameStateAudioSource.clip = PlayerInteractionAudioClips[(int)sound];
        GameStateAudioSource.Play();
    }
    #endregion
}
