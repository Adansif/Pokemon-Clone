using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum battleState {Start, actionSelection, moveSelection, RunningTurn, busy, partyScreen, battleOver}
public enum BattleAction{Move, SwitchPokemon, Item, Run}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] PokemonUnit playerPokemon;
    [SerializeField] PokemonUnit enemyPokemon;
    [SerializeField] BattleDialog dialogBox;
    [SerializeField] PartyScreen partyScreen;

    public event Action<bool> OnBattleOver;

    battleState state;
    battleState? prevState;
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
        

        actionSelection();
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
                currentInput = 0;
                moveSelection();
            }else if(currentAction == 1){
                currentInput = 0;
                prevState = state;
                openPartyScreen();
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

            var move = playerPokemon.Pokemon.Moves[currentMove];

            if (move .PP == 0) return ;
            dialogBox.enableMoveSelector(false);
            dialogBox.enableDialogText(true);
            StartCoroutine(RunTurns(BattleAction.Move));
            currentInput = 0;
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
                currentInput = 0;
                return;
            }
            if (selectedMember == playerPokemon.Pokemon){
                partyScreen.setMessageText("You can't switch with the same pokemon");
                currentInput = 0;
                return;
            }
            partyScreen.gameObject.SetActive(false);
           if (prevState == battleState.actionSelection){
            prevState = null;
                StartCoroutine(RunTurns(BattleAction.SwitchPokemon));
           }else{
                state = battleState.busy;
                StartCoroutine(switchPokemon(selectedMember));
           }
          
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
    
