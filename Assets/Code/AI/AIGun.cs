using UnityEngine;

public class AIGun : MonoBehaviour
{
  public float fireRate = 0.25f;

  private float nextFire = -1;
  private ParticleSystem muzzleFlash;
  private GameObject sparkEffect;

  private Camera AICamera;

  private void Start()
  {
    muzzleFlash = transform.Find("MuzzleFlash").GetComponent<ParticleSystem>();
    sparkEffect = Resources.Load<GameObject>("Prefabs/SparkEffect");
    AICamera = transform.GetComponentInParent<Camera>();
  }

  public void Shoot()
  {
    if (Time.time > nextFire)
    {
      nextFire = Time.time + fireRate;

      muzzleFlash.Play();

      RaycastHit hit;

      if (Physics.Raycast(AICamera.ViewportToWorldPoint(new Vector3(0.2f, 0.2f, 0.2f)), AICamera.transform.forward, out hit))
      {
        GameObject sparkGameObject = Instantiate(sparkEffect, hit.point, Quaternion.LookRotation(-AICamera.transform.forward));

        Destroy(sparkGameObject, 1.5f);

        if (hit.collider.CompareTag("Enemy"))
        {
          // balance
          hit.collider.gameObject.GetComponent<Enemy>().Damage(Mathf.Clamp(23.0f / (Vector3.Distance(transform.position, hit.point) / 14.0f), 0.0f, 20.0f));
        }
      }
    }
  }
}