using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpawnerController : BaseWeaponSpawner
{
    //// ��x�̐����Ɏ���������
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

    //    // ���퐶��
    //    FireController ctrl = (FireController)createWeapon(transform.position, player.Forward);

    //    // ���̐����^�C�}�[
    //    spawnTimer = onceSpawnTime;
    //    onceSpawnCount--;

    //    // 1��̐������I������烊�Z�b�g
    //    if (1 > onceSpawnCount)
    //    {
    //        spawnTimer = Stats.GetRandomSpawnTimer();
    //        onceSpawnCount = (int)Stats.SpawnCount;
    //    }
    //}
    // ��x�̐����Ɏ���������
    int onceSpawnCount;
    float onceSpawnTime = 0.1f;
    PlayerController player;

    // ��̔��ˊp�x�i-30��, -15��, 0��, +15��, +30���j
    private readonly float[] spreadAngles = { -30f, -15f, 0f, 15f, 30f };

    void Start()
    {
        onceSpawnCount = (int)Stats.SpawnCount;
        player = transform.parent.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (isSpawnTimerNotElapsed()) return;

        // �v���C���[�̐i�s�������擾
        Vector2 baseDirection = player.Forward;

        // 5�����ɖ�𔭎�
        foreach (float angle in spreadAngles)
        {
            // �p�x�����W�A���ɕϊ�
            float radian = angle * Mathf.Deg2Rad;
            Vector2 newDirection = new Vector2(
                baseDirection.x * Mathf.Cos(radian) - baseDirection.y * Mathf.Sin(radian),
                baseDirection.x * Mathf.Sin(radian) + baseDirection.y * Mathf.Cos(radian)
            );

            // ���퐶��
            FireController ctrl = (FireController)createWeapon(transform.position, newDirection);
            if (ctrl == null)
            {
                Debug.LogError("Fire creation failed! Check createWeapon()");
                return;
            }

            Debug.Log($"Fire spawned at {transform.position} with direction {newDirection}");
        }

        // ���̐����^�C�}�[
        spawnTimer = onceSpawnTime;
        onceSpawnCount--;

        // 1��̐������I������烊�Z�b�g
        if (1 > onceSpawnCount)
        {
            spawnTimer = Stats.GetRandomSpawnTimer();
            onceSpawnCount = (int)Stats.SpawnCount;
        }
    }
}
