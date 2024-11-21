using UnityEngine;

public class Enemy : MonoBehaviour
{
    // The minimum and maximum size an enemy can spawn at.
    [SerializeField] private float _minSize = 0.1f;
    [SerializeField] private float _maxSize = 0.5f;
    // The minimum and maximum speed an enemy can spawn with.
    [SerializeField] private float _minSpeed = 0.1f;
    [SerializeField] private float _maxSpeed = 10.0f;
    // The enemy object's Transform component.
    [SerializeField] private Transform _enemyTransform;
    [SerializeField] private BoxCollider2D _enemyCollider;

    // The enemy's speed and movement direction.
    private float _speed;
    private Vector3 _direction;
    private float scale;

    /// <summary>
    /// Calls other initializing methods to init the Enemy's start location and movement speed/direction. 
    /// </summary>
    /// <param name="worldMin">The minimum bounds of the screen.</param>
    /// <param name="worldMax">The maximum bounds of the screen.</param>
    /// <returns>Nothing.</returns>
    public void InitEnemy(Vector3 worldMin, Vector3 worldMax)
    {
        InitSize();
        InitLocationAndMovement(worldMin, worldMax);
        
    }

    /// <summary>
    /// Runs every frame. Moves the Enemy across the screen.
    /// </summary>
    /// <returns>Nothing.</returns>
    private void Update()
    {
        _enemyTransform.Translate(_direction * _speed * Time.deltaTime);
    }

    /// <summary>
    /// Sets the enemy to a random size.
    /// </summary>
    /// <returns>Nothing.</returns>
    private void InitSize()
    {
        scale = Random.Range(_minSize, _maxSize);
        _enemyTransform.localScale = new Vector3(scale, scale, 0);
    }

    /// <summary>
    /// Sets the enemy's location to a random location on the edge of the screen,
    ///  the movement speed to a random value between _minSpeed and _maxSpeed,
    ///  and the movement direction based on the spawn position.
    /// </summary>
    /// <param name="worldMin">The minimum bounds of the screen.</param>
    /// <param name="worldMax">The maximum bounds of the screen.</param>
    /// <returns>Nothing.</returns>
    private void InitLocationAndMovement(Vector3 worldMin, Vector3 worldMax)
    {
        // makes sure enemies spawn off the screen
        worldMin -= new Vector3(_enemyCollider.size.x/2, _enemyCollider.size.y/2, 0)*scale;
        worldMax += new Vector3(_enemyCollider.size.x/2, _enemyCollider.size.y/2, 0)*scale;

        int xOrY = UnityEngine.Random.Range(0, 2);
        int negOrPos = UnityEngine.Random.Range(0, 2);

        // pick random position on side of screen based on screen size
        // and whether we're on the top or bottom of the screen
        float pos = UnityEngine.Random.Range(
            xOrY == 0 ? worldMin.x : worldMin.y,
            xOrY == 0 ? worldMax.x : worldMax.y
        );

        // assign position
        if (xOrY == 0)
        {
            _enemyTransform.position = new Vector3(
                pos,
                negOrPos == 0 ? worldMin.y : worldMax.y,
                0
            );
        }
        else
        {
            _enemyTransform.position = new Vector3(
                negOrPos == 0 ? worldMin.x : worldMax.x,
                pos,
                0
            );
        }

        // assign movement direction based on start position
        int dir = negOrPos == 0 ? 1 : -1;
        _direction = new Vector3(
            xOrY == 1 ? 1 : 0,
            xOrY == 0 ? 1 : 0,
            0
        ) * dir;

        // assign random speed
        _speed = UnityEngine.Random.Range(_minSpeed, _maxSpeed);
    }

}
