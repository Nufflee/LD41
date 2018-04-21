using UnityEngine;

public class Gun : MonoBehaviour
{
  public float fireRate = 0.25f;

  private float nextFire = -1;

  private void Start()
  {
  }

  private void Update()
  {
    if (Input.GetMouseButton(0) && Time.time > nextFire)
    {
      nextFire = Time.time + fireRate;

      RaycastHit hit;

      if (Physics.Raycast(Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f)), Camera.main.transform.forward, out hit))
      {
        if (hit.collider.CompareTag("Enemy"))
        {
          // Damage the enemy
          print("it was hit!");
        }
      }
    }
  }
}