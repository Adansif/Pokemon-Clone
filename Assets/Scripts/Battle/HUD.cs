using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This USING es para importar los Text UI 
using UnityEngine.UI;


public class HUD : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar HPBar;
    [SerializeField] Text currentHPText;
    [SerializeField] Text maxHPText;

    Pokemon _pokemon;

    public void SetData(Pokemon pokemon){
        _pokemon = pokemon;

        nameText.text = pokemon.Base.Name;
        levelText.text = "Lv " + pokemon.Level;
        
        HPBar.SetHP((float)  pokemon.HP / pokemon.MaxHP);
    }

    public void updateHPText(Pokemon pokemon){
        maxHPText.text =pokemon.MaxHP.ToString();
        currentHPText.text = pokemon.HP.ToString();
    }

    public IEnumerator updateHP(){
        if (_pokemon.HpChanged){
            yield return HPBar.setHPSmooth((float)  _pokemon.HP / _pokemon.MaxHP);
            _pokemon.HpChanged = false;
        }
    }
}
