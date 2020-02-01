using UnityEngine;
using System.Collections;
using System.Linq;

[RequireComponent(typeof(Rigidbody))]
public class Pick : MonoBehaviour
{
    public Collider2D PickCollider;
    public Transform PlaceAnchor;
    public bool Picked = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var rigidbody = GetComponent<Rigidbody>();
        var camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        //if (rigidbody.IsSleeping())
            //rigidbody.isKinematic = true;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics2D.OverlapPointAll(camera.ScreenToWorldPoint(Input.mousePosition), 1 << 10).FirstOrDefault() == PickCollider)
            {
                Picked = true;
                StartCoroutine(Drag());
            }
        }
    }

    Vector3 GetPlacePlanePosition(Ray ray)
    {
        var hits = Physics.RaycastAll(ray, 100, 1 << 9);
        var maxHit = hits.MaxWith(hit => hit.distance);
        return maxHit.point;
    }

    IEnumerator Drag()
    {
        var camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        GetComponent<Rigidbody>().isKinematic = true;
        var pointerPos = GetPlacePlanePosition(new Ray(camera.ScreenToWorldPoint(Input.mousePosition), Vector3.forward));
        var objPos = GetPlacePlanePosition(new Ray(PlaceAnchor.transform.position.ToVector2().ToVector3(-10), Vector3.forward));

        while(Input.GetKey(KeyCode.Mouse0))
        {
            var currentPointerPos = GetPlacePlanePosition(new Ray(camera.ScreenToWorldPoint(Input.mousePosition), Vector3.forward));
            var delta = currentPointerPos - pointerPos;
            var targetPos = GetPlacePlanePosition(new Ray((objPos + delta).ToVector2().ToVector3(-10), Vector3.forward));
            Debug.DrawLine(camera.transform.position, currentPointerPos);
            transform.position = transform.position - PlaceAnchor.position + targetPos;
            yield return null;
        }
        GetComponent<Rigidbody>().isKinematic = false;
        Picked = false;
    }

    private void OnMouseDrag()
    {
        Debug.Log(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }
}
