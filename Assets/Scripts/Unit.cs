using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Unit : MonoBehaviour
{
    MapManager map;

    public void setTilemap(MapManager map) { this.map = map; }

    int movementRange = 4;
    bool isSelected;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //for unselecting the unit
            if (isSelected)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int gridPosition = map.groundMap.WorldToCell(mousePosition);

                if (map.highlighterMap.HasTile(gridPosition))
                {
                    Vector2 high = map.highlighterMap.CellToWorld(gridPosition);
                    gameObject.transform.position = new Vector2(high.x + 0.5f, high.y + 0.5f);
                }
                isSelected = false;
                map.highlighterMap.ClearAllTiles();
            }
        }
    }

    private void OnMouseDown()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int gridPosition = map.groundMap.WorldToCell(mousePosition);
        map.highlighterMap.ClearAllTiles();
        map.PlaceHighlight(gridPosition, movementRange);
        StartCoroutine(WaitforNextFrame());
    }

    IEnumerator WaitforNextFrame()
    {
        yield return new WaitForEndOfFrame();
        isSelected = true;
    }
}
