using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
  public float fireRate = 0.25f;

  private float nextFire = -1;
  private ParticleSystem muzzleFlash;
  private GameObject sparkEffect;
  private GameObject bulletHolePrefab;
  private GameObject glassBulletHolePrefab;
  private int magazineAmmo = 30;
  private int ammo = 90;
  private Text magazineAmmoText;
  private Text ammoText;
  private Animation animation;
  private AudioSource audioSource;
  public bool isReloading;
  private AudioClip gunShotClip;
  private AudioClip reloadClip;
  private GameObject shellPrefab;
  private Transform shellSpawn;

  private void Start()
  {
    muzzleFlash = transform.Find("MuzzleFlash").GetComponent<ParticleSystem>();
    sparkEffect = Resources.Load<GameObject>("Prefabs/SparkEffect");
    bulletHolePrefab = Resources.Load<GameObject>("Prefabs/BulletHole");
    glassBulletHolePrefab = Resources.Load<GameObject>("Prefabs/GlassBulletHole");
    Canvas canvas = transform.GetComponentInChildren<Canvas>();
    magazineAmmoText = canvas.transform.Find("MagazineAmmoText").GetComponent<Text>();
    ammoText = canvas.transform.Find("AmmoText").GetComponent<Text>();
    animation = GetComponent<Animation>();
    audioSource = GetComponent<AudioSource>();
    gunShotClip = Resources.Load<AudioClip>("Sounds/GunFire");
    reloadClip = Resources.Load<AudioClip>("Sounds/GunReload");
    shellPrefab = Resources.Load<GameObject>("Prefabs/Shell");
    shellSpawn = transform.Find("ShellSpawn");
  }

  public void AddAmmo(int amount) {
    ammo += amount;
    ammoText.text = ammo.ToString();
  }

  public void Shoot()
  {
    if (Time.time > nextFire && magazineAmmo > 0)
    {
      Quaternion recoilRotation = Quaternion.identity;

      if (transform.localRotation.eulerAngles.z > 0.0f)
      {
        recoilRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, transform.localRotation.eulerAngles.z / 3));
      }

      transform.Rotate(new Vector3(0, 0, 1), 1.0f);

      nextFire = Time.time + fireRate;

      muzzleFlash.Play();

      RaycastHit hit;

      magazineAmmoText.text = (--magazineAmmo).ToString();

      audioSource.PlayOneShot(gunShotClip, 1.0f);

      GameObject shell = Instantiate(shellPrefab, shellSpawn.position, shellSpawn.rotation);
      shell.GetComponent<Rigidbody>().AddForce(transform.up * 120f + transform.right * 120f);
      Destroy(shell, 30f);

      if (Physics.Raycast(recoilRotation * Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f)), Camera.main.transform.forward, out hit))
      {
        if (hit.collider.CompareTag("Enemy"))
        {
          GameObject sparkGameObject = Instantiate(sparkEffect, hit.point, Quaternion.LookRotation(-Camera.main.transform.forward));

          Destroy(sparkGameObject, 1.5f);

          // balance
          hit.collider.transform.parent.GetComponent<Enemy>().Damage(Mathf.Clamp(23.0f / (Vector3.Distance(transform.position, hit.point) / 14.0f), 0.0f, 25.0f));
        }

        if (hit.collider.CompareTag("Wall") || hit.collider.tag.Contains("Ground") || hit.collider.CompareTag("Glass"))
        {
          GameObject sparkGameObject = Instantiate(sparkEffect, hit.point, Quaternion.LookRotation(-Camera.main.transform.forward));

          Destroy(sparkGameObject, 1.5f);

          Vector3 offset = new Vector3(hit.normal.x == 0.0f ? 1 : hit.normal.x, hit.normal.y == 0.0f ? 1 : hit.normal.y, hit.normal.z == 0.0f ? 1 : hit.normal.z);

          if (hit.collider.CompareTag("Glass"))
          {
            GameObject glassBulletHole = Instantiate(glassBulletHolePrefab, new Vector3(hit.point.x + 0.01f * offset.x, hit.point.y + 0.01f * offset.y, hit.point.z + 0.01f * offset.z), Quaternion.LookRotation(hit.normal));
            Destroy(glassBulletHole, Random.Range(80f, 100f));
          }
          else
          {
            GameObject bulletHole = Instantiate(bulletHolePrefab, new Vector3(hit.point.x + 0.01f * offset.x, hit.point.y + 0.01f * offset.y, hit.point.z + 0.01f * offset.z), Quaternion.LookRotation(hit.normal));
            Destroy(bulletHole, Random.Range(80f, 100f));
          }
        }
      }
    }
  }

  private void Update()
  {
    if (isReloading) return;

    if (Input.GetMouseButton(0))
    {
      Shoot();
    }

    if (Input.GetKeyDown(KeyCode.R) && magazineAmmo < 30 && ammo > 0)
    {
      StartCoroutine(Reload());
    }

    if (transform.localRotation.eulerAngles.z > 0.0f && transform.localRotation.eulerAngles.z < 20.0f)
    {
      transform.Rotate(new Vector3(0, 0, 1), -0.08f);
    }
    else
    {
      transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0.0f);
    }
  }

  private IEnumerator Reload()
  {
    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0.0f);

    isReloading = true;

    animation.Play();

    yield return new WaitForSeconds(0.4f);

    audioSource.PlayOneShot(reloadClip);

    yield return new WaitForSeconds(animation.clip.length - 0.9f);

    // What if there's not enough ammo in ammo?
    if (ammo < 30 - magazineAmmo)
    {
      magazineAmmo = magazineAmmo + ammo;
      ammo = 0;
    }
    else
    {
      ammo = ammo - (30 - magazineAmmo);
      magazineAmmo = 30;
    }

    ammoText.text = ammo.ToString();
    magazineAmmoText.text = magazineAmmo.ToString();

    yield return new WaitForSeconds(0.5f);

    isReloading = false;
  }
}