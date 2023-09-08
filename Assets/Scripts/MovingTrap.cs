using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTrap : Trap
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform[] movePoints;
    private int points;
    protected override void Start()
    {
        base.Start();
        transform.position = movePoints[0].position;
    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoints[points].position, speed*Time.deltaTime);
        if(Vector2.Distance(transform.position, movePoints[points].position) < .25f)
        {
            points++;
            if(points >= movePoints.Length)
            {
                points = 0;
            }
        }
        if(transform.position.x > movePoints[points].position.x) 
        {
            transform.Rotate(new Vector3(0,0,rotationSpeed*Time.deltaTime));
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, -rotationSpeed * Time.deltaTime));
        }
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
