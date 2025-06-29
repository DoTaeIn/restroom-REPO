using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Reflection;

public class MiniMapManager : MonoBehaviour
{
    public GameObject roomIconPrefab;  // Image 프리팹
    public GameObject specialRoomIconPrefab;

    public RectTransform miniMapParent; // MiniMapPanel 등

    public int gridWidth = 5;
    public int gridHeight = 5;
    public float iconSpacing = 40f;
    void Awake()
    {
        iconSpacing = 40f;
    }

    private Dictionary<Vector2Int, GameObject> miniRoomIcons = new();

    public void BuildMiniMap(Dictionary<Vector2Int, Room> allRooms)
    {
        foreach (var pair in allRooms)
        {
            Vector2Int gridPos = pair.Key;

            GameObject icon = Instantiate(roomIconPrefab, miniMapParent);
            icon.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                gridPos.x * iconSpacing,
                gridPos.y * iconSpacing
            );

            miniRoomIcons[gridPos] = icon;
        }
        HighlightRoom(new Vector2Int(3, 0));
        FocusOnRoom(new Vector2Int(3, 0));
    }
     public void ReplaceRoomIcon(Vector2Int gridPos)
    {
        if ( miniRoomIcons.TryGetValue(gridPos, out GameObject oldIcon))
        {
            // 위치 저장 후 기존 아이콘 제거
            Vector2 anchoredPos = oldIcon.GetComponent<RectTransform>().anchoredPosition;
            Destroy(oldIcon);

            // 새 프리팹 생성 및 등록
            GameObject newIcon = Instantiate(specialRoomIconPrefab, miniMapParent);
            newIcon.GetComponent<RectTransform>().anchoredPosition = anchoredPos;

             miniRoomIcons[gridPos] = newIcon;
        }
        else
        {
            Debug.LogWarning($"MiniMap: No icon found at position {gridPos}");
        }
    }

     public void FocusOnRoom(Vector2Int currentRoomPos)
    {
        // 현재 방을 중심에 오도록 미니맵 패널을 반대로 이동시킴
        Vector2 offset = new Vector2(
            (currentRoomPos.x) * iconSpacing,
            (currentRoomPos.y) * iconSpacing
        );

        miniMapParent.anchoredPosition = -offset;
    }

    public void HighlightRoom(Vector2Int playerRoomPos)
    {
        foreach (var kvp in miniRoomIcons)
        {
            Image img = kvp.Value.GetComponent<Image>();
            img.color = kvp.Key == playerRoomPos ? Color.green : Color.gray;
        }
    }
}