using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _spawnInterval = 5.0f;
    [SerializeField] private int _maxEnemies = 30;
    private List<GameObject> _enemyList = new();
    private float _timer; 
    private int _currentEnemies;
    private bool _gameOver = false;

    public UnityAction AllEnemiesIsOver;

    private void Update()
    {   
        if(_gameOver)
        {
            return;
        }

        _timer += Time.deltaTime;
        
        if (_timer >= _spawnInterval)
        {
            _enemyList.RemoveAll(item => item == null);            
            _timer = 0f;
            if(_currentEnemies < _maxEnemies)
            {
                SpawnEnemy();
            }
        }

        if(_maxEnemies - _currentEnemies == 0 && _enemyList.Count < 1 && _gameOver == false)
        {           
           _gameOver = true;
            AllEnemiesIsOver?.Invoke();
        }
    }

    private void SpawnEnemy()
    {        
        var enemy = Instantiate(_enemyPrefab, transform.position, Quaternion.identity);
        _enemyList.Add(enemy);
        _currentEnemies++;
    }
}
