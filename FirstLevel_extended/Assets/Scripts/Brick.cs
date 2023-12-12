using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Brick : MonoBehaviour
{
    // Brick properties 
    [SerializeField]
    private int hitPoints = 1; // how much hits the brick take
    [SerializeField]
    private float reflectingForce = 0.2f; // how much velocity the brick collision adds to the ball (not needed for constant speed ball)
    [SerializeField]
    private BoxCollider boxCollider; // collider reference - allows disabling it, wait for animation to finish and only then destroy the Object
    [SerializeField]
    private ParticleSystem destructionEffect; // have a particle effect to play during brick destruction
    [SerializeField]
    private AudioClip destroySound; // have a soundeffect to play during destruction
    [SerializeField]
    private PlayableDirector director; // the reference to the "Timeline" that has the PlayableDirector component to play at destruction
    [SerializeField]
    private bool isMathBrick = false; // make the distinction if this is a math brick to implement the math learning goal.. 


    // Private fileds
    private int currentHitPoints; // the current hit points the brick has left
    //private AudioSource audioSource; // the reference to the bricks AudioSource Component for easy access in this script

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the hitpoints a brick has, get its AudioSource Component and assign the destroy sound clip to the source
        // (you can switch the clips to have a different sound per brick)
        currentHitPoints = hitPoints;
        //audioSource = GetComponent<AudioSource>();
        //audioSource.clip = destroySound;
    }

    public void SetIsMathBrick(bool isMath)
    {
        isMathBrick = isMath;
    }


    // A method that handles the reflection of the ball from a brick when collision is detected
    private void ReflectBall(Collision collision, Action<int> callback)
    {
        Rigidbody ballRb = GetComponent<Rigidbody>(); // get the Rigidbody of the ball that is colliding 

        // Calculate the reflection of the ball from a brick when collision is detected based on current ball velocity
        ballRb.AddForce(ballRb.velocity * reflectingForce, ForceMode.VelocityChange);
        callback?.Invoke(1); // when collided invoke the callback with number of healthpoints to deduct; in this case --> (1)
    }

    // A method that handles the damage the brick is taking and initiates the destruction of a brick when its healthpoints are depleted
    public void TakeDamage(int damage)
    {
        currentHitPoints -= damage;

        // when healthpoints of the brick are up initiate the destruction method
        if (currentHitPoints <= 0)
        {
            HandleDestruction(); // method that handles the destruction with all its effects
            return;
        }
    }

    // A method to handle the destruction when healthpoints are used up
    public void HandleDestruction()
    {
        if (isMathBrick) MathEvent(); 

        if (boxCollider) boxCollider.enabled = false; 

        if (destructionEffect) destructionEffect.Play(); 

        //if (destroySound && audioSource) audioSource.PlayOneShot(destroySound); // play the destruction sound clip when it has an audiosource

        if (director) director.Play();

        Destroy(gameObject, 1f);

        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.AddPoints(1); 
        }
    }

    private void MathEvent()
    {
        // Annahme: Diese Methode wird aufgerufen, wenn ein Math-Brick zerstört wird

        QuizManager quizManager = FindObjectOfType<QuizManager>();

        if (quizManager != null)
        {
            string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            if (currentScene == "Level1")
            {
                quizManager.SetQuizVisibility(true);
                quizManager.StartQuiz(QuizManager.MathOperation.Addition);
            }
            else if (currentScene == "Level2")
            {
                quizManager.SetQuizVisibility(true);
                quizManager.StartQuiz(QuizManager.MathOperation.Subtraction);
            }
            else if (currentScene == "Level3")
            {
                quizManager.SetQuizVisibility(true);
                quizManager.StartQuiz(QuizManager.MathOperation.Multiplication);
            }
            else if (currentScene == "Level4")
            {
                quizManager.SetQuizVisibility(true);
                quizManager.StartQuiz(QuizManager.MathOperation.Division);
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            ReflectBall(collision, (result) =>
            {
                TakeDamage(result);

                // Check if it's a Math-Brick and set the visibility of the quiz
                if (isMathBrick)
                {
                    QuizManager quizManager = FindObjectOfType<QuizManager>();
                    if (quizManager != null)
                    {
                        // Check the active scene and call the appropriate StartQuiz method
                        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                        if (currentScene == "Level1")
                        {
                            quizManager.SetQuizVisibility(true);
                            quizManager.StartQuiz(QuizManager.MathOperation.Addition);
                        }
                        else if (currentScene == "Level2")
                        {
                            quizManager.SetQuizVisibility(true);
                            quizManager.StartQuiz(QuizManager.MathOperation.Subtraction);
                        }
                    }
                }
            });
        }
    }

    // MathBrick subclass
    public class MathBrick : Brick
    {
        // Extend behaviours for MathBrick 
    }
    // Update is called once per frame
    void Update()
    {

    }
}
