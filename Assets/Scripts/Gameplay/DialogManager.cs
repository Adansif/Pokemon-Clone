using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;
    [SerializeField] int letterPerSeconds;

    public event Action OnShowDialog;
    public event Action OnCloseDialog;

    public static DialogManager Instance {get; private set;}

    private void Awake() {
        Instance = this;
    }

    int currentLine = 0;
    bool isTyping;
    Dialog dialog;

    public IEnumerator ShowDialog(Dialog dialog){
        yield return new WaitForEndOfFrame();

        OnShowDialog?.Invoke();

        this.dialog = dialog;
        dialogBox.SetActive(true);
        StartCoroutine(TypeDialog(dialog.Lines[0]));
    }

    public void HandleUpdate(){
        if(Input.GetKeyDown(KeyCode.Z) && !isTyping){
            currentLine++;
            if(currentLine < dialog.Lines.Count){
                StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
            }else{
                currentLine = 0;
                dialogBox.SetActive(false);
                OnCloseDialog?.Invoke();
            }
        }
    }

    public IEnumerator TypeDialog (string dialog){
        isTyping = true;

        dialogText.text = "";
        foreach (var letter in dialog.ToCharArray()){
            dialogText.text += letter;
            yield return new WaitForSeconds (1f/letterPerSeconds); //Esto nos permite ajustar la velocidad de las letras
        }
        isTyping = false;
    }
}
