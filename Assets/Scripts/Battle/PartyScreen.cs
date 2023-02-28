using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyScreen : MonoBehaviour
{

    [SerializeField]Text messageText;
    PartyMemberUI[] memberSlots;
    List<Pokemon> pokemons;

    public void init(){
        memberSlots = GetComponentsInChildren<PartyMemberUI>();
    }

    public void setPartyData(List<Pokemon> pokemons){ //Lista de pokemons en la party
        this.pokemons = pokemons;
        for (int i = 0; i < memberSlots.Length; i++){
            if (i < pokemons.Count){
                memberSlots[i].SetData(pokemons[i]);
            } else{
                memberSlots[i].gameObject.SetActive(false);
            }
        }
        messageText.text = "Choose a Pokemon";
    }

    public void updateMemberSelection(int selectedMember){
         for (int i = 0; i < pokemons.Count; i++){
            if (i == selectedMember){
                memberSlots[i].IsSelected(true);
            } else {
               memberSlots[i].IsSelected(false);
            }
         }
    }
    public void setMessageText(string message){
        messageText.text = message;
    }
}
