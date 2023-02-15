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
    private bool isInventoryPanel = false;
    private void Update()
    {
        //content in size ını 5 e böl, her panelin durduğu yere göre pozisyonları eşitle azı ve çoğuna göre oralara at.
        if (Input.GetMouseButtonUp(0))
        {
            if (ContentTransform.anchoredPosition.x > Screen.height * 1.5)//coop
            {
                isInventoryPanel = false;
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
            else if (ContentTransform.anchoredPosition.x <= Screen.height * 1.5 && ContentTransform.anchoredPosition.x > Screen.height * 0.5)
            { //equipment
                ContentTransform.DOLocalMoveX(Screen.height, 0.2f);
                if (!isInventoryPanel)
                {
                    PlayerImg.GetComponent<SpriteRenderer>().enabled = true;
                }
                isInventoryPanel = true;
                initialMenu.Equipment.DOScale(Vector3.one * 1.3f, 0.4f);
                initialMenu.Coop.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.Home.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.Levels.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.LeaderBoard.DOScale(Vector3.one * 0.9f, 0.4f);
                initialMenu.rectBG.DOLocalMoveX(initialMenu.EquipmentBTN.transform.localPosition.x, 0.3f).SetEase(Ease.OutQuad);
                initialMenu.rectBG.rotation = Quaternion.Euler(new Vector3(0, 0, 1));
            }
            else if (ContentTransform.anchoredPosition.x <= Screen.height * 0.5 && ContentTransform.anchoredPosition.x > -Screen.height * 0.5)
            {//home
                isInventoryPanel = false;
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
            else if (ContentTransform.anchoredPosition.x <= -Screen.height * 0.5f && ContentTransform.anchoredPosition.x > -Screen.height * 1.5)
            {//levels
                isInventoryPanel = false;
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
            else if (ContentTransform.anchoredPosition.x <= -Screen.height * 1.5f)
            {//leaderboard
                isInventoryPanel = false;
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
