using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
  private Text waveText;
  private GameObject crosshair;
  public GameObject enemy;
  public GameObject aiEnemy;
  public int waveNumber = 1;
  public int aiWaveNumber = 1;
  private bool waveInProgress;
  public bool aiWaveInProgress;
  private bool waveJustEnded;

  private Wave wave;
  public List<GameObject> enemies;

  private void Awake()
  {
    waveText = transform.Find("WaveText").GetComponent<Text>();
    crosshair = transform.Find("Crosshair").gameObject;
    enemy = Resources.Load<GameObject>("Prefabs/Enemy");
    aiEnemy = Resources.Load<GameObject>("Prefabs/AIEnemy");
    enemies = new List<GameObject>();
  }

  private void GenerateWave(int waveNumber)
  {
    int minEnemies = waveNumber, maxEnemies = waveNumber + waveNumber / 2;
    int enemiesCount = (int) Random.Range(minEnemies, maxEnemies);
    List<WaveEnemy> enemies = new List<WaveEnemy>();
    for (int i = 0; i < enemiesCount; i++)
    {
      WaveEnemy enemyType = new WaveEnemy
      {
        health = 100f + (waveNumber > 10 ? waveNumber * 0.75f : waveNumber * 0.35f),
        damage = 0.023f + (waveNumber > 5 ? waveNumber * 0.045f : waveNumber * 0.01f),
        color = new Color(waveNumber * .35f, waveNumber * .75f, waveNumber * .4f),
        level = Mathf.FloorToInt(waveNumber / 1.5f)
      };
      enemies.Add(enemyType);
    }

    wave = gameObject.AddComponent<Wave>();
    wave.enemies = enemies.ToArray();
    wave.waveManager = this;
  }

  private void Update()
  {
    if (waveInProgress == false && waveNumber > 1 && waveJustEnded)
    {
      crosshair.SetActive(false);
      waveJustEnded = false;
      waveText.enabled = true;
      waveText.text = "WAVE ENDED";
      StartCoroutine(WaveEndText());
    }

    if ((Exchange.instance.arePlacesSwitched ? AIGlobals.Instance.enemies : PlayerGlobals.Instance.enemies).Count == 0 && waveInProgress)
    {
      waveInProgress = false;
      waveJustEnded = true;
    }

    if ((Exchange.instance.arePlacesSwitched ? PlayerGlobals.Instance.enemies : AIGlobals.Instance.enemies).Count == 0 && aiWaveInProgress)
    {
      aiWaveInProgress = false;
    }

    if (Input.GetKeyDown(KeyCode.F) && waveInProgress == false && waveText.enabled == false)
    {
      GenerateWave(waveNumber);
      crosshair.SetActive(false);

      waveText.enabled = true;
      waveText.text = "WAVE " + waveNumber;

      enemies.ForEach(enemy =>
      {
        if (enemy.GetComponent<Enemy>().isDead && enemy.GetComponent<Enemy>().globals == (Exchange.instance.arePlacesSwitched ? AIGlobals.Instance : PlayerGlobals.Instance))
        {
          Destroy(enemy);
        }
      });

      waveInProgress = true;
      StartCoroutine(SpawnPlayerWave());
      waveNumber++;
    }
  }

  public void SpawnAIWave()
  {
    aiWaveInProgress = true;

    enemies.ForEach(enemy =>
    {
      if (enemy.GetComponent<Enemy>().isDead && enemy.GetComponent<Enemy>().globals == (Exchange.instance.arePlacesSwitched ? PlayerGlobals.Instance : AIGlobals.Instance))
      {
        Destroy(enemy);
      }
    });

    GenerateWave(aiWaveNumber);
    StartCoroutine(SpawnAIWaveCoroutine());
    aiWaveNumber++;
  }

  private IEnumerator SpawnPlayerWave()
  {
    wave.SpawnNextPlayerEnemy();
    waveText.color = new Color(waveText.color.r, waveText.color.g, waveText.color.b, 0.0f);
    while (waveText.color.a < 1.0f)
    {
      waveText.color = new Color(waveText.color.r, waveText.color.g, waveText.color.b, waveText.color.a + 0.03f);

      yield return null;
    }

    yield return new WaitForSeconds(1.0f);

    while (waveText.color.a > 0.0f)
    {
      waveText.color = new Color(waveText.color.r, waveText.color.g, waveText.color.b, waveText.color.a - 0.03f);

      yield return null;
    }

    waveText.enabled = false;
    crosshair.SetActive(true);

    while (wave.SpawnNextPlayerEnemy())
    {
      yield return new WaitForSeconds(1f);
    }
  }

  private IEnumerator SpawnAIWaveCoroutine()
  {
    wave.SpawnNextAIEnemy();

    yield return new WaitForSeconds(1.0f);

    while (wave.SpawnNextAIEnemy())
    {
      yield return new WaitForSeconds(1f);
    }
  }

  public IEnumerator WaveEndText()
  {
    waveText.color = new Color(waveText.color.r, waveText.color.g, waveText.color.b, 0.0f);

    while (waveText.color.a < 1.0f)
    {
      waveText.color = new Color(waveText.color.r, waveText.color.g, waveText.color.b, waveText.color.a + 0.03f);

      yield return null;
    }

    yield return new WaitForSeconds(1.0f);

    while (waveText.color.a > 0.0f)
    {
      waveText.color = new Color(waveText.color.r, waveText.color.g, waveText.color.b, waveText.color.a - 0.03f);

      yield return null;
    }

    waveText.enabled = false;
    crosshair.SetActive(true);
  }
}