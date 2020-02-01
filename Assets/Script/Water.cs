using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour
{

    public void FlowBack(Vector3 spout)
    {
        StartCoroutine(FlowBackInternal(spout));
    }

    IEnumerator FlowBackInternal(Vector3 spout)
    {
        RaycastHit hit;
        if(Physics.Raycast(new Ray(spout, -GameSystem.Curernt.ColliderObject.transform.up), out hit, 1000, 1 << 8))
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<SphereCollider>().enabled = false;
            var startPos = transform.position;
            /*foreach (var t in Utility.TimerNormalized(1 + .5f * Random.value))
            {
                transform.position = Vector3.Lerp(startPos, hit.point, t);
                yield return null;
            }*/
            GetComponent<Piece>().StopAllCoroutines();
            foreach(var t in Utility.TimerNormalized(1 + .5f * Random.value))
            {
                var pos = new Vector2(
                    Mathf.Lerp(startPos.x, spout.x, Mathf.Pow(t, 1)),
                    Mathf.Lerp(startPos.y, spout.y, Mathf.Pow(t, 5))
                );
                transform.position = pos.ToVector3(spout.z);
                if ((transform.position - spout).magnitude < 0.2f)
                    break;
                yield return null;
            }
        }
        Destroy(gameObject);
    }
}
