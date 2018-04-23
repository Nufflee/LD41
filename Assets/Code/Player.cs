using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour
{
  public float health = 0;

  private RigidbodyFirstPersonController controller;
  private Gun gun;

  // How long the object should shake for.
  public float shakeDuration = 0f;

  // Amplitude of the shake. A larger value shakes the camera harder.
  public float shakeAmount = 0.7f;
  public float decreaseFactor = 1.0f;

  public bool dead = false;

  private Vector3 originalPos;
  private DateTime spawnTime;

  private void Start()
  {
    controller = GetComponent<RigidbodyFirstPersonController>();
    gun = transform.GetChild(0).GetChild(0).GetComponent<Gun>();
    spawnTime = DateTime.Now;
  }

  // Update is called once per frame
  private void Update()
  {
    if (health < 100)
    {
      health += 0.005f;
    }

    if (gun.isReloading && controller.movementSettings.ForwardSpeed != 4.0f)
    {
      controller.movementSettings.ForwardSpeed = 4.0f;
      controller.movementSettings.BackwardSpeed = 2.5f;
      controller.movementSettings.StrafeSpeed = 2.5f;
    }
    else if (gun.isReloading == false && controller.movementSettings.ForwardSpeed != 8.0f)
    {
      controller.movementSettings.ForwardSpeed = 8.0f;
      controller.movementSettings.BackwardSpeed = 4.0f;
      controller.movementSettings.StrafeSpeed = 4.0f;
    }
  }

  public void Damage(float damage)
  {
    health -= damage;

    // Update health bar.
    GameObject.Find("PlayerHealthBar").transform.GetChild(0).GetComponent<Image>().fillAmount = (health / 100f);

    if (health <= 0 && dead == false)
    {
      TimeSpan deathTime = DateTime.Now - spawnTime;
      
      if (deathTime.Minutes == 0)
      {
        Statistics.instance.player.timeAlive = deathTime.Seconds + " sec";
      }
      else
      {
        Statistics.instance.player.timeAlive = deathTime.Minutes + " min " + deathTime.Seconds + " sec";
      }

      Statistics.instance.player.waveNumber = AIGlobals.Instance.WaveManager.waveNumber;
      Statistics.instance.player.score = Score.instance.playerScore;

      StartCoroutine(Die());
    }

    if (health > 100)
    {
      health = 100;
    }

    if (damage > 0)
    {
      Statistics.instance.player.healthSpent += (int) damage;
    }
  }

  private IEnumerator Die()
  {
    GameObject gunGameObject = gun.gameObject;

    gun.enabled = false;
    controller.enabled = false;
    gunGameObject.GetComponent<Rigidbody>().isKinematic = false;

    foreach (Collider collider in gunGameObject.GetComponentsInChildren<Collider>())
    {
      collider.enabled = true;
    }

    dead = true;
    FindObjectOfType<WaveManager>().enabled = false;
    gun.enabled = false;
    controller.enabled = false;
    gunGameObject.GetComponent<Rigidbody>().isKinematic = false;
    transform.GetChild(0).GetComponent<HeadBob>().enabled = false;
    GetComponent<Rigidbody>().isKinematic = true;
    transform.GetChild(0).GetComponent<UnityStandardAssets.ImageEffects.Grayscale>().enabled = true;
    originalPos = transform.GetChild(0).localPosition;
    controller.mouseLook.SetCursorLock(false);
    Camera.main.GetComponent<Rigidbody>().isKinematic = false;
    Camera.main.GetComponent<Collider>().enabled = true;
    Camera.main.GetComponent<Rigidbody>().AddForce(transform.right * 10);
    GameObject.Find("ScoreBackground").SetActive(false);
    Destroy(AIGlobals.Instance.Target);

    yield return new WaitForSeconds(2.5f);

    Statistics.instance.Show();
    LeaderboardController.instance.ShowPanel();
  }
}