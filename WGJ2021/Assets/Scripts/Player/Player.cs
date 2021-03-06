using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Animator animator;

    private CameraController camera;
    private UIController uiController;

    private Vector3 checkpoint; //last visited island position

    private float horizontalMovement, verticalMovement;
    private float speed = 2f;

    private float energy = 0f; //total energy amount
    private bool insideFog = false; //verifies if the player is inside the fog

    //nearby pet
    private bool nearPet = false;
    private GameObject pet;

    private bool isTalking = false;

    private ParticleSystem particles;
    private bool particleState = true;

    private DateTime gameOverTime;
    private int gameOver;
    private bool alreadyDying = false;

    private List<Color> colors = new List<Color>();
    private int colorIndex = 0;

    private bool alreadyFinal = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        camera = FindObjectOfType<CameraController>();
        uiController = FindObjectOfType<UIController>();

        particles = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();

        checkpoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        rigidBody.velocity = new Vector2(0f, 0f);
        if(!isTalking) Move(); //if talking to a pet, the player can't move

        particleController();

        if(nearPet && Input.GetButtonDown("Talk")) triggerDialogue();

        if(insideFog) decreaseEnergy();

        energyVerifier();

        StartCoroutine(changeColor());

        if(colors.Count == 3) final();
    }

    //movemente method
    private void Move(){
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");
        
        rigidBody.velocity = new Vector2(horizontalMovement*speed, verticalMovement*speed);

        if(horizontalMovement != 0 || verticalMovement != 0) animator.SetBool("Moving", true);
        else if(horizontalMovement == 0 && verticalMovement == 0) animator.SetBool("Moving", false);

        animator.SetFloat("Horizontal", horizontalMovement);
        animator.SetFloat("Vertical", verticalMovement);
    }

    private void particleController(){
        if((horizontalMovement != 0 || verticalMovement != 0) && !particleState){
            particleState = true;
            particles.Play();
        }
        else if(horizontalMovement == 0 && verticalMovement == 0 && particleState){
            particleState = false;
            particles.Stop();
        }

        if(horizontalMovement < 0) particles.transform.localRotation = Quaternion.Euler(360f, 90f, 90f);
        else if(horizontalMovement > 0) particles.transform.localRotation = Quaternion.Euler(180f, 90f, 90f);

        if(verticalMovement < 0) particles.transform.localRotation = Quaternion.Euler(270f, 90f, 90f);
        else if(verticalMovement > 0) particles.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
    }

    public float getEnergy(){
        return energy;
    }

    //increase energy amount when gets a right answer
    public void increaseEnergy(float amount){
        energy += amount;

        if(energy > 20f) energy = 20f;

        uiController.increaseEnergy();
    }

    //decrease energy amount when colliding with fog
    public void decreaseEnergy(){
        StartCoroutine(energyCoroutine());
    }

    private IEnumerator energyCoroutine(){
        yield return new WaitForSeconds(0.1f);

        energy -= 1f;

        if(energy < 0) energy = 0;

        uiController.allowDecrease();
    }

    private void energyVerifier(){
        if(!alreadyDying && energy == 0 && insideFog){
            gameOverTime = DateTime.Now;
            gameOver = gameOverTime.Hour * 3600 + gameOverTime.Minute * 60 + gameOverTime.Second + 5;

            alreadyDying = true;
        }

        int aux = DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second;

        if(alreadyDying && aux == gameOver && insideFog) gameOverController();

        if(insideFog == false) StartCoroutine(fogVerifier());
    }

    private IEnumerator fogVerifier(){
        yield return new WaitForSeconds(0.5f);

        if(insideFog == false) alreadyDying = false;
    }

    private void gameOverController(){
        transform.position = checkpoint;

        alreadyDying = false;
    }

    private IEnumerator changeColor(){
        yield return new WaitForSeconds(5f);

        if(colors.Count != 0){
            colorIndex++;

            if(colorIndex == colors.Count) colorIndex = 0;

            GetComponent<SpriteRenderer>().color = colors[colorIndex];
        }
    }

    //start dialogue with the nearby pet
    private void triggerDialogue(){
        isTalking = true;

        pet.GetComponent<Pet>().startDialogue();
    }

    //allow the player to move
    public void stopDialogue(){
        isTalking = false;
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Island"){
            camera.allowZoomIn();
            camera.cancelZoomOut();

            checkpoint = other.transform.position;
        }

        if(other.tag == "Pet"){
            nearPet = true;
            pet = other.gameObject;
        }

        if(other.tag == "Fog"){
            insideFog = true;
            if(energy > 0){
                Destroy(other.gameObject);
                decreaseEnergy();
            }

            other.GetComponent<Pet>().activateSpace();
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if(other.tag == "Island"){
            camera.allowZoomOut();
            camera.cancelZoomIn();
        }

        if(other.tag == "Fog") insideFog = false;

        if(other.tag == "Pet"){
            nearPet = false;
            pet = null;

            other.GetComponent<Pet>().deactivateSpace();
        }
    }

    public void getBlue(){
        Color blue = Color.blue;
        colors.Add(blue);
    }

    public void getRed(){
        Color red = Color.red;
        colors.Add(red);
    }

    public void getGreen(){
        Color green = Color.green;
        colors.Add(green);
    }

    private void final(){
        if(!alreadyFinal){
            alreadyFinal = true;

            isTalking = true;

            camera.transform.position = new Vector3(20, -10, camera.transform.position.z);
            camera.finalZoom();
        }
    }

    public void changeCheckpoint(){
        checkpoint = transform.position;
    }
}