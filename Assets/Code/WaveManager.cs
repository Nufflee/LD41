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
  private bool waveJustEnded;

  private void Start()
  {
    waveText = transform.Find("WaveText").GetComponent<Text>();
    crosshair = transform.Find("Crosshair").gameObject;
    enemy = Resources.Load<GameObject>("Prefabs/Enemy");
  }

  private void Update()
  {
    if ((Exchange.instance.arePlacesSwitched ? AIGlobals.Instance.enemies : PlayerGlobals.Instance.enemies).Count == 0 && waveJustEnded && waveInProgress == false)
    {
      waveJustEnded = false;

      crosshair.SetActive(false);

      waveText.enabled = true;
      waveText.text = "WAVE ENDED";

      StartCoroutine(WaveEndText());
    }

    if ((Exchange.instance.arePlacesSwitched ? AIGlobals.Instance.enemies : PlayerGlobals.Instance.enemies).Count == 0 && waveNumber > 1)
    {
      waveInProgress = false;
    }

    if (Input.GetKeyDown(KeyCode.F) && waveInProgress == false && waveText.enabled == false)
    {
      crosshair.SetActive(false);

      waveText.enabled = true;
      waveText.text = "WAVE " + waveNumber;

      waveInProgress = true;

      StartCoroutine(SpawnWave());
    }
  }

  private IEnumerator SpawnWave()
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

    float multiplier = waveNumber / 15.0f + 1;
    int enemyCount = (int) Random.Range(3 * multiplier, 6 * multiplier);

    for (int i = 0; i < enemyCount; i++)
    {
      SpawnEnemy();

      yield return new WaitForSeconds(4.5f * multiplier / enemyCount);
    }

    waveNumber++;
    waveJustEnded = true;
  }

  private IEnumerator WaveEndText()
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

  private void SpawnEnemy()
  {
    bool switched = FindObjectOfType<Exchange>().arePlacesSwitched;

    // Player Room
    GameObject playerRoomEnemy = Instantiate(enemy, new Vector3(0, 25, 0), Quaternion.identity);
    playerRoomEnemy.GetComponent<Enemy>().globals = switched ? AIGlobals.Instance : PlayerGlobals.Instance;

    PlayerGlobals.Instance.enemies.Add(playerRoomEnemy);

    // AI Room
    GameObject aiRoomEnemy = Instantiate(enemy, new Vector3(20, 25, 0), Quaternion.identity);
    aiRoomEnemy.GetComponent<Enemy>().globals = switched ? PlayerGlobals.Instance : AIGlobals.Instance;

    AIGlobals.Instance.enemies.Add(aiRoomEnemy);
  }
}