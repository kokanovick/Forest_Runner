using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxCamera : MonoBehaviour
{
    private GameObject mainCamera;
    [SerializeField] float parallaxEffect;
    private float length;
    private float xPosition;
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera");
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        xPosition = transform.position.x;
    }

    void Update()
    {
        float distanceMoved = mainCamera.transform.position.x * (1 - parallaxEffect);
        float distanceToMove = mainCamera.transform.position.x * parallaxEffect;
        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);
        if(distanceMoved > xPosition + length) 
        {
            xPosition = xPosition + length;
        }
    }
}
