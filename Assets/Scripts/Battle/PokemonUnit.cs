using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Usamos UI para importar la clase IMAGE
using UnityEngine.UI;
// Usame DG:TWEENING para importart DOTWEEN
using DG.Tweening;

public class PokemonUnit : MonoBehaviour
{
  

  [SerializeField] bool isPlayerPokemon;
  [SerializeField] HUD HUD;

  public bool IsPlayerPokemon{
    get{
      return isPlayerPokemon;
    }
  }
  public HUD Hud{
    get{
      return HUD;
    }
  }

  public Pokemon Pokemon {get; set;}

  Image image;
  Color originalColor;
  Vector3 originalPosition;
  private void Awake(){
    image = GetComponent<Image>();
    originalPosition = image.transform.localPosition;
    originalColor = image.color;
  }

  public void Setup(Pokemon pokemon){
    Pokemon = pokemon;
    
    if (pokemon.IsShiny()){
      image.sprite = isPlayerPokemon? Pokemon.Base.BackSpriteShiny: Pokemon.Base.FrontSpriteShiny;
    } else {
          image.sprite = isPlayerPokemon? Pokemon.Base.BackSprite: Pokemon.Base.FrontSprite;
    }

    HUD.SetData(pokemon);

    image.color = originalColor;
    playerEnterAnimation();
  }


// Utilizamos esta funcion en lugar de un Animator para hacer que los pokemon se deslicen para entrar al combate
  public void playerEnterAnimation(){
    if (isPlayerPokemon){
      image.transform.localPosition = new Vector3(-500f, originalPosition.y);
    } else{
      image.transform.localPosition = new Vector3(500f, originalPosition.y);
    }
    image.transform.DOLocalMoveX(originalPosition.x, 2f);
  }

  public void playAttackAnimation(){
    var sequence = DOTween.Sequence();
    if (isPlayerPokemon){
      sequence.Append(image.transform.DOLocalMoveX(originalPosition.x + 50F, 0.2f));
    } else {
      sequence.Append(image.transform.DOLocalMoveX(originalPosition.x - 50F, 0.2f));
    }
    sequence.Append(image.transform.DOLocalMoveX(originalPosition.x, 0.2f));
  }

  public void playHitAnimation(){
    var sequence = DOTween.Sequence();
    sequence.Append(image.DOColor(Color.gray, 0.1f));
    sequence.Append(image.DOColor(originalColor, 0.1f));
  }

  public void playFaintAnimation(){
    var sequence = DOTween.Sequence();
    sequence.Append(image.transform.DOLocalMoveY(originalPosition.y - 150f, 0.5f));
    sequence.Join(image.DOFade(0f, 0.5f));
  }
}
