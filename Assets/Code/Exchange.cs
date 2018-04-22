using UnityEngine;
using UnityEngine.AI;

public class Exchange : MonoBehaviour
{
  private GameObject player;
  private GameObject aiPlayer;

  public bool arePlacesSwitched;

  public static Exchange instance;

  void Start()
  {
    instance = this;
    player = GameObject.FindWithTag("Player");
    aiPlayer = GameObject.FindWithTag("AIPlayer");
    arePlacesSwitched = false;
    Invoke("SwitchPlaces", Random.Range(40, 70));
  }

  void SwitchPlaces()
  {
    arePlacesSwitched = !arePlacesSwitched;

    // Switch locations.
    aiPlayer.GetComponent<NavMeshAgent>().enabled = false;
    Vector3 p = aiPlayer.transform.position;
    aiPlayer.transform.position = player.transform.position;
    player.transform.position = p;
    aiPlayer.GetComponent<NavMeshAgent>().enabled = true;

    // Change the globals for existing enemies.
    foreach (GameObject enemy in PlayerGlobals.Instance.enemies)
    {
      enemy.GetComponent<Enemy>().globals = arePlacesSwitched ? AIGlobals.Instance : PlayerGlobals.Instance;
    }

    foreach (GameObject enemy in AIGlobals.Instance.enemies)
    {
      enemy.GetComponent<Enemy>().globals = arePlacesSwitched ? PlayerGlobals.Instance : AIGlobals.Instance;
    }

    Invoke("SwitchPlaces", Random.Range(40, 70));
  }
}