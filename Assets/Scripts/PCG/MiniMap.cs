using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform player;

    void LateUpdate()
    {
        //to follow the player
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;

        //to rotate with the player
        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
    }
}
