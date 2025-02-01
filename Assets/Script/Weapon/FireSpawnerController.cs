using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpawnerController : BaseWeaponSpawner
{
    //// 一度の生成に時差をつける
    //int onceSpawnCount;
    //float onceSpawnTime = 0.1f;
    //PlayerController player;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    onceSpawnCount = (int)Stats.SpawnCount;
    //    player = transform.parent.GetComponent<PlayerController>();
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (isSpawnTimerNotElapsed()) return;

    //    // 武器生成
    //    FireController ctrl = (FireController)createWeapon(transform.position, player.Forward);

    //    // 次の生成タイマー
    //    spawnTimer = onceSpawnTime;
    //    onceSpawnCount--;

    //    // 1回の生成が終わったらリセット
    //    if (1 > onceSpawnCount)
    //    {
    //        spawnTimer = Stats.GetRandomSpawnTimer();
    //        onceSpawnCount = (int)Stats.SpawnCount;
    //    }
    //}
    // 一度の生成に時差をつける
    int onceSpawnCount;
    float onceSpawnTime = 0.1f;
    PlayerController player;

    // 矢の発射角度（-30°, -15°, 0°, +15°, +30°）
    private readonly float[] spreadAngles = { -30f, -15f, 0f, 15f, 30f };

    void Start()
    {
        onceSpawnCount = (int)Stats.SpawnCount;
        player = transform.parent.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (isSpawnTimerNotElapsed()) return;

        // プレイヤーの進行方向を取得
        Vector2 baseDirection = player.Forward;

        // 5方向に矢を発射
        foreach (float angle in spreadAngles)
        {
            // 角度をラジアンに変換
            float radian = angle * Mathf.Deg2Rad;
            Vector2 newDirection = new Vector2(
                baseDirection.x * Mathf.Cos(radian) - baseDirection.y * Mathf.Sin(radian),
                baseDirection.x * Mathf.Sin(radian) + baseDirection.y * Mathf.Cos(radian)
            );

            // 武器生成
            FireController ctrl = (FireController)createWeapon(transform.position, newDirection);
            if (ctrl == null)
            {
                Debug.LogError("Fire creation failed! Check createWeapon()");
                return;
            }

            Debug.Log($"Fire spawned at {transform.position} with direction {newDirection}");
        }

        // 次の生成タイマー
        spawnTimer = onceSpawnTime;
        onceSpawnCount--;

        // 1回の生成が終わったらリセット
        if (1 > onceSpawnCount)
        {
            spawnTimer = Stats.GetRandomSpawnTimer();
            onceSpawnCount = (int)Stats.SpawnCount;
        }
    }
}
