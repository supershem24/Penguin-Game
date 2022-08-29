using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Unit : MonoBehaviour
{
    Tilemap map;

    public void setTilemap(Tilemap map) { this.map = map; }

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
        Vector3Int gridPosition = map.WorldToCell(mousePosition);

        TileBase tile = map.GetTile(gridPosition);

        int move = dataFromTiles[tile].movementCost;

        print(tile + ", " + move);
    }
}
