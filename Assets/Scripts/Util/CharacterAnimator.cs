using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] List<Sprite> walkDownSprite;
    [SerializeField] List<Sprite> walkUpSprite;
    [SerializeField] List<Sprite> walkLeftSprite;
    [SerializeField] List<Sprite> walkRightSprite;

    //Parameters
    public float MoveX{get; set;}
    public float MoveY{get; set;}
    public bool IsMoving{get; set;}

    SpriteAnimator walkDownAnim;
    SpriteAnimator walkUpAnim;
    SpriteAnimator walkLeftAnim;
    SpriteAnimator walkRightAnim;

    SpriteAnimator currentAnim;

    bool wasPreviouslyMoving;

    // States
    SpriteRenderer spriteRenderer;

    private void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        walkDownAnim = new SpriteAnimator(walkDownSprite, spriteRenderer);
        walkUpAnim = new SpriteAnimator(walkUpSprite, spriteRenderer);
        walkLeftAnim = new SpriteAnimator(walkLeftSprite, spriteRenderer);
        walkRightAnim = new SpriteAnimator(walkRightSprite, spriteRenderer);

        currentAnim = walkDownAnim;
    }

    private void Update() {
        var prevAnim = currentAnim;

        if (MoveX == 1){
            currentAnim = walkRightAnim;
        }else if(MoveX == -1){
            currentAnim = walkLeftAnim;
        }else if(MoveY == -1){
            currentAnim = walkDownAnim;
        }else if(MoveY == 1){
            currentAnim = walkUpAnim;
        }

        if (currentAnim != prevAnim || IsMoving != wasPreviouslyMoving){
            currentAnim.Start();
        }

        if(IsMoving){
            currentAnim.HandleUpdate();
        }else{
            spriteRenderer.sprite = currentAnim.Frames[0];
        }
        wasPreviouslyMoving = IsMoving;
    }
    
}
