using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : LivingEntity
{
    public enum Status
    {
        Idle,
        Trace,
        Attack,
        Die,
    }

    private Status currentStatus;
    public Status CurrentStatus
    {
        get { return currentStatus; }
        set
        {
            var prevStatus = currentStatus;
            currentStatus = value;

            switch (currentStatus)
            {
                case Status.Idle:
                    animator.SetBool("HasTarget", false);
                    agent.isStopped = true;
                    break;
                case Status.Trace:
                    animator.SetBool("HasTarget", true);
                    agent.isStopped = false;
                    break;
                case Status.Attack:
                    animator.SetBool("HasTarget", false);
                    agent.isStopped = true;
                    break;
                case Status.Die:
                    animator.SetTrigger("Die");
                    agent.isStopped = true;

                    break;
            }
        }
    }
    private NavMeshAgent agent;
    private Animator animator;
    public Transform target;
    public AudioSource audioSource;
    public AudioClip damageClip;
    public AudioClip dieClip;

    public ParticleSystem BloodPrefab;

    public float traceDistance;
    public float attackDistance;

    public float damage = 10f;
    public float lastAttackTime;
    public float attackInterval = 0.5f;
    public Renderer zombieRenderer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        switch (currentStatus)
        {
            case Status.Idle:
                UpdateIdle();
                break;
            case Status.Trace:
                UpdateTrace();
                break;
            case Status.Attack:
                UpdateAttack();
                break;
            case Status.Die:
                UpdateDie();
                break;
        }
    }

    private void UpdateDie()
    {
        return;
    }

    private void UpdateAttack()
    {
        if (target == null || (target != null &&
            Vector3.Distance(transform.position, target.position) > attackDistance))
        {
            CurrentStatus = Status.Trace;
            return;
        }

        var lookAt = target.position;
        lookAt.y = transform.position.y;
        transform.LookAt(transform.position);

        if (lastAttackTime + attackInterval < Time.time)
        {
            lastAttackTime = Time.time;

            var damagable = target.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.OnDamage(damage, transform.position, -transform.forward);
            }
        }
    }

    private void UpdateTrace()
    {

        if (target != null &&
            Vector3.Distance(transform.position, target.position) < attackDistance)
        {
            CurrentStatus = Status.Attack;
            return;
        }
        if (target == null ||
            Vector3.Distance(transform.position, target.position) > traceDistance)
        {
            CurrentStatus = Status.Idle;
            return;
        }
        agent.SetDestination(target.position);
    }

    private void UpdateIdle()
    {
        if (target != null &&
            Vector3.Distance(transform.position, target.position) < traceDistance)
        {
            CurrentStatus = Status.Trace;
            return;
        }
        target = FindTarget(traceDistance);
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        Debug.Log("Attack");
        base.OnDamage(damage, hitPoint, hitNormal);

        audioSource.PlayOneShot(damageClip);

        BloodPrefab.transform.position = hitPoint;
        BloodPrefab.transform.forward = hitNormal;
        BloodPrefab.Play();
    }

    protected override void Die()
    {
        base.Die();
        CurrentStatus = Status.Die;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        audioSource.PlayOneShot(dieClip);
    }

    public LayerMask targetLayer;

    protected Transform FindTarget(float radius)
    {
        var colliders = Physics.OverlapSphere(transform.position, radius, targetLayer.value);
        if (colliders.Length == 0)
        {
            return null;
        }
        var target = colliders.OrderBy(
            x => Vector3.Distance(x.transform.position, transform.position)).First();
        return target.transform;
    }

    public void SetUp(ZombieData data)
    {
        MaxHealth = data.maxHp;
        damage = data.damage;
        agent.speed = data.speed;

        zombieRenderer.material.color = data.skinColor;


    }
}
