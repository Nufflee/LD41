using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
  private Text waveText;
  private GameObject crosshair;
  public GameObject enemy;
  private int waveNumber = 1;
  private bool waveInProgress;
  private bool waveJustEnded;
  private bool noAnimationProgressing = true;

  private Wave wave;

  private void Start()
  {
    waveText = transform.Find("WaveText").GetComponent<Text>();
    crosshair = transform.Find("Crosshair").gameObject;
    enemy = Resources.Load<GameObject>("Prefabs/Enemy");
    
  }

  private void GenerateWave() {
    int minEnemies = waveNumber, maxEnemies = waveNumber + waveNumber / 2;
    int enemiesCount = (int)Random.Range(minEnemies, maxEnemies);
    List<WaveEnemy> enemies = new List<WaveEnemy>();
    for(int i = 0;i<enemiesCount;i++) {
      WaveEnemy enemyType = new WaveEnemy();
      enemyType.health = 100f + (waveNumber > 10 ? waveNumber * 0.75f : waveNumber * 0.35f);
      enemyType.damage = 0.023f + (waveNumber > 5 ? waveNumber * 0.045f : waveNumber * 0.01f);
      enemyType.color = new Color(waveNumber*.35f, waveNumber*.75f, waveNumber*.4f);
      enemyType.level = waveNumber;
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
      print("WAVE "+waveNumber+" NOT IN POGRESS!");
    }

    if (Input.GetKeyDown(KeyCode.F) && waveInProgress == false && noAnimationProgressing)
    {
      GenerateWave();
      crosshair.SetActive(false);

      waveText.enabled = true;
      waveText.text = "WAVE " + waveNumber;

      waveInProgress = true;
      print("STARTING WAVE "+waveNumber);
      StartCoroutine(SpawnWave());
      waveNumber++;
    }
  }

  public IEnumerator SpawnWave() {
        print("U CALLED THE MASTER!");
        wave.SpawnNextEnemy();
        noAnimationProgressing = false;
        waveText.color = new Color(waveText.color.r, waveText.color.g, waveText.color.b, 0.0f);
  	    print("COLOR SET UP");
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
        noAnimationProgressing = true;

        waveText.enabled = false;
        crosshair.SetActive(true);

        while(wave.SpawnNextEnemy()) {
          yield return new WaitForSeconds(1f);
        }
        print("DONE");
  }

  public IEnumerator WaveEndText()
  {
    noAnimationProgressing = false;
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
    noAnimationProgressing = true;
  }

}