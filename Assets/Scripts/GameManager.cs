using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public GameState State;
    public static GameManager Instance;
    public static Action<GameState> GameStateChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
        if (FindObjectOfType<Menu>() != null) { UpdateGameState(GameState.Menu); } else { UpdateGameState(GameState.Playing); }
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;
        GameStateChanged?.Invoke(State);
    }
}

public enum GameState
{
    Playing,
    Menu,
    GameOver,
    Victory,
}
