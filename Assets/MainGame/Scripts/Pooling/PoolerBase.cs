using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolerBase : CustomBehaviour
{
    public GameObject ObjectToPool;

    public List<GameObject> ObjectList;
    public List<GameObject> TempList;

    [SerializeField] protected int mPoolCount;
    [SerializeField] private Transform mParentTransform;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        //SubEvents();

    }
    public void CreatePool()
    {
        TempList = new List<GameObject>();
        if(TempList.IsNullOrEmpty())
        {
            for (int i = 0; i < mPoolCount; i++)
            {
                GameObject obj = Instantiate(ObjectToPool, mParentTransform);
                obj.SetActive(false);
                ObjectList.Add(obj);
                TempList.Add(obj);
            }
            
        }
        else
        {
            TempList.ForEach(x =>
            {
                ObjectList.Add(x);
            });
        }
    }

    public void ExpandPool()
    {
        for (int i = 0; i < mPoolCount; i++)
        {
            GameObject obj = Instantiate(ObjectToPool, mParentTransform);
            obj.SetActive(false);
            ObjectList.Add(obj);
        }
    }

    public virtual GameObject GetObjectFromPool()
    {       
        GameObject objectToReturn = null;
        if(ObjectList.Count != 0) 
        {
            var returnObject = ObjectList[0];
            if (!returnObject.activeInHierarchy)
            {
                returnObject.SetActive(true);
                objectToReturn = returnObject;
                ObjectList.Remove(objectToReturn);
            }
        }
        else
        {
            ExpandPool();
            GetObjectFromPool();
        }        
        return objectToReturn;
    }


    public void ReturnObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.parent = mParentTransform;
        obj.transform.localPosition = Vector3.zero;
        ObjectList.Add(obj);
    }
    public void DisableObject(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.parent = mParentTransform;
        obj.transform.localPosition = Vector3.zero;
    }

    private void LevelFailed()
    {
        DisableAllObjects();
    }

    private void DisableAllObjects()
    {
        TempList.ForEach(x =>
        {
            if (x.activeInHierarchy)
            {
                DisableObject(x);
            }
        });
        ClearObjPool();
    }

    private void ClearObjPool()
    {
        ObjectList.Clear();
    }

    private void SubEvents()
    {
        if (GameManager != null)
        {
            GameManager.OnLevelFailed += LevelFailed;
        }
    }

    private void UnSubEvents()
    {
        if (GameManager != null)
        {
            GameManager.OnLevelFailed -= LevelFailed;

        }
    }

    private void OnDisable()
    {
        UnSubEvents();
    }

    private void OnDestroy()
    {
        UnSubEvents();
    }
}
