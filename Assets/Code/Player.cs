using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour
{
  private float health = 100;

  private RigidbodyFirstPersonController controller;
  private Gun gun;

  private void Start()
  {
    controller = GetComponent<RigidbodyFirstPersonController>();
    gun = transform.GetChild(0).GetChild(0).GetComponent<Gun>();
  }

  // Update is called once per frame
  private void Update()
  {
    if (health < 100)
    {
      health += 0.005f;
    }

    print(gun.isReloading);

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
      Destroy(gameObject);
      // TODO: Death
    }
  }
}