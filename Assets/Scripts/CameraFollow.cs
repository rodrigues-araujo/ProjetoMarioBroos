using Unity.Mathematics;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player;// jogador
    [SerializeField] float minX, maxX; // limites da câmera

    void Start()
    {
        player =  GameObject.FindWithTag("Player").transform;
    }

    // faz a câmera seguir o jogador, respeitando os limites
     void FixedUpdate() 
    {
        if(player.position.x >= transform.position.x)
            transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), 0, transform.position.z);
    }
}
