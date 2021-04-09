using UnityEngine;

public class PlaceholderDestroy : MonoBehaviour
{
    public PlaceController testSave;
    private void Start()
    {
        testSave = GameObject.FindGameObjectWithTag("TestSave").GetComponent<PlaceController>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject == gameObject && testSave.actualMode == PlaceController.Mode.Delete)
                {
                    Destroy(gameObject);
                }

            }
            
        }
    }
}
