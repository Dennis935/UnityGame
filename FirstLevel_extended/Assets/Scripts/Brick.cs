using System;
using UnityEngine;
using UnityEngine.Playables;

public class Brick : MonoBehaviour
{
    [SerializeField] private int hitPoints = 1;
    [SerializeField] private float reflectingForce = 0.2f;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private ParticleSystem destructionEffect;
    [SerializeField] private AudioClip destroySound;
    [SerializeField] private PlayableDirector director;
    [SerializeField] private bool isMathBrick = false;
    [SerializeField] private Material mathBrickMaterial;

    private int currentHitPoints;

    void Start()
    {
        currentHitPoints = hitPoints;
    }

    public void SetIsMathBrick(bool isMath)
    {
        isMathBrick = isMath;
        if (isMathBrick)
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null && mathBrickMaterial != null)
            {
                renderer.material = mathBrickMaterial;
            }
        }
    }

    private void ReflectBall(Collision collision, Action<int> callback)
    {
        Rigidbody ballRb = GetComponent<Rigidbody>();
        ballRb.AddForce(ballRb.velocity * reflectingForce, ForceMode.VelocityChange);
        callback?.Invoke(1);
    }

    public void TakeDamage(int damage)
    {
        currentHitPoints -= damage;
        if (currentHitPoints <= 0)
        {
            HandleDestruction();
            return;
        }
    }

    public void HandleDestruction()
    {
        if (isMathBrick) MathEvent();
        if (boxCollider) boxCollider.enabled = false;
        if (destructionEffect) destructionEffect.Play();
        if (director) director.Play();
        Destroy(gameObject, 1f);
    }

    private void MathEvent()
    {
        QuizManager quizManager = FindObjectOfType<QuizManager>();
        quizManager?.SetQuizVisibility(true);
        quizManager?.StartQuiz();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            ReflectBall(collision, result =>
            {
                TakeDamage(result);
                if (isMathBrick)
                {
                    QuizManager quizManager = FindObjectOfType<QuizManager>();
                    quizManager?.SetQuizVisibility(true);
                }
            });
        }
    }

    public bool IsMathBrick => isMathBrick;
    public Material MathBrickMaterial => mathBrickMaterial;

    public class MathBrick : Brick
    {
        // Extend behaviors for MathBrick 
    }

    void Update()
    {
        // Your update logic here
    }
}
