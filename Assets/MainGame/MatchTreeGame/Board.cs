using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;

public sealed class Board : MonoBehaviour
{
    public static Board Instance { get; private set; }

    public Row[] rows;
    public Tile[,] Tiles { get; private set; }
    public int Width => Tiles.GetLength(0);
    public int Height => Tiles.GetLength(1);
    private readonly List<Tile> _selection = new List<Tile>();
    private const float TweenDuration = 0.25f;
    private void Awake() => Instance = this;
    private bool canSwap = false;
    public ScoreCounter scoreCounter;
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
        if (!Input.GetKeyDown(KeyCode.A)) return;

        foreach (var connectedTile in Tiles[0, 0].GetConnectedTiles()) connectedTile.icon.transform.DOScale(1.25f, TweenDuration).Play();
        
    }
    public async void Select(Tile tile)
    {
        if(!_selection.Contains(tile)) _selection.Add(tile);

        if (_selection.Count < 2) return;

        await Swap(_selection[0], _selection[1]);

        if (CanPop() && canSwap)
        {
            Pop();
            scoreCounter.popCount++;
            scoreCounter.scoreText.text = (scoreCounter.popCount + " / 3"); 
        }
        else if(canSwap)
        {
            await Swap(_selection[0], _selection[1]);
        }

        _selection.Clear();
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
                foreach (var connectedTile in connectedTiles) deflateSequence.Join(connectedTile.icon.transform.DOScale(Vector3.zero, TweenDuration));

                await deflateSequence.Play().AsyncWaitForCompletion();

                ScoreCounter.Instance.Score += tile.Item.value * connectedTiles.Count;
                var inflateSequence = DOTween.Sequence();
                inflateSequence.SetUpdate(true);

                foreach (var connectedTile in connectedTiles)
                {
                    connectedTile.Item = ItemDatabase.Items[UnityEngine.Random.Range(0, ItemDatabase.Items.Length)];

                    inflateSequence.Join(connectedTile.icon.transform.DOScale(Vector3.one, TweenDuration));
                }

                await inflateSequence.Play().AsyncWaitForCompletion();

                x = 0;
                y = 0;
            }
        }
    }

}
