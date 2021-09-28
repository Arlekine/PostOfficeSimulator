using UnityEngine;

public class PoliceSiren : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private float _changeTime;

    private float _nextChangeTime;
    
    private void Update()
    {
        if (Time.time > _nextChangeTime)
        {
            _nextChangeTime = Time.time + _changeTime;
            _light.color = _light.color == Color.red ? Color.blue : Color.red;
        }
    }
}