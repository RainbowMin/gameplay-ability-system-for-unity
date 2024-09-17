using GAS.Runtime;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private const float BoomDistance = 2.5f;
    private Player _player;
    private Rigidbody2D _rb;
    private AbilitySystemComponent _asc;

    private void Awake()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _asc = gameObject.GetComponent<AbilitySystemComponent>();

        GameRunner.Instance.RegisterEnemy(this);
    }

    private void Update()
    {
        if (Chase()) 
            Boom();
        
        if (_player != null)
        {
            var dir = (Vector2)(_player.transform.position - transform.position);
            dir.Normalize();
            transform.up = dir;
        }
    }

    private void OnDestroy()
    {
        GameRunner.Instance.UnregisterEnemy(this);
        _asc.AttrSet<AS_Fight>().HP.UnregisterPostBaseValueChange(_OnHpChange);
        _asc.AbilityContainer.AbilitySpecs()[GAbilityLib.Bomb.Name].UnregisterEndAbility(_OnBombEnd);
        _asc.AbilityContainer.AbilitySpecs()[GAbilityLib.Die.Name].UnregisterEndAbility(_OnBombEnd);
    }

    public void Init(Player player)
    {
        _player = player;

        _asc.InitWithPreset(1);
        _InitAttributes();

        _asc.AbilityContainer.AbilitySpecs()[GAbilityLib.Bomb.Name].RegisterEndAbility(_OnBombEnd);
        _asc.AbilityContainer.AbilitySpecs()[GAbilityLib.Die.Name].RegisterEndAbility(_OnBombEnd);
    }

    private void _InitAttributes()
    {
        _asc.AttrSet<AS_Fight>().InitHP(10);
        _asc.AttrSet<AS_Fight>().InitSpeed(5);
        _asc.AttrSet<AS_Fight>().InitAtk(20);

        _asc.AttrSet<AS_Fight>().HP.RegisterPostBaseValueChange(_OnHpChange);
    }

    private void _OnBombEnd()
    {
        Destroy(gameObject);
    }

    private void _OnHpChange(AttributeBase attributeBase, float oldValue, float newValue)
    {
        if(newValue <= 0)
        {
            _Die();
        }
    }

    private bool Chase()
    {
        if (_player == null
        || _asc.AttrSet<AS_Fight>().HP.CurrentValue <= 0
        || _asc.HasTag(GTagLib.Event_Ban_Move))
        {
            _rb.velocity = Vector2.zero;
            return false;
        }
        
        var delta = (Vector2)(_player.transform.position - transform.position);
        var speed = _asc.AttrSet<AS_Fight>().Speed.CurrentValue;
        _rb.velocity = delta.normalized * speed;
        
        var distance = delta.magnitude;
        return distance < BoomDistance;
    }

    private void Boom()
    {
        _asc.TryActivateAbility(GAbilityLib.Bomb.Name);

    }

    private void _Die()
    {
        GameRunner.Instance.AddScore();
        _asc.TryActivateAbility(GAbilityLib.Die.Name);
    }
}