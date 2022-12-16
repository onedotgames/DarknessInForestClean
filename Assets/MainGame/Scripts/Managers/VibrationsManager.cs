using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lofelt.NiceVibrations;

public class VibrationsManager : CustomBehaviour
{
    public bool IsVibrationOn { get; set; } = true;
    public HapticPatterns.PresetType prese;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        LofeltHaptics.Initialize();
        DeviceCapabilities.Init();
        
    }

    public void InitializeVibrationState()
    {
        IsVibrationOn = PlayerPrefs.GetInt(ConstantDatas.VIBRATION_STATE, 1) == 1;
    }

    public void SaveVibrationsChangeState()
    {
        PlayerPrefs.SetInt(ConstantDatas.VIBRATION_STATE, IsVibrationOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ToggleVibration()
    {
        IsVibrationOn = !IsVibrationOn;
        SaveVibrationsChangeState();
    }

    //public void Vibrate()
    //{
    //    if (IsVibrationOn)
    //    {
    //        Handheld.Vibrate();
    //    }
    //}

    public void PlayVibration(HapticPatterns.PresetType presetType)
    {        
        HapticPatterns.PlayPreset(presetType);
    }
}
