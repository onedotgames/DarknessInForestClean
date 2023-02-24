using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine.UI;
public sealed class Board : MonoBehaviour
{
    public static Board Instance { get; private set; }

    public Row[] rows;
    public Tile[,] Tiles { get; private set; }
    public int Width => Tiles.GetLength(0);
    public int Height => Tiles.GetLength(1);
    public List<Tile> _selection = new List<Tile>();
    private const float TweenDuration = 0.25f;
    private void Awake() => Instance = this;
    private bool canSwap = false;
    public ScoreCounter scoreCounter;
    public bool canSelect = true;
    public List<Button> TileButtons;
    public Vector2 clickedMousePos;
    public Vector2 finalPos;
    public LayerMask layerMask;
    private RaycastHit raycastHit;
    private void Start()
    {
        Tiles = new Tile[rows.Max(row => row.tiles.Length), rows.Length];

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var tile = rows[y].tiles[x];

                tile.x = x;
                tile.y = y;

                tile.Item = ItemDatabase.Items[UnityEngine.Random.Range(0, ItemDatabase.Items.Length)];
                Tiles[x, y] = tile;
            }
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickedMousePos = Input.mousePosition;
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, Mathf.Infinity, layerMask);
            if(hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("Match3Tile"))
                {
                    _selection.Add(hit.collider.gameObject.GetComponent<Tile>());
                }
            }
        }
        if (Input.GetMouseButton(0))
        {
            finalPos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log(clickedMousePos + " - " + finalPos);
            if(clickedMousePos.x + 50 < finalPos.x && clickedMousePos.y - 50 < finalPos.y && clickedMousePos.y + 50 > finalPos.y)
            {//sağ
                if(_selection[0] != null && _selection[0].Right != null)
                {
                    //_selection.Add(_selection[0].Right);
                    Select(_selection[0].Right);
                }
            }
            else if(clickedMousePos.x - 50 > finalPos.x && clickedMousePos.y - 50 < finalPos.y && clickedMousePos.y + 50 > finalPos.y)
            {//sol
                if (_selection[0] != null && _selection[0].Left != null)
                {
                    //_selection.Add(_selection[0].Left);
                    Select(_selection[0].Left);
                }
            }
            else if(clickedMousePos.y + 50 < finalPos.y && clickedMousePos.x - 50 < finalPos.x && clickedMousePos.x + 50 > finalPos.x)
            { //yukarı
                if (_selection[0] != null && _selection[0].Top != null)
                {
                    //_selection.Add(_selection[0].Top);
                    Select(_selection[0].Top);
                }
            }
            else if (clickedMousePos.y - 50 > finalPos.y && clickedMousePos.x - 50 < finalPos.x && clickedMousePos.x + 50 > finalPos.x)
            {//asağı
                if (_selection[0] != null && _selection[0].Bottom != null)
                {
                    //_selection.Add(_selection[0].Bottom);
                    Select(_selection[0].Bottom);
                }
            }
        }
        if (!canSelect)
        {
            for (int i = 0; i < TileButtons.Count; i++)
            {
                TileButtons[i].interactable = false;
            }
        }
        foreach (var connectedTile in Tiles[0, 0].GetConnectedTiles()) connectedTile.icon.transform.DOScale(1f, TweenDuration).Play();
        
    }
    public async void Select(Tile tile)
    {
        if(!_selection.Contains(tile)) _selection.Add(tile);

        if (_selection.Count < 2) return;

        await Swap(_selection[0], _selection[1]);
        canSelect = false;

        if (CanPop() && canSwap)
        {
            Pop();
            
        }
        else if(canSwap)
        {
            await Swap(_selection[0], _selection[1]);
        }

        _selection.Clear();
        canSelect = true;
        for (int i = 0; i < TileButtons.Count; i++)
        {
            TileButtons[i].interactable = true;
        }
    }

    public async Task Swap(Tile tile1, Tile tile2)
    {
        if(tile2 != tile1.Neighbours[0] && tile2 != tile1.Neighbours[1] && tile2 != tile1.Neighbours[2] && tile2 != tile1.Neighbours[3])
        {
            canSwap = false;
            _selection.Clear();
        }
        else
        {
            canSwap = true;
            var icon1 = tile1.icon;
            var icon2 = tile2.icon;

            var icon1Transform = icon1.transform;
            var icon2Transform = icon2.transform;

            var sequence = DOTween.Sequence();
            sequence.SetUpdate(true);
            sequence.Join(icon1Transform.DOMove(icon2Transform.position, TweenDuration))
                .Join(icon2Transform.DOMove(icon1Transform.position, TweenDuration));

            await sequence.Play().AsyncWaitForCompletion();

            icon1Transform.SetParent(tile2.transform);
            icon2Transform.SetParent(tile1.transform);
            tile1.icon = icon2;
            tile2.icon = icon1;

            var tile1Item = tile1.Item;
            tile1.Item = tile2.Item;
            tile2.Item = tile1Item;
        }
        
    }

    private bool CanPop()
    {
        for (var y = 0; y < Height; y++)
            for (var x = 0; x < Width; x++)
                if (Tiles[x, y].GetConnectedTiles().Skip(1).Count() >= 2) 
                    return true;
        
        return false;
    }

    private async void Pop()
    {
        for(var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var tile = Tiles[x, y];

                var connectedTiles = tile.GetConnectedTiles();

                if (connectedTiles.Skip(1).Count() < 2) continue;

                var deflateSequence = DOTween.Sequence();
                deflateSequence.SetUpdate(true);
                if (canSwap)
                {
                    foreach (var connectedTile in connectedTiles)
                    {
                        deflateSequence.Join(connectedTile.icon.transform.DOScale(Vector3.zero, TweenDuration));
                        connectedTile.PopEffect.Play();
                    }
                    await deflateSequence.Play().AsyncWaitForCompletion();
                }

                var inflateSequence = DOTween.Sequence();
                inflateSequence.SetUpdate(true);

                foreach (var connectedTile in connectedTiles)
                {
                    connectedTile.Item = ItemDatabase.Items[UnityEngine.Random.Range(0, ItemDatabase.Items.Length)];
                    inflateSequence.Join(connectedTile.icon.transform.DOScale(Vector3.one, TweenDuration));
                }

                scoreCounter.popCount++;
                scoreCounter.OpenPop();
                if (scoreCounter.popCount >= 5)
                {
                    scoreCounter.RadialShine.gameObject.SetActive(true);
                    StartCoroutine(SuccessRoutine());
                }

                await inflateSequence.Play().AsyncWaitForCompletion();

                x = 0;
                y = 0;
            }
        }
    }

    public IEnumerator SuccessRoutine()
    {
        yield return new WaitForSecondsRealtime(1f);
        scoreCounter.SuccessMinigame();
    }

}
