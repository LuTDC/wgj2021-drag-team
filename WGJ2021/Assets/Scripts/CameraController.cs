using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Player player;

    private bool canZoomIn = false, canZoomOut = false; 

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        //follow player
        transform.position = new Vector3(player.gameObject.transform.position.x, player.gameObject.transform.position.y, transform.position.z);

        if(canZoomIn) zoomIn();
        else if(canZoomOut) zoomOut();
    }

    //zoom in when near an island
    public void allowZoomIn(){
        canZoomIn = true;
    }

    public void cancelZoomIn(){
        canZoomIn = false;
    }

    //zoom out when get away from an island
    public void allowZoomOut(){
        canZoomOut = true;
    }

    public void cancelZoomOut(){
        canZoomOut = false;
    }

    private void zoomIn(){
        Camera camera = GetComponent<Camera>();

        camera.orthographicSize -= 0.005f;

        if(camera.orthographicSize <= 2.5f) canZoomIn = false;
    }

    private void zoomOut(){
        Camera camera = GetComponent<Camera>();

        camera.orthographicSize += 0.005f;

        if(camera.orthographicSize >= 5f) canZoomOut = false;
    }
}
