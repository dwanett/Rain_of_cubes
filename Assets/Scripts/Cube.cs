using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Renderer))]
public class Cube : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private float _minTimeLive;
    [SerializeField] private float _maxTimeLive;
    
    private Color _defaultColor;
    
    public float TimeLive { get; private set; }
    private void Awake()
    {
        _defaultColor = _renderer.material.color;
        TimeLive = Random.Range(_minTimeLive, _maxTimeLive);
    }
    
    private void OnValidate()
    {
        if (_minTimeLive > _maxTimeLive)
        {
            _minTimeLive = _maxTimeLive - 1;
        }
    }
    
    public bool TryChangeColor(Color color)
    {
        bool isChanged = _renderer.material.color == _defaultColor;
        
        if (isChanged)
            _renderer.material.color = color;

        return isChanged;
    }
    
    public void EnabledCube()
    {
        RestoreDefaultColor();
        TimeLive = Random.Range(_minTimeLive, _maxTimeLive);
        gameObject.SetActive(true);
    }
    
    public void DisabledCube()
    {
        gameObject.SetActive(false);
    }
    
    private void RestoreDefaultColor()
    {
        _renderer.material.color = _defaultColor;
    }
}
