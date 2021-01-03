using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class StartMenuManager : MonoBehaviour
{

    /// <summary>
    ///     GameObjects for the main menu sections
    /// </summary>
    public GameObject mainSection;
    public GameObject helpSection;
    public GameObject creditSection;
    
    /// <summary>
    ///     Async for loading a scene
    /// </summary>
    private AsyncOperation async;
        
    /// <summary>
    ///     Load the ExampleScene
    /// </summary>
    public void LoadSimulationScene()
    {
        async = SceneManager.LoadSceneAsync("MMI/Scenes/ExampleScene");
        async.allowSceneActivation = true;
    }

    /// <summary>
    ///     Open the Help panel
    /// </summary>
    public void helpClicked()
    {
        mainSection.SetActive(false);
        helpSection.SetActive(true);
    }
    
    /// <summary>
    ///     Open the Credit panel
    /// </summary>
    public void creditClicked()
    {
        mainSection.SetActive(false);
        creditSection.SetActive(true);
    }
    
    /// <summary>
    ///     Get back to main menu when in credit or help panel
    /// </summary>
    public void backToMainClicked()
    {
        helpSection.SetActive(false);
        creditSection.SetActive(false);
        mainSection.SetActive(true);
    }
    
    /// <summary>
    ///     Quit the application
    /// </summary>
    public void quitApplication()
    {
        Application.Quit();
    }

}