using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class RoomCinema : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer video;

    private void Start()
    {
        video.SetDirectAudioVolume(0, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            video.SetDirectAudioVolume(0, video.GetDirectAudioVolume(0) + 0.1f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            video.SetDirectAudioVolume(0, video.GetDirectAudioVolume(0) - 0.1f);
        }
    }
}
