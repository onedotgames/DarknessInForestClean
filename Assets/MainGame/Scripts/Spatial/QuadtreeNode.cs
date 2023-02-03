using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadtreeNode : CustomBehaviour
{
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
    }

    private Vector3 position;
    private Vector3 size;
    private List<GameObject> enemies;
    private QuadtreeNode[] children;

    public QuadtreeNode(Vector3 position, Vector3 size)
    {
        this.position = position;
        this.size = size;
        //enemies = new List<GameObject>();
        enemies = new List<GameObject>();
        children = null;
    }

    private void Divide()
    {
        children = new QuadtreeNode[4];
        Vector3 halfSize = size / 2;
        Vector3 childSize = size / 4;

        children[0] = new QuadtreeNode(position + new Vector3(-halfSize.x, halfSize.y, 0), childSize);
        children[1] = new QuadtreeNode(position + new Vector3(halfSize.x, halfSize.y, 0), childSize);
        children[2] = new QuadtreeNode(position + new Vector3(-halfSize.x, -halfSize.y, 0), childSize);
        children[3] = new QuadtreeNode(position + new Vector3(halfSize.x, -halfSize.y, 0), childSize);
    }

    //public void Insert(GameObject enemy)
    //{
    //    if (children != null)
    //    {
    //        for (int i = 0; i < children.Length; i++)
    //        {
    //            if (children[i].Contains(enemy.transform.position))
    //            {
    //                children[i].Insert(enemy);
    //                return;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        enemies.Add(enemy);
    //        if (enemies.Count > maxEnemiesPerNode && size.x > minSize)
    //        {
    //            Divide();
    //            List<GameObject> currentEnemies = enemies;
    //            enemies = new List<GameObject>();
    //            foreach (GameObject currentEnemy in currentEnemies)
    //            {
    //                Insert(currentEnemy);
    //            }
    //        }
    //    }
    //}
    //public GameObject FindClosestEnemy(Vector3 position)
    //{
    //    float closestDistance = Mathf.Infinity;
    //    GameObject closestEnemy = null;

    //    if (children != null)
    //    {
    //        for (int i = 0; i < children.Length; i++)
    //        {
    //            if (children[i].Contains(position))
    //            {
    //                GameObject enemy = children[i].FindClosestEnemy(position);
    //                if (enemy != null)
    //                {
    //                    float distance = Vector3.Distance(position, enemy.transform.position);
    //                    if (distance < closestDistance)
    //                    {
    //                        closestDistance = distance;
    //                        closestEnemy = enemy;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    else
    //    {
    //        foreach (GameObject enemy in enemies)
    //        {
    //            float distance = Vector3.Distance(position, enemy.transform.position);
    //            if (distance < closestDistance)
    //            {
    //                closestDistance = distance;
    //                closestEnemy = enemy;
    //            }
    //        }
    //    }

    //    return closestEnemy;
    //}
}
