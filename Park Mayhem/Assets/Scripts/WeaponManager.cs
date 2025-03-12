using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponManager : MonoBehaviour
{
    public int minDamage, maxDamage;
    public Camera playerCamera;
    public float range = 300f;
    public ParticleSystem muzzleflash;
    public GameObject impactEffect;
    public AudioSource gunSound;

    private EnemyManager enemyManager;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.timeScale > 0)
        {
            Fire();
            muzzleflash.Play();
            gunSound.Play();

        }
    }

    private void Fire()
    {
        RaycastHit hit;

        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            enemyManager = hit.transform.GetComponent<EnemyManager>();
            Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));

            if(enemyManager != null)
            {
                enemyManager.EnemyTakeDamage(Random.Range(minDamage, maxDamage));
            }
            
        }
    }
}
