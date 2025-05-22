using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 3f;
    public float changeDirectionTime = 2f; // Tiempo entre cambios de direcci�n
    private float timer;

    private Vector3 direction;

    void Start()
    {
        PickRandomDirection();
        timer = changeDirectionTime;
    }

    void Update()
    {
        // Movimiento constante
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // Contador para cambiar direcci�n
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            PickRandomDirection();
            timer = changeDirectionTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("El enemigo toc� al jugador. Aqu� perder�a una vida.");
        }
        else
        {
            PickRandomDirection();
            timer = changeDirectionTime; // Reinicia el temporizador
        }
    }

    void PickRandomDirection()
    {
        // Escoge una direcci�n aleatoria en el plano XZ
        int angle = Random.Range(0, 4) * 90; // 0, 90, 180, 270 grados
        transform.rotation = Quaternion.Euler(0, angle, 0);
        direction = transform.forward;
    }
}
