using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pet : MonoBehaviour
{
    [SerializeField]
    private GameObject dialogue; //dialogue box

    private Player player;

    [SerializeField]
    private Image fill, frame;

    [SerializeField]
    private Animator emotionAnimator;

    [SerializeField]
    private int color;

    [SerializeField]
    private GameObject arrows, space;

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

        arrows.SetActive(false);
        space.SetActive(false);

        fill.enabled = false;
        frame.enabled = false;
    }

    private IEnumerator closeDialogue(){
        if(color == 0) player.getRed();
        else if(color == 1) player.getGreen();
        else if(color == 2) player.getBlue();

        player.changeCheckpoint();

        yield return new WaitForSeconds(1f);

        player.stopDialogue();

        fill.enabled = true;
        frame.enabled = true;

        dialogue.SetActive(false);
    }

    //if the right answer was chosen
    public void rightAnswer(){
        player.increaseEnergy(10f);
        emotionAnimator.SetTrigger("Happy");

        StartCoroutine(closeDialogue());
    }

    //if the wrong answer was chosen
    public void wrongAnswer(){
        player.increaseEnergy(5f);
        emotionAnimator.SetTrigger("Sad");

        StartCoroutine(closeDialogue());
    }

    public int getColor(){
        return color;
    }

    public void activateSpace(){
        space.SetActive(true);
    }

    public void deactivateSpace(){
        space.SetActive(false);
    }
}
