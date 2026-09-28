using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   

    public static CanvasManager Instance; 
    [SerializeField] private GameObject _pauseCanvas; 
    {   
      void Awake()
    {  
        if(Instance !=null && Instance !=this) 
        {   
        Destroy(gameObject); 
        }   
        else 
        {
            Instance = this; 
        } 
    public void ChangeCanvasStatus()

        if(_pauseCanvas.activeInHierarchy)
            {  
                _pauseCanvas.SetActive(true);  
            }
            else 
            {  
                _pauseCanvas.SetActive(false); 
            }
    }

}
