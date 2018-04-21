using System.Runtime.InteropServices;
using UnityEngine;

public class Gun : MonoBehaviour
{
  public float fireRate = 0.25f;

  private float nextFire = -1;
  private ParticleSystem muzzleFlash;
  private GameObject sparkEffect;

  private void Start()
  {
    muzzleFlash = transform.Find("MuzzleFlash").GetComponent<ParticleSystem>();
    sparkEffect = Resources.Load<GameObject>("Prefabs/SparkEffect");
  }

  public void Shoot()
  {
    if (Time.time > nextFire)
    {
      nextFire = Time.time + fireRate;

      muzzleFlash.Play();

      RaycastHit hit;

      if (Physics.Raycast(Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f)), Camera.main.transform.forward, out hit))
      {
        GameObject sparkGameObject = Instantiate(sparkEffect, hit.point, Quaternion.LookRotation(-Camera.main.transform.forward));

        Destroy(sparkGameObject, 1.5f);

        if (hit.collider.CompareTag("Enemy"))
        {
          print("damage: " + 33.0f / (Vector3.Distance(transform.position, hit.point) / 2));
          hit.collider.gameObject.GetComponent<Enemy>().Damage(Mathf.Clamp(33.0f / (Vector3.Distance(transform.position, hit.point) / 8), 0.0f, 33.0f));
        }
      }
    }
  }

  private void Update()
  {
    if (Input.GetMouseButton(0))
    {
      Shoot();
    }
  }
}