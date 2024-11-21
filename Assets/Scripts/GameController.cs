using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Assertions;

public class GameController : MonoBehaviour
{
    // The number of enemies on screen.
    [SerializeField] private int _numEnemies;
    // The prefab used to create Enemy objects.
    [SerializeField] private Enemy _enemyPrefab;

    // References to the Player and UI in the Scene.
    [SerializeField] private Player _player;
    [SerializeField] private PointsUI _pointsUI;
    [SerializeField] private RestartButtonUI _restartButtonUI;
    [SerializeField] private WinUI _winUI;
    [SerializeField] private BoxCollider2D _leftWall;
    [SerializeField] private BoxCollider2D _rightWall;
    [SerializeField] private BoxCollider2D _topWall;
    [SerializeField] private BoxCollider2D _bottomWall;

    // The number of points the player currently has.
    private int _points;
    // The number of points needed to win
    [SerializeField] private int _winpoints = 1;
    // A list of all of the Enemies currently in the Scene.
    private List<Enemy> _enemies;

    private void Start ()
    {
        _enemies = new List<Enemy>();
        InitWallBounds();
        StartGame();
    }

    /// <summary>
    /// Runs every frame. Removes Enemies who have gone off-screen.
    /// </summary>
    /// <returns>Nothing.</returns>
    private void Update()
    {
        for(int i = 0; i < _enemies.Count; i++) {
            if(!TransformIsInWorldBounds(_enemies[i].GetComponent<Transform>())) {
                KillEnemy(_enemies[i]);
            }
        }
    }

    // The maximum world bounds.
    public Vector3 GetWorldMax () =>
                Camera.main.ScreenToWorldPoint(
                    new Vector3(
                        Screen.width,
                        Screen.height,
                        Camera.main.transform.position.z
                    )
                );

    // The minimum world bounds.
    public Vector3 GetWorldMin () =>
                Camera.main.ScreenToWorldPoint(
                    new Vector3(
                        0,
                        0,
                        -Camera.main.transform.position.z
                    )
                );

    /// <summary>
    /// Handles a collision between a player and an enemy.
    /// If the player is bigger, they grow bigger and earn points;
    /// if the enemy is bigger, the player loses.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="enemy"></param>
    /// <returns>Nothing.</returns>
    public void PlayerHitEnemy (Player player, Enemy enemy)
    {
        if(player.GetComponent<Transform>().lossyScale.x < enemy.GetComponent<Transform>().lossyScale.x)
            LoseGame();
        else {
            KillEnemy(enemy);
            IncreasePoints();
            _player.Grow();
        }
    }

    /// <summary>
    /// Sets up the start of the game.
    /// Resets the player and points.
    /// Spawns enemies.
    /// Tells UI about the game starting.
    /// </summary>
    /// <returns>Nothing.</returns>
    public void StartGame ()
    {
        _winUI.HideYouWin();
        _player.ResetPlayer(GetWorldMin(), GetWorldMax());
        _points = 0;
        _pointsUI.ChangePoints(_points);
        for(int i = 1; i <= _numEnemies; i++) {
            SpawnEnemy();
        }
    }

    /// <summary>
    /// Checks if a Transform is contained within the world bounds.
    /// </summary>
    /// <param name="t">The Transform.</param>
    /// <returns>True if the Transform is contained within the world bounds.</returns>
    private bool TransformIsInWorldBounds (Transform t)
    {
        if (t == null)
        {
            Assert.IsNotNull(t);
            return false;
        }

        Vector3 worldMax = GetWorldMax() + new Vector3(3, 6, 0)/2;
        Vector3 worldMin = GetWorldMin() - new Vector3(3, 6, 0)/2;

        Vector3 objMax = new Vector3(
                t.position.x + t.lossyScale.x,
                t.position.y + t.lossyScale.y,
                0
            );

        Vector3 objMin = new Vector3(
                t.position.x - t.lossyScale.x,
                t.position.y - t.lossyScale.y,
                0
            );

        if(objMin.x > worldMax.x || objMin.y > worldMax.y ||
            objMax.x < worldMin.x || objMax.y < worldMin.y
         )
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Destroys an enemy and spawns a new one.
    /// </summary>
    /// <param name="enemy">The Enemy to remove.</param>
    /// <returns>Nothing.</returns>
    private void KillEnemy (Enemy enemy)
    {
        _enemies.Remove(enemy);
        Destroy(enemy.gameObject);
        SpawnEnemy();
    }

    /// <summary>
    /// Adds a new Enemy to the Scene and to the _enemies List.
    /// </summary>
    /// <returns>Nothing.</returns>
    private void SpawnEnemy ()
    {
        Enemy newE = Instantiate(_enemyPrefab);
        newE.InitEnemy(GetWorldMin(), GetWorldMax());
        _enemies.Add(newE);
    }

    /// <summary>
    /// Ends the game. Destroys all Enemies on the screen. Resets the player position. Tells UI the game ended.
    /// </summary>
    /// <returns>Nothing.</returns>
    private void LoseGame ()
    {
        _player.ResetPlayer(GetWorldMin(), GetWorldMax());
        for(int i = 0; i < _enemies.Count; i++) {
            Destroy(_enemies[i].gameObject);
        }
        _enemies.Clear();

        _restartButtonUI.SetGameOver();
    }

    /// <summary>
    /// Increases the player's total points. Tells UI the new point value.
    /// </summary>
    /// <returns>Nothing.</returns>
    private void IncreasePoints ()
    {
        _pointsUI.ChangePoints(++_points);
        if(_points >= _winpoints) {
            WinGame();
        }
    }

    /// <summary>
    /// Displays the You Win text when the player wins the game
    /// </summary>
    /// <returns>Nothing.</returns>
    private void WinGame ()
    {
        _winUI.SetYouWin();
        LoseGame();
    }

    /// <summary>
    /// Adjusts wall bound colliders to world size
    /// </summary>
    /// <returns>Nothing.</returns>
    private void InitWallBounds() {

        // gives a Vector3 containing the size of the world
        Vector3 worldBound = GetWorldMax() - GetWorldMin();

        _leftWall.gameObject.transform.position = new Vector3(GetWorldMin().x-1, 0, 0);
        _rightWall.gameObject.transform.position = new Vector3(GetWorldMax().x+1, 0, 0);

        _bottomWall.gameObject.transform.position = new Vector3(0, GetWorldMin().y-1, 0);
        _topWall.gameObject.transform.position = new Vector3(0, GetWorldMax().y+1, 0);

        _leftWall.size = _rightWall.size = new Vector2(1, worldBound.y/2);
        _bottomWall.size = _topWall.size = new Vector2(worldBound.x/2, 1);

    }
}
