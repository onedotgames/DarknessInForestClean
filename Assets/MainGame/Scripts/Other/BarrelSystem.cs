using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelSystem : CustomBehaviour
{
    public int barrelCount;
    public float timer = 0;
    private bool canBarrelSystemStart = false;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        if(GameManager != null)
        {
            GameManager.OnStartGame += LevelStart;
            GameManager.OnLevelCompleted += LevelCompleted;
            GameManager.OnLevelFailed += LevelFailed;
            GameManager.OnRestartGame += RestartGame;
        }
    }

    private void Update()
    {
        if (canBarrelSystemStart)
        {
            timer += Time.deltaTime;
            if (timer >= 10)
            {
                timer = 0;
                SpawnBarrel();
            }
        }
    }

    private void LevelCompleted()
    {
        canBarrelSystemStart = false;
        timer = 0;
        barrelCount = 0;
        ReturnPool();
    }

    private void LevelFailed()
    {
        canBarrelSystemStart = false;
        timer = 0;
        barrelCount = 0;
        ReturnPool();
    }
    private void RestartGame()
    {
        canBarrelSystemStart = true;
        timer = 0;
        barrelCount = 0;
        ReturnPool();
    }

    private void ReturnPool()
    {
        var pool = GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.BarrelPooler];
        for (int i = 0; i < pool.TempList.Count; i++)
        {
            pool.ReturnObjectToPool(pool.TempList[i]);
        }
        var potionPool = GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.HealthPotPooler];
        for (int j = 0; j < potionPool.TempList.Count; j++)
        {
            potionPool.ReturnObjectToPool(potionPool.TempList[j]);
        }
        var magnetPool = GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.MagnetPooler];
        for (int k = 0; k < magnetPool.TempList.Count; k++)
        {
            magnetPool.ReturnObjectToPool(magnetPool.TempList[k]);
        }
        var bombPool = GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.BombPooler];
        for (int l = 0; l < bombPool.TempList.Count; l++)
        {
            bombPool.ReturnObjectToPool(bombPool.TempList[l]);
        }
    }

    void SpawnBarrel()
    {
        if(barrelCount < 10)
        {
            if(GameManager.BackgroundManager.mapType == MapType.Normal)
            {
                var pool = GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.BarrelPooler];
                var obj = pool.GetObjectFromPool();
                var x = Random.Range(-20, 20);
                var y = Random.Range(-20, 20);
                obj.transform.position = new Vector3(GameManager.PlayerManager.CurrentPlayer.transform.position.x + x, GameManager.PlayerManager.CurrentPlayer.transform.position.y + y, 0);
                barrelCount++;
            }
            else if(GameManager.BackgroundManager.mapType == MapType.Horizontal)
            {
                var pool = GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.BarrelPooler];
                var obj = pool.GetObjectFromPool();
                var x = Random.Range(-20, 20);
                var y = Random.Range(-5, 5);
                obj.transform.position = new Vector3(GameManager.PlayerManager.CurrentPlayer.transform.position.x + x,y, 0);
                barrelCount++;
            }
            else if(GameManager.BackgroundManager.mapType == MapType.Vertical)
            {
                var pool = GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.BarrelPooler];
                var obj = pool.GetObjectFromPool();
                var x = Random.Range(-5, 5);
                var y = Random.Range(-20, 20);
                obj.transform.position = new Vector3(x, GameManager.PlayerManager.CurrentPlayer.transform.position.y + y, 0);
                barrelCount++;
            }
        }
    }
    //10 saniyede bir barrel spawn olacak. tek seferde max 10 tane bulunabilir.

    private void LevelStart()
    {
        canBarrelSystemStart = true;
        timer = 0;
        barrelCount = 0;
    }

    private void OnDestroy()
    {
        if (GameManager != null)
        {
            GameManager.OnStartGame -= LevelStart;
            GameManager.OnLevelCompleted -= LevelCompleted;
            GameManager.OnLevelFailed -= LevelFailed;
            GameManager.OnRestartGame -= RestartGame;
        }
    }
}
