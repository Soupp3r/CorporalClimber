using UnityEngine;
using UnityEngine.UI;

public class AssignCameraToCanvas : MonoBehaviour
{
    [Tooltip("Tag of the Canvas to find")]
    public string canvasTag = "Canvas";

    [Tooltip("Tag of the camera to find and assign")]
    public string cameraTag = "MainCamera";

    [Tooltip("Should we search for objects every frame?")]
    public bool updateEveryFrame = false;

    private Canvas canvas;
    private Camera renderCamera;

    void Start()
    {
        FindAndAssign();
    }

    void Update()
    {
        if (updateEveryFrame)
        {
            FindAndAssign();
        }
    }

    void FindAndAssign()
    {
        // Find Canvas
        GameObject canvasObj = GameObject.FindGameObjectWithTag(canvasTag);
        
        if (canvasObj == null)
        {
            Debug.LogWarning($"No GameObject found with Canvas tag: '{canvasTag}'", this);
            return;
        }

        canvas = canvasObj.GetComponent<Canvas>();
        if (canvas == null)
        {
            Debug.LogWarning($"GameObject with tag '{canvasTag}' has no Canvas component", this);
            return;
        }

        // Find Camera
        GameObject cameraObj = GameObject.FindGameObjectWithTag(cameraTag);
        
        if (cameraObj == null)
        {
            Debug.LogWarning($"No GameObject found with Camera tag: '{cameraTag}'", this);
            return;
        }

        renderCamera = cameraObj.GetComponent<Camera>();
        if (renderCamera == null)
        {
            Debug.LogWarning($"GameObject with tag '{cameraTag}' has no Camera component", this);
            return;
        }

        // Assign camera to canvas
        canvas.worldCamera = renderCamera;
        Debug.Log($"Successfully assigned camera '{cameraObj.name}' to canvas '{canvasObj.name}'", this);
    }

    // Editor method to manually trigger assignment
    [ContextMenu("Assign Camera to Canvas Now")]
    public void AssignNow()
    {
        FindAndAssign();
    }
}