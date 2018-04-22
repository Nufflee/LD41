using UnityEngine;

public class AmmoPickupable : MonoBehaviour
{
  private bool isHealth;

  private void Start()
  {
    isHealth = Random.Range(0, 2) == 0;
  }

  void Update()
  {
    float playerDistance = Vector3.Distance(PlayerGlobals.Instance.Target.transform.position, transform.position);
    float aiDistance = Vector3.Distance(AIGlobals.Instance.Target.transform.position, transform.position);

    if (playerDistance < 2f)
    {
      // Add Ammo to the player.
      if (isHealth)
      {
        PlayerGlobals.Instance.Target.GetComponent<Player>().Damage(-Random.Range(5, 15));
      }
      else
      {
        PlayerGlobals.Instance.Target.transform.GetChild(0).GetChild(0).GetComponent<Gun>().AddAmmo(Random.Range(10, 30));
      }

      AIGlobals.Instance.pickupables.Remove(gameObject);
      Destroy(gameObject);
    }
    else if (aiDistance < 3f)
    {
      if (isHealth)
      {
        AIGlobals.Instance.Target.GetComponent<AIController>().Damage(-Random.Range(5, 15));
      }
      else
      {
        AIGlobals.Instance.Target.transform.GetChild(0).GetChild(0).GetComponent<AIGun>().AddAmmo(Random.Range(10, 30));
      }

      AIGlobals.Instance.pickupables.Remove(gameObject);
      Destroy(gameObject);
    }
  }
}