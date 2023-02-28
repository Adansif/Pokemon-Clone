using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CoverPlant : MonoBehaviour
{

    [SerializeField] PlayerController player;
    private SpriteRenderer sprite;

    public CoverPlant(){

    }
        
    public void Start(){
        sprite = GetComponent<SpriteRenderer>();
    }

    public void setLayer() {
       
        if (player.inGrass()){
            transform.position = player.transform.position;
            sprite.sortingOrder = 10;
            gameObject.SetActive(true);
        }                      
    }

    public void removeLayer(){
        gameObject.SetActive(false);
    }
}
