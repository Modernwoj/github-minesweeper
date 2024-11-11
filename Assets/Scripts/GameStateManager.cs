using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager 
{
    public enum GameState{
        Gameplay,
        GameOver,
    }

    public static GameState gamestate {  get; private set; }

    public static void ChangeGameState(GameState newGameState) => gamestate = newGameState;
}
