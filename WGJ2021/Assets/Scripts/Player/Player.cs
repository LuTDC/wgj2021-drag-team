using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private CameraController camera;

    private float horizontalMovement, verticalMovement;
    private float speed = 2f;

    private float energy = 100f;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        camera = FindObjectOfType<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move(){
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");
        
        rigidBody.velocity = new Vector2(horizontalMovement*speed, verticalMovement*speed);
    }

    public float getEnergy(){
        return energy;
    }

    public void decreaseEnergy(){
        energy -= 100;
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Island"){
            camera.allowZoomIn();
            camera.cancelZoomOut();
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if(other.tag == "Island"){
            camera.allowZoomOut();
            camera.cancelZoomIn();
        }
    }
}