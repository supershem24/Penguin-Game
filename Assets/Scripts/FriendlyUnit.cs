using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyUnit : Unit
{
    //for calcelling a move
    Vector2 pastPos;

    void Update()
    {
        //will be deleted soon
        if (Time.deltaTime < 0.1f)
            currentGridPos = map.groundMap.WorldToCell(gameObject.transform.position);


        if (Input.GetMouseButtonDown(0))
        {
            //for unselecting the unit
            if (isSelected)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int gridPosition = map.groundMap.WorldToCell(mousePosition);

                //this is for selecting a place for the unit to move
                if (map.highlighterMap.GetTile(gridPosition) == map.moveHighlight)
                {
                    map.highlighterMap.ClearAllTiles();
                    Vector2 newPos = map.highlighterMap.CellToWorld(gridPosition);
                    pastPos = gameObject.transform.position;
                    gameObject.transform.position = new Vector2(newPos.x + 0.5f, newPos.y + 0.5f);
                    currentGridPos = gridPosition;
                }
                else
                {
                    map.highlighterMap.ClearAllTiles();
                }
                isSelected = false;
                Unit.oneSelected = false;
            }
        }
    }

}
