using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CoverPlant coverPlant;
    [SerializeField] List<GrassAnimation> plantAnimations;

    public float moveSpeed;
    private bool isMoving;
    private bool isAbleToRun = true;

    public LayerMask collisionObjectsLayer;
    public LayerMask interactableLayer;
    public LayerMask grassLayer;
    public event Action OnEncountered;
    private  Vector2 input;
    private CharacterAnimator animator;

    public PlayerController(){

    }

//Awake() nos permite llamar al animator para poder sacar las animaciones
    private void Awake(){
        animator = GetComponent<CharacterAnimator>();
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void HandleUpdate()
{
    // Aquí verificamos si se está corriendo o caminando
    if (Input.GetKey(KeyCode.LeftShift) && isAbleToRun)
    {
        moveSpeed = 6f;
    }
    else
    {
        moveSpeed = 3f;
    }

    // Aquí comienza el movimiento
    if (!isMoving)
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        // Elimina el movimiento en diagonal
        if (input.x != 0) input.y = 0;

        if (input != Vector2.zero)
        {
            animator.MoveX = input.x;
            animator.MoveY = input.y;

            var targetPosition = transform.position;
            targetPosition.x += input.x;
            targetPosition.y += input.y;

            if (isAbleToWalk(targetPosition))
                StartCoroutine(Move(targetPosition));
        }
    }

    // Aquí finaliza el movimiento
    animator.IsMoving = isMoving;

    if(Input.GetKeyDown(KeyCode.Z)){
        Interact();
    }
}

void Interact(){
    var facingDir= new Vector3(animator.MoveX, animator.MoveY);
    var interactPos = transform.position + facingDir;

    Debug.DrawLine(transform.position, interactPos, Color.blue, 0.5f);

    var collider = Physics2D.OverlapCircle(interactPos, 0.3f, interactableLayer);

    if (collider != null){
        collider.GetComponent<Interactable>()?.Interact();
    }
}

    IEnumerator Move(Vector3 targetPosition){
        coverPlant.removeLayer();
        isMoving = true;      

        while ((targetPosition -transform.position).sqrMagnitude > Mathf.Epsilon){
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, this.moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition;

        foreach (var plantAnimation in plantAnimations) {
            StartCoroutine(plantAnimation.setAnimation());     
        }
        
        coverPlant.setLayer();
        isMoving = false;
        
        chechIfEncounter();
    }
    //Nos va a permitir saber si el objeto que tenemos enfrente va a collisionar con nosotros
    private bool isAbleToWalk(Vector2 targetPosition){
        if (Physics2D.OverlapCircle(targetPosition, 0.2f, collisionObjectsLayer | interactableLayer) != null){
            return false;
        }
        return true;
    }
    
    private void chechIfEncounter(){
        if (inGrass()){
            if (UnityEngine.Random.Range(1,101) <= 10){ //Colocamos UnityEngine delante de Random para que se diferencie del random de Using.
                animator.IsMoving = false; //Esto desactiva la animacion de caminar cuando estemos en batalla.
                OnEncountered();
            }
        }
    }

    // Este metodo es para ser llamado por coverPlant, GrassAnimation y ckeckIfEncounte(refactorizado)
    public bool inGrass(){
        if (Physics2D.OverlapCircle(transform.position, 0.2f, grassLayer) != null){
            return true;
        } else{
            return false;
        }
    }    
}
