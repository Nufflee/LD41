using UnityEngine;

public class Wave : MonoBehaviour
{
  public WaveEnemy[] enemies;

  public WaveManager waveManager;

  private Exchange exchange;

  private int currentEnemyCounter = 0;

  void Awake()
  {
    exchange = Exchange.instance;
  }

  private void SpawnEnemy(WaveEnemy enemy)
  {
    bool switched = exchange.arePlacesSwitched;

    // Player Room
    GameObject playerRoomEnemy = Instantiate(waveManager.enemy, new Vector3(0, 25, 0), Quaternion.identity);
    playerRoomEnemy.GetComponent<Enemy>().globals = switched ? AIGlobals.Instance : PlayerGlobals.Instance;
    playerRoomEnemy.GetComponent<Enemy>().maxHealth = enemy.health;
    playerRoomEnemy.GetComponent<Enemy>().damage = enemy.damage;
/*    Color color;
    ColorUtility.TryParseHtmlString("#005aff", out color);
    playerRoomEnemy.GetComponent<Renderer>().material.color = color;*/
    PlayerGlobals.Instance.enemies.Add(playerRoomEnemy);
    waveManager.enemies.Add(playerRoomEnemy);

    // AI Room
    GameObject aiRoomEnemy = Instantiate(waveManager.aiEnemy, new Vector3(20, 25, 0), Quaternion.identity);
    aiRoomEnemy.GetComponent<Enemy>().globals = switched ? PlayerGlobals.Instance : AIGlobals.Instance;
    aiRoomEnemy.GetComponent<Enemy>().maxHealth = enemy.health;
    aiRoomEnemy.GetComponent<Enemy>().damage = enemy.damage;
/*    ColorUtility.TryParseHtmlString("#ff5a00", out color);
    playerRoomEnemy.GetComponent<Renderer>().material.color = color;*/
    AIGlobals.Instance.enemies.Add(aiRoomEnemy);
    waveManager.enemies.Add(aiRoomEnemy);
  }

  public bool SpawnNextEnemy()
  {
    if (currentEnemyCounter >= enemies.Length) return false;

    SpawnEnemy(enemies[currentEnemyCounter]);
    currentEnemyCounter++;

    return true;
  }
}

public class WaveEnemy
{
  public float damage;
  public float health;
  public Color color;
  public int level;
}