using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PassaDeFase : MonoBehaviour
{
    public string nomeDaProximaFase; // Nome da próxima fase a ser carregada

    // Executado no início, antes do primeiro frame
    void Start()
     {

      }

    // Executado a cada frame
    void Update()
     { 
        
     }

    // Detecta a colisão e carrega a próxima fase
    private void OnCollisionEnter2D(Collision2D other) 
    {
        SceneManager.LoadScene(nomeDaProximaFase);  // Carrega a próxima fase
    }
}
