using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{

    [SerializeField]
    MapManager mapManager;


    [SerializeField]
    Unit currentUnit;


    // Start is called before the first frame update
    void Start()
    {
        currentUnit.setTilemap(mapManager);
    }

    // Update is called once per frame
    void Update()
    {
        //Camera Changes (CHOOSE DIFFERENT AXIS'S)
        Vector3 pos = Camera.main.transform.position;
        pos.y += Input.GetAxis("Vertical") * 0.02f;
        pos.x += Input.GetAxis("Horizontal") * 0.02f;
        Camera.main.transform.position = pos;
        Camera.main.orthographicSize -= Input.mouseScrollDelta.y * 0.2f;
    }
}
