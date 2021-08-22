using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    private Player player;
    private bool canDecrease = false;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canDecrease) decreaseEnergy(); //the player is losing energy
    }

    public void increaseEnergy(){
        StartCoroutine(energyCoroutine());
    }

    //increase energy gradually
    private IEnumerator energyCoroutine(){
        while(slider.value < player.getEnergy()){
            slider.value += 0.1f;

            yield return new WaitForSeconds(0.007f);
        }

        if(slider.value > player.getEnergy()) slider.value = player.getEnergy();
    }

    //decrease energy gradually
    private void decreaseEnergy(){
        slider.value -= 0.1f;

        if(slider.value <= player.getEnergy()){
            slider.value = player.getEnergy();
            canDecrease = false;
        }
    }

    public void allowDecrease(){
        canDecrease = true;
    }
}
