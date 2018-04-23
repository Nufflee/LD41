using UnityEngine;

public class Wave : MonoBehaviour
{
  public WaveEnemy[] enemies;

  public WaveManager waveManager;

  private Exchange exchange;

  private int aiEnemyCounter = 0;
  private int playerEnemyCounter = 0;

  void Awake()
  {
    exchange = Exchange.instance;
  }

  private void SpawnPlayerEnemy(WaveEnemy enemy)
  {
    bool switched = exchange.arePlacesSwitched;

    if (switched == false)
    {
      // Player Room
      GameObject playerRoomEnemy = Instantiate(waveManager.enemy, new Vector3(0, 25, 0), Quaternion.identity);
      playerRoomEnemy.GetComponent<Enemy>().globals = switched ? AIGlobals.Instance : PlayerGlobals.Instance;
      playerRoomEnemy.GetComponent<Enemy>().maxHealth = enemy.health;
      playerRoomEnemy.GetComponent<Enemy>().damage = enemy.damage;
      playerRoomEnemy.GetComponent<Enemy>().level = enemy.level;
      PlayerGlobals.Instance.enemies.Add(playerRoomEnemy);
      waveManager.enemies.Add(playerRoomEnemy);
    }
    else
    {
      // AI Room
      GameObject aiRoomEnemy = Instantiate(waveManager.aiEnemy, new Vector3(20, 25, 0), Quaternion.identity);
      aiRoomEnemy.GetComponent<Enemy>().globals = switched ? PlayerGlobals.Instance : AIGlobals.Instance;
      aiRoomEnemy.GetComponent<Enemy>().maxHealth = enemy.health;
      aiRoomEnemy.GetComponent<Enemy>().damage = enemy.damage;
      aiRoomEnemy.GetComponent<Enemy>().level = enemy.level;
      AIGlobals.Instance.enemies.Add(aiRoomEnemy);
      waveManager.enemies.Add(aiRoomEnemy);
    }
  }

  private void SpawnAIEnemy(WaveEnemy enemy)
  {
    bool switched = exchange.arePlacesSwitched;

    if (switched)
    {
      // Player Room
      GameObject playerRoomEnemy = Instantiate(waveManager.enemy, new Vector3(0, 25, 0), Quaternion.identity);
      playerRoomEnemy.GetComponent<Enemy>().globals = switched ? AIGlobals.Instance : PlayerGlobals.Instance;
      playerRoomEnemy.GetComponent<Enemy>().maxHealth = enemy.health;
      playerRoomEnemy.GetComponent<Enemy>().damage = enemy.damage;
      playerRoomEnemy.GetComponent<Enemy>().level = enemy.level;
      PlayerGlobals.Instance.enemies.Add(playerRoomEnemy);
      waveManager.enemies.Add(playerRoomEnemy);
    }
    else
    {
      // AI Room
      GameObject aiRoomEnemy = Instantiate(waveManager.aiEnemy, new Vector3(20, 25, 0), Quaternion.identity);
      aiRoomEnemy.GetComponent<Enemy>().globals = switched ? PlayerGlobals.Instance : AIGlobals.Instance;
      aiRoomEnemy.GetComponent<Enemy>().maxHealth = enemy.health;
      aiRoomEnemy.GetComponent<Enemy>().damage = enemy.damage;
      aiRoomEnemy.GetComponent<Enemy>().level = enemy.level;
      AIGlobals.Instance.enemies.Add(aiRoomEnemy);
      waveManager.enemies.Add(aiRoomEnemy);
    }
  }

  public bool SpawnNextPlayerEnemy()
  {
    if (playerEnemyCounter >= enemies.Length) return false;

    SpawnPlayerEnemy(enemies[playerEnemyCounter]);
    playerEnemyCounter++;

    return true;
  }

  public bool SpawnNextAIEnemy()
  {
    if (aiEnemyCounter >= enemies.Length) return false;

    SpawnAIEnemy(enemies[aiEnemyCounter]);
    aiEnemyCounter++;

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