using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.UI;

public class AIController : MonoBehaviour
{
  public NavMeshAgent agent { get; private set; }
  private Transform target;
  [SerializeField] public float health = 100f;
  [SerializeField] private float healthRegen = 0.003f;
  private bool preparingForWave;

  private AIGun aiGun;

  private void Start()
  {
    agent = GetComponent<NavMeshAgent>();
    aiGun = transform.GetChild(0).GetChild(0).GetComponent<AIGun>();

    agent.updateRotation = false;
    agent.updatePosition = true;
    AIGlobals.Instance.WaveManager.SpawnAIWave();
  }

  private void Update()
  {
    // Regenerate AI's Health.
    if (health < 100)
    {
      health += healthRegen;
    }

    if (AIGlobals.Instance.WaveManager.aiWaveInProgress == false && preparingForWave == false)
    {
      StartCoroutine(PrepareForWave());
    }

    // If there is no target and agent is unable to move, allow him to move.
    if (target == null && agent.isStopped) agent.isStopped = false;

    if (aiGun.isReloading && agent.speed != 1.8f)
    {
      agent.speed = 1.8f;
    }
    else if (aiGun.isReloading == false && agent.speed != 3.5f)
    {
      agent.speed = 3.5f;
    }

    // Get a target (closest enemy).
    target = GetClosestEnemy();

    // If we don't have a target, we wander for enemies.
    if (target == null && agent.velocity.magnitude <= 0.001f)
    {
      // If we have pickupables, go to them.
      if (GetClosestPickupable() != null)
      {
        agent.SetDestination(GetClosestPickupable().position);
        return;
      }

      // Get current AI's ground renderer.
      Renderer groundRenderer = (Exchange.instance.arePlacesSwitched ? PlayerGlobals.Instance : AIGlobals.Instance).Ground.GetComponent<Renderer>();

      NavMeshHit hit;

      NavMesh.SamplePosition(new Vector3(transform.position.x + Random.Range(-groundRenderer.bounds.extents.x, groundRenderer.bounds.extents.x), 1, transform.position.z + Random.Range(-groundRenderer.bounds.extents.z, groundRenderer.bounds.extents.z)), out hit, 2.0f, NavMesh.AllAreas);

      //  Get a position to wander.
      Vector3 wanderPosition = hit.position; // = new Vector3(transform.position.x + Random.Range(-groundRenderer.bounds.extents.x, groundRenderer.bounds.extents.x), 1, transform.position.z + Random.Range(-groundRenderer.bounds.extents.z, groundRenderer.bounds.extents.z));

      // Go to the wander position.
      agent.SetDestination(wanderPosition);

      return;
    }

    // If we don't have a target and we won't wander, we shouldn't do anything.
    if (target == null)
    {
      return;
    }

    float distance = Vector3.Distance(agent.transform.position, target.transform.position);

    // If we are close to our target, we need to start shooting.
    if (distance < 8.0f)
    {
      // Stop te agent.
      agent.isStopped = true;

      // Look at the target.
      transform.LookAt(target);

      // Start shooting.
      aiGun.Shoot();
    }
    else if (agent.velocity.magnitude <= 0.001f || agent.isOnNavMesh == false)
    {
      agent.isStopped = false;
      agent.SetDestination(target.position);
    }
  }

  private IEnumerator PrepareForWave()
  {
    preparingForWave = true;

    yield return new WaitForSeconds(Random.Range(3, 7));

    AIGlobals.Instance.WaveManager.SpawnAIWave();

    preparingForWave = false;
  }

  Transform GetClosestEnemy()
  {
    Transform closestEnemy = null;
    float minDist = Mathf.Infinity;
    Vector3 currentPos = transform.position;

    // Get if the places of exchange are switched.
    bool arePlacesSwitched = Exchange.instance && Exchange.instance.arePlacesSwitched;

    // Get enemies from the required global (AI or Player Global).
    List<GameObject> enemies = arePlacesSwitched ? PlayerGlobals.Instance.enemies : AIGlobals.Instance.enemies;

    foreach (GameObject enemy in enemies)
    {
      if (enemy == null) continue;

      float dist = Vector3.Distance(enemy.transform.position, currentPos);
      if (dist < minDist)
      {
        closestEnemy = enemy.transform;
        minDist = dist;
      }
    }

    return closestEnemy;
  }

  Transform GetClosestPickupable()
  {
    Transform closestPickupable = null;
    float minDist = Mathf.Infinity;
    Vector3 currentPos = transform.position;

    List<GameObject> pickupables = AIGlobals.Instance.pickupables;

    foreach (GameObject pickupable in pickupables)
    {
      if (pickupable == null) continue;

      float dist = Vector3.Distance(pickupable.transform.position, currentPos);
      if (dist < minDist)
      {
        closestPickupable = pickupable.transform;
        minDist = dist;
      }
    }

    return closestPickupable;
  }

  public void Damage(float damage)
  {
    health -= damage;

    if (health <= 0)
    {
      Destroy(gameObject);
      // TODO: Death
    }

    if (health > 100)
    {
      health = 100;
    }
  }
}