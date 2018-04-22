using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
  private float health = 100;

  // Use this for initialization
  private void Start()
  {
  }

  // Update is called once per frame
  private void Update()
  {
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