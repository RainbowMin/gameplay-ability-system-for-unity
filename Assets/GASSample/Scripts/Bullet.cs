using UnityEngine;
using GAS;
using GAS.Runtime;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D _rb;
    private AbilitySystemComponent _asc;
    [SerializeField]private GameplayEffectAsset _geBulletDamageAsset;
    private GameplayEffect _geBulletDamage;
    
    
    private void Awake()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _asc = gameObject.GetComponent<AbilitySystemComponent>();
        _geBulletDamage = new GameplayEffect(_geBulletDamageAsset);
    }

    public void Init(Vector2 position, Vector2 direction, float speed, float damage)
    {
        // 设置出生点，速度
        transform.position = position;
        _rb.velocity = direction * speed;

        _asc.InitWithPreset(1);
        _asc.AttrSet<AS_Bullet>().InitAtk(damage);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 伤害生效
        if(other.gameObject.TryGetComponent(out AbilitySystemComponent enemy))
        {
            if(enemy.HasTag(GTagLib.Faction_Enemy))
            {
                _asc.ApplyGameplayEffectTo(_geBulletDamage, enemy);
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}