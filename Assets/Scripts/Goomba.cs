using Unity.VisualScripting;
using UnityEngine;

public class Goomba : MonoBehaviour
{
    Rigidbody2D rbGoomba;
    [SerializeField] float speed = 2f;
    [SerializeField] Transform point1, point2;
    [SerializeField] LayerMask layer;
    [SerializeField] bool isColliding;

    Animator animGoomba;
    BoxCollider2D colliderGoomba;
    AudioSource audioSource; // Adiciona o AudioSource

    // Pega os componentes do Goomba no início
    private void Awake()
    {
        rbGoomba = GetComponent<Rigidbody2D>(); 
        animGoomba = GetComponent<Animator>();
        colliderGoomba = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    // Move o Goomba e faz ele virar quando bate no limite
    private void FixedUpdate()
    {
        rbGoomba.linearVelocity = new Vector2(speed, rbGoomba.linearVelocity.y); // Corrigido 'velocity'

        isColliding = Physics2D.Linecast(point1.position, point2.position, layer);

        if (isColliding)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            speed *= -1;
        }
    }

    // Executa ao encostar em outro Collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // Se o jogador pulou na cabeça
            if (transform.position.y + 0.5f < collision.transform.position.y)
            {
                collision.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
                collision.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 6, ForceMode2D.Impulse);
                
                animGoomba.SetTrigger("Death");
                speed = 0;
                colliderGoomba.enabled = false;

                if (audioSource != null && audioSource.clip != null)
                    audioSource.Play(); // Toca o som

                Destroy(gameObject, audioSource.clip.length);
            }
            else
            {
                // Se o jogador foi atingido de lado
                if (Mario.isGrow)
                {
                    Mario.isGrow = false;
                }
                else
                {
                    FindFirstObjectByType<Mario>().Death();

                    Goomba[] goomba = FindObjectsByType<Goomba>(FindObjectsSortMode.None);
                    for (int i = 0; i < goomba.Length; i++)
                    {
                        goomba[i].speed = 0;
                        goomba[i].animGoomba.speed = 0;
                    }
                } 
            }
        }
    }
}
