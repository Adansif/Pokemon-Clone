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
    [SerializeField] Image HPBarColor;
    public List<Sprite> statusSprites;
    [SerializeField] List<Sprite> healthSprite;

    Pokemon _pokemon;

    public void SetData(Pokemon pokemon){
        _pokemon = pokemon;

        nameText.text = pokemon.Base.Name;
        levelText.text = "Lv " + pokemon.Level;
        
        HPBar.SetHP((float)  pokemon.HP / pokemon.MaxHP);

        SetStatusImage();
        SetBarColor(_pokemon);
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

    public void SetBarColor(Pokemon pokemon){
        if(pokemon.HP <= pokemon.MaxHP/4){
            HPBarColor.sprite = healthSprite[2];
        }else if(pokemon.HP <= pokemon.MaxHP/2){
            HPBarColor.sprite = healthSprite[1];
        }else{
             HPBarColor.sprite = healthSprite[0];
        }
    }

    public void UpdateHPText(Pokemon pokemon){
        maxHPText.text =pokemon.MaxHP.ToString();
        currentHPText.text = pokemon.HP.ToString();
    }

    public IEnumerator UpdateHP(){
        if (_pokemon.HpChanged){
            yield return HPBar.SetHPSmooth((float)  _pokemon.HP / _pokemon.MaxHP);
            _pokemon.HpChanged = false;
        }
    }
}
