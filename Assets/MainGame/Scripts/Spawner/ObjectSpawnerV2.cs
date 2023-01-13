using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectSpawnerV2 : CustomBehaviour
{
    private ObjectPool<ObjectToPool> Pool;
    public Transform Parent;
    public ObjectToPool Obj;
    public int SpawnCooldown;


    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        Pool = new ObjectPool<ObjectToPool>(CreateObject, OnTakeFromPool, OnReturnToPool, OnDestroyObject, false, 200);
        Debug.Log("Object Spawner set: " + gameObject.name);
    }

    private ObjectToPool CreateObject()
    {
        ObjectToPool instance = Instantiate(Obj, Vector3.zero, Quaternion.identity);
        instance.Disable += ReturnObjectToPool;
        instance.gameObject.SetActive(false);
        return instance;
    }
    
    public void ReturnObjectToPool(ObjectToPool Instance)
    {
        Pool.Release(Instance);
    }

    private void OnTakeFromPool(ObjectToPool Instance)
    {
        //Instance.gameObject.SetActive(true);
        SpawnObject(Instance);
        Instance.transform.SetParent(Parent, true);
        Instance.BehaviourToInit.Initialize(GameManager);
    }

    public ObjectToPool GetFromPool()
    {
        if (Pool.IsUnityNull())
        {
            Debug.Log("Pool null");
        }
        var obj = Pool.Get();
        return obj;
    }

    private void OnReturnToPool(ObjectToPool Instance)
    {
        Instance.gameObject.SetActive(false);
    }

    private void OnDestroyObject(ObjectToPool Instance)
    {
        Destroy(Instance.gameObject);
    }

    public void SpawnEnemy()
    {
        var obj = Pool.Get();
        var enemy = obj.GetComponent<EnemyBase>();
        enemy.ActivateEnemy();
    }
    

    private void SpawnObject(ObjectToPool Instance)
    {
        Instance.transform.position = transform.position;
        Instance.BehaviourToInit.Initialize(GameManager);
    }
}
