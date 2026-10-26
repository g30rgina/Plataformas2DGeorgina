using UnityEngine;

public class coinrepaso : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter2D(Collider collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GameManager2.Instance.AddCoins(); 
            Destroy(gameObject); 

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
