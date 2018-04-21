using UnityEngine;

public class PlayerGlobals : Globals
{
  public override GameObject Ground { get; protected set; }
  public override GameObject Target { get; protected set; }
  
  public PlayerGlobals Instance { get; protected set; }

  private void Awake()
  {
    Instance = this;
    
    Ground = GameObject.FindWithTag("PlayerGround");
    Target = GameObject.FindWithTag("Player");
  }
}