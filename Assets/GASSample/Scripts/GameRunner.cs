using System.Collections.Generic;
using GAS;
using UnityEngine;
using UnityEngine.Serialization;

public class GameRunner : MonoBehaviour
{
    private bool _isRunning;

    private int _score;
    public static GameRunner Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        WaitForFirstGameStart();
    }

    private void WaitForFirstGameStart()
    {
        UIManager.Instance.ShowResultWindow();
    }

    public void StartGame()
    {
        // 重置数据
        _score = 0;
        // 重置UI
        UIManager.Instance.HideResultWindow();
        UIManager.Instance.SetScore(_score);
        // 恢复GAS运行
        GameplayAbilitySystem.GAS.Unpause();

        // 重置Player和Enemy
        DestroyPlayer();
        DestroyEnemies();
        CreatePlayer();
        _isRunning = true;
    }

    public void GameOver()
    {
        _isRunning = false;
        // 显示结算界面
        UIManager.Instance.ShowResultWindow();
        // 暂停GAS
        GameplayAbilitySystem.GAS.Pause();
    }

    public void AddScore(int addScore = 10)
    {
        _score += addScore;
        UIManager.Instance.SetScore(_score);
    }

    #region Player Management

    [SerializeField] private GameObject prefabPlayer;
    [SerializeField] private Vector3 _playerSpawnPosition = Vector3.zero;
    private Player _player;

    private void CreatePlayer()
    {
        if (_player != null) return;
        var go = Instantiate(prefabPlayer);
        go.transform.position = _playerSpawnPosition;
        _player = go.GetComponent<Player>();
        _player.Init();
    }

    private void DestroyPlayer()
    {
        if (_player == null) return;
        Destroy(_player.gameObject);
        _player = null;
    }

    #endregion


    #region Enemy Managment

    [SerializeField] private float enemySpawnInterval = 1.5f;
    [SerializeField] private GameObject prefabEnemy;
    private float _enemySpawnCounter;
    private readonly List<Enemy> _enemies = new();
    [SerializeField] private Rect enemySpawnRect;

    private void Update()
    {
        if (_isRunning)
        {
            _enemySpawnCounter += Time.deltaTime;
            if (_enemySpawnCounter >= enemySpawnInterval)
            {
                _enemySpawnCounter = 0;
                SpawnEnemy();
            }
        }
    }

    public void RegisterEnemy(Enemy enemy)
    {
        _enemies.Add(enemy);
    }

    public void UnregisterEnemy(Enemy enemy)
    {
        _enemies.Remove(enemy);
    }

    private void SpawnEnemy()
    {
        var position = new Vector3(
            Random.Range(enemySpawnRect.xMin, enemySpawnRect.xMax),
            Random.Range(enemySpawnRect.yMin, enemySpawnRect.yMax),
            0);
        var go = Instantiate(prefabEnemy);
        go.transform.position = position;
        var enemy = go.GetComponent<Enemy>();
        enemy.Init(_player);
    }

    private void DestroyEnemies()
    {
        foreach (var enemy in _enemies) Destroy(enemy.gameObject);
        _enemies.Clear();
    }

    #endregion
}