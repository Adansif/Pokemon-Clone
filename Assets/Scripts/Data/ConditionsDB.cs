using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionsDB
{
   private static int counter = 8;
   public static Dictionary<ConditionID, Conditions> Conditions {get; set;} = new Dictionary<ConditionID, Conditions>() {
        {
            ConditionID.psn,
            new Conditions(){
                Name = "Poison",
                StartMsg = "has been poisoned",
                OnAfterTurn = (Pokemon pokemon) =>{
                    pokemon.UpdateHP(pokemon.MaxHP/8);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} hurt himself due to poison");
                }
            }
        },
        {
            ConditionID.bpsn,
            new Conditions(){
                Name = "Bad Poison",
                StartMsg = "has been badly poisoned",
                OnAfterTurn = (Pokemon pokemon) =>{
                    pokemon.UpdateHP(pokemon.MaxHP /counter);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} hurt himself due to poison");
                    counter --;
                }
            }
        },
        {
            ConditionID.brn,
            new Conditions(){
                Name = "Burn",
                StartMsg = "has been burned",
                OnAfterTurn = (Pokemon pokemon) =>{
                    pokemon.UpdateHP(pokemon.MaxHP/16);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} hurt himself due to burn");
                }
            }
        }
   };

}

 public enum ConditionID{
        none, psn, bpsn, brn, sle, par, frz
    }