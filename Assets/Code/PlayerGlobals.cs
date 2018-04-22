using UnityEngine;

public class PlayerGlobals : Globals
{
  public override GameObject Ground { get; set; }
  public override GameObject Target { get; set; }
  public bool IsDead { get; set; }

  public static Globals Instance { get; set; }

  protected override void Awake()
  {
    base.Awake();

    Instance = this;

    Ground = GameObject.FindWithTag("PlayerGround");
    Target = GameObject.FindWithTag("Player");
  }
}