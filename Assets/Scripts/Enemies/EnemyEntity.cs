using System;
using UnityEngine;

[RequireComponent (typeof(PolygonCollider2D))]
[RequireComponent (typeof(BoxCollider2D))]
[RequireComponent (typeof(EnemyAI))]
public class EnemyEntity : MonoBehaviour
{
    [SerializeField] private EnemySO _enemySO;

    public event EventHandler OnTakeHit;
    public event EventHandler OnDeath;

    private int _currentHealth;

    private PolygonCollider2D _polygonCollider2D;
    private BoxCollider2D _boxCollider2D;
    private EnemyAI _enemyAI;

    private void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        _currentHealth = _enemySO.enemyHealth;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out Player player))
        {
            if (player.IsAlive())
            {
                player.TakeDamage(transform, _enemySO.enemyDamageAmount);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Враг получил урон: " + damage + ", здоровье было: " + _currentHealth);
        _currentHealth -= damage;

        Debug.Log("Здоровье стало: " + _currentHealth);
        OnTakeHit?.Invoke(this, EventArgs.Empty);

        DetectDeath();
    }

    public void PolygonColliderTurnOff()
    {
        _polygonCollider2D.enabled = false;
    }

    public void PolygonColliderTurnOn()
    {
        _polygonCollider2D.enabled = true;
    }

    private void DetectDeath()
    {
        if (_currentHealth <= 0)
        {
            _boxCollider2D.enabled = false;
            _polygonCollider2D.enabled = false;

            _enemyAI.SetDeathState();
            OnDeath?.Invoke(this, EventArgs.Empty);
        }
    }
}