bool checkIfMoveHits(Move move, Pokemon sourcePokemon, Pokemon targetPokemon){
    
    if(move._base.AlwaysHit){
        return true;
    }
    
    float moveAccuracy = move._base.Accuracy;

    int accuracy = sourcePokemon.StatBoost[Stat.Accuracy];
    int evasion = targetPokemon.StatBoost[Stat.Evasion];

    var boostValues = new float[] {1f, 4f/3f, 5f/3f, 2f, 7f/3f, 8f/3f, 3f};

    if (accuracy > 0){
        moveAccuracy *= boostValues[accuracy];
    }else{
        moveAccuracy /= boostValues[-accuracy];
    }

    if (evasion > 0){
        moveAccuracy /= boostValues[evasion];
    }else{
        moveAccuracy *= boostValues[-evasion];
    }

    if (UnityEngine.Random.Range(1, 101) <= moveAccuracy){
        return true;
    }else {
        return false;
    }
}
IEnumerator RunTurns(BattleAction playerAction){
    state = battleState.RunningTurn;

    if (playerAction == BattleAction.Move){
        playerPokemon.Pokemon.CurrentMove = playerPokemon.Pokemon.Moves[currentMove];
        enemyPokemon.Pokemon.CurrentMove = enemyPokemon.Pokemon.getRandomMove();

        int playerMovePriority = playerPokemon.Pokemon.CurrentMove._base.Priority;
        int enemyMovePriority = enemyPokemon.Pokemon.CurrentMove._base.Priority;

        //Check wich pokemon goes first
        bool playerGoesFirst = true;
        if (enemyMovePriority > playerMovePriority){
            playerGoesFirst = false;
        }else if(enemyMovePriority == playerMovePriority){
            playerGoesFirst = playerPokemon.Pokemon.Speed >= enemyPokemon.Pokemon.Speed;
        }


        var firstPokemon = (playerGoesFirst)? playerPokemon : enemyPokemon;
        var secondPokemon = (playerGoesFirst)? enemyPokemon : playerPokemon;

        var secondUnit = secondPokemon.Pokemon;

        // First turn
        yield return RunMove(firstPokemon, secondPokemon, firstPokemon.Pokemon.CurrentMove);
        yield return RunAfterTurn(firstPokemon);
        if (state == battleState.battleOver){
            yield break;
        } 

        if (secondUnit.HP > 0){
            // Second turn
            yield return RunMove(secondPokemon, firstPokemon, secondPokemon.Pokemon.CurrentMove);
            yield return RunAfterTurn(secondPokemon);

            if (state == battleState.battleOver){
                yield break;
            } 
        } 
    }else{
        if(playerAction == BattleAction.SwitchPokemon){
            var selectedMember = playerParty.Pokemons[currentMember];
            state = battleState.busy;
            yield return switchPokemon(selectedMember);
        }
        // Enemy will have the turn if  player switches pokemon

        var enemyMove = enemyPokemon.Pokemon.getRandomMove();
        yield return RunMove(enemyPokemon, playerPokemon, enemyMove);
        yield return RunAfterTurn(enemyPokemon);

        if (state == battleState.battleOver){
            yield break;
        } 
    }

    if (state != battleState.battleOver){
        actionSelection();
    }
}
    
    IEnumerator RunMoveEffects(MoveEffects effects, Pokemon source, Pokemon target, MoveTarget moveTarget){
       
        //Stat boosting
            if (effects.Boosts != null){
                if (moveTarget == MoveTarget.Self){
                    source.ApplyBoost(effects.Boosts);
                } else{
                    target.ApplyBoost(effects.Boosts);
                }
        // Status condition
                if (effects.Status != ConditionID.none){
                    target.SetStatus(effects.Status);
                }
         // volatile status condition
                if (effects.VolatileStatus != ConditionID.none){
                    target.SetVolatileStatus(effects.VolatileStatus);
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

        bool canRunMove = sourceUnit.Pokemon.OnBeforeMove();
        if (!canRunMove){
            yield return ShowStatusChanges(sourceUnit.Pokemon);
            yield return sourceUnit.Hud.updateHP();
            yield break;
        }
        yield return ShowStatusChanges(sourceUnit.Pokemon);

        move.PP--;
        yield return dialogBox.TypeDialog($"{sourceUnit.Pokemon.Base.Name} used {move._base.Name}");

        if(checkIfMoveHits(move, sourceUnit.Pokemon, targetUnit.Pokemon)){

        

            sourceUnit.playAttackAnimation();
            yield return new WaitForSeconds(1f);
            targetUnit.playHitAnimation();

            if (move._base.Category == MoveCategory.Status){
                yield return RunMoveEffects(move._base.Effects, sourceUnit.Pokemon, targetUnit.Pokemon, move._base.Target);
            } else{
                var damageDetails = targetUnit.Pokemon.isTakingDamage(move,sourceUnit.Pokemon);
                yield return targetUnit.Hud.updateHP();
                playerPokemon.Hud.updateHPText(playerPokemon.Pokemon);
                yield return showDamageDetails(damageDetails);
            }

            if(move._base.SecondaryEffects != null && move._base.SecondaryEffects.Count > 0 && targetUnit.Pokemon.HP > 0){
                foreach (var secondary in move._base.SecondaryEffects){
                    var random = UnityEngine.Random.Range(1,101);
                    if(random <= secondary.Chance){
                        yield return RunMoveEffects(secondary, sourceUnit.Pokemon, targetUnit.Pokemon, secondary.Target);
                    }
                }
            }

            targetUnit.Hud.setBarColor(targetUnit.Pokemon);

            if (targetUnit.Pokemon.HP <= 0){
                yield return dialogBox.TypeDialog($"{targetUnit.Pokemon.Base.Name} has fainted");
                targetUnit.playFaintAnimation();
                yield return new WaitForSeconds(2f);
                checkForBattleOver(targetUnit);
            }
        }else{
             yield return dialogBox.TypeDialog($"{targetUnit.Pokemon.Base.Name}'s attack missed");
        }

    }
    IEnumerator RunAfterTurn(PokemonUnit sourceUnit){

        if (state == battleState.battleOver){
            yield break;
        }
        yield return new WaitUntil (() => state == battleState.RunningTurn);

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
        
        if (playerPokemon.Pokemon.HP > 0){
            yield return dialogBox.TypeDialog($"Come back {playerPokemon.Pokemon.Base.name}");
            playerPokemon.playFaintAnimation();
            yield return new WaitForSeconds(2f);
        }

        playerPokemon.Setup(newPokemon);
         playerPokemon.Hud.updateHPText(playerPokemon.Pokemon);

        dialogBox.setMoveNames(newPokemon.Moves);

        yield return dialogBox.TypeDialog($"Go {newPokemon.Base.Name}.") ;

        state = battleState.RunningTurn;
    }
}
