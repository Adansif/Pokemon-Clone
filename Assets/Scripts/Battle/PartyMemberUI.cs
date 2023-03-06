using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PartyMemberUI : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar HPBar;
    [SerializeField] Text currentHPText;
    [SerializeField] Text maxHPText;
    [SerializeField] Sprite selectedSprite;
    [SerializeField] Sprite unselectedSprite;
    [SerializeField] Image icon;
    private Image imagen;
    private Color originalColor;

    Pokemon _pokemon;

    public void Update(){
        icon.sprite = _pokemon.Base.Icon;
        
    }

    public void SetData(Pokemon pokemon){
        _pokemon = pokemon;

        nameText.text = pokemon.Base.Name;
        levelText.text = "Lv " + pokemon.Level;
        //maxHPText.text =pokemon.MaxHP.ToString();
        //currentHPText.text = pokemon.HP.ToString();
        HPBar.SetHP((float)  pokemon.HP / pokemon.MaxHP);        
    }

    public IEnumerator updateHP(){
        yield return HPBar.setHPSmooth((float)  _pokemon.HP / _pokemon.MaxHP);
    }
    public void IsSelected(bool isSelected){
        imagen = GetComponent<Image>();
        if (isSelected){
            imagen.sprite = selectedSprite;
        } else{
             imagen.sprite = unselectedSprite;
        }
    }
}
