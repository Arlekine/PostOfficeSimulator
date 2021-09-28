using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PackagesSpawner : MonoBehaviour
{
    [SerializeField] private ClientsSpawner _clientsSpawner;
    [SerializeField] private List<PackagesGettingPoint> _packagesGettingPoints;
    [SerializeField] private List<PackageHolder> _packagesPrefabs = new List<PackageHolder>();
    [SerializeField] private float _spawnPause = 10f;
    [SerializeField] private int _spawnAmount = 20;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _spawnAreaRadious;
    
    [Space] 
    [SerializeField] private LayerMask _floorMask;
    [SerializeField] private float _trashDraggingHeight;

    [Header("Move Borders")] 
    [SerializeField] private Transform _leftTopBorder;
    [SerializeField] private Transform _rightDownBorder;

    private List<PackageHolder> _packagesInStore = new List<PackageHolder>();

    private float _nextSpawnTime = 0;
    
    private void Start()
    {
        _clientsSpawner.OnGameCompleted += () => { enabled = false; };
        foreach (var gettingPoint in _packagesGettingPoints)
        {
            gettingPoint.OnNewPackageDesireGetted += UpdatePackagesForReceiver;
        }
    }

    private void Update()
    {
        if (Time.time > _nextSpawnTime)
        {
            SpawnRandomPackages(_spawnAmount, new Package[0]);
        }
    }

    private void UpdatePackagesForReceiver(IPackageReceiver receiver)
    {
        var necessaryPackage = _packagesInStore.Find(x => x.Package == receiver.GetDesire());

        if (necessaryPackage == null)
        {
            SpawnRandomPackages(_spawnAmount, new Package[] {receiver.GetDesire()});
        }
    }

    private void SpawnRandomPackages(int spawnAmount, params Package[] necessaryPackages)
    {
        _nextSpawnTime = Time.time + _spawnPause;
        
        List<PackageHolder> packagesToSpawn = new List<PackageHolder>();

        foreach (var necessaryPackage in necessaryPackages)
        {
            var package = _packagesPrefabs.Find(x => x.Package == necessaryPackage);
            packagesToSpawn.Add(package);
        }

        for (int i = 0; i < spawnAmount - necessaryPackages.Length; i++)
        {
            int randomPackIndex = Random.Range(0, _packagesPrefabs.Count);
            packagesToSpawn.Add(_packagesPrefabs[randomPackIndex]);
        }

        foreach (var packageHolder in packagesToSpawn)
        {
            Vector2 random2DPos = Random.insideUnitCircle * _spawnAreaRadious;
            Vector3 spawnOffset = new Vector3(random2DPos.x, 0f, random2DPos.y);
            var pack = Instantiate(packageHolder, _spawnPoint.position + spawnOffset, Random.rotation);
            
            _packagesInStore.Add(pack);
            pack.OnPackageIssued += (p) => 
            {
                _packagesInStore.Remove(p);
            };
            
            pack.DraggableBody.Init(_floorMask, _trashDraggingHeight);
            pack.DraggableBody.SetMoveBorder(new Vector2(_leftTopBorder.position.x, _rightDownBorder.position.x), new Vector2(_rightDownBorder.position.z, _leftTopBorder.position.z));
        }
    }
}
