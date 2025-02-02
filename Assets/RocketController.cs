using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : BaseWeapon
{
    //// 状態
    //enum State
    //{
    //    Bomb,
    //    Explosion,
    //    DamageFloor,
    //    Destroy,
    //}
    //State state;


    //// アニメーター
    //Animator animator;
    //// アニメーション毎のタイマー
    //Dictionary<State, float> animationTimer;
    //// ダメージフロア滞在時間
    //float damageFloorCoolDownTime = 0.5f;
    //// 敵毎のタイマー（ダメージフロア時）
    //Dictionary<EnemyController, float> damageFloorTimer;

    //// ターゲット
    //public EnemyController Target;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    // 初期化
    //    animationTimer = new Dictionary<State, float>();
    //    damageFloorTimer = new Dictionary<EnemyController, float>();
    //    animator = GetComponent<Animator>();



    //    // 爆弾時
    //    animationTimer.Add(State.Bomb, Random.Range(0.5f, 1.5f));
    //    // 爆発時
    //    animationTimer.Add(State.Explosion, 0.66f);
    //    // ダメージフロア
    //    animationTimer.Add(State.DamageFloor, 30f);

    //    // 初期状態
    //    state = State.Bomb;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    // タイマー消化で次の状態へ
    //    if (animationTimer.ContainsKey(state))
    //    {
    //        animationTimer[state] -= Time.deltaTime;

    //    }
    //}

    //// 爆弾の状態を変える
    //void changeState(State next)
    //{
    //    // 爆発
    //    if (State.Explosion == next)
    //    {
    //        animator.SetTrigger("isExplosion");
    //        rigidbody2d.gravityScale = 0;
    //        rigidbody2d.velocity = Vector2.zero;
    //    }
    //    // ダメージフロアー
    //    else if (State.DamageFloor == next)
    //    {
    //        animator.SetTrigger("isDamageFloor");
    //        // 下側に入れる
    //        GetComponent<SpriteRenderer>().sortingOrder = 2;
    //    }
    //    // 終了
    //    else if (State.Destroy == next)
    //    {
    //        Destroy(gameObject);
    //    }

    //    // 現在の状態
    //    state = next;
    //}

    //// 衝突時
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    // 敵以外
    //    if (!collision.gameObject.TryGetComponent<EnemyController>(out var enemy)) return;

    //    // 爆弾
    //    if (State.Bomb == state)
    //    {
    //        attackEnemy(collision);
    //        changeState(State.Explosion);
    //    }
    //    // 爆発中
    //    else if (State.Explosion == state)
    //    {
    //        attackEnemy(collision);
    //    }
    //}

    //// 衝突している間
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    // ダメージフロアーじゃない
    //    if (State.DamageFloor != state) return;

    //    // 敵以外
    //    if (!collision.gameObject.TryGetComponent<EnemyController>(out var enemy)) return;

    //    // ターゲットのタイマーをセット
    //    damageFloorTimer.TryAdd(enemy, damageFloorCoolDownTime);
    //    // 敵毎にタイマーを消化
    //    damageFloorTimer[enemy] -= Time.deltaTime;

    //    // 一定時間でダメージ
    //    if (0 > damageFloorTimer[enemy])
    //    {
    //        attackEnemy(collision, stats.Attack / 2);
    //        damageFloorTimer[enemy] = damageFloorCoolDownTime;
    //    }
    //}
    // ロケットの状態
    enum State
    {
        Bomb,         // 飛行中
        Explosion,    // 爆発
        DamageFloor,  // 爆発後のダメージフロア
        Destroy       // 消滅
    }
    private State state;

    // アニメーター
    private Animator animator;

    // アニメーション毎のタイマー
    private Dictionary<State, float> animationTimer;

    // ダメージフロア滞在時間
    private float damageFloorCoolDownTime = 0.5f;

    // 敵毎のタイマー（ダメージフロア時）
    private Dictionary<EnemyController, float> damageFloorTimer;

    // ターゲット
    public EnemyController Target { get; private set; }

    // Rigidbody2D
    private Rigidbody2D rb;

    // 移動速度
    public float speed = 5f;

    void Start()
    {
        // 初期化
        animationTimer = new Dictionary<State, float>();
        damageFloorTimer = new Dictionary<EnemyController, float>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // 各状態のタイマー設定
        animationTimer[State.Bomb] = Random.Range(0.5f, 1.5f);
        animationTimer[State.Explosion] = 0.66f;
        animationTimer[State.DamageFloor] = 30f;

        // 初期状態
        state = State.Bomb;
    }

    void FixedUpdate()
    {
        if (state == State.Bomb && Target != null)
        {
            // ターゲットの方向を取得
            Vector2 direction = (Target.transform.position - transform.position).normalized;

            // ロケットをターゲットの方向に向かわせる
            rb.velocity = direction * speed;

            // ロケットの向きをターゲット方向に回転させる
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void Update()
    {
        // 現在の状態に応じたタイマー処理
        if (animationTimer.ContainsKey(state))
        {
            animationTimer[state] -= Time.deltaTime;
            if (animationTimer[state] <= 0)
            {
                switch (state)
                {
                    case State.Bomb:
                        ChangeState(State.Explosion);
                        break;
                    case State.Explosion:
                        ChangeState(State.DamageFloor);
                        break;
                    case State.DamageFloor:
                        ChangeState(State.Destroy);
                        break;
                }
            }
        }
    }

    public void SetTarget(EnemyController target)
    {
        Target = target;
    }

    private void ChangeState(State next)
    {
        state = next;

        switch (next)
        {
            case State.Explosion:
                animator.SetTrigger("isExplosion");
                rb.velocity = Vector2.zero; // 停止
                rb.gravityScale = 0;
                break;

            case State.DamageFloor:
                animator.SetTrigger("isDamageFloor");
                GetComponent<SpriteRenderer>().sortingOrder = 2; // 下側に描画
                break;

            case State.Destroy:
                Destroy(gameObject);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent<EnemyController>(out var enemy)) return;

        if (state == State.Bomb)
        {
            AttackEnemy(collision);
            ChangeState(State.Explosion);
        }
        else if (state == State.Explosion)
        {
            AttackEnemy(collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (state != State.DamageFloor) return;

        if (!collision.gameObject.TryGetComponent<EnemyController>(out var enemy)) return;

        if (!damageFloorTimer.ContainsKey(enemy))
        {
            damageFloorTimer[enemy] = damageFloorCoolDownTime;
        }

        damageFloorTimer[enemy] -= Time.deltaTime;

        if (damageFloorTimer[enemy] <= 0)
        {
            AttackEnemy(enemy, stats.Attack / 2);
            damageFloorTimer[enemy] = damageFloorCoolDownTime;
        }
    }

    private void AttackEnemy(EnemyController enemy, float damage = -1)
    {
        if (damage < 0)
        {
            damage = stats.Attack;
        }
        enemy.TakeDamage(damage);
    }
}
