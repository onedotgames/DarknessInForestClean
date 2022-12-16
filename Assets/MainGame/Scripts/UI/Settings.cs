using UnityEngine.UI;

public class Settings : UIPanel
{
    public CustomButton CloseButton;
    public CustomToggle SoundToggle;
    public CustomToggle VibrationToggle;

    private Image mToggleSoundBG;
    private Image mToggleVibrationBG;

    public override void Initialize(UIManager uIManager)
    {
        base.Initialize(uIManager);

        CloseButton.Initialize(uIManager, OnCloseButtonClicked);
        SoundToggle.Initialize(uIManager, OnSoundToggleClicked);
        VibrationToggle.Initialize(uIManager, OnVibrationToggleClicked);

        mToggleSoundBG = SoundToggle.GetComponentInChildren<Image>();
        mToggleVibrationBG = VibrationToggle.GetComponentInChildren<Image>();
    }

    public override void OpenPanel()
    {
        InitializeToggles();
        base.OpenPanel();
    }

    public override void ClosePanel()
    {
        base.ClosePanel();
    }

    private void InitializeToggles()
    {
        SoundToggle.isOn = GameManager.SoundManager.IsSoundOn;
        VibrationToggle.isOn = GameManager.VibrationsManager.IsVibrationOn;

        mToggleSoundBG.enabled = !SoundToggle.isOn;
        mToggleVibrationBG.enabled = !VibrationToggle.isOn;
    }

    #region Events
    private void OnCloseButtonClicked()
    {
        GameManager.SoundManager.PlayClickSound(ClickSounds.Click);
        ClosePanel();
    }

    private void OnSoundToggleClicked()
    {
        GameManager.SoundManager.PlayClickSound(ClickSounds.Click);
        GameManager.SoundManager.ToggleSound();

        mToggleSoundBG.enabled = !SoundToggle.isOn;
    }

    private void OnVibrationToggleClicked()
    {
        GameManager.SoundManager.PlayClickSound(ClickSounds.Click);
        GameManager.VibrationsManager.ToggleVibration();

        mToggleVibrationBG.enabled = !VibrationToggle.isOn;
    }
    #endregion
}
