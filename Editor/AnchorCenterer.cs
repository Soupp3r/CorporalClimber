using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq; // Needed for ToArray() conversion

public static class AnchorCenterer
{
    [MenuItem("Tools/UI/Anchor Centerer/Center Selected RectTransforms")]
    private static void CenterSelectedRectTransforms()
    {
        if (Selection.gameObjects.Length == 0)
        {
            Debug.LogWarning("No objects selected. Please select UI elements with RectTransforms.");
            return;
        }

        List<RectTransform> rectTransforms = GetRectTransforms(Selection.gameObjects);
        Undo.RecordObjects(rectTransforms.ToArray(), "Center Anchors");
        
        foreach (RectTransform rt in rectTransforms)
        {
            CenterAnchors(rt);
        }
        
        Debug.Log($"Centered anchors for {rectTransforms.Count} selected objects");
    }

    [MenuItem("Tools/UI/Anchor Centerer/Center All RectTransforms in Scene")]
    private static void CenterAllRectTransformsInScene()
    {
        RectTransform[] allRectTransforms = Object.FindObjectsByType<RectTransform>(FindObjectsSortMode.None);
        
        if (allRectTransforms.Length == 0)
        {
            Debug.LogWarning("No RectTransforms found in the scene.");
            return;
        }

        Undo.RecordObjects(allRectTransforms, "Center All Anchors");
        
        foreach (RectTransform rt in allRectTransforms)
        {
            CenterAnchors(rt);
        }
        
        Debug.Log($"Centered anchors for {allRectTransforms.Length} objects in scene");
    }

    private static List<RectTransform> GetRectTransforms(GameObject[] gameObjects)
    {
        List<RectTransform> rectTransforms = new List<RectTransform>();
        
        foreach (GameObject go in gameObjects)
        {
            RectTransform rt = go.GetComponent<RectTransform>();
            if (rt != null)
            {
                rectTransforms.Add(rt);
            }
        }
        
        return rectTransforms;
    }

    private static void CenterAnchors(RectTransform rt)
    {
        if (rt == null) return;

        // Store original position and size
        Vector2 originalPosition = rt.anchoredPosition;
        Vector2 originalSize = rt.sizeDelta;
        
        // Get parent dimensions if parent is a RectTransform
        Vector2 parentSize = Vector2.zero;
        if (rt.parent is RectTransform parentRT)
        {
            parentSize = parentRT.rect.size;
        }

        // Calculate normalized anchor positions (center)
        Vector2 centerAnchor = new Vector2(0.5f, 0.5f);
        
        // Set anchors
        rt.anchorMin = centerAnchor;
        rt.anchorMax = centerAnchor;
        
        // Restore position and size
        rt.anchoredPosition = originalPosition;
        rt.sizeDelta = originalSize;
    }
}