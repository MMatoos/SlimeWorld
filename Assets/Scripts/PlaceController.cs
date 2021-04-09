using UnityEngine;
using UnityEngine.Tilemaps;

public class PlaceController : MonoBehaviour
{
    public int leftTopX;
    public int leftTopY;
    public int rightDownX;
    public int rightDownY;
    [SerializeField] private Tilemap tilemap;
    public Tile tile;
    public GameObject enemy;
    [SerializeField] private GameObject player;
    private EditorController _editorController;
    private bool cursorOn;
    private BuildController buildController;
    public GameObject winPoint;
    
    private Vector3Int pos;
    public Mode actualMode;

    public enum Mode
    { Place, Delete, Enemy, PlayerSpawn, Fill, WinPoint, Nothing}
    void Start()
    {
        actualMode = Mode.Nothing;
        _editorController = GetComponent<EditorController>();
        buildController = GetComponent<BuildController>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && !cursorOn)
        {
            Vector3 x = Input.mousePosition;
            Vector3 xx = (Camera.main.ScreenToWorldPoint(x));
            pos = Vector3Int.FloorToInt(xx);
            pos.z = 0;
            xx.z = 0;

            if ((actualMode == Mode.Place || actualMode == Mode.Delete || actualMode == Mode.Fill) && !_editorController.isTesting)
            {
                SaveCorners(pos);
                Build(pos, tile);
            }
 
            if (actualMode == Mode.PlayerSpawn && !_editorController.isTesting)
            {
                PlayerSpawn(xx, player);
            }

            if (actualMode == Mode.WinPoint && !_editorController.isTesting)
            {
                WinPointSpawn(xx, winPoint);
            }
        }

        if (Input.GetMouseButtonDown(0) && !cursorOn)
        {            
            Vector3 x = Input.mousePosition;
            Vector3 xx = (Camera.main.ScreenToWorldPoint(x));
            pos = Vector3Int.FloorToInt(xx);
            pos.z = 0;
            
            if (actualMode == Mode.Enemy && !_editorController.isTesting)
            {
                xx.z = 0;
                Spawn(xx, enemy);
            }
        }
    }

    public void BuildButton()
    { 
        if(buildController.actualCategory == BuildController.Category.Blocks || buildController.actualCategory == BuildController.Category.Back)
            actualMode = Mode.Place;
        else if (buildController.actualCategory == BuildController.Category.Enemies)
            actualMode = Mode.Enemy;
        StartCoroutine(_editorController.ShowMessage("Mode: Build", 4f));
    }

    public void DestroyButton()
    {
        actualMode = Mode.Delete;
        StartCoroutine(_editorController.ShowMessage("Mode: Destroy", 4f));
    }

    public void SetSpawnButton()
    {
        actualMode = Mode.PlayerSpawn;
        StartCoroutine(_editorController.ShowMessage("Mode: Set Spawn", 4f));
    }
    
    public void SetWinPointButton()
    {
        actualMode = Mode.WinPoint;
        StartCoroutine(_editorController.ShowMessage("Mode: Set Win Point", 4f));
    }

    public void FillButton()
    {
        actualMode = Mode.Fill;
        StartCoroutine(_editorController.ShowMessage("Mode: Fill", 4f));
    }

    public void CursorOn()
    {
        cursorOn = true;
    }

    public void CursorOut()
    {
        cursorOn = false;
    }
    
    private void Build(Vector3Int pos, Tile tile)
    {
        if (actualMode == Mode.Place || actualMode == Mode.Fill)
        {
            if (actualMode == Mode.Fill)
            {
                int fillSize = 4;
                Vector3Int startPos = new Vector3Int(0, -fillSize/2, 0);
                Vector3Int currentPos = new Vector3Int();
 
                tilemap.SetTile(pos, tile);
                for (int i = 0; i < fillSize + 1; i++)
                {
                    currentPos = pos + startPos + new Vector3Int(0, i, 0);
                    if (!tilemap.HasTile(currentPos)) 
                    {
                        tilemap.SetTile(currentPos, tile);
                    }
                }
            }
            else if(actualMode == Mode.Place)
                tilemap.SetTile(pos,tile);
        }
        else if (actualMode == Mode.Delete)
        {
            tilemap.SetTile(pos, null);
        }
    }

    private void Spawn(Vector3 position, GameObject enemy)
    {
        Instantiate(enemy, position + new Vector3(0f, 0f, 0), Quaternion.identity);
    }

    private void PlayerSpawn(Vector3 position, GameObject player)
    {
        Destroy(GameObject.FindGameObjectWithTag("PlayerSpawn"));
        Instantiate(player, position + new Vector3(0f, 0f, 0), Quaternion.identity);  
    }

    private void WinPointSpawn(Vector3 position, GameObject winPoint)
    {
        Destroy(GameObject.FindGameObjectWithTag("WinPointSpawn"));
        Instantiate(winPoint, position + new Vector3(0, 0, 0), Quaternion.identity);
    }

    private void SaveCorners(Vector3Int position)
    {
        if (position.x <= leftTopX)
        {
            leftTopX = position.x;
        }
        if (position.y >= leftTopY)
        {
            leftTopY = position.y;
        }
        
        if (position.x >= rightDownX)
        {
            rightDownX = position.x;
        }
        if (position.y <= rightDownY)
        {
            rightDownY = position.y;
        }
    }
}
