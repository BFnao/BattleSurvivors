using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class RotatingSwordSpawnerController : BaseWeaponSpawner
{

    // Update is called once per frame
    void Update()
    {
        // �I�u�W�F�N�g�̃J�E���g
        weapons.RemoveAll(item => item == null);
        // 1�ł��c���Ă�����I��
        if (0 < weapons.Count) return;

        // �S�������Ȃ�����^�C�}�[����
        if (isSpawnTimerNotElapsed()) return;
        // ���퐶��
        for (int i = 0; i < Stats.SpawnCount; i++)
        {
            RoraringSwordController ctrl = (RoraringSwordController)createWeapon(transform.position, transform);

            // �����p�x
            ctrl.Angle = 360f / Stats.SpawnCount * i;
        }
    }
}
