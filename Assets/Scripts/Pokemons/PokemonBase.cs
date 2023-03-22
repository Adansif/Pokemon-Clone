using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Pokemon", menuName ="Pokemon/Create new pokemon")]
public class PokemonBase : ScriptableObject
{

    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] Sprite frontSprite;
    [SerializeField] Sprite frontSprite2;
    [SerializeField] Sprite frontSpriteShiny;
    [SerializeField] Sprite frontSpriteShiny2;

    [SerializeField] Sprite backSprite;
    [SerializeField] Sprite backSprite2;
    [SerializeField] Sprite backSpriteShiny;
    [SerializeField] Sprite backSpriteShiny2;

    [SerializeField] Sprite icon;

    [SerializeField] PokemonType type1;
    [SerializeField] PokemonType type2;

    //Move list

    [SerializeField] List<LearnableMoves> learnableMoves;

    //Base Stats
    [SerializeField] int maxHP;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int spAttack;
    [SerializeField] int spDefense;
    [SerializeField] int speed;
    //Ivs
    [SerializeField] int hpIV;
    [SerializeField] int attackIV;
    [SerializeField] int defenseIV;
    [SerializeField] int spAttackIV;
    [SerializeField] int spDefenseIV;
    [SerializeField] int speedIV;
    //Evs
    [SerializeField] int hpEV;
    [SerializeField] int attackEV;
    [SerializeField] int defenseEV;
    [SerializeField] int spAttackEV;
    [SerializeField] int spDefenseEV;
    [SerializeField] int speedEV;

    public string Name {
        get {return this.name;}
    }

    public string Description {
        get {return this.description;}
    }

    public Sprite FrontSprite{
        get {return this.frontSprite;}
    }
    public Sprite FrontSprite2{
        get {return this.frontSprite2;}
    }

    
    public Sprite FrontSpriteShiny{
        get {return this.frontSpriteShiny;}
    }
    public Sprite FrontSpriteShiny2{
        get {return this.frontSpriteShiny2;}
    }

    
    public Sprite BackSprite{
        get {return this.backSprite;}
    }
    public Sprite BackSprite2{
        get {return this.backSprite2;}
    }

    public Sprite BackSpriteShiny{
        get {return this.backSpriteShiny;}
    }
    public Sprite BackSpriteShiny2{
        get {return this.backSpriteShiny2;}
    }

    public Sprite Icon{
        get {return this.icon;}
    }

    public int MaxHP{
        get {return this.maxHP;}
    }

    public int Attack{
        get {return this.attack;}
    }

    public int Defense{
        get {return this.defense;}
    }

    public int SpAttack{
        get {return this.spAttack;}
    }

    public int SpDefense{
        get {return this.spDefense;}
    }

    public int Speed{
        get {return this.speed;}
    }

        public int HPIV{
        get {return this.hpIV;}
    }

    public int AttackIV{
        get {return this.attackIV;}
    }

    public int DefenseIV{
        get {return this.defenseIV;}
    }

    public int SpAttackIV{
        get {return this.spAttackIV;}
    }

    public int SpDefenseIV{
        get {return this.spDefenseIV;}
    }

    public int SpeedIV{
        get {return this.speed;}
    }

        public int HPEV{
        get {return this.hpEV;}
    }

    public int AttackEV{
        get {return this.attackEV;}
    }

    public int DefenseEV{
        get {return this.defenseEV;}
    }

    public int SpAttackEV{
        get {return this.spAttackEV;}
    }

    public int SpDefenseEV{
        get {return this.spDefenseEV;}
    }

    public int SpeedEV{
        get {return this.speed;}
    }
    public PokemonType Type1{
        get {return this.type1;}
    }
    public PokemonType Type2{
        get {return this.type2;}
    }

    public List<LearnableMoves> LearnableMoves{
        get {return this.learnableMoves;}
    }
    
}

[System.Serializable]
public class LearnableMoves{
    [SerializeField] MoveBase moveBase;
    [SerializeField] int level;

    public MoveBase Base{
        get {return this.moveBase;}
    }
    public int Level{
        get {return level;}
    }
}

public enum PokemonType{
    None,

    Normal,
    Fighting,
    Flying,
    Poison,
    Ground,
    Rock,
    Bug,
    Ghost,
    Steel,
    Fire,
    Water,
    Grass,
    Electric,
    Psychic,
    Ice,
    Dragon,
    Dark,
    Fairy

}

public enum Stat{ //Esto es para crear un dictionary para los stats
    Attack,
    Defense,
    SpAttack,
    SpDefense,
    Speed,

    // Boost for accuracy
    Accuracy,
    Evasion

}

