using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyRevolver : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private float bulletSpeed = 30f;
    private float maxBulletDistance = 50f;

    [SerializeField]
    private GameObject target;

    [SerializeField]
    private GameObject bulletSpawn;

    private bool canShoot = true;

    // Start is called before the first frame update
    private void Awake()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (target != null)
        {
            transform.LookAt(target.transform);
        }
        if (canShoot && Vector3.Distance(transform.position, target.transform.position) < 30f)
        {
            StartCoroutine(Shoot());
        }
    }

    private IEnumerator Shoot()
    {
        canShoot = false;

        var bulletInstance = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bulletInstance.transform.position = bulletSpawn.transform.position;

        var bulletRB = bulletInstance.GetComponent<Rigidbody>();
        Vector3 bulletDirection = bulletRB.velocity;
        StartCoroutine(BulletTravel(bulletInstance));

        yield return new WaitForSeconds(3);
        canShoot = true;
    }

    private IEnumerator BulletTravel(GameObject bulletInstance)
    {
        BoxCollider bulletCollider = bulletInstance.GetComponent<BoxCollider>();
        float bulletRadius = bulletCollider.bounds.extents.magnitude;

        Vector3 originalPosition = bulletInstance.transform.position;
        Vector3 originalForward = transform.forward;

        while (true)
        {
            bulletInstance.transform.position += (originalForward * bulletSpeed * Time.deltaTime);

            if (Vector3.Distance(bulletInstance.transform.position, originalPosition) > maxBulletDistance)
            {
                //Debug.Log("Bullet Travelled Too Far");
                Destroy(bulletInstance);
                yield break;
            }

            Collider[] hitColliders = Physics.OverlapBox(
                bulletInstance.transform.position,
                bulletCollider.size / 2,
                bulletInstance.transform.rotation);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider != bulletCollider)
                {
                    if (hitCollider.gameObject.layer == LayerMask.NameToLayer("Player"))
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    }
                    //Debug.Log("Bullet Hit Something");
                    Destroy(bulletInstance);
                    yield break;
                }
            }
            yield return null;
        }
    }
}
