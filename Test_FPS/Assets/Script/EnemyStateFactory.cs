
public class EnemyStateFactory 
{
    private Enemy _enemy;
    public EnemyStateFactory(Enemy currentContext)
    {
        _enemy = currentContext;
    }

    public EnemyState EnemyMoveState() { return new EnemyMove(_enemy, this); }
}
