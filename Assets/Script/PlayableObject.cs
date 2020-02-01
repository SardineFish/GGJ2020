using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Pick))]
public class PlayableObject: RepairableObject
{
    public float ShakeStrength = 1;
    public float ShakeSpeed = 1;
    public float ShakeTime = 1;

    private void Update()
    {
        if(GetComponent<Pick>().Picked && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(Break());
        }
    }
    IEnumerator Break()
    {
        Vector2 offset = Vector2.zero;
        foreach(var t in Utility.TimerNormalized(ShakeTime))
        {
            var strength = Mathf.Clamp01(Mathf.Pow(t * 3, .5f));
            if (!Input.GetKey(KeyCode.F))
                yield break;

            var shake = new Vector2(
                Mathf.PerlinNoise(Time.time * ShakeSpeed, 0),
                Mathf.PerlinNoise(0, Time.time * ShakeSpeed)
            );
            shake = shake * 2 - Vector2.one;
            shake *= ShakeStrength * strength;
            Debug.Log(shake);
            GetComponentsInChildren<BreakablePart>().ForEach(obj => obj.transform.position += (shake - offset).ToVector3() * Time.deltaTime);
            offset = shake;
            yield return null;
        }

        GetComponentsInChildren<BreakablePart>().ForEach(obj => obj.DoBreak());
        Destroy(gameObject);
    }

    public override void Repair(IEnumerable<PieceSet> pieces)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        var parts = GetComponentsInChildren<BreakablePart>();
        foreach (var piece in pieces)
        {
            var part = parts.Where(p => p.PieceTypePrefab.GetComponent<PieceSet>().Type == piece.Type).Where(p => p.CurrentState != BreakablePart.State.Repairing).First();
            part.RepairFrom(piece);
        }
    }
}