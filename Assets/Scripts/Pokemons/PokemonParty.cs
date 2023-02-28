using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PokemonParty : MonoBehaviour
{
    
    [SerializeField] List<Pokemon> partyPokemons;

    public List<Pokemon> Pokemons{
        get{
            return partyPokemons;
        }
    }
    
    private void Start(){
        foreach (var pokemon in partyPokemons){
            pokemon.init();
        }
    }
    public Pokemon getAlivePokemon(){
        return partyPokemons.Where(x => x.HP > 0).FirstOrDefault(); //Con esto evitamos tener que hacer un bucle FOR para buscar los pokemons que !isFainted
    }
}
