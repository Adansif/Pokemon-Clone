using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassAnimation : MonoBehaviour
{
     [SerializeField] PlayerController player;

    // Update is called once per frame
    public void setAnimation(){
        if (player.inGrass()){
            transform.position = player.transform.position;
            gameObject.SetActive(true);
            
        }
    }
    public void disableAnimation(){
        gameObject.SetActive(false);
    }
}
