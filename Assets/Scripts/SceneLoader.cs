using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{ 
    public static SceneLoader Instance;
    
    [SerializeField] private GameObject _loadingCanvas;
    [SerializeField] private Image _loadingBar;
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
        DontDestroyOnLoad(gameObject);

    } 


    public void ChangeScene(string sceneName)
    { 
        StartCoroutine(LoadNewScene(sceneName));
    }
    IEnumerator LoadNewScene(string sceneName)
    {  
        yield return null; 

        _loadingCanvas.SetActive(true); 

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName); 
        asyncLoad.allowSceneActivation = false; 

        float fakeLoadPercentaje = 0;
        while(!asyncLoad.isDone) 
        { 
        // _loadingBar.fillAmount = asyncLoad.progress;
        fakeLoadPercentaje += 0.01f; 
        Mathf.Clamp01(fakeLoadPercentaje);  

        _loadingBar.fillAmount = fakeLoadPercentaje; 

        if(asyncLoad.progress >= 0.9f && fakeLoadPercentaje>= 0.99f)
        { 
            asyncLoad.allowSceneActivation = true; 
        }
        yield return new WaitForSecondsRealtime(0.1f);
        } 
        Time.timeScale = 1; 
        _loadingCanvas.SetActive(false);
    }




}

