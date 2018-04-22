using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
  public float fireRate = 0.25f;

  private float nextFire = -1;
  private ParticleSystem muzzleFlash;
  private GameObject sparkEffect;
  private int magazineAmmo = 30;
  private int ammo = 90;
  private Text magazineAmmoText;
  private Text ammoText;

  private GameObject bulletPrefab;

  private Transform bulletSpawn;

  private void Start()
  {
    muzzleFlash = transform.Find("MuzzleFlash").GetComponent<ParticleSystem>();
    sparkEffect = Resources.Load<GameObject>("Prefabs/SparkEffect");
    Canvas canvas = transform.GetComponentInChildren<Canvas>();
    magazineAmmoText = canvas.transform.Find("MagazineAmmoText").GetComponent<Text>();
    ammoText = canvas.transform.Find("AmmoText").GetComponent<Text>();
    bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");
    bulletSpawn = transform.Find("BulletSpawn");
  }

  public void Shoot()
  {
    if (Time.time > nextFire && magazineAmmo > 0)
    {
      nextFire = Time.time + fireRate;

      muzzleFlash.Play();

      RaycastHit hit;

      magazineAmmoText.text = (--magazineAmmo).ToString();

      SpawnBullet();

      if (Physics.Raycast(Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f)), Camera.main.transform.forward, out hit))
      {
        GameObject sparkGameObject = Instantiate(sparkEffect, hit.point, Quaternion.LookRotation(-Camera.main.transform.forward));

        Destroy(sparkGameObject, 1.5f);

        if (hit.collider.CompareTag("Enemy"))
        {
          // balance
          hit.collider.gameObject.GetComponent<Enemy>().Damage(Mathf.Clamp(23.0f / (Vector3.Distance(transform.position, hit.point) / 14.0f), 0.0f, 25.0f));
        }
      }
    }
  }

  private void SpawnBullet() {
    // Create the Bullet from the Bullet Prefab
    var bullet = (GameObject)Instantiate(
        bulletPrefab,
        bulletSpawn.position,
        bulletSpawn.rotation);
      
  }

  private void Update()
  {
    if (Input.GetMouseButton(0))
    {
      Shoot();
    }
  }
}