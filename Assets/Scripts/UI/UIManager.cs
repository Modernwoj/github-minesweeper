using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonFromPrefab<UIManager>
{
    [SerializeField] GameplayUI _gameplayUI;

    public GameplayUI gameplayUI => _gameplayUI;
}
