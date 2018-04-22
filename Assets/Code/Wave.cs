using System.Collections;
using System.Collections.Generic;
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
    playerRoomEnemy.GetComponent<Enemy>().color = enemy.color;
    PlayerGlobals.Instance.enemies.Add(playerRoomEnemy);

    // AI Room
    GameObject aiRoomEnemy = Instantiate(waveManager.enemy, new Vector3(20, 25, 0), Quaternion.identity);
    aiRoomEnemy.GetComponent<Enemy>().globals = switched ? PlayerGlobals.Instance : AIGlobals.Instance;
    aiRoomEnemy.GetComponent<Enemy>().maxHealth = enemy.health;
    aiRoomEnemy.GetComponent<Enemy>().damage = enemy.damage;
    aiRoomEnemy.GetComponent<Enemy>().color = enemy.color;
    AIGlobals.Instance.enemies.Add(aiRoomEnemy);
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