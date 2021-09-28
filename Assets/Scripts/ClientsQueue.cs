using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ClientsQueue : MonoBehaviour
{
    public int QueueLength => _clients.Count;
    
    [SerializeField] private PackagesGettingPoint _packagesGetting;
    [SerializeField] private Transform _start;
    [SerializeField] private Transform[] _wayOut;
    [SerializeField] private Vector3 _queueDirection;
    [SerializeField] private float _distanceBetweenClients;

    private Vector3[] _wayOutVector;
    
    private List<ClientController> _clients = new List<ClientController>();

    private void Awake()
    {
        List<Vector3> wayOutVector = new List<Vector3>();

        foreach (var point in _wayOut)
        {
            wayOutVector.Add(point.position);
        }

        _wayOutVector = wayOutVector.ToArray();
    }

    public void AddClient(ClientController client)
    {
        _clients.Add(client);
        var moveTween = client.Mover.Move(GetQueuePositionForIndex(_clients.Count - 1));

        if (_clients.Count == 1)
        {
            moveTween.OnComplete(() =>
            {
                _packagesGetting.SetPackageReceiver(_clients[0]);
                _clients[0].Desire.DesireView.Show();
                _clients[0].OnRecievingCompleted += Dequeue;
            });

        }
    }

    public Vector3 GetSpawnPoint(float spawnDistance)
    {
        return _start.position + _queueDirection * spawnDistance;
    }

    private void Dequeue(ClientController client)
    {
        client.OnRecievingCompleted -= Dequeue;
        client.Mover.Move(_wayOutVector);
        _clients.Remove(client);

        for (int i = 0; i < _clients.Count; i++)
        {
            var moveTween = _clients[i].Mover.Move(GetQueuePositionForIndex(i));

            if (i == 0)
                moveTween.OnComplete(() =>
                {
                    _packagesGetting.SetPackageReceiver(_clients[0]);
                    _clients[0].Desire.DesireView.Show();
                    _clients[0].OnRecievingCompleted += Dequeue;
                });
        }
    }

    private Vector3 GetQueuePositionForIndex(int clientIndex)
    {
        return _start.position + _queueDirection * (_distanceBetweenClients * clientIndex);
    }
}
