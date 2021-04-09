using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private GameController gameController;

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("Game Controller").GetComponent<GameController>();
    }

    public void SelectLevelButton()
    {
        SceneManager.LoadScene("Select Level");
    }

    public void EditorButton()
    {
        gameController.actualMode = GameController.SceneMode.Editor;
        SceneManager.LoadScene("Editor");
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }

    public void BackButton()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void TutorialButton()
    {
        gameController.actualMode = GameController.SceneMode.Tutorial;
        SceneManager.LoadScene("Editor");
    }
    
    public void EnemiesZooButton()
    {
        gameController.actualMode = GameController.SceneMode.Zoo;
        SceneManager.LoadScene("Editor");
    }
    
    public void ParkourButton()
    {
        gameController.actualMode = GameController.SceneMode.Parkour;
        SceneManager.LoadScene("Editor");
    }
}
