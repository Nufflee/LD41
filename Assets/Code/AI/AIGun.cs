using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AIGun : MonoBehaviour
{
  public float fireRate = 0.25f;

  private float nextFire = -1;
  private ParticleSystem muzzleFlash;
  private GameObject sparkEffect;
  private Animation animation;
  private AudioSource audioSource;
  public bool isReloading;
  private int magazineAmmo = 30;
  private int ammo = 90;
  private Text magazineAmmoText;
  private Text ammoText;
  private AudioClip gunShotClip;
  private AudioClip reloadClip;
  private GameObject bulletHolePrefab;
  private Camera AICamera;
  private GameObject shellPrefab;
  private Transform shellSpawn;

  private void Start()
  {
    muzzleFlash = transform.Find("MuzzleFlash").GetComponent<ParticleSystem>();
    sparkEffect = Resources.Load<GameObject>("Prefabs/SparkEffect");
    bulletHolePrefab = Resources.Load<GameObject>("Prefabs/BulletHole");
    AICamera = transform.GetComponentInParent<Camera>();
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

  public void AddAmmo(int amount)
  {
    ammo += amount;
    ammoText.text = ammo.ToString();
  }

  public void Shoot()
  {
    if (magazineAmmo == 0 && isReloading == false)
    {
      StartCoroutine(Reload());

      return;
    }

    if (Time.time > nextFire && isReloading == false && magazineAmmo > 0)
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
      shell.GetComponent<Rigidbody>().AddForce(transform.right * 120f);
      Destroy(shell, 30f);

      if (Physics.Raycast(recoilRotation * AICamera.ViewportToWorldPoint(new Vector3(0.2f, 0.2f, 0.0f)), AICamera.transform.forward, out hit))
      {
        GameObject sparkGameObject = Instantiate(sparkEffect, hit.point, Quaternion.LookRotation(-AICamera.transform.forward));
        Destroy(sparkGameObject, 1.5f);

        if (hit.collider.CompareTag("Enemy"))
        {
          // balance
          hit.collider.transform.parent.GetComponent<Enemy>().Damage(Mathf.Clamp(23.0f / (Vector3.Distance(transform.position, hit.point) / 14.0f), 0.0f, 15.0f));
        }

        if (hit.collider.CompareTag("Wall"))
        {
          GameObject bulletHole = Instantiate(bulletHolePrefab, hit.point, Quaternion.LookRotation(-AICamera.transform.forward));
          Destroy(bulletHole, Random.Range(80f, 100f));
        }
      }
    }
  }

  private void Update()
  {
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

    if (ammo < 90)
    {
      ammo = 90;
    }
  }
}