using UnityEngine;
using UnityEngine.UI;
public class CanvasManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   

    public static CanvasManager Instance; 
    public  GameObject pauseCanvas;  
    public Button resumeButton;

    public GameObject gameOverCanvas;  
    public Button retryButton;

    
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
    } 


     public void ChangeCanvasStatus(GameObject canvas, Button selectedButton)
    {
        if(canvas.activeInHierarchy)
            {  
                canvas.SetActive(false);  
            }
            else 
            {  
                canvas.SetActive(true);  
                 selectedButton.Select();
            } 
    } 
    public void ChangeScene(string sceneName) 
    {  
        SceneLoader.Instance.ChangeScene(sceneName);
    } 

}
