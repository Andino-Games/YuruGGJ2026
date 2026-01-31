using UnityEngine;

public class TypeColor : MonoBehaviour
{
    [SerializeField] private GameColor _color;
    [SerializeField] private GameObject _gameObject;  
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ColorWheelController.Instance.selected == _color)
        {
            _gameObject.SetActive(false);
        }
        else
        {
            _gameObject.SetActive(true);
        }
    }
}
