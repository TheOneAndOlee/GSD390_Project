using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RevolverData : MonoBehaviour
{

    [SerializeField]
    private Material tracerMaterial;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private float bulletSpeed = 30f;
    private float maxBulletDistance = 50f;

    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private GameObject bulletSpawn;

    [SerializeField]
    private float bulletLifespan;

    private bool hasRicocheted = false;

    // Start is called before the first frame update
    private void Awake()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var bulletInstance = Instantiate(bulletPrefab, transform.position, mainCamera.transform.rotation);
            bulletInstance.transform.position = bulletSpawn.transform.position;

            var bulletRB = bulletInstance.GetComponent<Rigidbody>();
            Vector3 bulletDirection = bulletRB.velocity;

            bulletRB.collisionDetectionMode = CollisionDetectionMode.Continuous;


            AddBulletTracer(bulletInstance);

            StartCoroutine(BulletTravel(bulletInstance));
        }
    }

    private void AddBulletTracer(GameObject bullet)
    {
        TrailRenderer tracer = bullet.GetComponent<TrailRenderer>();
        tracer.material = tracerMaterial;
        tracer.time = 0.1f;
        tracer.startWidth = 0.1f;
        tracer.endWidth = 0.05f;
        tracer.minVertexDistance = 0.1f;
    }

    private IEnumerator BulletTravel(GameObject bulletInstance)
    {
        BoxCollider bulletCollider = bulletInstance.GetComponent<BoxCollider>();
        float bulletRadius = bulletCollider.bounds.extents.magnitude;


        Vector3 originalPosition = bulletInstance.transform.position;
        Vector3 bulletDirection = mainCamera.transform.forward;

        float elapsedTime = 0f;

        while (true)
        {
            bulletInstance.transform.position += (bulletDirection * bulletSpeed * Time.deltaTime);
                
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= bulletLifespan)
            {
                //Debug.Log("Bullet Expired");
                Destroy(bulletInstance);
                yield break;
            }

            if (Vector3.Distance(bulletInstance.transform.position, originalPosition) > maxBulletDistance)
            {
                //Debug.Log("Bullet Travelled Too Far");
                Destroy(bulletInstance);
                yield break;
            }

            Collider[] hitColliders =  Physics.OverlapBox(
                bulletInstance.transform.position,
                bulletCollider.size/2,
                bulletInstance.transform.rotation);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider != bulletCollider)
                {
                    if (hitCollider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
                        Destroy(bulletInstance);
                        yield break;
                    }
                    else if (hitCollider.gameObject.layer == LayerMask.NameToLayer("Wall") && !hasRicocheted)
                    {
                        RaycastHit hit;
                        if (Physics.Raycast(bulletInstance.transform.position, bulletDirection, out hit, bulletRadius))
                        {
                            bulletDirection = Vector3.Reflect(bulletDirection, hit.normal);
                            bulletInstance.transform.forward = bulletDirection;
                            hasRicocheted = true;
                            //Debug.Log("Bullet Richocheted");

                            bulletInstance.GetComponent<Rigidbody>().velocity = bulletDirection * bulletSpeed;

                        }
                    }
                    else
                    {
                        //Debug.Log("Bullet hit a surface");
                        Destroy(bulletInstance);
                        yield break;
                    }
                    hasRicocheted = false;
                }
            }

            yield return null;
        }
    }
}
