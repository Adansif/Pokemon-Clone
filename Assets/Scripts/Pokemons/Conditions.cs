using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conditions
{
   
   public string Name { get; set; }
   public string Description {get; set;}

   public string StartMsg {set; get;}
   
   public Action<Pokemon> OnAfterTurn {get; set;}
}
