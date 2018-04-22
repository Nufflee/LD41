using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MMAIGun : MonoBehaviour
{
  public float fireRate = 0.25f;

  private float nextFire = -1;
  private ParticleSystem muzzleFlash;
  private GameObject sparkEffect;
  private Animation animation;
  private bool isReloading;
  private int magazineAmmo = 30;
  private int ammo = 90;
  private Text magazineAmmoText;
  private Text ammoText;

  private Camera AICamera;

  private void Start()
  {
    muzzleFlash = transform.Find("MuzzleFlash").GetComponent<ParticleSystem>();
    sparkEffect = Resources.Load<GameObject>("Prefabs/SparkEffect");
    AICamera = transform.GetComponentInParent<Camera>();
    Canvas canvas = transform.GetComponentInChildren<Canvas>();
    magazineAmmoText = canvas.transform.Find("MagazineAmmoText").GetComponent<Text>();
    ammoText = canvas.transform.Find("AmmoText").GetComponent<Text>();
    animation = GetComponent<Animation>();
  }

  public void Shoot()
  {
    if (magazineAmmo == 0 && isReloading == false)
    {
      StartCoroutine(Reload());

      return;
    }

    if (Time.time > nextFire && isReloading == false)
    {
      nextFire = Time.time + fireRate;

      muzzleFlash.Play();

      RaycastHit hit;

      magazineAmmoText.text = (--magazineAmmo).ToString();

      if (Physics.Raycast(AICamera.ViewportToWorldPoint(new Vector3(0.2f, 0.2f, 0.2f)), AICamera.transform.forward, out hit))
      {
        GameObject sparkGameObject = Instantiate(sparkEffect, hit.point, Quaternion.LookRotation(-AICamera.transform.forward));

        Destroy(sparkGameObject, 1.5f);

        if (hit.collider.CompareTag("Enemy"))
        {
          // balance
          hit.collider.transform.parent.GetComponent<MMEnemy>().Damage(Mathf.Clamp(23.0f / (Vector3.Distance(transform.position, hit.point) / 14.0f), 0.0f, 20.0f));
        }
      }
    }
  }

  private IEnumerator Reload()
  {
    isReloading = true;

    animation.Play();

    yield return new WaitForSeconds(animation.clip.length - 0.5f);

    // What if there's not enough ammo in ammo?
    if (ammo < 30 - magazineAmmo)
    {
      magazineAmmo = magazineAmmo + ammo;
      ammo = 0;
    }
    else
    {
      ammo = 90;
      magazineAmmo = 30;
    }

    ammoText.text = ammo.ToString();
    magazineAmmoText.text = magazineAmmo.ToString();

    yield return new WaitForSeconds(0.5f);

    isReloading = false;
  }
}