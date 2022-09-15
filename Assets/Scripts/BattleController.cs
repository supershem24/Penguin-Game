using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{

    [SerializeField]
    Canvas canvas;
    [SerializeField]
    MapManager mapManager;
    [SerializeField]
    GameObject attackMenu;
    AttackMenu attackMenuCS;

    [SerializeField]
    List<Unit> Units;
    public List<FriendlyUnit> friendlyUnits;
    public List<EnemyUnit> enemyUnits;



    // Start is called before the first frame update
    void Start()
    {
        attackMenuCS = attackMenu.GetComponent<AttackMenu>();
        Unit.setTilemap(mapManager);
        Unit.setBattleController(this);
        for(int i = 0; i < Units.Count; i++)
        {
            if(Units[i].GetType() == typeof(FriendlyUnit))
            {
                friendlyUnits.Add(Units[i] as FriendlyUnit);
            }
            else
            {
                enemyUnits.Add(Units[i] as EnemyUnit);
            }
        }
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
        //canvas.transform.position = pos;
        
    }

    public void BringUpAttackMenu(Vector2 unitPos)
    {
        attackMenu.transform.position = new Vector2(unitPos.x + 1.25f, unitPos.y + 1f);
        attackMenu.SetActive(true);
    }
}