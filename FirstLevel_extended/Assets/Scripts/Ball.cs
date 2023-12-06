using System;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private GameController gameController; // Controller for the gameplay

    [SerializeField]
    private AudioSource audioSource; // Audiosource that can play clips when the ball hits objects

    [SerializeField]
    private List<AudioClip> clips; // List of different soundeffects that are played when the ball hits objects 

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            audioSource.clip = clips.Count > 0 ? clips[0] : null;
            audioSource.Play();
        }
        if (collision.gameObject.CompareTag("Brick"))
        {
            audioSource.clip = clips.Count > 1 ? clips[1] : null;
            audioSource.Play();
        }
        if (collision.gameObject.CompareTag("Border"))
        {
            audioSource.clip = clips.Count > 2 ? clips[2] : null;
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