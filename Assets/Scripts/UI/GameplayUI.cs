using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] TMP_Text text;

    public void ShowVictoryScreen()
    {
        gameOverScreen.SetActive(true);
        text.text = "Victory";
        text.color = Color.yellow;
    }

    public void ShowDefeatScreen()
    {
        gameOverScreen.SetActive(true);
        text.text = "Defeat";
        text.color = Color.red;
    }

    public void HideGameOverScreen()
    {
        gameOverScreen.SetActive(false);
    }
}
