using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField] AudioClip collectSound; // som ao coletar a estrela
    [SerializeField] float volume = 1f; // volume do som
    

    // Executa quando algo encosta no objeto com o Collider2D
    private void OnTriggerEnter2D(Collider2D collision)
    {
        FindObjectOfType<GerenciadorDeSons>().TocarSomDaStar(); // toca o som do gerenciador

        if (collision.CompareTag("Player")) // verifica se quem encostou foi o jogador
        {
            if (collectSound != null)
                AudioSource.PlayClipAtPoint(collectSound, Camera.main.transform.position, volume); // toca o som

            Destroy(gameObject); // destrói a estrela
        }
    }
}
