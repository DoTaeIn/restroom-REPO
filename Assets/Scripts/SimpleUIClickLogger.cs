using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(GraphicRaycaster))]
public class SimpleUIClickLogger : MonoBehaviour
{
    GraphicRaycaster _raycaster;
    EventSystem     _events;

    void Awake()
    {
        _raycaster = GetComponent<GraphicRaycaster>();
        _events    = EventSystem.current;
    }

    void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        // 1) Create pointer data at mouse position
        var data = new PointerEventData(_events) { position = Input.mousePosition };

        // 2) Raycast against UI
        var results = new List<RaycastResult>();
        _raycaster.Raycast(data, results);

        // 3) Log the top‐most hit, or “nothing”
        if (results.Count > 0)
            Debug.Log("Clicked UI: " + results[0].gameObject.name);
        else
            Debug.Log("Clicked: nothing in UI");
    }
}