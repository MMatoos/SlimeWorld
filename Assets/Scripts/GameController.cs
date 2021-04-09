using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum SceneMode
    {Menu, Tutorial, Zoo, Parkour, Play, Editor}
    public SceneMode actualMode = SceneMode.Menu;
    private static GameController gameController;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (gameController == null)
        {
            gameController = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
