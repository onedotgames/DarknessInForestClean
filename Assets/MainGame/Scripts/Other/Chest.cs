using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Chest : MonoBehaviour
{
    [Header("Chest Variables")]
    public bool canChestOpen = false;
    public GameManager GameManager;

    private void Update()
    {
        if (canChestOpen)
        {
            canChestOpen = false;
            OpenChest();
        }
    }
    public void OpenChest()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
        StartCoroutine(ThrowExpCoinRoutine());
    }

    public IEnumerator ThrowExpCoinRoutine()
    {
        var randomCount = Random.Range(1, 10);
        for (int i = 0; i < randomCount; i++)
        {
            var coinPool = GameManager.PoolingManager.CoinPoolerList[(int)CoinType.Small];
            var coin = coinPool.GetObjectFromPool();
            coin.transform.position = transform.position;
            var expPool = GameManager.PoolingManager.ExpPoolerList[(int)ExpPoolerType.SmallExperience];
            var exp = expPool.GetObjectFromPool();
            exp.transform.position = transform.position;
            var coinEndValue = new Vector3
                (
                    (coin.transform.position.x + (UnityEngine.Random.Range(-1f, 1f))),
                    (coin.transform.position.y - (UnityEngine.Random.Range(0.5f, 0.8f))),
                    (coin.transform.position.z)
                );
            coin.GetComponent<BoxCollider2D>().enabled = false;
            coin.transform.DOJump(coinEndValue, 1, 1, 0.5f).OnComplete(() => coin.GetComponent<BoxCollider2D>().enabled = true);
            var expEndValue = new Vector3
                (
                    (exp.transform.position.x + (UnityEngine.Random.Range(-1f, 1f))),
                    (exp.transform.position.y - (UnityEngine.Random.Range(0.5f, 0.8f))),
                    (exp.transform.position.z)
                );
            exp.GetComponent<BoxCollider2D>().enabled = false;
            exp.transform.DOJump(expEndValue, 1, 1, 0.5f).OnComplete(() => exp.GetComponent<BoxCollider2D>().enabled = true); ;
        }
        yield return new WaitForSeconds(1.5f);
        GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.ChestPooler].ReturnObjectToPool(this.gameObject);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
