﻿using UnityEngine;

public class AmmoPickupable : MonoBehaviour
{
  private void Update()
  {
    transform.position += new Vector3(0, Mathf.Sin(Time.time * 3.5f) / 145.0f, 0);
    transform.Rotate(new Vector3(0, 1, 0), 0.8f);

    float playerDistance = Vector3.Distance(PlayerGlobals.Instance.Target.transform.position, transform.position);
    float aiDistance = Vector3.Distance(AIGlobals.Instance.Target.transform.position, transform.position);

    if (playerDistance < 2f)
    {
      if (PlayerGlobals.Instance.Target.transform.GetChild(0).GetChild(0).GetComponent<Gun>().ammo < 270)
      {
        // Add Ammo to the player.
        PlayerGlobals.Instance.Target.transform.GetChild(0).GetChild(0).GetComponent<Gun>().AddAmmo(Random.Range(10, 30));
        PlayerGlobals.Instance.Target.transform.GetChild(0).GetChild(0).GetComponent<Gun>().audioSource.PlayOneShot(PlayerGlobals.Instance.Target.transform.GetChild(0).GetChild(0).GetComponent<Gun>().pickUp);

        AIGlobals.Instance.pickupables.Remove(gameObject);
        Destroy(gameObject);
      }
    }
    else if (aiDistance < 3f)
    {
      if (AIGlobals.Instance.Target.transform.GetChild(0).GetChild(0).GetComponent<AIGun>().ammo < 270)
      {
        AIGlobals.Instance.Target.transform.GetChild(0).GetChild(0).GetComponent<AIGun>().AddAmmo(Random.Range(10, 30));
        AIGlobals.Instance.Target.transform.GetChild(0).GetChild(0).GetComponent<AIGun>().audioSource.PlayOneShot(AIGlobals.Instance.Target.transform.GetChild(0).GetChild(0).GetComponent<AIGun>().pickUp);
        
        AIGlobals.Instance.pickupables.Remove(gameObject);
        Destroy(gameObject);
      }
    }
  }
}