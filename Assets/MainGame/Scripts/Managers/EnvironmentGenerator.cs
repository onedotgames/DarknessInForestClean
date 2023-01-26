using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentGenerator : CustomBehaviour
{
    [SerializeField] private int _spawnAreaWidth;
    [SerializeField] private GameObject ObjectToSpawn;
    private float _leftOfLeft, _rightOfLeft, _leftOfRight, _rightOfRight, _bottomOfTop, _topOfTop, _topOfBottom, _bottomOfBottom;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CalculateSpawnArea();
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
        Debug.Log(randomTop);
        var randomBottom = Random.Range(_topOfBottom, _bottomOfBottom);
        Debug.Log(randomBottom);

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
                Instantiate(ObjectToSpawn, point, Quaternion.identity);
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
                Instantiate(ObjectToSpawn, point, Quaternion.identity);
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
                Instantiate(ObjectToSpawn, point, Quaternion.identity);
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
                Instantiate(ObjectToSpawn, point, Quaternion.identity);
            }
        }
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
