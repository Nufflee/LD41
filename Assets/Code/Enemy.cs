using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
  public Globals globals;

  private NavMeshAgent agent;

  private Transform healthBar;

  private float engageDistance = 5.0f;
  private bool engaged;

  public float maxHealth;
  private float health;
  public float damage;
  public int level;
  public Color32 color;

  // Use this for initialization
  private void Start()
  {
    if (globals == null)
    {
      Debug.LogError("You forgot to set globals!");

      return;
    }
    health = maxHealth;
    GetComponent<Renderer>().material.color = color;
    healthBar = transform.GetChild(0);
    transform.GetChild(0).Find("LevelText").GetComponent<Text>().text = ""+(level+1);
    agent = GetComponent<NavMeshAgent>();
  }

  // Update is called once per frame
  private void Update()
  {
    /*if(globals.name == PlayerGlobals.Instance.name) {
      GetComponent<Renderer>().material.color = Color.red;
    } else {
      GetComponent<Renderer>().material.color = Color.blue;
    }*/
    if (globals.Target == null) return;
    healthBar.LookAt(PlayerGlobals.Instance.Target.transform);
    healthBar.eulerAngles = new Vector3(0f, healthBar.eulerAngles.y, 0f);

    if (transform.position.y < 2.0f)
    {
      agent.enabled = true;
    }

    if (agent.isOnNavMesh == false) return;

    agent.baseOffset = 0.75f + Mathf.Sin(Time.time * 4.0f) / 26.0f;

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

      NavMesh.SamplePosition(new Vector3(Random.Range(-groundRenderer.bounds.extents.x, groundRenderer.bounds.extents.x), 1, Random.Range(-groundRenderer.bounds.extents.z, groundRenderer.bounds.extents.z)), out hit, 1.0f, NavMesh.AllAreas);

      Vector3 wanderPosition = new Vector3(transform.position.x + Random.Range(-groundRenderer.bounds.extents.x, groundRenderer.bounds.extents.x), 1, transform.position.z + Random.Range(-groundRenderer.bounds.extents.z, groundRenderer.bounds.extents.z));

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
    health -= damage;

    engaged = true;

    if (health <= 0)
    {
      Destroy(gameObject);
    }

    healthBar.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = health / maxHealth;
  }

  private void OnDestroy()
  {
    PlayerGlobals.Instance.enemies.Remove(gameObject);
    AIGlobals.Instance.enemies.Remove(gameObject);
  }
}