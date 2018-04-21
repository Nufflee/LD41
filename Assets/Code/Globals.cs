using UnityEngine;

public abstract class Globals : MonoBehaviour
{
  public abstract GameObject Ground { get; protected set; }
  public abstract GameObject Target { get; protected set; }
  public abstract WaveManager WaveManager { get; protected set; }

  public static Globals Instance { get; protected set; }

  private void Awake()
  {
  }
}