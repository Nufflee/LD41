using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickupable : MonoBehaviour {

	void Update () {
		float playerDistance = Vector3.Distance(PlayerGlobals.Instance.Target.transform.position, transform.position);
        float aiDistance = Vector3.Distance(AIGlobals.Instance.Target.transform.position, transform.position);
		if(playerDistance < 2f) {
			// Add Ammo to the player.
			PlayerGlobals.Instance.Target.transform.GetChild(0).GetChild(0).GetComponent<Gun>().AddAmmo(Random.Range(10, 30));
            AIGlobals.Instance.pickupables.Remove(gameObject);
            Destroy(gameObject);
		} else if(aiDistance < 3f) {
            AIGlobals.Instance.Target.transform.GetChild(0).GetChild(0).GetComponent<AIGun>().AddAmmo(Random.Range(10, 30));
            AIGlobals.Instance.pickupables.Remove(gameObject);
			Destroy(gameObject);
		}
        
	}
}
