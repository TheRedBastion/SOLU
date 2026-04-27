using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    public InputActionAsset InputActions;

    private InputAction pauseAction;

    private void Awake()
    {
        RefreshInput();
    }
    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }

    public void RefreshInput()
    {
        var playerActionMap = InputActions.FindActionMap("Player");
        pauseAction = playerActionMap.FindAction("Pause");
    }

    void Update()
    {
        if(pauseAction.WasPressedThisFrame())
        {
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void QuitGame()
    {
        Resume();
        SceneManager.LoadScene(0);
    }
}