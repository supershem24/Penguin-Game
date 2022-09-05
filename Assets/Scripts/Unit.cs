using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Unit : MonoBehaviour
{
    protected static MapManager map;
    public Vector3Int currentGridPos;

    public static void setTilemap(MapManager map) { Unit.map = map; }

    public int movementRange;
    public bool isSelected;
    //shows if any unit is selected
    public static bool oneSelected;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //will be deleted soon
        if (Time.deltaTime < 0.1f)
            currentGridPos = map.groundMap.WorldToCell(gameObject.transform.position);

    }
}
