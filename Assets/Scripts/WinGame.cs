using UnityEngine;
using UnityEngine.Serialization;

public class WinGame : MonoBehaviour
{
    [FormerlySerializedAs("saveTest")] public EditorController editorController;

    private void Start()
    {
        editorController = GameObject.FindGameObjectWithTag("TestSave").GetComponent<EditorController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            editorController.isCompleted = true;
        }
    }
}
