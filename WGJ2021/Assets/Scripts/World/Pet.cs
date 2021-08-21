using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour
{
    [SerializeField]
    private GameObject dialogue; //dialogue box

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        dialogue.SetActive(false);

        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startDialogue(){
        dialogue.SetActive(true);
    }

    //if the right answer was chosen
    public void rightAnswer(){
        player.increaseEnergy(10f);
        player.stopDialogue();

        dialogue.SetActive(false);
    }

    //if the wrong answer was chosen
    public void wrongAnswer(){
        player.increaseEnergy(5f);
        player.stopDialogue();

        dialogue.SetActive(false);
    }
}
