using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.UI;

public class AIController : MonoBehaviour
{
  public UnityEngine.AI.NavMeshAgent agent { get; private set; }

  private Transform target;
  private float health = 100;

  private void Start()
  {
    agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

    agent.updateRotation = false;
    agent.updatePosition = true;
  }

  private void Update()
  {
    if (health < 100)
    {
      health += 0.003f;
    }

    print("ai: " + health);
  }

  private void FixedUpdate()
  {
    if (target == null && agent.isStopped) agent.isStopped = false;

    // Get a target.
    target = GetClosestEnemy();

    // If we don't have a target, don't do anything.
    if (target == null && agent.velocity.magnitude <= 0.001f)
    {
      // Wander

      Renderer groundRenderer = AIGlobals.Instance.Ground.GetComponent<Renderer>();

      NavMeshHit hit;

      NavMesh.SamplePosition(new Vector3(Random.Range(-groundRenderer.bounds.extents.x, groundRenderer.bounds.extents.x), 1, Random.Range(-groundRenderer.bounds.extents.z, groundRenderer.bounds.extents.z)), out hit, 1.0f, NavMesh.AllAreas);

      Vector3 wanderPosition = new Vector3(transform.position.x + Random.Range(-groundRenderer.bounds.extents.x, groundRenderer.bounds.extents.x), 1, transform.position.z + Random.Range(-groundRenderer.bounds.extents.z, groundRenderer.bounds.extents.z));

      agent.SetDestination(wanderPosition);

      return;
    }

    if (target == null)
    {
      return;
    }

    float distance = Vector3.Distance(agent.transform.position, target.transform.position);

    // If we are close to our target, we need to start shooting.
    if (distance < 8.0f)
    {
      agent.isStopped = true;

      // Look at the target.
      transform.LookAt(target);

      // Start shooting.
      transform.GetChild(0).GetChild(0).GetComponent<AIGun>().Shoot();
    }
    else if (agent.velocity.magnitude <= 0.001f)
    {
      // Wander

      Renderer groundRenderer = AIGlobals.Instance.Ground.GetComponent<Renderer>();

      NavMeshHit hit;

      NavMesh.SamplePosition(new Vector3(Random.Range(-groundRenderer.bounds.extents.x, groundRenderer.bounds.extents.x), 1, Random.Range(-groundRenderer.bounds.extents.z, groundRenderer.bounds.extents.z)), out hit, 1.0f, NavMesh.AllAreas);

      Vector3 wanderPosition = new Vector3(transform.position.x + Random.Range(-groundRenderer.bounds.extents.x, groundRenderer.bounds.extents.x), 1, transform.position.z + Random.Range(-groundRenderer.bounds.extents.z, groundRenderer.bounds.extents.z));

      agent.SetDestination(wanderPosition);
    }
  }

  Transform GetClosestEnemy()
  {
    Transform tMin = null;
    float minDist = Mathf.Infinity;
    Vector3 currentPos = transform.position;
    bool arePlacesSwitched = Exchange.instance ? Exchange.instance.arePlacesSwitched : false;
    List<GameObject> enemies = arePlacesSwitched ? PlayerGlobals.Instance.enemies : AIGlobals.Instance.enemies;
    foreach (GameObject t in enemies)
    {
      float dist = Vector3.Distance(t.transform.position, currentPos);
      if (dist < minDist)
      {
        tMin = t.transform;
        minDist = dist;
      }
    }

    return tMin;
  }

  public void Damage(float damage)
  {
    health -= damage;

    GameObject.Find("AIHealthBar").transform.GetChild(0).GetComponent<Image>().fillAmount = (health / 100f);

    if (health <= 0)
    {
      Destroy(gameObject);
      // TODO: Death
    }
  }
}