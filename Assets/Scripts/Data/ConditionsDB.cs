using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionsDB
{
    public static void Init(){
        foreach (var kvp in Conditions){
            var conditionId = kvp.Key;
            var condition = kvp.Value;

            condition.Id = conditionId;
        }
    }
   public static Dictionary<ConditionID, Conditions> Conditions {get; set;} = new Dictionary<ConditionID, Conditions>()
    {
        {
            ConditionID.psn,                // Create the status poisoned on the pokemon
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
            ConditionID.bpsn,               // Creates the status badly poisoned on the pokemon. TODO: calculate the damage properly
            new Conditions(){
                Name = "Bad Poison",
                StartMsg = "has been badly poisoned",
                OnAfterTurn = (Pokemon pokemon) =>{
                    pokemon.UpdateHP(pokemon.MaxHP/4);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} hurt himself due to poison");
                }
            }
        },
        {
            ConditionID.brn,                // Creates the status for being poisoned. Need to aply the half damage
            new Conditions(){
                Name = "Burn",
                StartMsg = "has been burned",
                OnAfterTurn = (Pokemon pokemon) =>{
                    pokemon.UpdateHP(pokemon.MaxHP/16);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} hurt himself due to burn");
                }
            }
        },
        {
            ConditionID.par,
            new Conditions(){
                Name = "Paralyze",
                StartMsg = "has been paralyzed",
                OnBeforeMove =(Pokemon pokemon) =>{
                    if (Random.Range(1, 5) == 2){
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is paralyzed and can't move");
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            ConditionID.frz,
            new Conditions(){
                Name = "Freeze",
                StartMsg = "has been frozen",
                OnBeforeMove =(Pokemon pokemon) =>{
                    if (Random.Range(1, 5) == 1){
                        pokemon.CureStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is not frozen anymore");
                        return true;
                    }
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is frozen and can't move");
                    return false;
                }
            }
        },
        {
            ConditionID.sle,
            new Conditions(){
                Name = "Sleep",
                StartMsg = "has fallen asleep",
                OnStart = (Pokemon pokemon)=>{
                    // Sleep for 1-3 turns
                    pokemon.StatusTime = Random.Range(1,4);
                    Debug.Log($"Will be asleep for {pokemon.StatusTime} moves");
                },
                OnBeforeMove =(Pokemon pokemon) =>{
                    
                    if (pokemon.StatusTime <= 0){
                        pokemon.CureStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} woke up!");
                        return true;
                    }

                    pokemon.StatusTime--;
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is sleeping and can't move");
                    return false;
                }
            }
        },
        //Volatile status
        {
            ConditionID.confusion,
            new Conditions(){
                Name = "Confusion",
                StartMsg = "has been confused",
                OnStart = (Pokemon pokemon)=>{
                    // Sleep for 1-4 turns
                    pokemon.VolatileStatusTime = Random.Range(1,5);
                    Debug.Log($"Will be confused for {pokemon.VolatileStatusTime} moves");
                },
                OnBeforeMove =(Pokemon pokemon) =>{
                    
                    if (pokemon.VolatileStatusTime <= 0){
                        pokemon.CureVolatileStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} kicked out of confusion!");
                        return true;
                    }

                    pokemon.VolatileStatusTime--;

                    //50% odds to make a move
                    if(Random.Range(1,3) == 1){
                        return true;
                    }

                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is confused");
                    pokemon.UpdateHP(pokemon.Attack);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} hurt himself due to confusion");
                    return false;
                }
            }
        }
    };
}


 public enum ConditionID{
        none, psn, bpsn, brn, sle, par, frz,
        confusion
    }