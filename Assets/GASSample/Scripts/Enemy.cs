using UnityEngine;

public class Enemy : MonoBehaviour
{
    private const float BoomDistance = 2.5f;
    private Player _player;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        GameRunner.Instance.RegisterEnemy(this);
    }

    private void OnDestroy()
    {
        GameRunner.Instance.UnregisterEnemy(this);
    }
    
    private void Update()
    {
        if (Chase()) Boom();
        
        if (_player != null)
        {
            var dir = (Vector2)(_player.transform.position - transform.position);
            dir.Normalize();
            transform.up = dir;
        }
    }

    public void Init(Player player)
    {
        _player = player;
    }

    private bool Chase()
    {
        if (_player == null)
        {
            _rb.velocity =Vector2.zero;
            return false;
        }
        
        var delta = (Vector2)(_player.transform.position - transform.position);
        var speed = 5;
        _rb.velocity = delta.normalized * speed;
        
        var distance = delta.magnitude;
        return distance < BoomDistance;
    }

    private void Boom()
    {
    }

    private void Die()
    {
        GameRunner.Instance.AddScore();
    }
}