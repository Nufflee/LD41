using UnityEngine;
using System.Collections.Generic;

public abstract class Globals : MonoBehaviour
{
  public abstract GameObject Ground { get; protected set; }
  public abstract GameObject Target { get; protected set; }
  public WaveManager WaveManager { get; private set; }
  public List<GameObject> enemies { get; set; }

  public static Globals Instance { get; protected set; }
  
  protected virtual void Awake()
  {
    Instance = this;

    enemies = new List<GameObject>();
    
    WaveManager = FindObjectOfType<WaveManager>();
  }
}