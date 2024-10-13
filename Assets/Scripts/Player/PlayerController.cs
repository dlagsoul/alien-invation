using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Variables de movimiento
    public float moveSpeed = 5f;
    public float runningSpeed = 10f;
    public float maxStamina = 100f;
    public float runningStaminaCost = 50f;
    public float jumpStaminaCost = 30f;
    public float staminaRecovery = 30f;
    public float jumpForce = 5f;
    private float stamina;
    private float waitRecoveryTimer = 0f;
    private float staminaCostTimer = 0f;
    private float staminaRecoveryTimer = 0f;
    private bool isRunning = false;
    public bool isJumping = false;
    private Rigidbody rb;
    [SerializeField] private UIBarController _staminaController;
    private Animator _animator;

    void Start()
    {
        // Inicialización
        rb = GetComponent<Rigidbody>();
        stamina = maxStamina;
        _animator = GetComponentInChildren<Animator>();
        ManageStaminaBar();
    }
    void FixedUpdate()
    {
        MovePlayer();
        Jump();
        ManageRunning();
        ManageStaminaBar();
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Animaciones

        float finalSpeed = isRunning ? runningSpeed : moveSpeed;

        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
        Vector3 movement = moveDirection * finalSpeed * Time.deltaTime;

        Vector2 movement2D = new Vector2(moveX, moveZ);
        
        _animator.SetFloat("Move X", movement2D.x);
        _animator.SetFloat("Move Y", movement2D.y);

        rb.MovePosition(rb.position + movement);
    }

    void Jump() {
        // Salto
        RaycastHit hit;
        bool touchingGround = Physics.Raycast(transform.position, Vector3.down, out hit, 0.08f, LayerMask.GetMask("Ground"));
        if (touchingGround && Input.GetButton("Jump")) {
            // Aplicar fuerza hacia arriba
            rb.AddForce(Vector3.up * jumpForce * rb.mass / 2 , ForceMode.Impulse);
            // Si saltas mientras corres, gasta stamina
            if (isRunning) {
                stamina = Mathf.Clamp(stamina - jumpStaminaCost / 4, 0, maxStamina);
            }
        }
        Debug.DrawRay(transform.position, Vector3.down * 0.08f, Color.red);
        isJumping = !touchingGround;
    }

    void ManageRunning() {
        if (!isJumping) {
            // Incrementar los timers
            staminaCostTimer += Time.deltaTime;
            waitRecoveryTimer += Time.deltaTime;
            staminaRecoveryTimer += Time.deltaTime;

            if (Input.GetButton("Running") && stamina > 0) {
                isRunning = true;

                // Si ha pasado el tiempo para reducir stamina
                if (staminaCostTimer >= 0.1f) {
                    stamina = Mathf.Clamp(stamina - runningStaminaCost * 2 * Time.deltaTime, 0, maxStamina);
                    staminaCostTimer = 0f;  // Reiniciamos el temporizador
                }

                // Reiniciar el temporizador de recuperación al correr
                waitRecoveryTimer = 0f;
                staminaRecoveryTimer = 0f;
            } else {
                isRunning = false;

                // Recuperar stamina si ha pasado el tiempo de espera
                if (waitRecoveryTimer >= 1.5f && staminaRecoveryTimer >= 0.1f) {
                    stamina = Mathf.Clamp(stamina + staminaRecovery * 2 * Time.deltaTime, 0, maxStamina);
                    staminaRecoveryTimer = 0f;  // Reiniciamos el temporizador de recuperación
                }
            }
        }
    }

    void ManageStaminaBar() {
        _staminaController.SetFillAmount(stamina, maxStamina);
        _staminaController.SetCounterText(stamina);
    }

}
