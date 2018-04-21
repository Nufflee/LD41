using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
  private Text waveText;
  private GameObject crosshair;
  private GameObject enemy;
  private int waveNumber = 1;
  private bool waveInProgress;
  [HideInInspector] public List<GameObject> enemies = new List<GameObject>();

  private void Start()
  {
    waveText = transform.Find("WaveText").GetComponent<Text>();
    crosshair = transform.Find("Crosshair").gameObject;
    enemy = Resources.Load<GameObject>("Prefabs/Enemy");
  }

  private void Update()
  {
    if (enemies.Count == 0 && waveNumber > 1)
    {
      waveInProgress = false;
    }

    if (Input.GetKeyDown(KeyCode.F) && waveInProgress == false)
    {
      crosshair.SetActive(false);

      waveText.enabled = true;
      waveText.text = "Wave " + waveNumber;

      waveInProgress = true;

      StartCoroutine(SpawnWave());
    }
  }

  private IEnumerator SpawnWave()
  {
    yield return new WaitForSeconds(2.0f);

    waveText.enabled = false;
    crosshair.SetActive(true);

    float multiplier = waveNumber / 15.0f + 1;
    int enemyCount = (int) Random.Range(3 * multiplier, 6 * multiplier);

    print(enemyCount);

    for (int i = 0; i < enemyCount; i++)
    {
      SpawnEnemy();

      yield return new WaitForSeconds(4.5f * multiplier / enemyCount);
    }

    waveNumber++;
  }

  private void SpawnEnemy()
  {
    GameObject gameObject = Instantiate(enemy, new Vector3(5, 25, 5), Quaternion.identity);
    gameObject.GetComponent<Enemy>().globals = PlayerGlobals.Instance;

    enemies.Add(gameObject);
  }
}