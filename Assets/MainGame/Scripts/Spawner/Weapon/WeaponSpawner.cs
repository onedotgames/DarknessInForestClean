using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class WeaponSpawner : CustomBehaviour
{
    private ObjectPool<WeaponToSpawn> Pool;
    public Transform Parent;
    public WeaponToSpawn Obj;
    public int SpawnCooldown;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        Pool = new ObjectPool<WeaponToSpawn>(CreateObject, OnTakeFromPool, OnReturnToPool, OnDestroyObject, false, 200);
        Debug.Log("Object Spawner set: " + gameObject.name);
    }

    private WeaponToSpawn CreateObject()
    {
        WeaponToSpawn instance = Instantiate(Obj, Vector3.zero, Quaternion.identity);
        instance.Disable += ReturnObjectToPool;
        instance.gameObject.SetActive(false);
        return instance;
    }

    private void ReturnObjectToPool(WeaponToSpawn Instance)
    {
        Pool.Release(Instance);
    }

    private void OnTakeFromPool(WeaponToSpawn Instance)
    {
        //Instance.gameObject.SetActive(true);
        
        Instance.transform.SetParent(Parent, true);
        //Instance.BehaviourToInit.Initialize(GameManager);
    }

    public WeaponToSpawn GetFromPool()
    {
        if (Pool.IsUnityNull())
        {
            Debug.Log("Pool null");
        }
        var obj = Pool.Get();
        SpawnObject(obj);
        return obj;
    }

    private void OnReturnToPool(WeaponToSpawn Instance)
    {
        Instance.gameObject.SetActive(false);
    }

    private void OnDestroyObject(WeaponToSpawn Instance)
    {
        Destroy(Instance.gameObject);
    }


    private void SpawnObject(WeaponToSpawn Instance)
    {
        Instance.transform.position = transform.position;
        Instance.BehaviourToInit.Initialize(GameManager);
    }
}
