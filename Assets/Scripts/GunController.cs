using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun;

    private float currentFireRate; //연사속도

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        GunFireRateCal();
        TryFire();
    }

    private void GunFireRateCal()
    {
        if(currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime; // 1/60
        }
    }
    
    private void TryFire()
    {
        if(Input.GetButton("Fire1") && currentFireRate <= 0)
        {
            Fire();
        }
    }
    //발사전
    private void Fire()
    {
        currentFireRate = currentGun.fireRate;
        Shoot();
    }

    private void Shoot()
    {
        Debug.Log("총알 발사함");
        PlaySE(currentGun.fire_Sound);
        currentGun.muzzleFlash.Play();
    }

    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
