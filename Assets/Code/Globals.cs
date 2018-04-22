using UnityEngine;
using System.Collections.Generic;

public abstract class Globals : MonoBehaviour
{
  public abstract GameObject Ground { get; set; }
  public abstract GameObject Target { get; set; }
  public WaveManager WaveManager { get; private set; }
  public List<GameObject> enemies { get; set; }

  protected virtual void Awake()
  {
    enemies = new List<GameObject>();
    
    WaveManager = FindObjectOfType<WaveManager>();
  }
}