using EEA.Enemy;
using EEA.General;
using EEA.Ship;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EEA.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private Sea sea;
        [SerializeField] private int outerAreaDamage;

        private GameStateManager gameStateManager;

        private List<IPlayer> livePlayers = new List<IPlayer>();

        public List<IPlayer> Players => livePlayers;
        private Color[] colors =
        {
            Color.red,
            Color.blue,
            Color.green,
            Color.cyan,
            Color.magenta,
            Color.gray,
        };

        private void Awake()
        {
            livePlayers = FindObjectsOfType<EnemyBot>().Select(s => s.GetComponent<IPlayer>()).ToList();
            livePlayers.Add(FindObjectOfType<Player.Player>());

            gameStateManager = FindObjectOfType<GameStateManager>();

            InitializeShips();

            StartCoroutine(CheckShipInRadius());
        }

        private void InitializeShips()
        {
            int i = 0;
            foreach (var player in livePlayers)
            {
                player.ShipMovement.ShipStats.SetID(i.ToString());
                player.ShipMovement.ShipStats.SetColor(colors[i++]);
                player.ShipMovement.ShipStats.onDeath += OnShipDeath;
            }
        }

        private IEnumerator CheckShipInRadius()
        {
            while (true)
            {
                if (gameStateManager.GameState == GameState.GAME)
                {
                    Vector2 pos;
                    foreach (var player in livePlayers)
                    {
                        pos = new Vector2(player.ShipMovement.transform.position.x, player.ShipMovement.transform.position.z);
                        if (pos.sqrMagnitude > (sea.SafeRadius * sea.SafeRadius))
                        {
                            player.ShipMovement.ShipStats.TakeDamage(outerAreaDamage);
                        }
                    }
                }

                yield return new WaitForSeconds(1);
            }
        }

        void OnShipDeath(string id)
        {
            int index = livePlayers.FindIndex(s => s.ShipMovement.ShipStats.ID == id);

            var iPlayer = livePlayers[index].GameObject;

            livePlayers.RemoveAt(index);

            Destroy(iPlayer);

            if (livePlayers.Count < 2)
            {
                if (!livePlayers[0].ShipMovement.Anchor)
                {
                    livePlayers[0].ShipMovement.ToggleAnchor();
                }

                if (livePlayers[0].ShipMovement.ShipStats.IsPlayer)
                {
                    gameStateManager.ChangeGameState(GameState.WIN);
                }
                else
                {
                    gameStateManager.ChangeGameState(GameState.LOSE);
                }
            }
        }
    }
}