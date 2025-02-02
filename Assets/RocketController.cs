using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : BaseWeapon
{
    //// ���
    //enum State
    //{
    //    Bomb,
    //    Explosion,
    //    DamageFloor,
    //    Destroy,
    //}
    //State state;


    //// �A�j���[�^�[
    //Animator animator;
    //// �A�j���[�V�������̃^�C�}�[
    //Dictionary<State, float> animationTimer;
    //// �_���[�W�t���A�؍ݎ���
    //float damageFloorCoolDownTime = 0.5f;
    //// �G���̃^�C�}�[�i�_���[�W�t���A���j
    //Dictionary<EnemyController, float> damageFloorTimer;

    //// �^�[�Q�b�g
    //public EnemyController Target;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    // ������
    //    animationTimer = new Dictionary<State, float>();
    //    damageFloorTimer = new Dictionary<EnemyController, float>();
    //    animator = GetComponent<Animator>();



    //    // ���e��
    //    animationTimer.Add(State.Bomb, Random.Range(0.5f, 1.5f));
    //    // ������
    //    animationTimer.Add(State.Explosion, 0.66f);
    //    // �_���[�W�t���A
    //    animationTimer.Add(State.DamageFloor, 30f);

    //    // �������
    //    state = State.Bomb;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    // �^�C�}�[�����Ŏ��̏�Ԃ�
    //    if (animationTimer.ContainsKey(state))
    //    {
    //        animationTimer[state] -= Time.deltaTime;

    //    }
    //}

    //// ���e�̏�Ԃ�ς���
    //void changeState(State next)
    //{
    //    // ����
    //    if (State.Explosion == next)
    //    {
    //        animator.SetTrigger("isExplosion");
    //        rigidbody2d.gravityScale = 0;
    //        rigidbody2d.velocity = Vector2.zero;
    //    }
    //    // �_���[�W�t���A�[
    //    else if (State.DamageFloor == next)
    //    {
    //        animator.SetTrigger("isDamageFloor");
    //        // �����ɓ����
    //        GetComponent<SpriteRenderer>().sortingOrder = 2;
    //    }
    //    // �I��
    //    else if (State.Destroy == next)
    //    {
    //        Destroy(gameObject);
    //    }

    //    // ���݂̏��
    //    state = next;
    //}

    //// �Փˎ�
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    // �G�ȊO
    //    if (!collision.gameObject.TryGetComponent<EnemyController>(out var enemy)) return;

    //    // ���e
    //    if (State.Bomb == state)
    //    {
    //        attackEnemy(collision);
    //        changeState(State.Explosion);
    //    }
    //    // ������
    //    else if (State.Explosion == state)
    //    {
    //        attackEnemy(collision);
    //    }
    //}

    //// �Փ˂��Ă����
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    // �_���[�W�t���A�[����Ȃ�
    //    if (State.DamageFloor != state) return;

    //    // �G�ȊO
    //    if (!collision.gameObject.TryGetComponent<EnemyController>(out var enemy)) return;

    //    // �^�[�Q�b�g�̃^�C�}�[���Z�b�g
    //    damageFloorTimer.TryAdd(enemy, damageFloorCoolDownTime);
    //    // �G���Ƀ^�C�}�[������
    //    damageFloorTimer[enemy] -= Time.deltaTime;

    //    // ��莞�ԂŃ_���[�W
    //    if (0 > damageFloorTimer[enemy])
    //    {
    //        attackEnemy(collision, stats.Attack / 2);
    //        damageFloorTimer[enemy] = damageFloorCoolDownTime;
    //    }
    //}
    // ���P�b�g�̏��
    enum State
    {
        Bomb,         // ��s��
        Explosion,    // ����
        DamageFloor,  // ������̃_���[�W�t���A
        Destroy       // ����
    }
    private State state;

    // �A�j���[�^�[
    private Animator animator;

    // �A�j���[�V�������̃^�C�}�[
    private Dictionary<State, float> animationTimer;

    // �_���[�W�t���A�؍ݎ���
    private float damageFloorCoolDownTime = 0.5f;

    // �G���̃^�C�}�[�i�_���[�W�t���A���j
    private Dictionary<EnemyController, float> damageFloorTimer;

    // �^�[�Q�b�g
    public EnemyController Target { get; private set; }

    // Rigidbody2D
    private Rigidbody2D rb;

    // �ړ����x
    public float speed = 5f;

    void Start()
    {
        // ������
        animationTimer = new Dictionary<State, float>();
        damageFloorTimer = new Dictionary<EnemyController, float>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // �e��Ԃ̃^�C�}�[�ݒ�
        animationTimer[State.Bomb] = Random.Range(0.5f, 1.5f);
        animationTimer[State.Explosion] = 0.66f;
        animationTimer[State.DamageFloor] = 30f;

        // �������
        state = State.Bomb;
    }

    void FixedUpdate()
    {
        if (state == State.Bomb && Target != null)
        {
            // �^�[�Q�b�g�̕������擾
            Vector2 direction = (Target.transform.position - transform.position).normalized;

            // ���P�b�g���^�[�Q�b�g�̕����Ɍ����킹��
            rb.velocity = direction * speed;

            // ���P�b�g�̌������^�[�Q�b�g�����ɉ�]������
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void Update()
    {
        // ���݂̏�Ԃɉ������^�C�}�[����
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
                rb.velocity = Vector2.zero; // ��~
                rb.gravityScale = 0;
                break;

            case State.DamageFloor:
                animator.SetTrigger("isDamageFloor");
                GetComponent<SpriteRenderer>().sortingOrder = 2; // �����ɕ`��
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
