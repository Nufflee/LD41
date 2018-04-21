using UnityEngine;

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

    print("health: " + health);

    if (health <= 0)
    {
      Destroy(gameObject);
      // TODO: Death
    }
  }
}