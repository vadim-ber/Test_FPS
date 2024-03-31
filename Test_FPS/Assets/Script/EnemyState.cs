using UnityEngine;
using UnityEngine.UIElements;

public abstract class EnemyState 
{
    protected Enemy _enemy;
    protected EnemyStateFactory _factory;
    public EnemyState(Enemy enemy, EnemyStateFactory factory)
    {
        _enemy = enemy;
        _factory = factory;
    }

    public abstract void CheckSwitchState();
    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
    protected void SwitchState(EnemyState newState)
    {
        ExitState();
        newState.EnterState();
        _enemy.CurrentState = newState;
    }
}

public class EnemyMove : EnemyState
{
    public EnemyMove(Enemy enemy, EnemyStateFactory stateFactory): base(enemy, stateFactory)
    {
        _enemy = enemy;
        _factory = stateFactory;
    }
    public override void CheckSwitchState()
    {
        if (_enemy.IsDead)
        {
            SwitchState(new EnemyDeath(_enemy, _factory));
            return;
        }
        if (Vector3.Distance(_enemy.transform.position, _enemy.Player.transform.position) <=
            _enemy.Agent.stoppingDistance)
        {
            SwitchState(new EnemyAttack(_enemy, _factory));
            return;
        } 
        if(_enemy.Hit)
        {
            SwitchState(new EnemyHit(_enemy, _factory));
            return;
        }        
    }

    public override void EnterState()
    {        
        _enemy.Animator.CrossFade("IdleBlend", 0.3f);
    }

    public override void ExitState()
    {
       
    }

    public override void UpdateState()
    {
        CheckSwitchState();
        if (!_enemy.Agent.enabled)
        {
            return;
        }
        _enemy.Animator.SetFloat("Speed", _enemy.Agent.velocity.magnitude);
        _enemy.Agent.SetDestination(_enemy.Player.transform.position);        
    }
}

public class EnemyHit : EnemyState
{
    public EnemyHit(Enemy enemy, EnemyStateFactory stateFactory) : base(enemy, stateFactory)
    {
        _enemy = enemy;
        _factory = stateFactory;
    }

    public override void CheckSwitchState()
    {
        if (_enemy.IsDead)
        {
            SwitchState(new EnemyDeath(_enemy, _factory));
            return;
        }
        if (_enemy.Hit)
        {
            SwitchState(new EnemyHit(_enemy, _factory));
            return;
        }
        if (!_enemy.Hit)
        {
            SwitchState(new EnemyMove(_enemy, _factory));
            return;
        }        
    }

    public override void EnterState()
    {
        _enemy.Animator.Play("hit");
    }

    public override void ExitState()
    {
        _enemy.Hit = false;
    }

    public override void UpdateState()
    {        
        CheckSwitchState();
    }    
}

public class EnemyAttack : EnemyState
{
    private float _attackCooldown = 2.0f;
    private float _currentAttckCoolDown;
    public EnemyAttack(Enemy enemy, EnemyStateFactory stateFactory) : base(enemy, stateFactory)
    {
        _enemy = enemy;
        _factory = stateFactory;
    }

    public override void CheckSwitchState()
    {
        if (_enemy.IsDead)
        {
            SwitchState(new EnemyDeath(_enemy, _factory));
            return;
        }
        if (Vector3.Distance(_enemy.transform.position, _enemy.Player.transform.position) >
            _enemy.Agent.stoppingDistance)
        {
            SwitchState(new EnemyMove(_enemy, _factory));
            return;
        }
        if (_enemy.Hit)
        {
            SwitchState(new EnemyHit(_enemy, _factory));
            return;
        }
    }

    public override void EnterState()
    {
        _currentAttckCoolDown = 0;
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        CheckSwitchState();
        RotateToPlayer();
        _currentAttckCoolDown -= Time.deltaTime;
        if(_currentAttckCoolDown <= 0)
        {
            Attack();            
            _currentAttckCoolDown = _attackCooldown;
        }        
    }

    private void Attack()
    {
        var random = Random.Range(1, 2);
        _enemy.Animator.Play("attack" + random);
    }

    private void RotateToPlayer()
    {
        Vector3 toTarget = (_enemy.Agent.steeringTarget - _enemy.gameObject.transform.position).normalized;
        if (toTarget != Vector3.zero)
        {
            Quaternion toTargetRotation = Quaternion.LookRotation(toTarget, Vector3.up);
            _enemy.gameObject.transform.rotation = Quaternion.RotateTowards(_enemy.gameObject.transform.rotation, toTargetRotation,
                _enemy.Agent.angularSpeed * Time.deltaTime);
        }
    }
}

public class EnemyDeath : EnemyState
{
    public EnemyDeath(Enemy enemy, EnemyStateFactory stateFactory) : base(enemy, stateFactory)
    {
        _enemy = enemy;
        _factory = stateFactory;
    }

    public override void CheckSwitchState()
    {
        
    }

    public override void EnterState()
    {        
        _enemy.Animator.StopPlayback();
        _enemy.Animator.Play("die");
        _enemy.Agent.enabled = false;
        _enemy.Collder.enabled = false;
        var splash = GameObject.Instantiate(_enemy.DeathSplashPrefab, _enemy.transform.position,
            Quaternion.identity);
        GameObject.Destroy(_enemy.gameObject);
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        
    }
}
