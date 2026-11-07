using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkeletonTeam : Entity
{
    [Header("Combat")]
    protected float damage = 20f;

    [Header("Health")]
    protected float health = 100f;

    [Header("Enemy Collection")]

    protected CrusaderFaction[] enemiesArray;
    protected List <CrusaderFaction> enemies;
    protected float distanceToEnemy;

    protected void CacheEnemy()
    {
        enemiesArray = FindObjectsByType<CrusaderFaction>(FindObjectsSortMode.InstanceID);
        enemies = enemiesArray.ToList();
    }

    protected CrusaderFaction ClosestEnemy()
    {
        CrusaderFaction enemy = null;
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

    protected void SelfDestruct()
    {
        if (IsDead())
        {
            Destroy(this.gameObject);
        }
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
}