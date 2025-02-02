using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketSpawnerController : BaseWeaponSpawner
{
    //// Update is called once per frame
    //void Update()
    //{
    //    if (isSpawnTimerNotElapsed()) return;

    //    // 次のタイマー
    //    spawnTimer = Stats.GetRandomSpawnTimer();

    //    // 敵がいない場合は処理を終了
    //    List<EnemyController> enemies = enemySpawner.GetEnemies();
    //    if (enemies.Count == 0) return;

    //    for (int i = 0; i < Stats.SpawnCount; i++)
    //    {
    //        // 武器生成
    //        RocketController ctrl = (RocketController)createWeapon(transform.position);

    //        // 最も近い敵を取得
    //        EnemyController closestEnemy = GetClosestEnemy(enemies);
    //        if (closestEnemy != null)
    //        {
    //            ctrl.Target = closestEnemy;
    //        }
    //    }
    //}

    ///// <summary>
    ///// 一番近い敵を取得
    ///// </summary>
    //private EnemyController GetClosestEnemy(List<EnemyController> enemies)
    //{
    //    EnemyController closestEnemy = null;
    //    float closestDistance = float.MaxValue;
    //    Vector3 spawnerPosition = transform.position;

    //    foreach (EnemyController enemy in enemies)
    //    {
    //        float distance = Vector3.Distance(spawnerPosition, enemy.transform.position);
    //        if (distance < closestDistance)
    //        {
    //            closestDistance = distance;
    //            closestEnemy = enemy;
    //        }
    //    }

    //    return closestEnemy;
    //}
    // Update is called once per frame
    void Update()
    {
        if (isSpawnTimerNotElapsed()) return;

        // 次のスポーンタイマー設定
        spawnTimer = Stats.GetRandomSpawnTimer();

        // 敵がいない場合は処理を終了
        List<EnemyController> enemies = enemySpawner.GetEnemies();
        if (enemies.Count == 0) return;

        for (int i = 0; i < (int)Stats.SpawnCount; i++)
        {
            // 最も近い敵を取得
            EnemyController closestEnemy = GetClosestEnemy(enemies);
            if (closestEnemy == null) continue;

            // ロケット生成
            RocketController rocket = createWeapon(transform.position) as RocketController;
            if (rocket != null)
            {
                rocket.SetTarget(closestEnemy);
            }
        }
    }

    /// <summary>
    /// 一番近い敵を取得
    /// </summary>
    private EnemyController GetClosestEnemy(List<EnemyController> enemies)
    {
        EnemyController closestEnemy = null;
        float closestDistanceSqr = float.MaxValue;
        Vector3 spawnerPosition = transform.position;

        foreach (EnemyController enemy in enemies)
        {
            float distanceSqr = (enemy.transform.position - spawnerPosition).sqrMagnitude;
            if (distanceSqr < closestDistanceSqr)
            {
                closestDistanceSqr = distanceSqr;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }
}
