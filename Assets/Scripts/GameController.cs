using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {freeRoam, battle}

public class GameController : MonoBehaviour
{

    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera playerCamera;
    
    GameState state;

    private void Awake(){
        ConditionsDB.Init();
    }

    private void Start(){
        playerController.OnEncountered += startBattle;
        battleSystem.OnBattleOver += endBattle;
    }
    void startBattle(){
        state = GameState.battle;
        battleSystem.gameObject.SetActive(true);
        playerCamera.gameObject.SetActive(false);

        var playerParty = playerController.GetComponent<PokemonParty>();
        var wildPokemon = FindObjectOfType<MapArea>().GetComponent<MapArea>().getRandomWildPokemon();
        battleSystem.startBattle(playerParty, wildPokemon);
    }

    void endBattle(bool isWon){
        state = GameState.freeRoam;
        battleSystem.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);
    }

    private void Update(){
        if (state == GameState.freeRoam){
            playerController.HandleUpdate();
        }else if (state == GameState.battle){
        battleSystem.HandleUpdate();
        }
    } 
    
}
