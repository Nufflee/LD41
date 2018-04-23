using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour
{
  public float health = 0;

  private RigidbodyFirstPersonController controller;
  private Gun gun;

  private void Start()
  {
    controller = GetComponent<RigidbodyFirstPersonController>();
    gun = transform.GetChild(0).GetChild(0).GetComponent<Gun>();

    StartCoroutine(Test());
  }

  private IEnumerator Test()
  {
    yield return new WaitForSeconds(2.0f);
    GameObject gunGameObject = gun.gameObject;

    gun.enabled = false;
    controller.enabled = false;
    gunGameObject.GetComponent<Rigidbody>().isKinematic = false;
  }

  // Update is called once per frame
  private void Update()
  {
    if (health < 100)
    {
      health += 0.005f;
    }

    if (gun.isReloading && controller.movementSettings.ForwardSpeed != 4.0f)
    {
      controller.movementSettings.ForwardSpeed = 4.0f;
      controller.movementSettings.BackwardSpeed = 2.5f;
      controller.movementSettings.StrafeSpeed = 2.5f;
    }
    else if (gun.isReloading == false && controller.movementSettings.ForwardSpeed != 8.0f)
    {
      controller.movementSettings.ForwardSpeed = 8.0f;
      controller.movementSettings.BackwardSpeed = 4.0f;
      controller.movementSettings.StrafeSpeed = 4.0f;
    }
  }

  public void Damage(float damage)
  {
    health -= damage;

    // Update health bar.
    GameObject.Find("PlayerHealthBar").transform.GetChild(0).GetComponent<Image>().fillAmount = (health / 100f);

    if (health <= 0)
    {
      GameObject gunGameObject = gun.gameObject;

      gun.enabled = false;
      controller.enabled = false;
      gunGameObject.GetComponent<Rigidbody>().isKinematic = false;
      // TODO: Death
    }

    if (health > 100)
    {
      health = 100;
    }
  }
}