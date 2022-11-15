using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.Managers
{
    public enum GameState
    {
        START, GAME, WIN, LOSE, TUTORIAL
    }
    public class GameStateManager : MonoBehaviour
    {
        private GameUIManager gameUIManager;
        private GameState gameState;

        public GameState GameState => gameState;

        private void Start()
        {
            gameState = GameState.START;
            gameUIManager = FindObjectOfType<GameUIManager>();
        }

        public void ChangeGameState(GameState state)
        {
            gameState = state;

            switch (state)
            {
                case GameState.START:

                    break;
                case GameState.GAME:
                    break;
                case GameState.WIN:
                    gameUIManager.HideGameUI();
                    gameUIManager.ShowWinUI();
                    break;
                case GameState.LOSE:
                    gameUIManager.HideGameUI();
                    gameUIManager.ShowLoseUI();
                    break;
                case GameState.TUTORIAL:
                    break;
                default:
                    break;
            }
        }
    }
}