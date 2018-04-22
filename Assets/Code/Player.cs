using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
  private float health = 100;

  // Update is called once per frame
  private void Update()
  {
    if (health < 100)
    {
      health += 0.005f;
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