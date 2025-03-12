using System;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public int EnemyHealth = 200;
    //Navmesh
    public NavMeshAgent enemyAgent;
    public Transform player;
    public LayerMask enemyGroundLayer;
    public LayerMask playerGroundLayer;

    //Patrolling
    public Vector3 walkPoint;
    public float walkPointRange;
    public bool walkPointSet;

    //
    public float sightRange, attackRange;
    public bool enemySightRange, enemyAttackRange;
    public float rotationSpeed = 2f;

    public float attackDelay;
    public bool isAttacking;
    public Transform attackPoint;
    public GameObject projectile;
    public float projectileForce = 18f;
    public Animator enemyAnimator;
    private GameManager gameManager;
    public AudioSource spellVoice;

    //Particle
    public ParticleSystem deadEffect;


    void Start()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        enemyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        enemySightRange = Physics.CheckSphere(transform.position, sightRange, playerGroundLayer);
        enemyAttackRange = Physics.CheckSphere(transform.position, attackRange, playerGroundLayer);

        if(!enemySightRange && !enemyAttackRange)
        {
            Patrolling();
            enemyAnimator.SetBool("Patrolling",true);
            enemyAnimator.SetBool("PlayerDetecting",false);
            enemyAnimator.SetBool("PlayerAttacking",false);
        }
        else if(enemySightRange && !enemyAttackRange)
        {
            DetectPlayer();
            enemyAnimator.SetBool("Patrolling", false);
            enemyAnimator.SetBool("PlayerDetecting", true);
            enemyAnimator.SetBool("PlayerAttacking", false);
        }
        else if (enemySightRange && enemyAttackRange)
        {
            AttackPLayer();
            enemyAnimator.SetBool("Patrolling", false);
            enemyAnimator.SetBool("PlayerDetecting", false);
            enemyAnimator.SetBool("PlayerAttacking", true);
        }

    }
    void Patrolling()
    {
        if (walkPointSet == false)
        {
            float randomZPos = Random.Range(-walkPointRange, walkPointRange);
            float randomXPos = Random.Range(-walkPointRange, walkPointRange);

            walkPoint = new Vector3(transform.position.x + randomXPos, transform.position.y, transform.position.z + randomZPos);

            if(Physics.Raycast(walkPoint, -transform.up, 2f, enemyGroundLayer))
            {
                walkPointSet = true;
            }

            
        }
        if (walkPointSet == true)
        {
            enemyAgent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }
    //Vector3 dir = player.position - transform.position;
    //dir.y = 0;
    //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 0);
    void DetectPlayer()
    {
        enemyAgent.SetDestination(player.position);

        Vector3 direction = player.position - transform.position;
        direction.y = 0; // Düþmanýn sadece yatay eksende dönmesini saðla
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
    void AttackPLayer()
    {
        enemyAgent.SetDestination(transform.position);

        Vector3 direction = player.position - transform.position;
        direction.y = 0; // Düþmanýn sadece yatay eksende dönmesini saðla
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        if (isAttacking == false)
        {
            Rigidbody rb = Instantiate(projectile, attackPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * projectileForce, ForceMode.Impulse);
            spellVoice.Play();

            isAttacking = true;
            Invoke("ResetAttack", attackDelay);
        }
    }
    void ResetAttack()
    {
        isAttacking = false;
    }

    public void EnemyTakeDamage(int DamageAmount)
    {
        EnemyHealth -= DamageAmount;
        if(EnemyHealth <= 0)
        {
            EnemyDeath();
        }
    }

    private void EnemyDeath()
    {
        Destroy(gameObject);
        gameManager= FindAnyObjectByType<GameManager>();
        gameManager.AddKill();
        Instantiate(deadEffect, transform.position, Quaternion.identity);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
