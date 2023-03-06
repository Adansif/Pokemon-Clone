using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pokemon
{
    [SerializeField] PokemonBase _base;
    [SerializeField] int level;
    public bool isShiny;
    public PokemonBase Base {
        get{
            return this._base;
        }
    }
    public int Level{
        get{
            return this.level;
        }
    }

    public int HP {get; set;}

    public List<Move> Moves {get; set;}
    public Dictionary<Stat, int> Stats{get; private set;}
    public Dictionary<Stat, int> StatBoost{get; private set;}
    public Conditions Status {get; private set;}
    public int StatusTime{get; set;}
    public bool HpChanged {get; set;}

    public Queue<string> StatusChanges{get; private set;} = new Queue<string>();
    //Se usa para almacenar una lista de elementos pero puedes sacar elementos de ella

    public void init(){
        
        // Generate Moves based on level
        Moves = new List<Move>();
        foreach (var move in Base.LearnableMoves){
            if (move.Level <= Level)
            Moves.Add(new Move(move.Base));

            if (Moves.Count >= 4)
                break;
        }
        CalculateStats();

        HP = MaxHP;

        ResetStatBoost();
        CheckIfShiny();
    }

    void ResetStatBoost(){
        StatBoost = new Dictionary<Stat, int>(){
            {Stat.Attack, 0},
            {Stat.Defense, 0},
            {Stat.SpAttack, 0},
            {Stat.SpDefense, 0},
            {Stat.Speed, 0},
        };
    }

    void CalculateStats(){
        Stats = new Dictionary<Stat, int>();
        Stats.Add(Stat.Attack,  Mathf.FloorToInt(((2 * Base.Attack)+ Base.AttackIV + (Base.AttackEV/4) * Level)/100f) + 5);
        Stats.Add(Stat.Defense, Mathf.FloorToInt(((2 * Base.Defense)+ Base.DefenseIV + (Base.DefenseEV/4) * Level)/100f) + 5);
        Stats.Add(Stat.SpAttack,  Mathf.FloorToInt(((2 * Base.SpAttack)+ Base.SpAttackIV + (Base.SpAttackEV/4) * Level)/100f) + 5);
        Stats.Add(Stat.SpDefense, Mathf.FloorToInt(((2 * Base.SpDefense)+ Base.SpDefenseIV + (Base.SpDefenseEV/4) * Level)/100f) + 5);
        Stats.Add(Stat.Speed, Mathf.FloorToInt(((2 * Base.Speed)+ Base.SpeedIV + (Base.SpeedEV/4) * Level)/100f) + 5);

        MaxHP = Mathf.FloorToInt(((2 * Base.MaxHP)+ Base.HPIV + (Base.HPEV/4) * Level)/100f) + (10 + Level);
    }

    int getStat(Stat stat){
        int statVal = Stats[stat];

        // Stat boost
        int boost = StatBoost[stat];
        var boostValues = new float[] {1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f};

        if (boost >= 0){
            statVal = Mathf.FloorToInt(statVal * boostValues[boost]);
        } else{
            statVal = Mathf.FloorToInt(statVal / boostValues[-boost]);
        }
        return statVal;
    }

    public void ApplyBoost(List<StatBoost> statBoosts){
        foreach(var statBoost in statBoosts){
            var stat = statBoost.stat;
            var boost = statBoost.boost;

            StatBoost[stat] = Mathf.Clamp(StatBoost[stat] + boost, -6, 6);

            if (boost > 0){
                StatusChanges.Enqueue($"{Base.Name}'s {stat} rose!");
            } else{
                StatusChanges.Enqueue($"{Base.Name}'s {stat} fell!");
            }

            Debug.Log($"{stat} has been boosted to {StatBoost[stat]}");
        }
    }

    public int Attack{
        get {return getStat(Stat.Attack);}
    }
    public int Defense{
        get {return getStat(Stat.Defense);}
    }
    public int SpAttack{
        get {return getStat(Stat.SpAttack);}
    }
    public int SpDefense{
        get {return getStat(Stat.SpDefense);}
    }
    public int Speed{
        get {return getStat(Stat.Speed);}
    }
    public bool IsShiny(){
        return this.isShiny;
    }
    public int MaxHP{ get; private set; }

    public DamageDetails isTakingDamage(Move move, Pokemon attacker){
        float critical = 1f;
        if (Random.value * 100f <= 6.25f){
            critical = 2f;
        }

        float type = TypeChart.getEffectiveness(move._base.Type, attacker.Base.Type1) * TypeChart.getEffectiveness(move._base.Type, attacker.Base.Type2);

        var damageDetails = new DamageDetails(){
            TypeEffectiveness = type,
            Critical = critical,
            isFainted = false
        };

        float attack = (move._base.Category == MoveCategory.Special)? attacker.SpAttack : attacker.Attack;  // Operador condicional para no usar if else
        float defense = (move._base.Category == MoveCategory.Special)? SpDefense : Defense;

        float modifiers =  type * critical;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move._base.Power * ((float) attack / defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        UpdateHP(damage);
        return damageDetails;
    }

    public void UpdateHP (int damage){
        HP = Mathf.Clamp(HP - damage, 0, MaxHP);
        HpChanged = true;
    }

    public void SetStatus(ConditionID conditionID){
        Status = ConditionsDB.Conditions[conditionID];
        Status?.OnStart?.Invoke(this);
        StatusChanges.Enqueue($"{Base.Name} {Status.StartMsg}");
    }

    public void CureStatus(){
        Status = null;
    }

    public Move getRandomMove(){
        int random = Random.Range(0, Moves.Count);
        return Moves[random];
    }

    public bool OnBeforeMove(){
        if(Status?.OnBeforeMove != null){
            return Status.OnBeforeMove(this);
        }
        return true;
    }

    public void OnAfterTurn(){
        
        Status?.OnAfterTurn?.Invoke(this);
    }

    public void OnBattleOver(){
        ResetStatBoost();
    }
    private void CheckIfShiny(){
         if (Random.Range(1,11) <= 5){
            isShiny = true;
        } else{
            isShiny = false;
        }
    }
}

public class DamageDetails{
    public bool isFainted {get; set;}
    public float Critical {get;set;}
    public float TypeEffectiveness{get;set;}
}