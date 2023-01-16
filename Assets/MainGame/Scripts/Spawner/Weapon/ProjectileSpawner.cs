using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectileSpawner : CustomBehaviour
{
    private ObjectPool<ProjectileToSpawn> Pool;
    public Transform Parent;
    public ProjectileToSpawn Obj;
    public int SpawnCooldown;
    [SerializeField] private int _objectCount;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        Pool = new ObjectPool<ProjectileToSpawn>(CreateObject, OnTakeFromPool, OnReturnToPool, OnDestroyObject, false, _objectCount);
        Debug.Log("Object Spawner set: " + gameObject.name);
    }

    private ProjectileToSpawn CreateObject()
    {
        ProjectileToSpawn instance = Instantiate(Obj, Vector3.zero, Quaternion.identity);
        instance.Disable += ReturnObjectToPool;
        instance.gameObject.SetActive(false);
        return instance;
    }

    public void ReturnObjectToPool(ProjectileToSpawn Instance)
    {
        Pool.Release(Instance);
    }

    private void OnTakeFromPool(ProjectileToSpawn Instance)
    {
        //Instance.gameObject.SetActive(true);
        if(Instance != null)
        {
            Instance.transform.SetParent(Parent, true);

        }
        //Instance.BehaviourToInit.Initialize(GameManager);
    }

    public ProjectileToSpawn GetFromPool()
    {
        if (Pool.IsUnityNull())
        {
            Debug.Log("Pool null");
        }
        var obj = Pool.Get();
        SpawnObject(obj);
        return obj;
    }

    private void OnReturnToPool(ProjectileToSpawn Instance)
    {
        Instance.gameObject.SetActive(false);
    }

    private void OnDestroyObject(ProjectileToSpawn Instance)
    {
        Destroy(Instance.gameObject);
    }


    private void SpawnObject(ProjectileToSpawn Instance)
    {
        if(Instance != null)
        {
            Instance.transform.position = transform.position;

        }
        else
        {
            Debug.Log("Instance null");
        }
        //Instance.BehaviourToInit.Initialize(GameManager);
    }
}
