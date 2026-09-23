using UnityEngine;

public class Coin : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.coins= 5;
    }

    // Update is called once per frame
    void Update()
    {
        
    } 
  
void OnTriggerEnter2D(Collider2D collision)
{
    if(collision.gameObject.CompareTag("Player"))

    { 
        GameManager.Instace.Addcoin();
        Destroy(gameObject); 
    } 
} 

}
