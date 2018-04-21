using UnityEngine;

public class AIGlobals : Globals
{
  public override GameObject Ground { get; protected set; }
  public override GameObject Target { get; protected set; }

  public static Globals Instance { get; protected set; }

  protected override void Awake()
  {
    base.Awake();

    Instance = this;

    Ground = GameObject.FindWithTag("AIGround");
    Target = GameObject.FindWithTag("AIPlayer");
  }
}