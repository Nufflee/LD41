using System.Collections;
using DigitalRuby.Tween;
using UnityEngine;
using UnityEngine.UI;

public class Statistics : MonoBehaviour
{
  public static Statistics instance;

  public Statistic player = new Statistic();
  public Statistic ai = new Statistic();

  private void Awake()
  {
    instance = this;
  }

  public void Show()
  {
    Transform aiPanel = transform.Find("AIStatisticsPanel");
    aiPanel.Find("ScoreText").GetComponent<Text>().text = ai.score.ToString();
    aiPanel.Find("TimeAliveText").GetComponent<Text>().text = ai.timeAlive;
    aiPanel.Find("AmmoPickedUpText").GetComponent<Text>().text = ai.ammoPickedUp.ToString();
    aiPanel.Find("AmmoUsedText").GetComponent<Text>().text = ai.ammoSpent.ToString();
    aiPanel.Find("DamageDealtText").GetComponent<Text>().text = ai.damageDealt.ToString();
    aiPanel.Find("WaveNumberText").GetComponent<Text>().text = (AIGlobals.Instance.WaveManager.aiWaveNumber - 1).ToString();

    Transform playerPanel = transform.Find("PlayerStatisticsPanel");
    playerPanel.Find("ScoreText").GetComponent<Text>().text = player.score.ToString();
    playerPanel.Find("TimeAliveText").GetComponent<Text>().text = player.timeAlive;
    playerPanel.Find("AmmoPickedUpText").GetComponent<Text>().text = player.ammoPickedUp.ToString();
    playerPanel.Find("AmmoUsedText").GetComponent<Text>().text = player.ammoSpent.ToString();
    playerPanel.Find("DamageDealtText").GetComponent<Text>().text = player.damageDealt.ToString();
    playerPanel.Find("WaveNumberText").GetComponent<Text>().text = (player.waveNumber - 1).ToString();

    Vector3 aiStartPosition = aiPanel.localPosition;
    aiPanel.gameObject.Tween("", aiStartPosition, new Vector3(-281.24f, -59.767f, 0.125f), 1.0f, TweenScaleFunctions.EaseOutBounce, (t) => { aiPanel.localPosition = t.CurrentValue; });

    Vector3 playerStartPosition = playerPanel.localPosition;
    playerPanel.gameObject.Tween("a", playerStartPosition, new Vector3(-25.2f, -59.763f, 0.125f), 1.0f, TweenScaleFunctions.EaseOutBounce, (t) => { playerPanel.localPosition = t.CurrentValue; });
  }
}

public struct Statistic
{
  public int enemiesKilled;
  public string timeAlive;
  public int ammoPickedUp;
  public int healthPickedUp;
  public int ammoSpent;
  public int healthSpent;
  public int damageDealt;
  public int waveNumber;
  public int score;
}