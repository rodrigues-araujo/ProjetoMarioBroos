using UnityEngine;

public class GerenciadorDeSons : MonoBehaviour
{
    public AudioSource somDaStar;
    public static GerenciadorDeSons instance;

    // Executado ao iniciar o script (antes do Start)
    void Awake()
    {
        // Mantém o objeto entre as cenas e impede duplicação
        DontDestroyOnLoad(this.gameObject);

        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Executado no primeiro frame
    void Start() 
    { 

    }

    // Executado a cada frame
    void Update() 
    { 
        
    }

    // Reproduz o som da chave
    public void TocarSomDaStar()
    {
        somDaStar.Play();
    }
}
