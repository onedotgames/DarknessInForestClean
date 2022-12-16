using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public string Name;
    public int LevelNumber;
    public int CoinCount = 50;
    public int SelectedPlayerId;
    public int SelectedLevelNumber;
    public bool TutorialLevelFinished;

    public List<PurchaseData> PurchaseData;
}
