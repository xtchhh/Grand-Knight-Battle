using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CrusaderFaction : Entity
{
    [Header("Health")]
    public float health = 100f;

    [Header("Enemy Collection")]
    protected SkeletonTeam[] enemiesArray;
    protected List <SkeletonTeam> enemies;

    protected void CacheEnemy()
    {
        enemiesArray = FindObjectsByType<SkeletonTeam>(FindObjectsSortMode.InstanceID);
        enemies = enemiesArray.ToList();
    }

    protected SkeletonTeam ClosestEnemy()
    {
        float distanceToEnemy = 0;
        SkeletonTeam enemy = null;
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
}