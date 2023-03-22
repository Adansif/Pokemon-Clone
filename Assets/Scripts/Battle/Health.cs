using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] PokemonUnit pokemon;
    [SerializeField] List<Sprite> healthSprite;
    private SpriteRenderer spriteRenderer;
    public void Awake(){
        if(pokemon.Pokemon.HP <= pokemon.Pokemon.MaxHP/4){
            spriteRenderer.sprite = healthSprite[0];
        }else if(pokemon.Pokemon.HP <= pokemon.Pokemon.MaxHP/2){
            spriteRenderer.sprite = healthSprite[1];
        }else{
             spriteRenderer.sprite = healthSprite[0];
        }
    }
}
