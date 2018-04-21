using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
  public Globals globals;

  private NavMeshAgent agent;
  private GameObject target;

  private Transform healthBar;

  private float engageDistance = 5.0f;
  private bool engaged;

  private int health = 100;

  // Use this for initialization
  private void Start()
  {
    if (globals == null)
    {
      Debug.LogError("You forgot to set globals!");

      return;
    }

    target = globals.Target;
    healthBar = transform.GetChild(0);
    agent = GetComponent<NavMeshAgent>();
  }

  // Update is called once per frame
  private void Update()
  {
    if (target == null) return;

    healthBar.LookAt(target.transform);
    healthBar.eulerAngles = new Vector3(0f, healthBar.eulerAngles.y, 0f);

    if (transform.position.y < 2.0f)
    {
      agent.enabled = true;
    }

    if (agent.isOnNavMesh == false) return;

    agent.baseOffset = 0.75f + Mathf.Sin(Time.time * 4.0f) / 26.0f;

    float distance = Vector3.Distance(agent.transform.position, target.transform.position);

    if (target == AIGlobals.Instance.Target)
    {
      print(distance);
    }

    if (distance < 2.0f)
    {
      target.GetComponent<Player>().Damage(0.06f);

      return;
    }

    if (distance > engageDistance && agent.velocity.magnitude <= 0.001f)
    {
      // Wander

      Renderer groundRenderer = globals.Ground.GetComponent<Renderer>();

      NavMeshHit hit;

      NavMesh.SamplePosition(new Vector3(Random.Range(-groundRenderer.bounds.extents.x, groundRenderer.bounds.extents.x), 1, Random.Range(-groundRenderer.bounds.extents.z, groundRenderer.bounds.extents.z)), out hit, 1.0f, NavMesh.AllAreas);

      Vector3 wanderPosition = hit.position;

      agent.SetDestination(wanderPosition);
    }
    else if (distance <= engageDistance || engaged)
    {
      // Set the agent's position to the player's position.
      agent.SetDestination(target.transform.position);

      // Look at the player.
      transform.LookAt(target.transform);
      transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z);
    }
  }

  public void Damage(int damage)
  {
    health -= damage;

    engaged = true;

    if (health <= 0)
    {
      Destroy(gameObject);
    }

    healthBar.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = health / 100f;
  }

  private void OnDestroy()
  {
    globals.WaveManager.aiEnemies.Remove(gameObject);
    globals.WaveManager.playerEnemies.Remove(gameObject);
  }
}