using UnityEngine;

public abstract class Globals : MonoBehaviour
{
  public abstract GameObject Ground { get; protected set; }
  public abstract GameObject Target { get; protected set; }
  public WaveManager WaveManager { get; private set; }

  public static Globals Instance { get; protected set; }
  
  protected virtual void Awake()
  {
    Instance = this;
    
    WaveManager = FindObjectOfType<WaveManager>();
  }
}