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
    public PlayAnimation playerAnim;
    private void Update()
    {
        //content in size ını 5 e böl, her panelin durduğu yere göre pozisyonları eşitle azı ve çoğuna göre oralara at.
        if (Input.GetMouseButtonUp(0))
        {
            if (ContentTransform.position.x > Screen.height * 1.5)//coop
            {
                ContentTransform.DOLocalMoveX(Screen.height * 2, 0.2f);
                PlayerImg.SetActive(false);
                initialMenu.Coop.DOScale(Vector3.one * 2, 0.4f);
                initialMenu.Equipment.DOScale(Vector3.one, 0.4f);
                initialMenu.Home.DOScale(Vector3.one, 0.4f);
                initialMenu.Levels.DOScale(Vector3.one, 0.4f);
                initialMenu.LeaderBoard.DOScale(Vector3.one, 0.4f);
            }
            else if (ContentTransform.position.x <= Screen.height * 1.5 && ContentTransform.position.x > Screen.height * 0.5)
            { //equipment
                ContentTransform.DOLocalMoveX(Screen.height, 0.2f);
                PlayerImg.SetActive(true);
                playerAnim.StopLoop();
                initialMenu.Equipment.DOScale(Vector3.one * 2, 0.4f);
                initialMenu.Coop.DOScale(Vector3.one, 0.4f);
                initialMenu.Home.DOScale(Vector3.one, 0.4f);
                initialMenu.Levels.DOScale(Vector3.one, 0.4f);
                initialMenu.LeaderBoard.DOScale(Vector3.one, 0.4f);
            }
            else if (ContentTransform.position.x <= Screen.height * 0.5 && ContentTransform.position.x > -Screen.height * 0.5)
            {//home
                ContentTransform.DOLocalMoveX(0, 0.2f);
                PlayerImg.SetActive(true);
                playerAnim.PlayLoop();
                initialMenu.Home.DOScale(Vector3.one * 2, 0.4f);
                initialMenu.Equipment.DOScale(Vector3.one, 0.4f);
                initialMenu.Coop.DOScale(Vector3.one, 0.4f);
                initialMenu.Levels.DOScale(Vector3.one, 0.4f);
                initialMenu.LeaderBoard.DOScale(Vector3.one, 0.4f);
            }
            else if (ContentTransform.position.x <= -Screen.height * 0.5f && ContentTransform.position.x > -Screen.height * 1.5)
            {//levels
                ContentTransform.DOLocalMoveX(-Screen.height, 0.2f);
                PlayerImg.SetActive(false);
                initialMenu.Levels.DOScale(Vector3.one * 2, 0.4f);
                initialMenu.Equipment.DOScale(Vector3.one, 0.4f);
                initialMenu.Home.DOScale(Vector3.one, 0.4f);
                initialMenu.Coop.DOScale(Vector3.one, 0.4f);
                initialMenu.LeaderBoard.DOScale(Vector3.one, 0.4f);
            }
            else if (ContentTransform.position.x <= -Screen.height * 1.5f)
            {//leaderboard
                ContentTransform.DOLocalMoveX(-Screen.height * 2, 0.2f);
                PlayerImg.SetActive(false);
                initialMenu.LeaderBoard.DOScale(Vector3.one * 2, 0.4f);
                initialMenu.Equipment.DOScale(Vector3.one, 0.4f);
                initialMenu.Home.DOScale(Vector3.one, 0.4f);
                initialMenu.Levels.DOScale(Vector3.one, 0.4f);
                initialMenu.Coop.DOScale(Vector3.one, 0.4f);
            }
        }
       
    }
}
