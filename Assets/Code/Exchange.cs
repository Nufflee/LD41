using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Exchange : MonoBehaviour
{
  private GameObject player;
  private GameObject aiPlayer;

  public bool arePlacesSwitched;

  void Start()
  {
    player = GameObject.FindWithTag("Player");
    aiPlayer = GameObject.FindWithTag("AIPlayer");
    Invoke("SwitchPlaces", Random.Range(1, 1));
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


    foreach (GameObject enemy in PlayerGlobals.Instance.enemies)
    {
      enemy.GetComponent<Enemy>().globals = AIGlobals.Instance;
    }

    foreach (GameObject enemy in AIGlobals.Instance.enemies)
    {
      enemy.GetComponent<Enemy>().globals = PlayerGlobals.Instance;
    }

    Globals playerGlobals = PlayerGlobals.Instance;
    print(PlayerGlobals.Instance.Target);
    PlayerGlobals.Instance.Ground = AIGlobals.Instance.Ground;
    AIGlobals.Instance.Ground = playerGlobals.Ground;
    print(PlayerGlobals.Instance.Target);

    // Change the globals.
    // aiPlayer.GetComponent<AIController>().SetGlobals(arePlacesSwitched ? PlayerGlobals.Instance : AIGlobals.Instance);
    Invoke("SwitchPlaces", Random.Range(30, 80));
  }
}