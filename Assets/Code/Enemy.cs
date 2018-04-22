using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
  public Globals globals;

  private NavMeshAgent agent;
  private Rigidbody rigidbody;

  private Transform healthBar;

  private float engageDistance = 5.0f;
  private bool engaged;
  private bool isDead;

  public float maxHealth;
  private float health;
  public float damage;
  public int level;
  private GameObject ammoPickupablePrefab;

  // Use this for initialization
  private void Start()
  {
    if (globals == null)
    {
      Debug.LogError("You forgot to set globals!");

      return;
    }

    health = maxHealth;
    healthBar = transform.GetChild(0);
    transform.GetChild(0).Find("LevelText").GetComponent<Text>().text = "" + (level + 1);
    ammoPickupablePrefab = Resources.Load<GameObject>("Prefabs/PU_Ammo");
    agent = GetComponent<NavMeshAgent>();
    rigidbody = GetComponent<Rigidbody>();
  }

  // Update is called once per frame
  private void Update()
  {
    if (globals.Target == null || isDead) return;
    healthBar.LookAt(PlayerGlobals.Instance.Target.transform);
    healthBar.eulerAngles = new Vector3(0f, healthBar.eulerAngles.y, 0f);

    if (transform.position.y < 2.0f)
    {
      agent.enabled = true;
    }

    if (agent.isOnNavMesh == false) return;

    agent.baseOffset = 2.75f + Mathf.Sin(Time.time * 4.0f) / 26.0f;

    float distance = Vector3.Distance(agent.transform.position, globals.Target.transform.position);

    if (distance < 2.0f)
    {
      if (globals.Target.GetComponent<Player>() != null)
      {
        globals.Target.GetComponent<Player>().Damage(damage);
      }
      else
      {
        globals.Target.GetComponent<AIController>().Damage(damage);
      }

      return;
    }

    if (distance > engageDistance && agent.velocity.magnitude <= 0.001f)
    {
      // Wander

      Renderer groundRenderer = globals.Ground.GetComponent<Renderer>();

      NavMeshHit hit;

      NavMesh.SamplePosition(new Vector3(transform.position.x + Random.Range(-groundRenderer.bounds.extents.x, groundRenderer.bounds.extents.x), 1, transform.position.z + Random.Range(-groundRenderer.bounds.extents.z, groundRenderer.bounds.extents.z)), out hit, 2.0f, NavMesh.AllAreas);

      Vector3 wanderPosition = hit.position;
      // = new Vector3(transform.position.x + Random.Range(-groundRenderer.bounds.extents.x, groundRenderer.bounds.extents.x), 1, transform.position.z + Random.Range(-groundRenderer.bounds.extents.z, groundRenderer.bounds.extents.z));

      agent.SetDestination(wanderPosition);
    }
    else if (distance <= engageDistance || engaged)
    {
      // Set the agent's position to the player's position.
      agent.SetDestination(globals.Target.transform.position);

      // Look at the player.
      transform.LookAt(globals.Target.transform);
      transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z);
    }
  }

  public void Damage(float damage)
  {
    if (isDead) return;

    health -= damage;

    engaged = true;

    if (health <= 0)
    {
      rigidbody.isKinematic = true;
      agent.enabled = false;
      rigidbody.isKinematic = false;
      transform.Find("Torus_000").GetComponent<Renderer>().material.SetFloat("_Glow", 1);
      transform.Find("Torus_002").GetComponent<Renderer>().material.SetFloat("_Glow", 1);
      isDead = true;

      PlayerGlobals.Instance.enemies.Remove(gameObject);
      AIGlobals.Instance.enemies.Remove(gameObject);

      if (Random.Range(0, 2) == 0)
      {
        GameObject pickupable = Instantiate(ammoPickupablePrefab, new Vector3(transform.position.x + 1.5f, transform.position.y, transform.position.z), transform.rotation);

        AIGlobals.Instance.pickupables.Add(pickupable);

        Destroy(pickupable, 20.0f);
      }


      Destroy(gameObject, 15.0f);
    }

    healthBar.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = health / maxHealth;
  }

  private void OnDestroy()
  {
    PlayerGlobals.Instance.enemies.Remove(gameObject);
    AIGlobals.Instance.enemies.Remove(gameObject);
  }
}