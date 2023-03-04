using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
public sealed class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter Instance { get; private set; }
    public GameObject MiniGamePanel;
    [SerializeField] private SkillManager skillManager;
    public TextMeshProUGUI scoreText;
    public GameManager GameManager;
    public int popCount = 0;
    public HUD HUD;
    public List<RectTransform> PopOpenList;
    public MiniGameType miniGameType;
    public RectTransform RadialShine;
    public Button SuccessBTN;
    public Button SkipBTN;
    
    private void Update()
    {
        RadialShine.DORotate(RadialShine.rotation.eulerAngles + new Vector3(0, 0, 5), 0.3f).SetUpdate(true);
    }

    public void OpenPop()
    {
        for (int i = 0; i < popCount; i++)
        {
            if(i == 0 || i== 1)
            {
                PopOpenList[i].DOScale(Vector3.one, 0.5f).SetUpdate(true);
            }
            else if (i == 2 || i == 3)
            {
                PopOpenList[i].DOScale(Vector3.one * 1.2f, 0.5f).SetUpdate(true);
            }
            else if (i == 4)
            {
                PopOpenList[i].DOScale(Vector3.one * 1.4f, 0.5f).SetUpdate(true);
            }
        }
    }

    public void SuccessMinigame()
    {
        if (popCount >= 5)
        {
            SuccessBTN.gameObject.SetActive(true);
            SuccessBTN.transform.DOScale(Vector3.one * 0.65f, 0.5f).SetUpdate(true);
        }
    }

    private void Awake() => Instance = this;


    public void SuccessBTNClick()
    {
        MiniGamePanel.transform.DOScale(Vector3.zero, 1f);
        skillManager.IsMiniGameDone = true;
        RadialShine.gameObject.SetActive(false);

        if (skillManager.IsMiniGameDone)
        {
            skillManager.IsMiniGameDone = false;
            Time.timeScale = 1;
            popCount = 0;
            if (miniGameType == MiniGameType.Evolve)
            {
                GameManager.SkillManager.AllWeaponsV2[(int)GameManager.SkillManager.selectedWeaponData.Weapon.SkillSO.PoolerType].EvolveWeapon();
            }
            else if (miniGameType == MiniGameType.Chest)
            {
                var chest = GameObject.FindGameObjectWithTag("Chest");
                chest.GetComponent<Chest>().canChestOpen = true;
                chest.GetComponent<Chest>().GameManager = GameManager;
            }
        }

        PopOpenList.ForEach(x => x.DOScale(Vector3.zero, 0.5f).SetUpdate(true));
        
        SuccessBTN.gameObject.SetActive(false);
        SuccessBTN.transform.localScale = Vector3.zero;
        MiniGamePanel.SetActive(false);
        GameManager.UIManager.GetPanel(Panels.Hud).OpenPanel();

    }

}
public enum MiniGameType
{
    Evolve = 0,
    Chest = 1
}
