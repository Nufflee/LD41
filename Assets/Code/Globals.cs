using UnityEngine;

public abstract class Globals : MonoBehaviour
{
  public abstract GameObject Ground { get; protected set; }
  public abstract GameObject Target { get; protected set; }
}