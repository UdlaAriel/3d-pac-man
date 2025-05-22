using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 3f;
    public float changeDirectionTime = 2f;
    private float timer;

    private Vector3 direction;
    private Transform player; // Referencia al jugador

    public float detectionRange = 5f; // Distancia de detección

    private enum State { Roaming, Chasing }
    private State currentState = State.Roaming;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        PickRandomDirection();
        timer = changeDirectionTime;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            currentState = State.Chasing;
        }
        else
        {
            currentState = State.Roaming;
        }

        if (currentState == State.Chasing)
        {
            // Dirección hacia el jugador
            direction = (player.position - transform.position).normalized;
        }
        else
        {
            // Movimiento aleatorio por tiempo
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                PickRandomDirection();
                timer = changeDirectionTime;
            }
        }

        // Mover al enemigo
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("El enemigo tocó al jugador. Aquí perdería una vida.");
        }
        else
        {
            if (currentState == State.Roaming)
            {
                PickRandomDirection();
                timer = changeDirectionTime;
            }
        }
    }

    void PickRandomDirection()
    {
        int angle = Random.Range(0, 4) * 90; // 0, 90, 180, 270 grados
        transform.rotation = Quaternion.Euler(0, angle, 0);
        direction = transform.forward;
    }
}
