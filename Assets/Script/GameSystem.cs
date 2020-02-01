using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public static GameSystem Curernt { get; private set; }
    public GameObject PiecePrefab;
    public GameObject ColliderObject;
    public float Gravity = 60;

    public GameObject Bottle;
    public GameObject Window;
    public GameObject FlowerPod;
    public GameObject Box;
    public GameObject WaterCup;

    public RepairableObject Pipe;

    public GameSystem()
    {
        Curernt = this;
    }

    private void Start()
    {
    }

    private void Update()
    {
        Physics.gravity = -ColliderObject.transform.up * Gravity;
        if (Input.GetKeyDown(KeyCode.R))
            Repare();
    }

    public void Repare()
    {
        StartCoroutine(DoRepare());
    }
    IEnumerator DoRepare()
    {
        var pieces = GameObject.FindGameObjectsWithTag("Pieces").Select(obj => obj.GetComponent<PieceSet>()).Where(p =>
        {
            p.Pieces = p.Pieces.Where(t => t).ToList();
            return p.Pieces.Count > 0;
        });

        pieces.ForEach(p => p.Pieces.ForEach(t => t.StartBrownianMotion()));
        foreach(var _ in Utility.Timer(.2f))
        {
            if (!Input.GetKey(KeyCode.R))
            {
                pieces.ForEach(p => p.Pieces.ForEach(t => t.StopAllCoroutines()));
                yield break;
            }
            yield return null;
        }

        if (pieces.Any(p => p.Type == PieceType.Glass) && pieces.Any(p => p.Type == PieceType.Wood))
        {
            var window = Instantiate(Window).GetComponent<RepairableObject>();
            window.Repair(pieces);
        }
        else if (pieces.Any(p => p.Type == PieceType.Pottery) && pieces.Any(p => p.Type == PieceType.Dirt))
        {
            Instantiate(FlowerPod).GetComponent<RepairableObject>().Repair(pieces);
        }
        else if (pieces.Any(p => p.Type == PieceType.Water) && pieces.Any(p => p.Type == PieceType.Glass))
            Instantiate(WaterCup).GetComponent<RepairableObject>().Repair(pieces);
        else if (pieces.Any(p => p.Type == PieceType.Glass))
            Instantiate(Bottle).GetComponent<RepairableObject>().Repair(pieces);
        else if (pieces.Any(p => p.Type == PieceType.Wood))
            Instantiate(Box).GetComponent<RepairableObject>().Repair(pieces);
        else if (pieces.Any(p => p.Type == PieceType.Water))
            Pipe.Repair(pieces);
            
    }
}