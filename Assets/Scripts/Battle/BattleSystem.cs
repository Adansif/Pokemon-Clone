using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum battleState {Start, actionSelection, moveSelection, performMove, busy, partyScreen, battleOver}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] PokemonUnit playerPokemon;
    [SerializeField] PokemonUnit enemyPokemon;
    [SerializeField] BattleDialog dialogBox;
    [SerializeField] PartyScreen partyScreen;

    public event Action<bool> OnBattleOver;

    battleState state;
    int currentAction;
    int currentMove;
    int currentMember;
    int currentInput;

    PokemonParty playerParty;
    Pokemon wildPokemon;

    public void startBattle(PokemonParty playerParty, Pokemon wildPokemon){
        this.playerParty = playerParty;
        this.wildPokemon = wildPokemon;
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle(){
        playerPokemon.Setup(playerParty.getAlivePokemon());
        playerPokemon.Hud.updateHPText(playerPokemon.Pokemon);
        enemyPokemon.Setup(wildPokemon);
        partyScreen.init();

        dialogBox.setMoveNames(playerPokemon.Pokemon.Moves);

        yield return dialogBox.TypeDialog($"A wild {enemyPokemon.Pokemon.Base.Name} had appear.") ;
        

        ChooseFirstTurn();
    }

    void ChooseFirstTurn(){
        if (playerPokemon.Pokemon.Speed >= enemyPokemon.Pokemon.Speed ){
            actionSelection();
        } else{
            StartCoroutine(enemyMove());
        }
    }

    void battleOver(bool won){
        state = battleState.battleOver;
        playerParty.Pokemons.ForEach(p => p.OnBattleOver());
        OnBattleOver(won);
    }

    void actionSelection(){
        state = battleState.actionSelection;
        dialogBox.SetDialog("Choose an option");
        dialogBox.enableOptionSelector(true);
    }
    void openPartyScreen(){
        state = battleState.partyScreen;
        partyScreen.setPartyData(playerParty.Pokemons);
        partyScreen.gameObject.SetActive(true);
    }

    void moveSelection(){
        state = battleState.moveSelection;
        dialogBox.enableOptionSelector(false);
        dialogBox.enableDialogText(false);
        dialogBox.enableMoveSelector(true);
    }
    void checkForBattleOver(PokemonUnit faintedUnit){
        if (faintedUnit.IsPlayerPokemon){
            var nextPokemon = playerParty.getAlivePokemon();
            if (nextPokemon != null){
                openPartyScreen();
            } else {
                battleOver(false);
            }
        }else{
            battleOver(true);
        }
    }
    public void HandleUpdate(){
        if (state == battleState.actionSelection){
            resolveActionSelector();
        } else if (state == battleState.moveSelection){
            resolveMoveSelector();
        }else if (state == battleState.partyScreen){
            resolvePartyScreenSelector();
        }
    }

    void resolveActionSelector(){
        
        currentAction = InputSelector(3);

        currentAction = Mathf.Clamp(currentAction, 0, 3);

        dialogBox.updateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Z)){
            if (currentAction == 0){
                moveSelection();
            }else if(currentAction == 1){
                openPartyScreen();
                currentInput = 0;
            }else if(currentAction == 2){
                //TODO: Bag menu
            }else if(currentAction == 3){
                battleOver(true);
            }
        }
    }

    void resolveMoveSelector(){
          
        currentMove = InputSelector((playerPokemon.Pokemon.Moves.Count -1));

        currentMove = Mathf.Clamp(currentMove, 0, playerPokemon.Pokemon.Moves.Count - 1);

        dialogBox.updateMoveSelection(currentMove, playerPokemon.Pokemon.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.Z)){
            dialogBox.enableMoveSelector(false);
            dialogBox.enableDialogText(true);
            StartCoroutine(playerMove());
        } else if (Input.GetKeyDown(KeyCode.X)){
                dialogBox.enableMoveSelector(false);
                dialogBox.enableDialogText(true);
                actionSelection();
                currentInput = 0;
        }
    }
    void resolvePartyScreenSelector(){
        
        currentMember = InputSelector(playerParty.Pokemons.Count -1);

        currentMember = Mathf.Clamp(currentMember, 0, playerParty.Pokemons.Count - 1);

        partyScreen.updateMemberSelection(currentMember);

        if (Input.GetKeyDown(KeyCode.Z)){
            var selectedMember = playerParty.Pokemons[currentMember];
            if (selectedMember.HP <= 0){
                partyScreen.setMessageText("You can't send out a fainted pokemon");
                return;
            }
            if (selectedMember == playerPokemon.Pokemon){
                partyScreen.setMessageText("You can't switch with the same pokemon");
                return;
            }
            partyScreen.gameObject.SetActive(false);
            state = battleState.busy;
            StartCoroutine(switchPokemon(selectedMember));
        } else if(Input.GetKeyDown(KeyCode.X)){
            partyScreen.gameObject.SetActive(false);
            actionSelection();
            currentInput = 0; //Importante mantener los currenntImput =  0  ya que si no, al darle a la x para salir de la opcion, los current% cogeral el valor final de Imput y podrian salirse de los limites
        }
    }
    private int  InputSelector( int input){ // Esta funcion es la que permite seleccionar los elementos en batalla. No tocar, funciona porque Don Bosco asi lo quiere
         if (Input.GetKeyDown(KeyCode.S) && currentInput < (input-1)){
            currentInput += 2;
        } else if (Input.GetKeyDown(KeyCode.W) && currentInput > 1){
            currentInput -= 2;
        } else if(Input.GetKeyDown(KeyCode.D) && currentInput < input){
            currentInput++;
        }else if(Input.GetKeyDown(KeyCode.A) && currentInput > 0){
            currentInput--;
        }
        return currentInput;
    }
    

    IEnumerator playerMove(){
        state = battleState.performMove;

        var move = playerPokemon.Pokemon.Moves[currentMove];
        yield return RunMove(playerPokemon, enemyPokemon, move);
        
        // Si el estado no ha cambiado por RunMove hara lo siguiente:
        if (state == battleState.performMove){
            StartCoroutine(enemyMove());
        }
    }

    IEnumerator enemyMove(){
        state = battleState.performMove;

        var move = enemyPokemon.Pokemon.getRandomMove();
        playerPokemon.Hud.updateHPText(playerPokemon.Pokemon);
        yield return RunMove(enemyPokemon, playerPokemon, move);
        
        if (state == battleState.performMove){
            actionSelection();
        }
    }
    
    IEnumerator RunMoveEffects(Move move, Pokemon source, Pokemon target){
        var effects = move._base.Effects;
        //Stat boosting
            if (effects.Boosts != null){
                if (move._base.Target == MoveTarget.Self){
                    source.ApplyBoost(effects.Boosts);
                } else{
                    target.ApplyBoost(effects.Boosts);
                }
        // Status condition
                if (effects.Status != ConditionID.none){
                    target.SetStatus(effects.Status);
                }
                yield return ShowStatusChanges(source);
                yield return ShowStatusChanges(target);
            }
    }

    IEnumerator ShowStatusChanges(Pokemon pokemon){
        while (pokemon.StatusChanges.Count > 0){
            var message = pokemon.StatusChanges.Dequeue();
            yield return dialogBox.TypeDialog(message);
        }
    }

    IEnumerator RunMove(PokemonUnit sourceUnit, PokemonUnit targetUnit, Move move){
        move.PP--;
        yield return dialogBox.TypeDialog($"{sourceUnit.Pokemon.Base.Name} used {move._base.Name}");

    sourceUnit.playAttackAnimation();
        yield return new WaitForSeconds(1f);
        targetUnit.playHitAnimation();

        if (move._base.Category == MoveCategory.Status){
            yield return RunMoveEffects(move, sourceUnit.Pokemon, targetUnit.Pokemon);
        } else{
            var damageDetails = targetUnit.Pokemon.isTakingDamage(move,sourceUnit.Pokemon);
            yield return targetUnit.Hud.updateHP();
            playerPokemon.Hud.updateHPText(playerPokemon.Pokemon);
            yield return showDamageDetails(damageDetails);
        }

        

        if (targetUnit.Pokemon.HP <= 0){
            yield return dialogBox.TypeDialog($"{targetUnit.Pokemon.Base.Name} has fainted");
            targetUnit.playFaintAnimation();
            yield return new WaitForSeconds(2f);
            checkForBattleOver(targetUnit);
        }

        // Statuses like burn or poisoned will hurt the pokemon after every turn
        sourceUnit.Pokemon.OnAfterTurn();
        yield return ShowStatusChanges(sourceUnit.Pokemon);
        yield return sourceUnit.Hud.updateHP();
         if (sourceUnit.Pokemon.HP <= 0){
            yield return dialogBox.TypeDialog($"{sourceUnit.Pokemon.Base.Name} has fainted");
            sourceUnit.playFaintAnimation();
            yield return new WaitForSeconds(2f);
            checkForBattleOver(sourceUnit);
        }
    }
    IEnumerator showDamageDetails(DamageDetails damageDetails){
        if (damageDetails.Critical > 1f){
            yield return dialogBox.TypeDialog("A critical hit!");
        }
        if (damageDetails.TypeEffectiveness == 2f || damageDetails.TypeEffectiveness == 4f){
            yield return dialogBox.TypeDialog("Is super effective!");
        } else if (damageDetails.TypeEffectiveness == 0.5f || damageDetails.TypeEffectiveness == 0.25f){
            yield return dialogBox.TypeDialog("Is not very effective...");
        }
    }

    IEnumerator switchPokemon(Pokemon newPokemon){

        bool isCurrentPokemonFainted = true;
        if (playerPokemon.Pokemon.HP > 0){
            isCurrentPokemonFainted = false;
            yield return dialogBox.TypeDialog($"Come back {playerPokemon.Pokemon.Base.name}");
            playerPokemon.playFaintAnimation();
            yield return new WaitForSeconds(2f);
        }

        playerPokemon.Setup(newPokemon);

        dialogBox.setMoveNames(newPokemon.Moves);

        yield return dialogBox.TypeDialog($"Go {newPokemon.Base.Name}.") ;

        if (isCurrentPokemonFainted){
            ChooseFirstTurn();
        }else {
            StartCoroutine(enemyMove());
        }
    }
}
