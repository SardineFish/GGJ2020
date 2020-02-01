using UnityEngine;
using System.Collections;
using System.Linq;

public class Clock : MonoBehaviour
{
    public Collider2D PickCollider;

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
                Time.timeScale = 5;
            }
        }
        else
            Time.timeScale = 1;
    }
}
