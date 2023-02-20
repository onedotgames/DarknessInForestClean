using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnvironmentObject : CustomBehaviour
{
    [SerializeField] private SpriteRenderer _rnd;
    [SerializeField] private ObjectToPool _objToPool;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        if (GameManager != null)
        {
            GameManager.OnStartGame += StartGame;
            GameManager.OnLevelCompleted += EndGame;
            GameManager.OnLevelFailed += EndGame;
            GameManager.OnRestartGame += RestartGame;
        }
    }
    private void OnDestroy()
    {
        if (GameManager != null)
        {
            GameManager.OnStartGame -= StartGame;
            GameManager.OnLevelCompleted -= EndGame;
            GameManager.OnLevelFailed -= EndGame;
            GameManager.OnRestartGame -= RestartGame;
        }
    }
    private void StartGame()
    {
        
    }
    private void OpenGhost()
    {
        var _mat = this._rnd.material;
        //_mat.EnableKeyword("GHOST_ON");
        this._rnd.material.EnableKeyword("GHOST_ON");
    }

    private void CloseGhost()
    {
        var _mat = this._rnd.material;
        //_mat.DisableKeyword("GHOST_ON");
        this._rnd.material.DisableKeyword("GHOST_ON");
    }
    private void EndGame()
    {
        CloseGhost();
        GameManager.PoolingManager.EnvironmentObjPoolers[(int)EnvironmentObjType.Tree].ReturnObjectToPool(_objToPool);
    }
    private void RestartGame()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TAGS.Player))
        {

            OpenGhost();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TAGS.Player))
        {
            CloseGhost();            
        }
    }
}
