using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Unit : MonoBehaviour
{
    MapManager map;

    public void setTilemap(MapManager map) { this.map = map; }

    int movementRange = 4;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int gridPosition = map.groundMap.WorldToCell(mousePosition);
        map.PlaceHighlight(gridPosition, movementRange);
    }
}
