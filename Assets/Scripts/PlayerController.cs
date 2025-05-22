using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public int pelletQuantity;

    private Rigidbody rb;
    private PlayerInput playerInput;

    private Vector2 inputDirection = Vector2.zero;
    private Vector3 currentDirection = Vector3.zero;
    private bool isBlocked = false;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();

        // Si se presiona una nueva dirección, la actualizamos
        if (input != Vector2.zero)
        {
            inputDirection = input;

            // Convertimos la dirección 2D a 3D sobre plano XZ
            currentDirection = new Vector3(inputDirection.x, 0f, inputDirection.y).normalized;

            // Solo cambiamos de dirección si no estamos bloqueados
            if (!isBlocked)
            {
                // Permitimos cambio inmediato si no hay muro
                TryMove(currentDirection);
            }
        }
    }

    void FixedUpdate()
    {
        if (!isBlocked && currentDirection != Vector3.zero)
        {
            Vector3 newPosition = rb.position + currentDirection * speed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);
        }
    }

    void TryMove(Vector3 direction)
    {
        // Se puede agregar lógica extra si se desea bloquear movimiento hasta que se valide el giro
        isBlocked = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isBlocked = true;
            currentDirection = Vector3.zero;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isBlocked = false;

            // Si había una dirección guardada, intentar mover de nuevo
            if (inputDirection != Vector2.zero)
            {
                currentDirection = new Vector3(inputDirection.x, 0f, inputDirection.y).normalized;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pellet"))
        {
            Destroy(other.gameObject);
            pelletQuantity++;
        }
    }
}

