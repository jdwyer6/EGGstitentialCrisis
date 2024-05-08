using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public int maxAmmo = 10;
    private int currentAmmo;
    public float bulletSpeed = 1000f;
    public float reloadTime = 1f;
    private bool isReloading = false;
    public GameObject muzzleFlash;
    public GameObject gunPrefab;

    private Queue<GameObject> bulletPool;
    private AudioManager am;
    public Animator anim;

    void Start()
    {
        currentAmmo = maxAmmo;
        bulletPool = new Queue<GameObject>();
        am = FindObjectOfType<AudioManager>();
        gunPrefab.SetActive(true);

        // Initialize the bullet pool
        for (int i = 0; i < maxAmmo; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Enqueue(bullet);
        }
    }

    void Update()
    {
        if(GetComponent<ModeSelection>().handEnabled == false)
        {
            gunPrefab.SetActive(false);
            return;
        } else {
            gunPrefab.SetActive(true);
        }

        if (isReloading)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse0) && currentAmmo > 0)
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        am.Play("Gunshotthumper");
        Debug.Log("Shoot");
        StartCoroutine(initMuzzleFlash());
        anim.SetTrigger("Recoil");
        currentAmmo--;

        GameObject bullet = bulletPool.Dequeue();
        bullet.SetActive(true);
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = firePoint.forward * bulletSpeed;

        // Enqueue the bullet again for future use
        bulletPool.Enqueue(bullet);
    }

    IEnumerator Reload()
    {
        isReloading = true;
        anim.SetTrigger("Reload");
        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
    }

    IEnumerator initMuzzleFlash() {
        GameObject muzzleFlashInstance = Instantiate(muzzleFlash, firePoint.position, firePoint.rotation);
        Light light = muzzleFlashInstance.GetComponentInChildren<Light>();
        yield return new WaitForSeconds(0.05f);
        light.enabled = false;
        yield return new WaitForSeconds(4f);
        Destroy(muzzleFlashInstance);
    }
}
