using Cinemachine;
using UnityEngine;
using UnityEngine.U2D;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera cam;
    [SerializeField]
    private PixelPerfectCamera pixelCam;

    private EditorController editorController;

    private GameObject player;
    private GameObject playerPlaceholder;
    private int zoom;
    
    private void Start()
    {
        editorController = GameObject.FindGameObjectWithTag("TestSave").GetComponent<EditorController>();
        zoom = 0;
    }

    private void Update()
    {
        if(editorController.isTesting)
            ZoomIn();
    }

    public void CameraFollow()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam.Follow = player.transform;
        ZoomIn();
    }

    public void CameraFollowPlaceholder()
    {
        playerPlaceholder = GameObject.FindGameObjectWithTag("PlayerSpawn");
        cam.transform.position = playerPlaceholder.transform.position + new Vector3(0, 0, -10);
    }

    public void MoveRight()
    {
        cam.transform.position += new Vector3(1, 0, 0);
    }
    
    public void MoveLeft()
    {
        cam.transform.position += new Vector3(-1, 0, 0);
    }
    
    public void MoveUp()
    {
        cam.transform.position += new Vector3(0, 1, 0);
    }
    
    public void MoveDown(int x)
    {
        cam.transform.position += new Vector3(0, -1, 0);
        Debug.Log(x);
    }

    public void ZoomIn()
    {
        if (zoom != 1)
        {
            pixelCam.refResolutionX /= 2;
            pixelCam.refResolutionY /= 2;
            zoom++;
        }
    }
    
    public void ZoomOut()
    {
        if (zoom != -1)
        {
            pixelCam.refResolutionX *= 2;
            pixelCam.refResolutionY *= 2;
            zoom--;
        }
    }
}
