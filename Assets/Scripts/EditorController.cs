using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class EditorController : MonoBehaviour
{
    [SerializeField]
    private Canvas gameCanvas;
    [SerializeField]
    private Canvas editorCanvas;
    [SerializeField]
    private Canvas backgroundCanvas;
    
    [SerializeField]
    private Text displayMessage1;
    
    public LevelSaveData savedGame;
    
    public GameObject player;
    public GameObject playerSpawner;
    public GameObject winPoint;
    public GameObject winPointSpawner;
    
    private Tilemap tilemap;
    private AllTiles allTiles;
    private PlaceController placeController;
    private CameraController cameraFollow;
    private GameController gameController;
    private HealthController healthController;
    private StatsController statsController;
    
    private int sizeX;
    private int sizeY;
    public int saveNumber;

    public bool isCompleted;
    public bool isTesting;
    private bool isEditor;
    private bool isLoaded;

    private void Start()
    {
        statsController = GetComponent<StatsController>();
        placeController = GetComponent<PlaceController>();
        allTiles = GetComponent<AllTiles>();
        
        tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
        cameraFollow = GameObject.FindGameObjectWithTag("MCamera").GetComponent<CameraController>();
        healthController = GameObject.FindGameObjectWithTag("Health Controller").GetComponent<HealthController>();
        gameController = GameObject.FindGameObjectWithTag("Game Controller").GetComponent<GameController>();

        if (gameController.actualMode == GameController.SceneMode.Editor)
        {
            isEditor = true;
        }
        StartCoroutine(ShowMessage("Click ESC to quit.", 4f));
    }

    void Update()
    {
        if (Input.GetKeyDown("escape") && !isTesting)
        {
            gameController.actualMode = GameController.SceneMode.Menu;
            SceneManager.LoadScene("Main Menu");
        }
        
        if (Input.GetKeyDown("escape") && !isTesting && isEditor)
        {
            gameController.actualMode = GameController.SceneMode.Menu;
            SceneManager.LoadScene("Main Menu");
        }

        if ((Input.GetKeyDown("escape") && isTesting && isEditor) || (healthController.playerHealth <= 0 && isTesting && isEditor)
            || (isCompleted && isTesting && isEditor))
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length != 0)
            {
                GameObject[] aliveEnemies = GameObject.FindGameObjectsWithTag("Enemy");
                for (int i = 0; i < aliveEnemies.Length; i++)
                {
                    Destroy(aliveEnemies[i]);
                }
            }
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Destroy(player);
            GameObject winFlag = GameObject.FindGameObjectWithTag("Win Point");
            Destroy(winFlag);

            savedGame = SaveManager.Load("testedLevel");
            LoadEnemies();
            LoadPlayerSpawn();
            LoadWinPoint();
            statsController.LoadStats();
            isTesting = false;
            isCompleted = false;
            gameCanvas.enabled = !gameCanvas.enabled;
            editorCanvas.enabled = !editorCanvas.enabled;
            backgroundCanvas.enabled = !backgroundCanvas.enabled;
            cameraFollow.CameraFollowPlaceholder();
        }

        if ((Input.GetKeyDown("escape") && isTesting && !isEditor) || (healthController.playerHealth <= 0 && isTesting && !isEditor)
                                                                   || (isCompleted && isTesting && !isEditor))
        {
            gameController.actualMode = GameController.SceneMode.Menu;
            SceneManager.LoadScene("Select Level");
        }

        if (!isEditor && !isLoaded)
        {
            if (gameController.actualMode == GameController.SceneMode.Tutorial || gameController.actualMode == GameController.SceneMode.Parkour
                || gameController.actualMode == GameController.SceneMode.Zoo)
            {
                LoadButton();
                isLoaded = true;
                PlayButton();
            }
        }
    }

    public void PlayButton()
    {
        if (GameObject.FindGameObjectsWithTag("PlayerSpawn").Length != 0 && GameObject.FindGameObjectsWithTag("WinPointSpawn").Length != 0)
        {
            SaveTiles();
            SaveEnemies();
            SavePlayerSpawn();
            SaveWinPoint();
            statsController.SaveStats();
            SaveManager.Save(savedGame, "testedLevel"); //save

            SpawnPlayer();
            SpawnEnemies();
            SpawnWinPoint();
            statsController.InsertStats();
            healthController.UpdateHealth();
            cameraFollow.CameraFollow();
            isTesting = true;
            StartCoroutine(ShowMessage(" ", 1f));
            editorCanvas.enabled = !editorCanvas.enabled;
            gameCanvas.enabled = !gameCanvas.enabled;
            backgroundCanvas.enabled = !backgroundCanvas.enabled;
        }
        else
        {
            StartCoroutine(ShowMessage("No player's spawnpoint or win point placed!", 4f));
        } 
    }

    public void SaveButton()
    {
        if (!isTesting)
        {
            SaveTiles();
            SaveEnemies();
            SavePlayerSpawn();
            SaveWinPoint();
            statsController.SaveStats();
            SaveManager.Save(savedGame, "save_");
            StartCoroutine(ShowMessage("Saved on slot: " + saveNumber, 4f));
        }
    }

    public void LoadButton()
    {
        int number = GameObject.FindGameObjectWithTag("SaveLoadNumber").GetComponent<SaveLoadNumber>().number;
        if (!isTesting)
        {
            if (gameController.actualMode == GameController.SceneMode.Tutorial)
            {
                savedGame = SaveManager.Load("tutorial");
            }
            else if (gameController.actualMode == GameController.SceneMode.Zoo)
            {
                savedGame = SaveManager.Load("zoo");
            }
            else if (gameController.actualMode == GameController.SceneMode.Parkour)
            {
                savedGame = SaveManager.Load("parkour");
            }
            else
            {
                savedGame = SaveManager.Load("save_" + number);
            }
            
            LoadTiles();
            LoadEnemies();
            LoadPlayerSpawn();
            LoadWinPoint();
            SaveSize();
            statsController.LoadStats();
            
            if(gameController.actualMode == GameController.SceneMode.Editor)
                StartCoroutine(ShowMessage("Loaded from slot: " + number, 4f));
        }
    }

    private void SaveSize()
    {
        sizeX = savedGame.sizeX;
        sizeY = savedGame.sizeY;
        placeController.leftTopX = savedGame.leftTopX;
        placeController.leftTopY = savedGame.leftTopY;
        placeController.rightDownX = savedGame.rightDownX;
        placeController.rightDownY = savedGame.rightDownY;
    }

    void SaveTiles()
    {
        sizeX = Mathf.Abs(placeController.leftTopX) + Mathf.Abs(placeController.rightDownX);
        sizeY = Mathf.Abs(placeController.leftTopY) + Mathf.Abs(placeController.rightDownY);
        if (placeController.leftTopX < 0 && placeController.rightDownX >= 0)
            sizeX += 1;
        if (placeController.leftTopY > 0 && placeController.rightDownY <= 0)
            sizeY += 1;

        SaveSize();
        
        savedGame.savedTiles = new SingleTile[sizeX*sizeY];
        InstantiateArray(savedGame.savedTiles, sizeX*sizeY);
        
        int count = 0;
        for (int y = 0; y < savedGame.sizeY; y++)
        {
            for (int x = 0; x < savedGame.sizeX; x++)
            {
                savedGame.savedTiles[count].position = new Vector3Int(placeController.leftTopX + x, placeController.leftTopY - y, 0); //saved position for all tiles
                savedGame.savedTiles[count].index = CheckTileIndex(tilemap, savedGame.savedTiles[count].position);
                count++;
            }
        }
    }

    void LoadTiles()
    {
        tilemap.ClearAllTiles();
        for (int i = 0; i < savedGame.sizeX * savedGame.sizeY; i++)
        {
            if (savedGame.savedTiles[i].index != 999)
            {
                tilemap.SetTile(savedGame.savedTiles[i].position, allTiles.allTiles[savedGame.savedTiles[i].index]);
            }
            else
            {
                tilemap.SetTile(savedGame.savedTiles[i].position, null);
            }
        }
    }

    int CheckTileIndex(Tilemap tilemap, Vector3Int position)
    {
        for (int i = 0; i < allTiles.tiles.Length; i++)
        {
            if (tilemap.GetTile(position) == allTiles.tiles[i])
            {
                return i;
            }
        }

        for (int i = 0; i < allTiles.back.Length; i++)
        {
            if (tilemap.GetTile(position) == allTiles.back[i])
            {
                return i+allTiles.tiles.Length;
            }
        }
        return 999; 
    }
    
    private void SpawnEnemies()
    {
        GameObject[] enemyPlaceholders = GameObject.FindGameObjectsWithTag("Spawn");
        for (int i = 0; i < enemyPlaceholders.Length; i++)
        {
            if (CheckEnemyIndex(enemyPlaceholders[i]) != 999)
            {
                Instantiate(allTiles.enemies[CheckEnemyIndex(enemyPlaceholders[i])], enemyPlaceholders[i].transform.position,
                    Quaternion.identity);
                Destroy(enemyPlaceholders[i]);
            }
        }
    }

    private int CheckEnemyIndex(GameObject enemy)
    {
        for (int i = 0; i < allTiles.enemies.Length; i++)
        {
            if (enemy.name == allTiles.enemies[i].name + "(Clone)")
            {
                return i;
            }
        }
        return 999;
    }
    
    private int CheckEnemyPlaceholderIndex(GameObject enemy)
    {
        for (int i = 0; i < allTiles.enemiesPlaceholders.Length; i++)
        {
            if (enemy.name == allTiles.enemiesPlaceholders[i].name + "(Clone)" )
            {
                return i;
            }
        }
        return 999;
    }

    private void SpawnPlayer()
    {
        Instantiate(player, GameObject.FindGameObjectWithTag("PlayerSpawn").transform.position, Quaternion.identity);
        Destroy(GameObject.FindGameObjectWithTag("PlayerSpawn"));
    }
    

    private void SaveEnemies()
    {
        GameObject[] enemyPlaceholders = GameObject.FindGameObjectsWithTag("Spawn");
        int size = enemyPlaceholders.Length;
        savedGame.enemiesSize = size;
        savedGame.savedEnemies = new Enemy[size];
        InstantiateArray1(savedGame.savedEnemies, size);
        for (int i = 0; i < size; i++)
        {
            savedGame.savedEnemies[i].position = enemyPlaceholders[i].transform.position;
            savedGame.savedEnemies[i].index = CheckEnemyPlaceholderIndex(enemyPlaceholders[i]);
        }
    }

    private void LoadEnemies()
    {
        GameObject[] enemyPlaceholders = GameObject.FindGameObjectsWithTag("Spawn");
        for (int i = 0; i < enemyPlaceholders.Length; i++)
        {
            Destroy(enemyPlaceholders[i]);
        }
        for (int i = 0; i < savedGame.enemiesSize; i++)
        {
            if (savedGame.savedEnemies[i].index != 999)
            {
                Instantiate(allTiles.enemiesPlaceholders[savedGame.savedEnemies[i].index] ,savedGame.savedEnemies[i].position, Quaternion.identity);
            }
        }
    }
    
    void InstantiateArray(SingleTile[] savedTiles, int size)
    {
        for (int i = 0; i < size; i++)
        {
            savedTiles[i] = new SingleTile();
        }
    }
    
    void InstantiateArray1(Enemy[] savedEnemies, int size)
    {
        for (int i = 0; i < size; i++)
        {
            savedEnemies[i] = new Enemy();
        }
    }

    void SavePlayerSpawn()
    {
        if (GameObject.FindGameObjectsWithTag("PlayerSpawn").Length != 0)
        {
            Vector3 spawnPosition = GameObject.FindGameObjectWithTag("PlayerSpawn").transform.position;
            savedGame.playerPosition = spawnPosition;
        }
    }

    void LoadPlayerSpawn()
    {
        Destroy(GameObject.FindGameObjectWithTag("PlayerSpawn"));
        Instantiate(playerSpawner, savedGame.playerPosition, Quaternion.identity);
    }
    
    private void SpawnWinPoint()
    {
        Instantiate(winPoint, GameObject.FindGameObjectWithTag("WinPointSpawn").transform.position, Quaternion.identity);
        Destroy(GameObject.FindGameObjectWithTag("WinPointSpawn"));
    }
    
    void SaveWinPoint()
    {
        if (GameObject.FindGameObjectsWithTag("WinPointSpawn").Length != 0)
        {
            Vector3 spawnPosition = GameObject.FindGameObjectWithTag("WinPointSpawn").transform.position;
            savedGame.winPointPosition = spawnPosition;
        }
    }
    
    void LoadWinPoint()
    {
        Destroy(GameObject.FindGameObjectWithTag("WinPointSpawn"));
        Instantiate(winPointSpawner, savedGame.winPointPosition, Quaternion.identity);
    }
    
    public IEnumerator ShowMessage (string message1, float delay)
    {
        displayMessage1.gameObject.SetActive(true);
        displayMessage1.text = message1;

        yield return new WaitForSeconds(delay);
        displayMessage1.gameObject.SetActive(false);
    }
}