public class TypeChart{
    static float[][] chart = {
    /*                                                                                                                                                                      
        TODO: Crear una clase que contenga todas 
        */                                                                                                                                                                      
        /*Normal*/                  new float[] {1f,     1f,     1f,     1f,     1f,     0.5f,     1f,     0f,     0.5f,     1f,     1f,     1f,     1f,     1f,     1f,     1f,     1f,     1f},
        /*Lucha*/                  new float[] {2f,     1f,     0.5f,     0.5f,     0.5f,     2f,     0.5f,     0f,     2f,     1f,     1f,     1f,     1f,     0.5f,     2f,     1f,     2f,     0.5f},
        /*Volador*/                  new float[] {1f,     2f,     1f,     1f,     1f,     0.5f,     2f,     1f,     0.5f,     1f,     1f,     2f,     0.5f,     1f,     1f,     1f,     1f,     1f},
        /*Veneno*/                  new float[] {1f,     1f,     1f,     0.5f,     0.5f,     0.5f,     1f,     0.5f,     0f,     1f,     1f,     2f,     1f,     1f,     1f,     1f,     1f,     2f},
        /*Tierra*/                  new float[] {1f,     1f,     0f,     2f,     2f,     2f,     0.5f,     1f,     2f,     2f,     1f,     0.5f,     2f,     1f,     1f,     1f,     1f,     1f},
        /*Roca*/                  new float[] {1f,     0.5f,     2f,     1f,     1f,     1f,     2f,     1f,     0.5f,     2f,     1f,     1f,     1f,     1f,     2f,     1f,     1f,     1f},
        /*Bicho*/                  new float[] {1f,     0.5f,     0.5f,     0.5f,     0.5f,     1f,     1f,     0.5f,     0.5f,     0.5f,     1f,     2f,     1f,     2f,     1f,     1f,     2f,     0.5f},
        /*Fantasma*/                  new float[] {0f,     1f,     1f,     1f,     1f,     1f,     1f,     2f,     1f,     1f,     1f,     1f,     1f,     2f,     1f,     1f,     0.5f,     1f},
        /*Acero*/                  new float[] {1f,     1f,     1f,     1f,     1f,     2f,     1f,     1f,     0.5f,     0.5f,     0.5f,     1f,     0.5f,     1f,     2f,     1f,     1f,     2f},
        /*Fuego*/                  new float[] {1f,     1f,     1f,     1f,     1f,     0.5f,     2f,     1f,     2f,     0.5f,     0.5f,     2f,     1f,     1f,     2f,     0.5f,     1f,     1f},
        /*Agua*/                  new float[] {1f,     1f,     1f,     1f,     1f,     2f,     1f,     1f,     1f,     2f,     0.5f,     0.5f,     1f,     1f,     1f,     0.5f,     1f,     1f},
        /*Planta*/                  new float[] {1f,     1f,     0.5f,     0.5f,     2f,     2f,     0.5f,     1f,     0.5f,     0.5f,     2f,     0.5f,     1f,     1f,     1f,     0.5f,     1f,     1f},
        /*Electrico*/                  new float[] {1f,     1f,     2f,     1f,     2f,     1f,     1f,     1f,     1f,     1f,     2f,     0.5f,     0.5f,     1f,     1f,     0.5f,     1f,     1f},
        /*Psiquico*/                  new float[] {1f,     2f,     1f,     2f,     0f,     1f,     1f,     1f,     0.5f,     1f,     1f,     1f,     1f,     0.5f,     1f,     1f,     0f,     1f},
        /*Hielo*/                  new float[] {1f,     1f,     2f,     1f,     1f,     1f,     1f,     1f,     0.5f,     0.5f,     0.5f,     2f,     1f,     1f,     0.5f,     2f,     1f,     1f},
        /*Dragon*/                  new float[] {1f,     1f,     1f,     1f,     2f,     1f,     1f,     1f,     0.5f,     1f,     1f,     1f,     1f,     1f,     1f,     2f,     1f,     0f},
        /*Siniestro*/                  new float[] {1f,     0.5f,     1f,     1f,     1f,     1f,     1f,     2f,     1f,     1f,     1f,     1f,     1f,     2f,     1f,     1f,     0.5f,     0.5f},
        /*Hada*/                  new float[] {1f,     2f,     1f,     0.5f,     1f,     1f,     1f,     1f,     0.5f,     0.5f,     1f,     1f,     1f,     1f,     1f,     2f,     2f,     1f}
        
    };
    public static float getEffectiveness(PokemonType attacktype, PokemonType defenseType){
        if (attacktype == PokemonType.None || defenseType == PokemonType.None){
            return 1;
        }

        int row = (int)attacktype  -1;
        int col = (int)defenseType  -1;
        return chart[row][col];
    }
}
