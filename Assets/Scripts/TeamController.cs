using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CrusaderTeam : MonoBehaviour
{
    [Header("Combat")]
    protected float damage = 20f;

    [Header("Health")]
    protected float health = 100f;

    [Header("Enemy Collection")]

    public EntityController[] enemiesArray;
    protected List <EntityController> enemies;
    protected EntityController enemy = null;
    private float distanceToEnemy;

    protected void CacheEnemy()
    {
        enemiesArray = FindObjectsByType<SkeletonTeam>(FindObjectsSortMode.InstanceID);
        enemies = enemiesArray.ToList();
    }

    protected EntityController ClosestEnemy()
    {
        float closest = Mathf.Infinity;

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null)
            {
                continue;
            }

            distanceToEnemy = Vector3.Distance(this.transform.position, enemies[i].transform.position);

            if (distanceToEnemy < closest)
            {
                closest = distanceToEnemy;
                enemy = enemies[i];
            }
        }
        return enemy;
    }

    protected bool IsDead()
    {
        if (health <= 0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    protected void SelfDestruct()
    {
        if (IsDead())
        {
            Destroy(this.gameObject);
        }
    }
}