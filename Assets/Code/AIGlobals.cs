using UnityEngine;

public class AIGlobals : Globals
{
  public override GameObject Ground { get; set; }
  public override GameObject Target { get; set; }

  public static Globals Instance { get; set; }

  protected override void Awake()
  {
    base.Awake();

    Instance = this;

    Ground = GameObject.FindWithTag("AIGround");
    Target = GameObject.FindWithTag("AIPlayer");
  }
}