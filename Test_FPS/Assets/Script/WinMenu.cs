using UnityEngine;

public class WinMenu : MonoBehaviour
{
    [SerializeField] private GameObject _winMenu;
    private EnemySpawner _enemySpawner;

    private void Awake()
    {
        _enemySpawner = FindObjectOfType<EnemySpawner>();
        if( _enemySpawner != null )
        {
            _enemySpawner.AllEnemiesIsOver += ShowWinMenu;
        }
    }

    private void ShowWinMenu()
    {
        Cursor.visible = true;
        _winMenu.SetActive( true );
    }
}
