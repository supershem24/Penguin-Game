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
        
    }
}
