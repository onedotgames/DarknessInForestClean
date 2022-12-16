using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelSystem : CustomBehaviour
{
    public int barrelCount;
    public float timer = 0;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 10)
        {
            timer = 0;
            SpawnBarrel();
        }
    }

    void SpawnBarrel()
    {
        if(barrelCount < 10)
        {
            var pool = GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.BarrelPooler];
            var obj = pool.GetObjectFromPool();
            var x = Random.Range(-20, 20);
            var y = Random.Range(-20, 20);
            obj.transform.position = new Vector3(GameManager.PlayerManager.CurrentPlayer.transform.position.x + x, GameManager.PlayerManager.CurrentPlayer.transform.position.y + y, 0);
            barrelCount++;
        }
    }
    //10 saniyede bir barrel spawn olacak. tek seferde max 10 tane bulunabilir.
}
