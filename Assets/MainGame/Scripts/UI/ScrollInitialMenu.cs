using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScrollInitialMenu : MonoBehaviour
{
    public RectTransform ContentTransform;
    public InitialMenu initialMenu;
    public GameObject PlayerImg;
    [Header("Bool Checks")]
    public bool isEquipment = false;
    public bool isCoop = false;
    public bool isHome = true;
    public bool isLevels = false;
    public bool isLeaderboard = false;
    private void Update()
    {
        //content in size ını 5 e böl, her panelin durduğu yere göre pozisyonları eşitle azı ve çoğuna göre oralara at.
        if (Input.GetMouseButtonUp(0) && initialMenu.isSwipe == true)
        {
            if (isCoop && ContentTransform.anchoredPosition.x <= Screen.height * 1.8f && ContentTransform.anchoredPosition.x > Screen.height)
            { // coop to equipment
                Debug.Log("Coop to equipment");
                isCoop = false;
                isEquipment = true;
                ContentTransform.DOLocalMoveX(Screen.height, 0.2f);
                PlayerImg.GetComponent<SpriteRenderer>().enabled = true;
                initialMenu.Equipment.DOScale(Vector3.one * 1.3f, 0.4f);
                initialMenu.Coop.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.Home.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.Levels.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.LeaderBoard.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.rectBG.DOLocalMoveX(initialMenu.EquipmentBTN.transform.localPosition.x, 0.3f).SetEase(Ease.OutQuad);
                initialMenu.rectBG.rotation = Quaternion.Euler(new Vector3(0, 0, 1));
            }
            else if (isEquipment && ContentTransform.anchoredPosition.x > Screen.height * 1.2f && ContentTransform.anchoredPosition.x <= Screen.height * 2f)
            { // equipment to coop
                isEquipment = false;
                isCoop = true;
                ContentTransform.DOLocalMoveX(Screen.height * 2, 0.2f);
                PlayerImg.GetComponent<SpriteRenderer>().enabled = false;
                initialMenu.Coop.DOScale(Vector3.one * 1.3f, 0.4f);
                initialMenu.Equipment.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.Home.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.Levels.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.LeaderBoard.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.rectBG.DOLocalMoveX(initialMenu.CoopBTN.transform.localPosition.x, 0.3f).SetEase(Ease.OutQuad);
                initialMenu.rectBG.rotation = Quaternion.Euler(new Vector3(0, 0, -1));
            }
            else if (isEquipment && ContentTransform.anchoredPosition.x <= Screen.height * 0.8f && ContentTransform.anchoredPosition.x > 0)
            { //equipment to home
                isEquipment = false;
                isHome = true;
                ContentTransform.DOLocalMoveX(0, 0.2f);
                PlayerImg.GetComponent<SpriteRenderer>().enabled = true;
                initialMenu.Home.DOScale(Vector3.one * 1.3f, 0.4f);
                initialMenu.Equipment.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.Coop.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.Levels.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.LeaderBoard.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.rectBG.DOLocalMoveX(initialMenu.HomeBTN.transform.localPosition.x, 0.3f).SetEase(Ease.OutQuad);
                initialMenu.rectBG.rotation = Quaternion.Euler(new Vector3(0, 0, 3));
            }
            else if (isHome && ContentTransform.anchoredPosition.x > Screen.height * 0.2f && ContentTransform.anchoredPosition.x <= Screen.height * 1)
            { // home to equipment
                isHome = false;
                isEquipment = true;
                ContentTransform.DOLocalMoveX(Screen.height, 0.2f);
                PlayerImg.GetComponent<SpriteRenderer>().enabled = true;
                initialMenu.Equipment.DOScale(Vector3.one * 1.3f, 0.4f);
                initialMenu.Coop.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.Home.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.Levels.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.LeaderBoard.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.rectBG.DOLocalMoveX(initialMenu.EquipmentBTN.transform.localPosition.x, 0.3f).SetEase(Ease.OutQuad);
                initialMenu.rectBG.rotation = Quaternion.Euler(new Vector3(0, 0, 1));
            }
            else if (isHome && ContentTransform.anchoredPosition.x <= -Screen.height * 0.2f && ContentTransform.anchoredPosition.x > -Screen.height)
            { // home to levels
                isHome = false;
                isLevels = true;
                ContentTransform.DOLocalMoveX(-Screen.height, 0.2f);
                PlayerImg.GetComponent<SpriteRenderer>().enabled = false;
                initialMenu.Levels.DOScale(Vector3.one * 1.3f, 0.4f);
                initialMenu.Equipment.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.Home.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.Coop.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.LeaderBoard.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.rectBG.DOLocalMoveX(initialMenu.LevelsBTN.transform.localPosition.x, 0.3f).SetEase(Ease.OutQuad);
                initialMenu.rectBG.rotation = Quaternion.Euler(new Vector3(0, 0, 5));
            }
            else if(isLevels && ContentTransform.anchoredPosition.x > -Screen.height * 0.8f && ContentTransform.anchoredPosition.x <= 0)
            { //levels to home
                isLevels = false;
                isHome = true;
                ContentTransform.DOLocalMoveX(0, 0.2f);
                PlayerImg.GetComponent<SpriteRenderer>().enabled = true;
                initialMenu.Home.DOScale(Vector3.one * 1.3f, 0.4f);
                initialMenu.Equipment.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.Coop.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.Levels.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.LeaderBoard.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.rectBG.DOLocalMoveX(initialMenu.HomeBTN.transform.localPosition.x, 0.3f).SetEase(Ease.OutQuad);
                initialMenu.rectBG.rotation = Quaternion.Euler(new Vector3(0, 0, 3));
            }
            else if(isLevels && ContentTransform.anchoredPosition.x <= -Screen.height * 1.2f && ContentTransform.anchoredPosition.x > -Screen.height * 2)
            { // levels to leaderboard
                isLevels = false;
                isLeaderboard = true;
                ContentTransform.DOLocalMoveX(-Screen.height * 2, 0.2f);
                PlayerImg.GetComponent<SpriteRenderer>().enabled = false;
                initialMenu.LeaderBoard.DOScale(Vector3.one * 1.3f, 0.4f);
                initialMenu.Equipment.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.Home.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.Levels.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.Coop.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.rectBG.DOLocalMoveX(initialMenu.LeaderBoardBTN.transform.localPosition.x, 0.3f).SetEase(Ease.OutQuad);
                initialMenu.rectBG.rotation = Quaternion.Euler(new Vector3(0, 0, 7));
            }
            else if(isLeaderboard && ContentTransform.anchoredPosition.x > -Screen.height * 1.8f && ContentTransform.anchoredPosition.x < -Screen.height)
            { // leaderboard to levels
                isLeaderboard = false;
                isLevels = true;
                ContentTransform.DOLocalMoveX(-Screen.height, 0.2f);
                PlayerImg.GetComponent<SpriteRenderer>().enabled = false;
                initialMenu.Levels.DOScale(Vector3.one * 1.3f, 0.4f);
                initialMenu.Equipment.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.Home.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.Coop.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.LeaderBoard.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.rectBG.DOLocalMoveX(initialMenu.LevelsBTN.transform.localPosition.x, 0.3f).SetEase(Ease.OutQuad);
                initialMenu.rectBG.rotation = Quaternion.Euler(new Vector3(0, 0, 5));
            }
            else if (isCoop && ContentTransform.anchoredPosition.x > Screen.height * 1.8f && ContentTransform.anchoredPosition.x <= Screen.height * 2)
            { //coop to coop
                ContentTransform.DOLocalMoveX(Screen.height * 2, 0.2f);
                PlayerImg.GetComponent<SpriteRenderer>().enabled = false;
                initialMenu.Coop.DOScale(Vector3.one * 1.3f, 0.4f);
                initialMenu.Equipment.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.Home.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.Levels.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.LeaderBoard.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.rectBG.DOLocalMoveX(initialMenu.CoopBTN.transform.localPosition.x, 0.3f).SetEase(Ease.OutQuad);
                initialMenu.rectBG.rotation = Quaternion.Euler(new Vector3(0, 0, -1));
            }
            else if (isEquipment && ContentTransform.anchoredPosition.x <= Screen.height * 1.2f && ContentTransform.anchoredPosition.x > Screen.height * 0.8f)
            {//equipment to equipment
                ContentTransform.DOLocalMoveX(Screen.height, 0.2f);
                PlayerImg.GetComponent<SpriteRenderer>().enabled = true;
                initialMenu.Equipment.DOScale(Vector3.one * 1.3f, 0.4f);
                initialMenu.Coop.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.Home.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.Levels.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.LeaderBoard.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.rectBG.DOLocalMoveX(initialMenu.EquipmentBTN.transform.localPosition.x, 0.3f).SetEase(Ease.OutQuad);
                initialMenu.rectBG.rotation = Quaternion.Euler(new Vector3(0, 0, 1));
            }
            else if (isHome && ContentTransform.anchoredPosition.x <= Screen.height * 0.2f && ContentTransform.anchoredPosition.x > -Screen.height * 0.2f)
            {//home to home
                ContentTransform.DOLocalMoveX(0, 0.2f);
                PlayerImg.GetComponent<SpriteRenderer>().enabled = true;
                initialMenu.Home.DOScale(Vector3.one * 1.3f, 0.4f);
                initialMenu.Equipment.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.Coop.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.Levels.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.LeaderBoard.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.rectBG.DOLocalMoveX(initialMenu.HomeBTN.transform.localPosition.x, 0.3f).SetEase(Ease.OutQuad);
                initialMenu.rectBG.rotation = Quaternion.Euler(new Vector3(0, 0, 3));
            }
            else if (isLevels && ContentTransform.anchoredPosition.x <= -Screen.height * 0.8f && ContentTransform.anchoredPosition.x > -Screen.height * 1.2f)
            {// levels to levels
                ContentTransform.DOLocalMoveX(-Screen.height, 0.2f);
                PlayerImg.GetComponent<SpriteRenderer>().enabled = false;
                initialMenu.Levels.DOScale(Vector3.one * 1.3f, 0.4f);
                initialMenu.Equipment.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.Home.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.Coop.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.LeaderBoard.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.rectBG.DOLocalMoveX(initialMenu.LevelsBTN.transform.localPosition.x, 0.3f).SetEase(Ease.OutQuad);
                initialMenu.rectBG.rotation = Quaternion.Euler(new Vector3(0, 0, 5));
            }
            else if (isLeaderboard && ContentTransform.anchoredPosition.x <= -Screen.height * 1.8f && ContentTransform.anchoredPosition.x <= -Screen.height * 2)
            {// leaderboard to leaderboard
                ContentTransform.DOLocalMoveX(-Screen.height * 2, 0.2f);
                PlayerImg.GetComponent<SpriteRenderer>().enabled = false;
                initialMenu.LeaderBoard.DOScale(Vector3.one * 1.3f, 0.4f);
                initialMenu.Equipment.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.Home.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.Levels.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.Coop.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.rectBG.DOLocalMoveX(initialMenu.LeaderBoardBTN.transform.localPosition.x, 0.3f).SetEase(Ease.OutQuad);
                initialMenu.rectBG.rotation = Quaternion.Euler(new Vector3(0, 0, 7));
            }
        }
    }
}
