using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Exchange : MonoBehaviour
{

    private GameObject player;
    private GameObject aiPlayer;
	
	private bool arePlacesSwitched;


    void Start()
    {
        player = GameObject.FindWithTag("Player");
        aiPlayer = GameObject.FindWithTag("AIPlayer");
        Invoke("SwitchPlaces", Random.Range(30, 80));
        arePlacesSwitched = false;
    }

    void SwitchPlaces()
    {
        arePlacesSwitched = !arePlacesSwitched;

		// Switch locations
        aiPlayer.GetComponent<NavMeshAgent>().enabled = false;
        Vector3 p = aiPlayer.transform.position;
        aiPlayer.transform.position = player.transform.position;
        player.transform.position = p;
        aiPlayer.GetComponent<NavMeshAgent>().enabled = true;

        // Change the globals.
		aiPlayer.GetComponent<AIController>().SetGlobals(arePlacesSwitched ? PlayerGlobals.Instance : AIGlobals.Instance);
        Invoke("SwitchPlaces", Random.Range(30, 80));
    }
}