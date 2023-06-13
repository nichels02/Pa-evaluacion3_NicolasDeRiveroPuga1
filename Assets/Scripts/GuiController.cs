using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GuiController : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject winCanvas;
    [SerializeField] public string LevelToLoad;

    private bool isPaused;

    public void PauseButton(InputAction.CallbackContext context){
        if(!context.started) return;

        isPaused = !isPaused;
        pauseCanvas.SetActive(isPaused);
    }

    public void ExitFunction(){
        SceneManagerController.Instance.LoadScene("MenuPrincipal");
    }

    public void NextLevel()
    {
        SceneManagerController.Instance.LoadScene(LevelToLoad);
    }
}
