using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
  public GameObject target;

  private Transform defaultCameraLocation;

  private bool startPlayingTheGame = false;

  public void PlayBtn()
  {
    MMWaveManager waveManager = FindObjectOfType<MMWaveManager>();
    waveManager.enabled = false;
    target.GetComponent<MMAIController>().enabled = false;
    target.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
    target.GetComponent<Rigidbody>().isKinematic = true;
    Destroy(target);
    Camera.main.gameObject.GetComponent<Animator>().enabled = false;
    startPlayingTheGame = true;
    defaultCameraLocation = GameObject.Find("DefaultCameraLocation").transform;
    target = Instantiate(Resources.Load<GameObject>("MainMenuPrefabs/MainMenuAI"), defaultCameraLocation.transform.position, new Quaternion(0f, 180f, 0f, 0f));
    target.GetComponent<MMAIController>().enabled = false;
    target.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
    target.GetComponent<Rigidbody>().isKinematic = true;
    GetComponent<Canvas>().enabled = false;
    Destroy(GameObject.FindWithTag("AIPlayer"), 1.5f);
    foreach (GameObject enemy in waveManager.enemies)
    {
      Destroy(enemy, 1f);
    }

    waveManager.enemies.Clear();
    waveManager.StopAllCoroutines();
  }

  public void ExitBtn()
  {
    Application.Quit();
  }

  void Update()
  {
    if (startPlayingTheGame && Camera.main.transform.localPosition != defaultCameraLocation.position && Camera.main.transform.localRotation.eulerAngles != new Vector3(0f, 180f, 0f))
    {
      Camera.main.transform.localPosition = Vector3.MoveTowards(Camera.main.transform.localPosition, defaultCameraLocation.position, 18f * Time.deltaTime);
      Camera.main.transform.localRotation = Quaternion.SlerpUnclamped(Camera.main.transform.localRotation, new Quaternion(0f, 180f, 0f, 0f), 0.02f * Time.deltaTime);
    }
    else if (startPlayingTheGame)
    {
      if(Camera.main.transform.localRotation.eulerAngles != new Vector3(0f, 180f, 0f)) Camera.main.transform.localRotation = Quaternion.SlerpUnclamped(Camera.main.transform.localRotation, new Quaternion(0f, 180f, 0f, 0f), 2f * Time.deltaTime);
      else UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
  }
}