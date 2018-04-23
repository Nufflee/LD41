using UnityEngine;

public class HealthPickupable : MonoBehaviour
{
  void Update()
  {
    transform.position += new Vector3(0, Mathf.Sin(Time.time * 3.5f) / 145.0f, 0);
    transform.Rotate(new Vector3(0, 1, 0), 0.8f);

    float playerDistance = Vector3.Distance(PlayerGlobals.Instance.Target.transform.position, transform.position);
    float aiDistance = Vector3.Distance(AIGlobals.Instance.Target.transform.position, transform.position);

    if (playerDistance < 2f)
    {
      if (PlayerGlobals.Instance.Target.GetComponent<Player>().health < 100)
      {
        PlayerGlobals.Instance.Target.GetComponent<Player>().Damage(-Random.Range(5, 15));
        PlayerGlobals.Instance.Target.transform.GetChild(0).GetChild(0).GetComponent<Gun>().audioSource.PlayOneShot(PlayerGlobals.Instance.Target.transform.GetChild(0).GetChild(0).GetComponent<Gun>().pickUp);

        AIGlobals.Instance.pickupables.Remove(gameObject);
        Destroy(gameObject);
      }
    }
    else if (aiDistance < 3f)
    {
      if (AIGlobals.Instance.Target.GetComponent<AIController>().health < 100)
      {
        AIGlobals.Instance.Target.GetComponent<AIController>().Damage(-Random.Range(5, 15));
        AIGlobals.Instance.Target.transform.GetChild(0).GetChild(0).GetComponent<AIGun>().audioSource.PlayOneShot(AIGlobals.Instance.Target.transform.GetChild(0).GetChild(0).GetComponent<AIGun>().pickUp);

        AIGlobals.Instance.pickupables.Remove(gameObject);
        Destroy(gameObject);
      }
    }
  }
}