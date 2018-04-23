using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LeaderboardController : MonoBehaviour
{
  private InputField usernameField;

  private Text LeaderboardDataSendError;

  private GameObject leaderboardScorePanel;

  public static LeaderboardController instance;

  void Start()
  {
    usernameField = GameObject.Find("UsernameField").GetComponent<InputField>();
    LeaderboardDataSendError = GameObject.Find("LeaderboardDataSendError").GetComponent<Text>();
    leaderboardScorePanel = GameObject.Find("SendLeaderboardScorePanel");
    leaderboardScorePanel.SetActive(false);
    instance = this;
  }

  // Update is called once per frame
  public void ShowPanel()
  {
    leaderboardScorePanel.SetActive(true);
  }

  public void SendButton()
  {
    string username = usernameField.text;
    if (username == "")
    {
      ShowErrorMessage("Username field is empty!");
      return;
    }

    StartCoroutine(SendData(username, Score.instance.blueScore));
  }

  public void CancelButton()
  {
    leaderboardScorePanel.SetActive(false);
  }

  IEnumerator SendData(string username, float score)
  {
    WWWForm form = new WWWForm();
    form.AddField("username", username);
    form.AddField("score", "" + score);

    using (UnityWebRequest www = UnityWebRequest.Post("http://nufflee.com/ld41/scores", form))
    {
      yield return www.SendWebRequest();

      if (www.isNetworkError || www.isHttpError)
      {
        ShowErrorMessage(www.error);
      }
      else
      {
        leaderboardScorePanel.SetActive(false);
      }
    }
  }

  void ShowErrorMessage(string message)
  {
    LeaderboardDataSendError.text = "Error: " + message;
  }
}