using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosLimiter : MonoBehaviour
{
    [SerializeField] Transform start;
    [SerializeField] Transform end;
    [SerializeField] Transform groundLevel;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(start.position, new Vector2(start.position.x, start.position.y + 1000));
        Gizmos.DrawLine(start.position, new Vector2(start.position.x, start.position.y - 1000));

        Gizmos.DrawLine(end.position, new Vector2(end.position.x, end.position.y + 1000));
        Gizmos.DrawLine(end.position, new Vector2(end.position.x, end.position.y - 1000));

        Gizmos.DrawLine(groundLevel.position, new Vector2(groundLevel.position.x + 1000, groundLevel.position.y));
        Gizmos.DrawLine(groundLevel.position, new Vector2(groundLevel.position.x - 1000, groundLevel.position.y));
    }
}
