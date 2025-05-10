using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mario : MonoBehaviour
{
    // Configurações de movimento
    Rigidbody2D rbPlayer;
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 15f;
    [SerializeField] bool querPular;
    [SerializeField] bool inFloor = true;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;

    // Componentes
    Animator animPlayer;
    CapsuleCollider2D playerCollider;
    
    // Estados
    [SerializeField] bool dead = false;
    public static bool isGrow;

    // Áudio
    AudioSource somMorte;
    [SerializeField] AudioClip somPulo;
    [SerializeField] float volumePulo = 0.2f;

    // Inicialização
    private void Awake()
    {
        animPlayer = GetComponent<Animator>();
        rbPlayer = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        somMorte = GetComponent<AudioSource>();
    }

    private void Start()
    {
        dead = false;
        isGrow = false;
    }

    // Atualizações por frame
    private void Update()
    {
        if (dead) return;

        // Verifica se está no chão
        inFloor = Physics2D.Linecast(transform.position, groundCheck.position, groundLayer);
        Debug.DrawLine(transform.position, groundCheck.position, Color.blue);

        animPlayer.SetBool("Jump", !inFloor);

        // Controle de pulo e som
        if (Input.GetButtonDown("Jump") && inFloor)
        {
            querPular = true;
            if (somPulo != null && somMorte != null)
                somMorte.PlayOneShot(somPulo, volumePulo);
        }
        else if (Input.GetButtonUp("Jump") && rbPlayer.velocity.y > 0)
        {
            rbPlayer.velocity = new Vector2(rbPlayer.velocity.x, rbPlayer.velocity.y * 0.5f);
        }

        animPlayer.SetBool("Grow", isGrow);
    }

    // Movimento no FixedUpdate
    private void FixedUpdate()
    {
        if (dead) return;
        Mover();
        Pular();
    }

    void Mover()
    {
        float xMove = Input.GetAxisRaw("Horizontal");
        rbPlayer.velocity = new Vector2(xMove * speed, rbPlayer.velocity.y);
        animPlayer.SetFloat("Speed", Mathf.Abs(xMove));

        // Vira o personagem na direção do movimento
        if (xMove > 0) transform.eulerAngles = new Vector2(0, 0);
        else if (xMove < 0) transform.eulerAngles = new Vector2(0, 180);
    }

    void Pular()
    {
        if (querPular)
        {
            rbPlayer.velocity = Vector2.up * jumpForce;
            querPular = false;
        }
    }

    // Morte do personagem
    public void Death()
    {
        StartCoroutine(DeathCorotine());
    }

    IEnumerator DeathCorotine()
    {
        if (!dead)
        {
            dead = true;
            animPlayer.SetTrigger("Death");
            somMorte.Play();
            yield return new WaitForSeconds(0.5f);
            rbPlayer.velocity = Vector2.zero;
            rbPlayer.AddForce(Vector2.up * 15f, ForceMode2D.Impulse);
            playerCollider.isTrigger = true;
            Invoke("RestartGame", 2.5f);
        }
    }

    
    // A voltar para a Fase atuasl
    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Efeito de crescimento (opcional)
    public void AtivarEfeitoCrescer(float duracao)
    {
        StopCoroutine("DesativarCrescimentoDepoisDeTempo");
        StartCoroutine(DesativarCrescimentoDepoisDeTempo(duracao));
    }

    IEnumerator DesativarCrescimentoDepoisDeTempo(float duracao)
    {
        isGrow = true;
        animPlayer.SetBool("Grow", isGrow);
        yield return new WaitForSeconds(duracao);
        isGrow = false;
        animPlayer.SetBool("Grow", isGrow);
        animPlayer.Play("Idle");
    }
}