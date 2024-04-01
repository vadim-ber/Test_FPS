using UnityEngine;
using UnityEngine.UI;

public class StatisticsCollector : MonoBehaviour
{
    [SerializeField] private Text _killsText;
    [SerializeField] private Text _damageDoneText;
    [SerializeField] private Text _deathsText;
    [SerializeField] private Text _winsText;
    private readonly string _kills = "Kills";
    private readonly string _damageDone = "DamageDone";
    private readonly string _deaths = "Deaths";
    private readonly string _wins = "Wins";
    private EnemySpawner _enemySpawner; 

    private void Awake()
    {
        CheckAndInitialize(_kills, 0f);
        CheckAndInitialize(_damageDone, 0f);
        CheckAndInitialize(_deaths, 0f);
        CheckAndInitialize(_wins, 0f);
        _killsText.text = PlayerPrefs.GetFloat(_kills).ToString();
        _damageDoneText.text = PlayerPrefs.GetFloat(_damageDone).ToString();
        _deathsText.text = PlayerPrefs.GetFloat(_deaths).ToString();
        _winsText.text = PlayerPrefs.GetFloat(_wins).ToString();
        _enemySpawner = FindObjectOfType<EnemySpawner>();
        if(_enemySpawner != null)
        {            
            _enemySpawner.AllEnemiesIsOver += AddWin;
        }        
    }

    public void EnemyKill()
    {
        float currentKills = PlayerPrefs.GetFloat(_kills);
        PlayerPrefs.SetFloat(_kills, currentKills + 1);
        PlayerPrefs.Save();
        _killsText.text = PlayerPrefs.GetFloat(_kills).ToString();
    }

    public void DamageDone(float damage)
    {
        float currentDamage = PlayerPrefs.GetFloat(_damageDone);
        PlayerPrefs.SetFloat(_damageDone, currentDamage + damage);
        PlayerPrefs.Save();
        _damageDoneText.text = PlayerPrefs.GetFloat(_damageDone).ToString();
    }

    public void Deaths()
    {
        float currentDeaths = PlayerPrefs.GetFloat(_deaths);
        PlayerPrefs.SetFloat(_deaths, currentDeaths + 1);
        PlayerPrefs.Save();
        _deathsText.text = PlayerPrefs.GetFloat(_deaths).ToString();        
    }

    public void AddWin()
    {
        float currentWins = PlayerPrefs.GetFloat(_wins);
        PlayerPrefs.SetFloat(_wins, currentWins + 1);
        PlayerPrefs.Save();
        _winsText.text = PlayerPrefs.GetFloat(_wins).ToString();
        print("test");
    }

    private void CheckAndInitialize(string key, float defaultValue)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return;
        }
        PlayerPrefs.SetFloat(key, defaultValue);
        PlayerPrefs.Save();
    }

    private void OnDisable()
    {
        if(_enemySpawner != null)
        {
            _enemySpawner.AllEnemiesIsOver -= AddWin;
        }
    }
}
