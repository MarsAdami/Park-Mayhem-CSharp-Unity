using System;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private CharacterController characterController;
    public float playerSpeed = 10f;
    public float gravity = -14f;
    public int PlayerHealth = 100;
    private Vector3 gravityVector;

    //GroundCheck
    public Transform groundCheckPoint;
    public float groundCheckRadius = 0.35f;
    public LayerMask groundLayer;

    public bool isGrounded;
    public float jumpSpeed = 0.5f;

    //UI
    public Slider healthBar;
    public Text healthText;
    public CanvasGroup damageScreenUI;
    public AudioSource takeDamageSound;

    private GameManager gameManager;



    void Start()
    {
        characterController = GetComponent<CharacterController>();
        gameManager = FindAnyObjectByType<GameManager>();
        damageScreenUI.alpha = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        GroundCheck();
        PlayerJumpAndGravity();
        DamageScreenCleaner();

        
    }

    void MovePlayer()
    {
        Vector3 moveVector = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;

        characterController.Move(moveVector * playerSpeed * Time.deltaTime);

        
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheckPoint.position, groundCheckRadius, groundLayer);
    }

    void PlayerJumpAndGravity()
    {
        gravityVector.y += gravity * Time.deltaTime;
        characterController.Move(gravityVector * Time.deltaTime);

        if (isGrounded && gravityVector.y < 0)
        {
            gravityVector.y = -3f;
        }

        if (Input.GetButton("Jump") && isGrounded)
        {
            gravityVector.y = Mathf.Sqrt(jumpSpeed * -2f * gravity);
        }
    }

    public void PlayerTakeDamage(int DamageAmount)
    {
        PlayerHealth -= DamageAmount;
        healthBar.value -= DamageAmount;
        HealthTextUpdate();
        damageScreenUI.alpha = 1;
        takeDamageSound.Play();


        if (PlayerHealth <= 0)
        {
            PlayerDeath();
            healthBar.value = 0;
            HealthTextUpdate();
        }
    }

    private void PlayerDeath()
    {
        gameManager.RestartGame();
    }

    void HealthTextUpdate()
    {
        healthText.text = PlayerHealth.ToString();
    }

    void DamageScreenCleaner()
    {
        if (damageScreenUI.alpha > 0)
        {
            damageScreenUI.alpha -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EndTrigger"))
        {
            gameManager.WinLevel();
        }
    }
}
