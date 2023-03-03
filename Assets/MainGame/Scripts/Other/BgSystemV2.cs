using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class BgSystemV2 : MonoBehaviour
{
    public GameObject BackGround;
    public GameObject BGSystem;
    public float TileX;
    public GameObject ownBGObject;
    public SwipeLevels swipeLevels;
    public List<SpriteRenderer> bgSprites;
    public BGList bglist;
    [SerializeField] private LayerMask _layerMask;

    private void Update()
    {
        if(swipeLevels.currentLevelMat != null && !swipeLevels.GameManager.ChallengeManager.isChallengeLevel)
        {
            for (int i = 0; i < bgSprites.Count; i++)
            {
                bgSprites[i].sprite = swipeLevels.currentLevelMat;
            }
        }
    }

    private bool CheckOverlap(Vector3 point)
    {
        var isTrue = false;
        if (Physics2D.OverlapCircle(point, 1, _layerMask) != null)
        {
            isTrue = true;
        }
        return isTrue;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ownBGObject = transform.parent.transform.parent.gameObject;
            
            if (gameObject.name == "Left")
            {
                if (!CheckOverlap(new Vector3(transform.position.x + (TileX * -2), 
                    transform.position.y, transform.position.z)))
                {
                    //Instantiate(ObjectToSpawn, point, Quaternion.identity);
                    //SpawnObject(point);
                    //if (GameManager.ChallengeManager.isChallengeLevel)
                    //    SpawnChallengeObject(point);

                    GameObject newBG = Instantiate(BackGround);
                    newBG.transform.parent = BGSystem.transform;
                    newBG.transform.position = ownBGObject.transform.position;
                    newBG.transform.position = new Vector3(transform.position.x + (TileX * -2), 
                        transform.position.y, transform.position.z);
                    GameObject newTop = Instantiate(BackGround);
                    GameObject newDown = Instantiate(BackGround);
                    newTop.transform.parent = BGSystem.transform;
                    newDown.transform.parent = BGSystem.transform;
                    newTop.transform.position = newBG.transform.position;
                    newDown.transform.position = newBG.transform.position;
                    newTop.transform.position = new Vector3(newBG.transform.position.x, 
                        newBG.transform.position.y + (2 * TileX), newBG.transform.position.z);
                    newDown.transform.position = new Vector3(newBG.transform.position.x, 
                        newBG.transform.position.y + (-2 * TileX), newBG.transform.position.z);
                    bglist.backgrounds.Add(newBG);
                    bglist.backgrounds.Add(newTop);
                    bglist.backgrounds.Add(newDown);

                }
                else { Debug.Log("BG LEFT OVERLAP VAR"); }
                
            }
            else if(gameObject.name == "Right")
            {
                if (!CheckOverlap(new Vector3(transform.position.x + (TileX * 2),
                        transform.position.y, transform.position.z)))
                {
                    //Instantiate(ObjectToSpawn, point, Quaternion.identity);
                    //SpawnObject(point);
                    //if (GameManager.ChallengeManager.isChallengeLevel)
                    //    SpawnChallengeObject(point);

                    GameObject newBG = Instantiate(BackGround);
                    newBG.transform.parent = BGSystem.transform;
                    newBG.transform.position = ownBGObject.transform.position;
                    newBG.transform.position = new Vector3(transform.position.x + (TileX * 2), 
                        transform.position.y, transform.position.z);
                    GameObject newTop = Instantiate(BackGround);
                    GameObject newDown = Instantiate(BackGround);
                    newTop.transform.parent = BGSystem.transform;
                    newDown.transform.parent = BGSystem.transform;
                    newTop.transform.position = newBG.transform.position;
                    newDown.transform.position = newBG.transform.position;
                    newTop.transform.position = new Vector3(newBG.transform.position.x, 
                        newBG.transform.position.y + (2 * TileX), newBG.transform.position.z);
                    newDown.transform.position = new Vector3(newBG.transform.position.x, 
                        newBG.transform.position.y + (-2 * TileX), newBG.transform.position.z);
                    bglist.backgrounds.Add(newBG);
                    bglist.backgrounds.Add(newTop);
                    bglist.backgrounds.Add(newDown);

                }
                else { Debug.Log("BG RIGHT OVERLAP VAR"); }
           
            }
            else if (gameObject.name == "Top")
            {
                if (!CheckOverlap(new Vector3(transform.position.x,
                        transform.position.y + (2 * TileX), transform.position.z)))
                {
                    //Instantiate(ObjectToSpawn, point, Quaternion.identity);
                    //SpawnObject(point);
                    //if (GameManager.ChallengeManager.isChallengeLevel)
                    //    SpawnChallengeObject(point);

                    GameObject newBG = Instantiate(BackGround);
                    newBG.transform.parent = BGSystem.transform;
                    newBG.transform.position = ownBGObject.transform.position;
                    newBG.transform.position = new Vector3(transform.position.x, 
                        transform.position.y + (2 * TileX), transform.position.z);
                    bglist.backgrounds.Add(newBG);

                }
                else { Debug.Log("BG TOP OVERLAP VAR"); }
                
            }
            else if (gameObject.name == "Down")
            {
                if (!CheckOverlap(new Vector3(transform.position.x,
                        transform.position.y + (-2 * TileX), transform.position.z)))
                {
                    //Instantiate(ObjectToSpawn, point, Quaternion.identity);
                    //SpawnObject(point);
                    //if (GameManager.ChallengeManager.isChallengeLevel)
                    //    SpawnChallengeObject(point);

                    GameObject newBG = Instantiate(BackGround);
                    newBG.transform.parent = BGSystem.transform;
                    newBG.transform.position = ownBGObject.transform.position;
                    newBG.transform.position = new Vector3(transform.position.x, 
                        transform.position.y + (-2 * TileX), transform.position.z);
                    bglist.backgrounds.Add(newBG);

                }
                else { Debug.Log("BG DOWN OVERLAP VAR"); }
                
            }
        }
    }
}
