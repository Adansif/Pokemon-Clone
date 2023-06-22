using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassAnimation : MonoBehaviour
{
     [SerializeField] PlayerController player;

    void Update() {
    Debug.Log("Player position: " + player.transform.position);
    Debug.Log("Object position: " + transform.position);
}

    public IEnumerator SetAnimation(){
        if (player.transform.position == gameObject.transform.position){
            Debug.Log("Player detected at position: " + player.transform.position);
            gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            gameObject.SetActive(false);
        }
    }
}
