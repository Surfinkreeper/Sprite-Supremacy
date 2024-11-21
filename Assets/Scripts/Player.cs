using UnityEngine;

public class Player : MonoBehaviour
{
    // The scale the player is when the game starts.
    [SerializeField] private float _originalScale = 1.0f;
    // How much the player grows every time it eats an enemy.
    [SerializeField] private float _growth = 0.1f;
    // How fast the player moves.
    [SerializeField] private float _speed = 1.0f;
    // The player's Transform component.
    [SerializeField] private  Transform _playerTransform;
    // The GameController in the scene.
    [SerializeField] private  GameController _gameController;
    [SerializeField] private  Rigidbody2D _playerRigid;

    private void Start()
    {
        _playerRigid = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Runs every frame. Moves the player with WASD input from the player.
    /// </summary>
    /// <returns>Nothing.</returns>
    private void FixedUpdate()
    {
        Vector2 dir = new Vector2();

        if(Input.GetKey(KeyCode.W)) {
            dir += Vector2.up;
        }
        if(Input.GetKey(KeyCode.A)) {
            dir += Vector2.left;
        }
        if(Input.GetKey(KeyCode.D)) {
            dir += Vector2.right;
        }
        if(Input.GetKey(KeyCode.S)) {
            dir += Vector2.down;
        }
        _playerRigid.MovePosition(_playerRigid.position + dir.normalized * _speed * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Resets the player size and center the player on screen.
    /// </summary>
    /// <param name="worldMin">The minimum bounds of the screen.</param>
    /// <param name="worldMax">The maximum bounds of the screen.</param>
    /// <returns>Nothing.</returns>
    public void ResetPlayer(Vector3 worldMin, Vector3 worldMax)
    {
        _playerTransform.localScale = new Vector3(_originalScale,_originalScale,_originalScale);
        _playerTransform.position = new Vector3(0, 0, 0);
    }

    /// <summary>
    /// Grows the player size using _growth.
    /// </summary>
    /// <returns>Nothing.</returns>
    public void Grow ()
    {
        _playerTransform.localScale += new Vector3(_growth, _growth, 0);
        
    }

    /// <summary>
    /// Detects collisions between the Player and Enemies.
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerEnter2D(Collider2D collider)
    {
        _gameController.PlayerHitEnemy(this, collider.GetComponent<Enemy>());
    }
}
