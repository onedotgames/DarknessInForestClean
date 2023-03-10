using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BGSystem : MonoBehaviour
{
    public List<GameObject> backGrounds;

    public GameObject CenterBG;
    public int currentCenterIndex;
    public int lastCenterIndex;

    public float horizontal;
    public float vertical;

    [Header("Group Lists")]
    public List<GameObject> Horizontal1;//yataylar
    public List<GameObject> Horizontal2;
    public List<GameObject> Horizontal3;

    public List<GameObject> Vertical1;//dikeyler
    public List<GameObject> Vertical2;
    public List<GameObject> Vertical3;

    public bool CanMove = false;

    private void Awake()
    {
        lastCenterIndex = 4;
        currentCenterIndex = 4;
        CenterBG = backGrounds[4];
    }

    private void Update()
    {
        Methods();
    }

    private void Methods()
    {
        if (lastCenterIndex != currentCenterIndex && CanMove)
        {
            CanMove = false;
            if (backGrounds[currentCenterIndex].transform.position.x > backGrounds[lastCenterIndex].transform.position.x &&
                    backGrounds[currentCenterIndex].transform.position.y == backGrounds[lastCenterIndex].transform.position.y)
            {//sağa
                Debug.Log("Sağ" + lastCenterIndex + " - " + currentCenterIndex);
                WhichGroupMoveVerticalRight();
            }
            else if (backGrounds[currentCenterIndex].transform.position.x > backGrounds[lastCenterIndex].transform.position.x &&
                backGrounds[currentCenterIndex].transform.position.y > backGrounds[lastCenterIndex].transform.position.y)
            { //sağ üste
                Debug.Log("Sağ üst" + lastCenterIndex + " - " + currentCenterIndex);
                WhichGroupMoveVerticalRight();
                WhichGroupMoveHorizontalUp();
            }
            else if (backGrounds[currentCenterIndex].transform.position.x > backGrounds[lastCenterIndex].transform.position.x &&
                backGrounds[currentCenterIndex].transform.position.y < backGrounds[lastCenterIndex].transform.position.y)
            {
                Debug.Log("Sağ alt" + lastCenterIndex + " - " + currentCenterIndex);
                WhichGroupMoveVerticalRight();
                WhichGroupMoveHorizontalDown();
            }
            else if (backGrounds[currentCenterIndex].transform.position.x < backGrounds[lastCenterIndex].transform.position.x &&
                backGrounds[currentCenterIndex].transform.position.y == backGrounds[lastCenterIndex].transform.position.y)
            {//sola
                Debug.Log("Sol" + lastCenterIndex + " - " + currentCenterIndex);
                WhichGroupMoveVerticalLeft();
            }
            else if (backGrounds[currentCenterIndex].transform.position.x < backGrounds[lastCenterIndex].transform.position.x &&
                backGrounds[currentCenterIndex].transform.position.y < backGrounds[lastCenterIndex].transform.position.y)
            {//sol alt
                Debug.Log("Sol alt" + lastCenterIndex + " - " + currentCenterIndex);
                WhichGroupMoveVerticalLeft();
                WhichGroupMoveHorizontalDown();
            }
            else if (backGrounds[currentCenterIndex].transform.position.x < backGrounds[lastCenterIndex].transform.position.x &&
                backGrounds[currentCenterIndex].transform.position.y > backGrounds[lastCenterIndex].transform.position.y)
            {
                Debug.Log("Sol üst" + lastCenterIndex + " - " + currentCenterIndex);
                WhichGroupMoveVerticalLeft();
                WhichGroupMoveHorizontalUp();
            }
            else if (backGrounds[currentCenterIndex].transform.position.y < backGrounds[lastCenterIndex].transform.position.y &&
                backGrounds[currentCenterIndex].transform.position.x == backGrounds[lastCenterIndex].transform.position.x)
            {
                Debug.Log("ALT" + lastCenterIndex + " - " + currentCenterIndex);
                WhichGroupMoveHorizontalDown();
            }
            else if (backGrounds[currentCenterIndex].transform.position.y > backGrounds[lastCenterIndex].transform.position.y &&
                backGrounds[currentCenterIndex].transform.position.x == backGrounds[lastCenterIndex].transform.position.x)
            {
                Debug.Log("üst" + lastCenterIndex + " - " + currentCenterIndex);
                WhichGroupMoveHorizontalUp();
            }
        }
    }
    public void CheckCenter()
    {
        for (int i = 0; i < backGrounds.Count; i++)
        {
            bool isCenter = backGrounds[i].GetComponent<BGPart>().PlayerOnMe;
            if (isCenter)
            {
                CenterBG = backGrounds[i];
                currentCenterIndex = i;
            }
        }
    }


    void WhichGroupMoveHorizontalDown()
    {
                if (currentCenterIndex == 3 || currentCenterIndex == 4 || currentCenterIndex == 5)
                {
                    if (lastCenterIndex == 0 || lastCenterIndex == 1 || lastCenterIndex == 2)
                    {
                        for (int i = 0; i < Horizontal3.Count; i++)
                        {
                            Horizontal3[i].SetActive(false);
                            Horizontal3[i].transform.position = new Vector3(Horizontal3[i].transform.position.x, Horizontal3[i].transform.position.y - (3 * horizontal), Horizontal3[i].transform.position.z);
                            Horizontal3[i].SetActive(true);
                            Debug.Log(Horizontal3[i].name);
                        }
                    }

                }
                else if (currentCenterIndex == 0 || currentCenterIndex == 1 || currentCenterIndex == 2)
                {
                    if (lastCenterIndex == 6 || lastCenterIndex == 7 || lastCenterIndex == 8)
                    {
                        for (int i = 0; i < Horizontal2.Count; i++)
                        {
                            Horizontal2[i].SetActive(false);
                            Horizontal2[i].transform.position = new Vector3(Horizontal2[i].transform.position.x, Horizontal2[i].transform.position.y - (3 * horizontal), Horizontal2[i].transform.position.z);
                            Horizontal2[i].SetActive(true);
                            Debug.Log(Horizontal2[i].name);
                        }
                    }
                }
                else if (currentCenterIndex == 6 || currentCenterIndex == 7 || currentCenterIndex == 8)
                {
                    if (lastCenterIndex == 3 || lastCenterIndex == 4 || lastCenterIndex == 5)
                    {
                        for (int i = 0; i < Horizontal1.Count; i++)
                        {
                            Horizontal1[i].SetActive(false);
                            Horizontal1[i].transform.position = new Vector3(Horizontal1[i].transform.position.x, Horizontal1[i].transform.position.y - (3 * horizontal), Horizontal1[i].transform.position.z);
                            Horizontal1[i].SetActive(true);
                            Debug.Log(Horizontal1[i].name);
                        }
                    }
                }
          
    }

    void WhichGroupMoveHorizontalUp()
    {
                if (currentCenterIndex == 0 || currentCenterIndex == 1 || currentCenterIndex == 2)
                {
                    if (lastCenterIndex == 3 || lastCenterIndex == 4 || lastCenterIndex == 5)
                    {
                        for (int i = 0; i < Horizontal3.Count; i++)
                        {
                            Horizontal3[i].SetActive(false);
                            Horizontal3[i].transform.position = new Vector3(Horizontal3[i].transform.position.x, Horizontal3[i].transform.position.y + (3 * horizontal), Horizontal3[i].transform.position.z);
                            Horizontal3[i].SetActive(true);
                            Debug.Log(Horizontal3[i].name);
                        }
                    }
                }
                else if (currentCenterIndex == 6 || currentCenterIndex == 7 || currentCenterIndex == 8)
                {
                    if (lastCenterIndex == 0 || lastCenterIndex == 1 || lastCenterIndex == 2)
                    {
                        for (int i = 0; i < Horizontal2.Count; i++)
                        {
                            Horizontal2[i].SetActive(false);
                            Horizontal2[i].transform.position = new Vector3(Horizontal2[i].transform.position.x, Horizontal2[i].transform.position.y + (3 * horizontal), Horizontal2[i].transform.position.z);
                            Horizontal2[i].SetActive(true);
                            Debug.Log(Horizontal2[i].name);
                        }
                    }
                }
                else if (currentCenterIndex == 3 || currentCenterIndex == 4 || currentCenterIndex == 5)
                {
                    if (lastCenterIndex == 6 || lastCenterIndex == 7 || lastCenterIndex == 8)
                    {
                        for (int i = 0; i < Horizontal1.Count; i++)
                        {
                            Horizontal1[i].SetActive(false);
                            Horizontal1[i].transform.position = new Vector3(Horizontal1[i].transform.position.x, Horizontal1[i].transform.position.y + (3 * horizontal), Horizontal1[i].transform.position.z);
                            Horizontal1[i].SetActive(true);
                            Debug.Log(Horizontal1[i].name);
                        }
                    }
                }
    }

    void WhichGroupMoveVerticalLeft()
    {
                if (currentCenterIndex == 0 || currentCenterIndex == 3 || currentCenterIndex == 6)
                {
                    if (lastCenterIndex == 1 || lastCenterIndex == 4 || lastCenterIndex == 7)
                    {
                        for (int i = 0; i < Vertical3.Count; i++)
                        {
                            Vertical3[i].SetActive(false);
                            Vertical3[i].transform.position = new Vector3(Vertical3[i].transform.position.x - (3 * vertical), Vertical3[i].transform.position.y, Vertical3[i].transform.position.z);
                            Vertical3[i].SetActive(true);
                        }
                    }
                }
                else if (currentCenterIndex == 2 || currentCenterIndex == 5 || currentCenterIndex == 8)
                {
                    if (lastCenterIndex == 0 || lastCenterIndex == 3 || lastCenterIndex == 6)
                    {
                        for (int i = 0; i < Vertical2.Count; i++)
                        {
                            Vertical2[i].SetActive(false);
                            Vertical2[i].transform.position = new Vector3(Vertical2[i].transform.position.x - (3 * vertical), Vertical2[i].transform.position.y, Vertical2[i].transform.position.z);
                            Vertical2[i].SetActive(true);
                        }
                    }
                }
                else if (currentCenterIndex == 1 || currentCenterIndex == 4 || currentCenterIndex == 7)
                {
                    if (lastCenterIndex == 2 || lastCenterIndex == 5 || lastCenterIndex == 8)
                    {
                        for (int i = 0; i < Vertical1.Count; i++)
                        {
                            Vertical1[i].SetActive(false);
                            Vertical1[i].transform.position = new Vector3(Vertical1[i].transform.position.x - (3 * vertical), Vertical1[i].transform.position.y, Vertical1[i].transform.position.z);
                            Vertical1[i].SetActive(true);
                        }
                    }
                }
    }

    void WhichGroupMoveVerticalRight()
    {
                if (currentCenterIndex == 2 || currentCenterIndex == 5 || currentCenterIndex == 8)
                {
                    if (lastCenterIndex == 1 || lastCenterIndex == 4 || lastCenterIndex == 7)
                    {
                        for (int i = 0; i < Vertical1.Count; i++)
                        {
                            Vertical1[i].SetActive(false);
                            Vertical1[i].transform.position = new Vector3(Vertical1[i].transform.position.x + (3 * vertical), Vertical1[i].transform.position.y, Vertical1[i].transform.position.z);
                            Vertical1[i].SetActive(true);
                        }
                    }
                }
                else if (currentCenterIndex == 0 || currentCenterIndex == 3 || currentCenterIndex == 6)
                {
                    if (lastCenterIndex == 2 || lastCenterIndex == 5 || lastCenterIndex == 8)
                    {
                        for (int i = 0; i < Vertical2.Count; i++)
                        {
                            Vertical2[i].SetActive(false);
                            Vertical2[i].transform.position = new Vector3(Vertical2[i].transform.position.x + (3 * vertical), Vertical2[i].transform.position.y, Vertical2[i].transform.position.z);
                            Vertical2[i].SetActive(true);
                        }
                    }
                }
                else if (currentCenterIndex == 1 || currentCenterIndex == 4 || currentCenterIndex == 7)
                {
                    if (lastCenterIndex == 0 || lastCenterIndex == 3 || lastCenterIndex == 6)
                    {
                        for (int i = 0; i < Vertical3.Count; i++)
                        {
                            Vertical3[i].SetActive(false);
                            Vertical3[i].transform.position = new Vector3(Vertical3[i].transform.position.x + (3 * vertical), Vertical3[i].transform.position.y, Vertical3[i].transform.position.z);
                            Vertical3[i].SetActive(true);
                        }
                    }
                }
    }
}
