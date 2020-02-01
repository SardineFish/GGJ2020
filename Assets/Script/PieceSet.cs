using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PieceType
{
    Water,
    Wood,
    Glass,
    Pottery,
    Dirt,
}


public class PieceSet : MonoBehaviour
{
    public List<Piece> Pieces = new List<Piece>();
    public PieceType Type; 
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
