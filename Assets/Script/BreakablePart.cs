using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(SpriteRenderer))]
public class BreakablePart : MonoBehaviour
{
    public float Force = 1;
    public float Radius = 2;
    public GameObject PieceTypePrefab;
    private void OnEnable()
    {
    }

    public void DoBreak()
    {
        var renderer = GetComponent<SpriteRenderer>();
        var sprite = renderer.sprite;
        var wrapper = Instantiate(PieceTypePrefab);
        var pieceSet = wrapper.GetComponent<PieceSet>();
        for (int y = (int)sprite.rect.y; y < sprite.rect.height; y++)
        {
            for (int x = (int)sprite.rect.x; x < sprite.rect.width; x++)
            {
                var obj = Instantiate(GameSystem.Curernt.PiecePrefab);
                obj.transform.parent = wrapper.transform;
                var peice = obj.GetComponent<Piece>();
                peice.Color = sprite.texture.GetPixel(x, y);
                var pos = new Vector2(x, y);
                peice.transform.position = transform.position + ((pos - sprite.pivot) / sprite.pixelsPerUnit).ToVector3();
                peice.Explode(transform.position, Force, Radius);
                pieceSet.Pieces.Add(peice);
            }
        }
        renderer.enabled = false;
    }

    public void RepairFrom(PieceSet pieces)
    {
        StartCoroutine(RepairInternal(pieces));
    }

    IEnumerator RepairInternal(PieceSet pieces)
    {
        var renderer = GetComponent<SpriteRenderer>();
        renderer.enabled = false;
        var sprite = renderer.sprite;

        var piecesMap = new Dictionary<Piece, List<Piece>>();
        foreach (var p in pieces.Pieces)
            piecesMap[p] = new List<Piece>();

        for (int y = (int)sprite.rect.y; y < sprite.rect.height; y++)
        {
            for (int x = (int)sprite.rect.x; x < sprite.rect.width; x++)
            {
                var obj = Instantiate(GameSystem.Curernt.PiecePrefab);
                obj.transform.parent = pieces.transform;
                var piece = obj.GetComponent<Piece>();
                piece.Color = sprite.texture.GetPixel(x, y);
                var pos = new Vector2(x, y);
                piece.TargetPosition = transform.position + ((pos - sprite.pivot) / sprite.pixelsPerUnit).ToVector3();
                piece.GetComponent<Rigidbody>().isKinematic = true;

                var hostPiece = pieces.Pieces[Random.Range(0, pieces.Pieces.Count)];
                if (!piecesMap.ContainsKey(hostPiece))
                    piecesMap[hostPiece] = new List<Piece>();
                piecesMap[hostPiece].Add(piece);
                piece.transform.parent = hostPiece.transform;
                piece.transform.localPosition = Vector3.zero;
                piece.transform.localRotation = Quaternion.identity;
            }
            yield return null;
        }

        while(piecesMap.Count >0)
        {
            var count = Mathf.CeilToInt(piecesMap.Count * Random.value * .8f);
            for (var i = 0; i < count; i++)
            {
                var host = piecesMap.Keys.RandomTake(1).First();
                if (piecesMap[host].Count>0)
                {
                    var p = piecesMap[host][0];
                    piecesMap[host].RemoveAt(0);
                    p.transform.parent = pieces.transform;
                    p.StartRepair();
                }
                if (piecesMap[host].Count <= 0)
                {
                    piecesMap.Remove(host);
                    Destroy(host.gameObject);
                }
            }
            yield return new WaitForSeconds(.1f);
        }

        yield return new WaitForSeconds(1);

        Destroy(pieces.gameObject);
        renderer.enabled = true;
    }

    [EditorButton]
    public void Clear()
    {
        /*Pieces.ForEach(p => Destroy(p.gameObject));
        Pieces.Clear();*/
    }

    // Update is called once per frame
    void Update()
    {

    }
}
