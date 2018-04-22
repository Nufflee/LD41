using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MMWaveManager : MonoBehaviour
{
  private Text waveText;
  private int waveNumber = 1;
  private bool waveInProgress;
  private bool waveGenerating;

  private GameObject enemy;

  public List<GameObject> enemies;

  public static MMWaveManager Instance;

  private void Start()
  {
    Instance = this;
    enemy = Resources.Load<GameObject>("MainMenuPrefabs/MainMenuEnemy");
  }

  private void Update()
  {
    if (enemies.Count == 0 && waveNumber > 1)
    {
      waveInProgress = false;
    }

    if (waveInProgress == false && !waveGenerating)
    {
      waveGenerating = true;
      waveInProgress = true;

      StartCoroutine(SpawnWave());
    }
  }

  private IEnumerator SpawnWave()
  {
    yield return new WaitForSeconds(2.0f);

    float multiplier = waveNumber / 30.0f + 1;
    int enemyCount = (int) Random.Range(3 * multiplier, 6 * multiplier);

    for (int i = 0; i < enemyCount; i++)
    {
      SpawnEnemy();

      yield return new WaitForSeconds(8.5f * multiplier / enemyCount);
    }

    waveNumber++;
    waveGenerating = false;
  }

  private void SpawnEnemy()
  {
    // Player Room
    GameObject playerRoomEnemy = Instantiate(enemy, new Vector3(0, 25, 0), Quaternion.identity);
    playerRoomEnemy.GetComponent<MMEnemy>().globals = PlayerGlobals.Instance;

    enemies.Add(playerRoomEnemy);
    // AI Room
    GameObject aiRoomEnemy = Instantiate(enemy, new Vector3(20, 25, 0), Quaternion.identity);
    aiRoomEnemy.GetComponent<MMEnemy>().globals = AIGlobals.Instance;

    enemies.Add(aiRoomEnemy);
  }
}