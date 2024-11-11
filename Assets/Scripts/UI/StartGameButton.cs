using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StartGameButton : MonoBehaviour
{
    [Header("GameSettings")]
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] int mines;

    public void OnClick()
    {
        GameplayManager.Instance.CheckCreateGameBoard(width,height,mines);
    }

}
