using System.Collections.Generic;
using UnityEngine;

public class EnvironmentGenerator : CustomBehaviour
{
    [SerializeField] private float _spawnCooldown;
    [SerializeField] private int _spawnAreaWidth;
    [SerializeField] private GameObject ObjectToSpawn;
    [SerializeField] private LayerMask _layerMask;
    private ObjectSpawnerV2 _pool;
    private List<ObjectToPool> TreeList;
    private float _timeValue;
    private bool _timerOn = false;
    private float _leftOfLeft, _rightOfLeft, _leftOfRight, _rightOfRight, _bottomOfTop, _topOfTop, _topOfBottom, _bottomOfBottom;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        if(gameManager != null)
        {
            gameManager.OnStartGame += StartGame;
            gameManager.OnLevelCompleted += EndGame;
            gameManager.OnLevelFailed += EndGame;
        }
        //var pooler = GameManager.PoolingManager.EnvironmentObjPoolers[(int)EnvironmentObjType.Tree];
        //_pool = pooler;
        //TreeList = new List<ObjectToPool>();
        _pool = GameManager.PoolingManager.EnvironmentObjPoolers[(int)EnvironmentObjType.Tree];
    }

    private void SpawnObject(Vector3 point)
    {
        
        var obj = _pool.GetFromPool();
        obj.objectTransform.position = point;
        //TreeList.Add(obj);
        obj.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        if (GameManager != null)
        {
            GameManager.OnStartGame -= StartGame;
            GameManager.OnLevelCompleted -= EndGame;
            GameManager.OnLevelFailed -= EndGame;
        }
    }
    private void StartGame()
    {
        _timerOn = true;
        //TreeList.Clear();
    }

    private void EndGame()
    {
        _timerOn = false;
        //for (int i = 0; i < TreeList.Count; i++)
        //{
        //    _pool.ReturnObjectToPool(TreeList[i]);
        //}

    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    CalculateSpawnArea();
        //}

        if (!GameManager.IsGamePaused && GameManager.IsGameStarted && _timerOn)
        {
            _timeValue += Time.deltaTime;
            if (_timeValue >= _spawnCooldown)
            {
                _timerOn = false;

                CalculateSpawnArea();

                _timeValue = 0;
                _timerOn = true;
            }
        }

        
    }
    private void CalculateSpawnArea()
    {
        var playerTransformPosition = GameManager.PlayerManager.CurrentPlayer.transform.position;
        _rightOfLeft = playerTransformPosition.x - (MainCamera.orthographicSize/2 + 1);
        _leftOfLeft = _rightOfLeft - _spawnAreaWidth;

        _leftOfRight = playerTransformPosition.x + (MainCamera.orthographicSize/2 + 1);
        _rightOfRight = _leftOfRight + _spawnAreaWidth;

        var randomLeft = Random.Range(_leftOfLeft, _rightOfLeft);
        Debug.Log(randomLeft);
        var randomRight = Random.Range(_leftOfRight, _rightOfRight);
        Debug.Log(randomRight);

        _bottomOfTop = playerTransformPosition.y + (MainCamera.orthographicSize + 1);
        _topOfTop = _bottomOfTop + _spawnAreaWidth;

        _topOfBottom = playerTransformPosition.y - (MainCamera.orthographicSize + 1);
        _bottomOfBottom = _topOfBottom - _spawnAreaWidth;

        var randomTop = Random.Range(_bottomOfTop, _topOfTop);
        //Debug.Log(randomTop);
        var randomBottom = Random.Range(_topOfBottom, _bottomOfBottom);
        //Debug.Log(randomBottom);

        if (GetHorizontal() > 0)
        {
            if (GetVertical() > 0)
            {
                //TopRight
                var randomRightT = Random.Range(playerTransformPosition.x, _rightOfRight);
                float randomRightR = randomRight;
                var top = randomTop;
                if (randomRightT >= _leftOfRight)
                {
                    randomRightR = randomRight;
                    top = Random.Range(playerTransformPosition.y, _topOfTop);
                }
                else
                {
                    randomRightR = Random.Range(playerTransformPosition.x, _leftOfRight);
                    top = randomTop;
                }
                var point = new Vector3(randomRightR, top, playerTransformPosition.z);
                if (!CheckOverlap(point))
                {
                    //Instantiate(ObjectToSpawn, point, Quaternion.identity);
                    SpawnObject(point);

                }
                else 
                { Debug.Log("OVERLAP VAR"); }
            }
            else
            {
                //BottomRight
                var randomRightT = Random.Range(playerTransformPosition.x, _rightOfRight);
                float randomRightR = randomRight;
                var bot = randomBottom;
                if (randomRightT >= _leftOfRight)
                {
                    randomRightR = randomRight;
                    bot = Random.Range(_bottomOfBottom, playerTransformPosition.y);
                }
                else
                {
                    randomRightR = Random.Range(playerTransformPosition.x, _leftOfRight);
                    bot = randomBottom;
                }
                var point = new Vector3(randomRightR, bot, playerTransformPosition.z);
                if (!CheckOverlap(point))
                {
                    //Instantiate(ObjectToSpawn, point, Quaternion.identity);
                    SpawnObject(point);

                }
                else { Debug.Log("OVERLAP VAR"); }
            }
        }
        else
        {
            if (GetVertical() > 0)
            {
                
                //TopLeft
                var randomLeftT = Random.Range(_leftOfLeft, playerTransformPosition.x);
                float randomLeftL = randomLeft;
                var top = randomTop;
                if (randomLeftT <= _rightOfLeft)
                {
                    randomLeftL = randomLeft;
                    top = Random.Range(playerTransformPosition.y, _topOfTop);
                }
                else
                {
                    randomLeftL = Random.Range(_rightOfLeft, playerTransformPosition.x);
                    top = randomTop;
                }
                var point = new Vector3(randomLeftL, top, playerTransformPosition.z);
                if (!CheckOverlap(point))
                {
                    //Instantiate(ObjectToSpawn, point, Quaternion.identity);
                    SpawnObject(point);

                }
                else { Debug.Log("OVERLAP VAR"); }
            }
            else
            {
                //BottomLeft
                var randomLeftT = Random.Range(_leftOfLeft, playerTransformPosition.x);
                float randomLeftL = randomLeft;
                var bot = randomBottom;
                if (randomLeftT <= _rightOfLeft)
                {
                    randomLeftL = randomLeft;
                    bot = Random.Range(_bottomOfBottom, playerTransformPosition.y);
                }
                else
                {
                    randomLeftL = Random.Range(_rightOfLeft, playerTransformPosition.x);
                    bot = randomBottom;
                }
                var point = new Vector3(randomLeftL, bot, playerTransformPosition.z);
                if (!CheckOverlap(point))
                {
                    //Instantiate(ObjectToSpawn, point, Quaternion.identity);
                    SpawnObject(point);

                }
                else { Debug.Log("OVERLAP VAR"); }
            }
        }
    }

    private bool CheckOverlap(Vector3 point)
    {
        var isTrue = false;
        if(Physics2D.OverlapCircle(point, 1, _layerMask) != null)
        {
            isTrue = true;
        }
        return isTrue;
    }

    public float GetHorizontal()
    {
        return GameManager.JoystickManager.variableJoystick.Horizontal;
    }

    public float GetVertical()
    {
        return GameManager.JoystickManager.variableJoystick.Vertical;
    }
}
