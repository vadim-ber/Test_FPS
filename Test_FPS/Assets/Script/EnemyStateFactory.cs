
public class EnemyStateFactory 
{
    private Enemy _enemy;
    public EnemyStateFactory(Enemy currentContext)
    {
        _enemy = currentContext;
    }

    public EnemyState EnemyMoveState() { return new EnemyMove(_enemy, this); }
    public EnemyState EnemyHit() { return new EnemyHit(_enemy, this); }
    public EnemyState EnemyAttack() { return new EnemyAttack(_enemy, this); }
    public EnemyState EnemyDeath() { return new EnemyMove(_enemy, this); }
}
