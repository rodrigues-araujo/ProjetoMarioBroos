using UnityEngine;

public class Mushroom : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] bool moveLeft;
    [SerializeField] AudioClip collectSound;

    private AudioSource audioSource;
    private bool wasCollected = false; // Evita pegar mais de uma vez
// Configura o áudio e define direção inicial
    void Start()
    {
        moveLeft = false;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (moveLeft)  // Move o cogumelo na direção atual
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        else
            transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3) // Trata colisão com parede ou jogador
        {
            moveLeft = !moveLeft;
        }

        if (collision.CompareTag("Player") && !wasCollected)
        {
            wasCollected = true;

            // Chama a função no Mario para ativar crescimento por 10 segundos
            collision.GetComponent<Mario>().AtivarEfeitoCrescer(10f);

            // Toca som
            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
            }

            Destroy(gameObject); // some o cogumelo
        }
    }
}
