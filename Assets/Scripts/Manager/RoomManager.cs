using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private float canStayInRoomTime = 30f;

    private void OnCollisionStay(Collision _collision)
    {
        if (_collision.transform.CompareTag("Player"))
        {
            canStayInRoomTime -= Time.deltaTime;
            if (canStayInRoomTime <= 0)
            {
                player.isSoundOn = true;
            }
        }
    }

    private void OnCollisionExit(Collision _collision)
    {
        if (_collision.transform.CompareTag("Player"))
        {
            canStayInRoomTime = 30f;
        }
    }
}