using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Pipe : RepairableObject
{
    public Collider2D PickCollider;
    public Transform Spout;
    public GameObject WaterPrefab;
    public GameObject WaterPiecesPrefab;
    public float Range = .2f;

    GameObject waterCollection;
    public override void Repair(IEnumerable<PieceSet> pieces)
    {

    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        if (Physics2D.OverlapPointAll(camera.ScreenToWorldPoint(Input.mousePosition), 1 << 10).FirstOrDefault() == PickCollider)
        {
            if (Input.GetKey(KeyCode.F))
            {
                if (Random.value < .5f)
                {
                    if(!waterCollection)
                    {
                        waterCollection = Instantiate(WaterPiecesPrefab);
                        waterCollection.transform.position = Spout.position;
                    }
                    var obj = Instantiate(WaterPrefab);
                    obj.transform.parent = waterCollection.transform;
                    obj.transform.position = Spout.position + Random.insideUnitCircle.ToVector3() * Range;
                    waterCollection.GetComponent<PieceSet>().Pieces.Add(obj.GetComponent<Piece>());
                }
            }
        }
    }
}
