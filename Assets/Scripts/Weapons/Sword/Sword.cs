using System;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private int _damageAmount = 2;

    [SerializeField] PolygonCollider2D _polygonCollider2D;

    public event EventHandler OnSwordSwing;

    private void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
    }

    private void Start()
    {
        AttackColliderTurnOff();
    }

    public void Attack()
    {
        AttackColliderTurnOff();
        AttackColliderTurnOn();

        OnSwordSwing?.Invoke(this, EventArgs.Empty);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Меч коснулся: " + collision.name);
        if (collision.transform.TryGetComponent(out EnemyEntity enemyEntity))
        {
            Debug.Log("Найден враг! Наношу урон: " + _damageAmount);
            enemyEntity.TakeDamage(_damageAmount);
        }
    }

    public void AttackColliderTurnOff()
    {
        _polygonCollider2D.enabled = false;
    }

    public void AttackColliderTurnOn()
    {
        _polygonCollider2D.enabled = true;
    }
}
