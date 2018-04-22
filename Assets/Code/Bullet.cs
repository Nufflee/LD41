using UnityEngine;
using System.Collections.Generic;

public class Bullet : MonoBehaviour
{
  public float speed = 80f;

  void Start()
  {
    Destroy(gameObject, 5f);
  }

  void Update()
  {
    // Move bullet.
    transform.position += transform.right * speed * Time.deltaTime + (-transform.up * Time.deltaTime * 1f);
  }

  private void OnCollisionEnter(Collision other)
  {
    // TODO: Bullet holes.
    Destroy(gameObject);
  }
}