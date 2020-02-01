using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class RepairableObject : MonoBehaviour
{
    public abstract void Repair(IEnumerable<PieceSet> pieces);
}
