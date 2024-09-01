using UnityEngine;
using GAS;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D _rb;
    
    private void Awake()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
    }

    public void Init(Vector2 position, Vector2 direction, float speed, float damage)
    {
        // 设置出生点，速度
        transform.position = position;
        _rb.velocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        /*
        // 伤害生效
        if(other.gameObject.TryGetComponent(out AbilitySystemComponent enemy))
        {
        }
        else
        {
            //Destroy(gameObject);
        }
        */
    }
}