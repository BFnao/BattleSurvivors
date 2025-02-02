using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketSpawnerController : BaseWeaponSpawner
{
    //// Update is called once per frame
    //void Update()
    //{
    //    if (isSpawnTimerNotElapsed()) return;

    //    // ���̃^�C�}�[
    //    spawnTimer = Stats.GetRandomSpawnTimer();

    //    // �G�����Ȃ��ꍇ�͏������I��
    //    List<EnemyController> enemies = enemySpawner.GetEnemies();
    //    if (enemies.Count == 0) return;

    //    for (int i = 0; i < Stats.SpawnCount; i++)
    //    {
    //        // ���퐶��
    //        RocketController ctrl = (RocketController)createWeapon(transform.position);

    //        // �ł��߂��G���擾
    //        EnemyController closestEnemy = GetClosestEnemy(enemies);
    //        if (closestEnemy != null)
    //        {
    //            ctrl.Target = closestEnemy;
    //        }
    //    }
    //}

    ///// <summary>
    ///// ��ԋ߂��G���擾
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

        // ���̃X�|�[���^�C�}�[�ݒ�
        spawnTimer = Stats.GetRandomSpawnTimer();

        // �G�����Ȃ��ꍇ�͏������I��
        List<EnemyController> enemies = enemySpawner.GetEnemies();
        if (enemies.Count == 0) return;

        for (int i = 0; i < (int)Stats.SpawnCount; i++)
        {
            // �ł��߂��G���擾
            EnemyController closestEnemy = GetClosestEnemy(enemies);
            if (closestEnemy == null) continue;

            // ���P�b�g����
            RocketController rocket = createWeapon(transform.position) as RocketController;
            if (rocket != null)
            {
                rocket.SetTarget(closestEnemy);
            }
        }
    }

    /// <summary>
    /// ��ԋ߂��G���擾
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
