using System;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private GameController gameController; 

    [SerializeField]
    private AudioSource audioSource; 

    [SerializeField]
    private List<AudioClip> clips; 
    private CameraShake cameraShake;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            audioSource.clip = clips[0];
            audioSource.Play();
        }
        if (collision.gameObject.CompareTag("Brick"))
        {
            //audioSource.clip = clips[1];
            //audioSource.Play();
            StartCoroutine(cameraShake.Shake(0.1f, 0.1f)); 
        }
        if (collision.gameObject.CompareTag("Border"))
        {
            audioSource.clip = clips[0];
            audioSource.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DeathZone"))
        {
            gameController.LooseALife();
            Console.WriteLine("Hello");
        }
    }
}