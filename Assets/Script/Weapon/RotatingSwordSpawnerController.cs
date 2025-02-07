using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class RotatingSwordSpawnerController : BaseWeaponSpawner
{

    // Update is called once per frame
    void Update()
    {
        // オブジェクトのカウント
        weapons.RemoveAll(item => item == null);
        // 1つでも残っていたら終了
        if (0 < weapons.Count) return;

        //// 全部無くなったらタイマー消化
        //if (isSpawnTimerNotElapsed()) return;
        // 武器生成
        for (int i = 0; i < Stats.SpawnCount; i++)
        {
            RotatingSwordController ctrl = (RotatingSwordController)createWeapon(transform.position, transform);

            // 初期角度
            ctrl.Angle = 360f / Stats.SpawnCount * i;
        }
    }
}
