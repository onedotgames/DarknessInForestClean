using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameBase : MonoBehaviour
{
    public List<Sprite> Weapons = new List<Sprite> ();
    public List<Image> BigCircleImages = new List<Image> ();
    public List<Image> MediumCircleImages = new List<Image>();
    public List<Image> SmallCircleImages = new List<Image>();
    public List<Sprite> TempWeapons = new List<Sprite> ();

    public Image TargetBox;
    public Sprite TargetImage;
    public GameObject BigCircle;
    public GameObject MediumCircle;
    public GameObject SmallCircle;
    public bool miniGameSucces = false;
    public WeaponManager weaponManager;
    public GameManager GameManager;
    public HUD HUD;

    private void Start()
    {
        PrepareMiniGame();
        if(BigCircleImages[0].sprite == TargetImage)
        {
            BigCircle.GetComponent<Circle>().isTrueImage = true;
        }
        if(MediumCircleImages[0].sprite == TargetImage)
        {
            MediumCircle.GetComponent<Circle>().isTrueImage = true;
        }
        if(SmallCircleImages[0].sprite == TargetImage)
        {
            SmallCircle.GetComponent<Circle>().isTrueImage = true;
        }
    }
    public void PrepareMiniGame()
    {
        //TargetImage = Weapons[Random.Range(0, Weapons.Count)];
        //TargetBox.sprite = TargetImage;
        for (int i = 0; i < Weapons.Count; i++)
        {
            TempWeapons.Add(Weapons[i]);
        }
        SetImages();
        BigCircle.GetComponent<Circle>().isTrueImage = false;
        MediumCircle.GetComponent<Circle>().isTrueImage = false;
        SmallCircle.GetComponent<Circle>().isTrueImage = false;

    }
    private void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            if (BigCircle.GetComponent<Circle>().isTrueImage && MediumCircle.GetComponent<Circle>().isTrueImage && SmallCircle.GetComponent<Circle>().isTrueImage)
            {
                gameObject.SetActive(false);
                weaponManager.isMiniGameDone = true;
                PrepareMiniGame();
            }
            if (weaponManager.isMiniGameDone)
            {
                weaponManager.isMiniGameDone = false;
                //GameManager.WeaponManager.selectedWeaponData.Weapon.UpdateWeapon();
                GameManager.PoolingManager.WeaponPooler[(int)GameManager.WeaponManager.selectedWeaponData.Weapon.SkillSO.PoolerType].ObjectList.ForEach(x => x.GetComponent<WeaponBase>().UpdateWeapon());
                TargetImage = Weapons[Random.Range(0, Weapons.Count)];
                HUD.OpenPanel();
                Time.timeScale = 1;
            }
        }      
    }

    public void SetImages()
    {    
        for (int i = 0; i < BigCircleImages.Count; i++)
        {
            var j = Random.Range(0, TempWeapons.Count);
            BigCircleImages[i].sprite = TempWeapons[j];
            TempWeapons.Remove(TempWeapons[j]);
            if(TempWeapons.Count == 0)
            {
                for (int k = 0; k < Weapons.Count; k++)
                {
                    TempWeapons.Add(Weapons[k]);
                }
            }
        }
        TempWeapons.Clear();
        for (int i = 0; i < Weapons.Count; i++)
        {
            TempWeapons.Add(Weapons[i]);
        }
        for (int i = 0; i < MediumCircleImages.Count; i++)
        {
            var t = Random.Range(0, TempWeapons.Count);
            MediumCircleImages[i].sprite = TempWeapons[t];
            TempWeapons.Remove(TempWeapons[t]);
            if (TempWeapons.Count == 0)
            {
                for (int l = 0; l < Weapons.Count; l++)
                {
                    TempWeapons.Add(Weapons[l]);
                }
            }
        }
        TempWeapons.Clear();
        for (int i = 0; i < Weapons.Count; i++)
        {
            TempWeapons.Add(Weapons[i]);
        }
        for (int i = 0; i < SmallCircleImages.Count; i++)
        {
            var y = Random.Range(0, TempWeapons.Count);
            SmallCircleImages[i].sprite = TempWeapons[y];
            TempWeapons.Remove(TempWeapons[y]);
            if (TempWeapons.Count == 0)
            {
                for (int m = 0; m < Weapons.Count; m++)
                {
                    TempWeapons.Add(Weapons[m]);
                }
            }
        }
    }
}
