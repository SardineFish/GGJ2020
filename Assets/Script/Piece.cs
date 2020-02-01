using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Mathematics;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody))]
public class Piece : MonoBehaviour
{
    public float BrownianMotionStrength = 10;
    public float BrownianMotionSpeed = 100;
    public float RepairTime = 1;

    public Vector3 TargetPosition;

    private void Awake()
    {
        //GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/white-8");
    }
    public Color Color
    {
        get => GetComponent<SpriteRenderer>().color;
        set => GetComponent<SpriteRenderer>().color = value;
    }
    public static Piece Create(Transform parent, Color color)
    {
        var obj = new GameObject("piece");
        var piece = obj.AddComponent<Piece>();
        piece.Color = color;
        return piece;
    }
    public void Explode(Vector3 pos, float force, float radius)
    {
        GetComponent<Rigidbody>().AddExplosionForce(force, pos, radius);
    }
    public void StartBrownianMotion()
    {
        StartCoroutine(BrowianMotion());
    }

    IEnumerator BrowianMotion()
    {
        GetComponent<SphereCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        var pos = transform.position;
        var r = UnityEngine.Random.value * 100000;
        float t = 0;
        while(true)
        {
            t += Time.deltaTime;
            t = Mathf.Clamp01(t);
            var motion = new Vector2(
                Mathf.PerlinNoise(Time.time * BrownianMotionSpeed, r),
                Mathf.PerlinNoise(r, Time.time * BrownianMotionSpeed)
            );
            transform.position = pos + motion.ToVector3() * Time.deltaTime * BrownianMotionStrength * t;
            yield return null;
        }
    }

    public void StartRepair()
    {
        GetComponent<SphereCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        float2 pos = new float2(TargetPosition.x, TargetPosition.y);
        pos -= 1 / 16;
        pos *= 8;
        pos = math.floor(pos);
        pos = pos / 8;
        TargetPosition = new Vector3(pos.x, pos.y, TargetPosition.z);
        transform.localScale = Vector3.one;
        StartCoroutine(RepairInternal());
    }

    IEnumerator RepairInternal()
    {
        foreach (var t in Utility.TimerNormalized(RepairTime))
        {
            transform.position = Vector3.Lerp(transform.position, TargetPosition, t);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, t);
            yield return null;
        }
    }
}