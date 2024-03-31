using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Health _health;
    [SerializeField] private Animator _animator;
    [SerializeField] private EnemyState _currentState;
    [SerializeField] private Transform _deathSplashPrefab;
    [SerializeField] private Transform _lootPrefab;
    [SerializeField] private Collider _collider;
    private Player _player;
    private EnemyStateFactory _enemyStateFactory;
    private bool _isDead = false;
    private bool _hit = false;

    public EnemyState CurrentState
    {
        get => _currentState;
        set => _currentState = value;
    }
    public NavMeshAgent Agent
    {
        get => _agent;
        set => _agent = value;
    }
    public Animator Animator
    {
        get => _animator;
    }
    public Player Player
    {
        get => _player;
    }
    public bool Hit
    {
        get => _hit;
        set => _hit = value;
    }
    public bool IsDead
    {
        get => _isDead;
    }
    public Transform DeathSplashPrefab
    {
        get => _deathSplashPrefab;
    }
    public Collider Collder
    {
        get => _collider;
    }

    private void Awake()
    {
        _health.OnDie += OnDeath;
        _health.OnTakeDamage += OnHit;
        _enemyStateFactory = new EnemyStateFactory(this);   
        _currentState = new EnemyMove(this, _enemyStateFactory);
    }

    private void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    void Update()
    {
        _currentState.UpdateState();       
    }

    private void OnDeath()
    {
        _isDead = true;
        _agent.enabled = false;
        _health.OnDie -= OnDeath;
        _health.OnTakeDamage -= OnHit;
        int random = Random.Range(0, 3);
        if(random > 1)
        {
            Instantiate(_lootPrefab, transform.position, Quaternion.identity);
        }       
    }

    private void OnHit()
    {
        _hit = true;
    }
}
