using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] Transform[] levelPart;
    [SerializeField] Vector3 nextPosition;
    [SerializeField] float distanceToSpawn;
    [SerializeField] float distanceToDelete;
    [SerializeField] Transform player;
    void Update()
    {
        DeletePlatform();
        GeneratePlatform();
    }

    private void GeneratePlatform()
    {
        while (Vector2.Distance(player.transform.position, nextPosition) < distanceToSpawn) 
        {
            Transform part = levelPart[Random.Range(0, levelPart.Length)];
            Vector2 newPosition = new Vector2(nextPosition.x - part.Find("StartPoint").position.x, 0);
            Transform newPart = Instantiate(part, newPosition, transform.rotation, transform);
            nextPosition = newPart.Find("EndPoint").position;
        }
    }
    private void DeletePlatform()
    {
        if(transform.childCount > 0)
        {
            Transform partToDelete = transform.GetChild(0);
            if(Vector2.Distance(player.transform.position,partToDelete.transform.position) > distanceToDelete)
            {
                Destroy(partToDelete.gameObject);
            }
        }
    }
}
