using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerGlobals : Globals
{
  public override GameObject Ground { get; protected set; }
  public override GameObject Target { get; protected set; }
  public bool IsDead { get; set; }

  public static Globals Instance { get; protected set; }
  
  protected override void Awake()
  {
    base.Awake();

    Instance = this;

    Ground = GameObject.FindWithTag("PlayerGround");
    Target = GameObject.FindWithTag("Player");
  }
}