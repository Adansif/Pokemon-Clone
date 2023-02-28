using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialog : MonoBehaviour
{
    [SerializeField] int letterPerSeconds;
    [SerializeField] Color highlightedColor;

    [SerializeField] Text dialogText;
    [SerializeField] GameObject optionSelector;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveDetails;

    [SerializeField] List<Text> actionTexts;
    [SerializeField] List<Text> moveTexts;

    [SerializeField] Text ppText;
    [SerializeField] Text typeText;

    public void SetDialog(string dialog){
        dialogText.text = dialog;
    }

    public IEnumerator TypeDialog (string dialog){
        dialogText.text = "";
        foreach (var letter in dialog.ToCharArray()){
            dialogText.text += letter;
            yield return new WaitForSeconds (1f/letterPerSeconds); //Esto nos permite ajustar la velocidad de las letras
        }
        yield return new WaitForSeconds(1f);
    }

    public void enableDialogText (bool isEnabled){
        dialogText.enabled = isEnabled;
    }
    public void enableOptionSelector (bool isEnabled){
        optionSelector.SetActive(isEnabled);
    }
    public void enableMoveSelector (bool isEnabled){
        moveSelector.SetActive(isEnabled);
        moveDetails.SetActive(isEnabled);
    }

    public void updateActionSelection(int selectedAction){
        for (int i=0; i<actionTexts.Count; i++){
            if (i == selectedAction){
                actionTexts[i].color = highlightedColor;
            } else{
                actionTexts[i].color = Color.black;
            }
        }
    }
    public void updateMoveSelection(int selectedMove, Move move){
        for (int i=0; i<moveTexts.Count; i++){
            if (i == selectedMove){
                moveTexts[i].color = highlightedColor;
            } else{
                moveTexts[i].color = Color.black;
            }
        }
        ppText.text = $"PP {move.PP}/{move._base.PP}";
        typeText.text = move._base.Type.ToString();
    }
    
    public void setMoveNames(List<Move> moves){
        for (int i=0; i<moveTexts.Count; ++i){
            if (i < moves.Count){
                moveTexts[i].text = moves[i]._base.Name;
            }else{
                moveTexts[i].text = "-";
            }
        }
    }
}
