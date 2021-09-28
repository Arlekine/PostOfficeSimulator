using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class ClientsSpawner : MonoBehaviour
{
    public Action<ClientController> OnNewClientSpawned;
    public Action OnGameCompleted;
    
    [SerializeField] private List<ClientController> _clientsPrefabs = new List<ClientController>();
    [SerializeField] private List<ClientsQueue> _queues = new List<ClientsQueue>();

    [Header("Spawn parameters")] 
    [SerializeField] private float _spawnTime = 6f;
    [SerializeField] private float _spawnDistance = 10f;

    [Header("Win Loose Conditions")] 
    [SerializeField] private float _completedClientsToWin = 20;  
    [SerializeField] private float _enqueuedClientsToLoose = 8;
    [SerializeField] private float _winOffsetTime = 5f;

    [Header("Score")] 
    [SerializeField] private int _pointsForRightPackage;
    [SerializeField] private int _pointsForIncorrectPackage;
    [SerializeField] private ScoreView _scoreView;
    
    [Space] 
    [SerializeField] private ResultPanel _resultPanel;

    [Space]
    [SerializeField] private string _queueLooseHeaderText;
    [SerializeField] private string _queueLooseScoreText;
    [SerializeField] private string _queueLooseButtonText;
    
    [Space]
    [SerializeField] private string _scoreLooseHeaderText;
    [SerializeField] private string _scoreLooseButtonText;
    
    [Space]
    [SerializeField] private string _winHeaderText;
    [SerializeField] private string _winButtonText;

    private int _score;
    private int _completedClients;
    private List<ClientController> _spawnedClients = new List<ClientController>();
    
    private void Start()
    {
        // foreach (var queue in _queues)
        // {
        //     queue.OnClientComplete += RemoveClient;
        // }

        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnClient();
            yield return new WaitForSeconds(_spawnTime);
        }
    }

    private void SpawnClient()
    {
        int clientPrefabIndex = Random.Range(0, _clientsPrefabs.Count);
        var shortestQueue = GetShortestQueue();

        var newClient = Instantiate(_clientsPrefabs[clientPrefabIndex], shortestQueue.GetSpawnPoint(_spawnDistance),
            Quaternion.identity);
        
        newClient.Desire.SelectRandomDesire();
        newClient.OnPackageRecieved += RemoveClient;
        shortestQueue.AddClient(newClient);

        OnNewClientSpawned?.Invoke(newClient);

        newClient.Mover.OnMoveCompleted += () =>
        {
            newClient.Mover.OnMoveCompleted = null;
            _spawnedClients.Add(newClient);
            print(_spawnedClients.Count);
            if (_spawnedClients.Count >= _enqueuedClientsToLoose)
                LooseByQueue();
        };
    }

    private void RemoveClient(ClientController client, bool desireFulfilled)
    {
        print("REmove");
        _score += desireFulfilled ? _pointsForRightPackage : _pointsForIncorrectPackage;
        _scoreView.SetScore(_score);
        _spawnedClients.Remove(client);
        _completedClients += 1;

        if (_completedClients >= _completedClientsToWin)
        {
            if (_score >= 0)
                StartCoroutine(Win());
            else
                LooseByScore();
        }

        client.Mover.OnMoveCompleted += () =>
        {
            Destroy(client.gameObject);
        };
    }

    private int GetQueuesSumLength()
    {
        int length = 0;

        foreach (var queue in _queues)
        {
            length += queue.QueueLength;
        }

        return length;
    }

    private ClientsQueue GetShortestQueue()
    {
        int shortestQueueIndex = 0;
        int shortestLength = _queues[0].QueueLength;

        for (int i = 1; i < _queues.Count; i++)
        {
            if (_queues[i].QueueLength < shortestLength)
                shortestQueueIndex = i;
        }

        return _queues[shortestQueueIndex];
    }

    private IEnumerator Win()
    {
        OnGameCompleted?.Invoke();
        StopCoroutine(SpawnRoutine());
        
        yield return new WaitForSecondsRealtime(_winOffsetTime);
        
        _resultPanel.Show(true, _score.ToString(), _winHeaderText, _winButtonText);
        
    }
    

    private void LooseByQueue()
    {
        OnGameCompleted?.Invoke();
        StopCoroutine(SpawnRoutine());
        _resultPanel.Show(false, _queueLooseScoreText, _queueLooseHeaderText, _queueLooseButtonText);
    }
    
    private void LooseByScore()
    {
        OnGameCompleted?.Invoke();
        StopCoroutine(SpawnRoutine());
        _resultPanel.Show(false, _score.ToString(), _scoreLooseHeaderText, _scoreLooseButtonText);
    }
}
