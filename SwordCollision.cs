using UnityEngine;

public class SwordCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cube"))
        {
            Destroy(collision.gameObject);  // Destroy the cube when it is hit by the sword
            Debug.Log("Cube destroyed!");
        }
    }
}