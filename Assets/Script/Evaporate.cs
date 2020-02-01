using UnityEngine;
using System.Collections;

public class Evaporate : MonoBehaviour
{
    public float Time = 10;
    public SpriteRenderer SpriteRenderer;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(Time + Time * Random.value);
        foreach (var t in Utility.TimerNormalized(1))
        {
            var color = SpriteRenderer.color;
            color.a = 1 - t;
            yield return null;
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
