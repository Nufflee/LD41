using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
  private Text orangeText;
  private Text blueText;

  public static Score instance;

  private void Awake()
  {
    instance = this;
  }

  private void Start()
  {
    orangeText = transform.Find("OrangeText").GetComponent<Text>();
    blueText = transform.Find("BlueText").GetComponent<Text>();
  }

  public void OrangeScore(int count)
  {
    orangeText.text = (int.Parse(orangeText.text) + count).ToString();
  }

  public void BlueScore(int count)
  {
    blueText.text = (int.Parse(blueText.text) + count).ToString();
  }
}