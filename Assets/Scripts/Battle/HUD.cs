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
    [SerializeField] Image statusImage;
    public List<Sprite> statusSprites;

    Pokemon _pokemon;

    public void SetData(Pokemon pokemon){
        _pokemon = pokemon;

        nameText.text = pokemon.Base.Name;
        levelText.text = "Lv " + pokemon.Level;
        
        HPBar.SetHP((float)  pokemon.HP / pokemon.MaxHP);

        SetStatusImage();
        _pokemon.OnStatusChanged += SetStatusImage;
    }

    public void SetStatusImage(){
        if(_pokemon.Status == null){
            statusImage.enabled = false;
        }else{
           if(_pokemon.Status.Id.ToString().Equals("brn")){
                statusImage.sprite = statusSprites[0];
                statusImage.enabled = true;
           }else if(_pokemon.Status.Id.ToString().Equals("frz")){
                statusImage.sprite = statusSprites[1];
           }else if(_pokemon.Status.Id.ToString().Equals("psn") || _pokemon.Status.Id.ToString().Equals("bpsn")){
                statusImage.sprite = statusSprites[2];
                statusImage.enabled = true;
           }else if(_pokemon.Status.Id.ToString().Equals("par")){
                statusImage.sprite = statusSprites[3];
                statusImage.enabled = true;
                statusImage.enabled = true;
           }else if(_pokemon.Status.Id.ToString().Equals("sle")){
                statusImage.sprite = statusSprites[4];
                statusImage.enabled = true;
           }
        }
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
