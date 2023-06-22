using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CoverPlant : MonoBehaviour
{

    [SerializeField] PlayerController player;
    private SpriteRenderer sprite;
        
    public void Start(){
        sprite = GetComponent<SpriteRenderer>();
    }

    public void SetLayer() {
       
        if (player.IsInGrass()){
            transform.position = player.transform.position;
            sprite.sortingOrder = 10;
            gameObject.SetActive(true);
        }                      
    }

    public void RemoveLayer(){
        gameObject.SetActive(false);
    }
}

//This is just a testing comment for a git commit experiment
