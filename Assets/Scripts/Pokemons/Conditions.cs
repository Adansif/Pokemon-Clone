using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conditions
{
   
   public string Name { get; set; }
   public string Description {get; set;}

   public string StartMsg {set; get;}
   
   public Func<Pokemon, bool> OnBeforeMove {get; set;} //Use Func cause Action doesnt let return anything and we need to return a bool
   public Action<Pokemon> OnAfterTurn {get; set;}
   public Action<Pokemon> OnStart{get; set;}
}
