using EEA.Ship;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace EEA.Managers
{
    public class ShipManager : MonoBehaviour
    {
        private GameStateManager gameStateManager;
        private AttributeManager attributeManager;

        private List<ShipMovement> liveShips = new List<ShipMovement>();
        
        public List<ShipMovement> Ships => liveShips;
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
            liveShips = FindObjectsOfType<ShipMovement>().ToList();
            gameStateManager = FindObjectOfType<GameStateManager>();
            attributeManager = FindObjectOfType<AttributeManager>();

            InitializeShips();
        }

        private void InitializeShips()
        {
            int i = 0;
            foreach (var ship in liveShips)
            {
                ship.ShipStats.SetID(i.ToString());
                ship.ShipStats.SetColor(colors[i++]);
                ship.ShipStats.onDeath += OnShipDeath;
            }
        }

        void OnShipDeath(string id)
        {
            int index = liveShips.FindIndex(s => s.ShipStats.ID == id);

            liveShips.RemoveAt(index);

            if(liveShips.Count < 2)
            {
                if (!liveShips[0].Anchor)
                {
                    liveShips[0].ToggleAnchor();
                }

                if (liveShips[0].ShipStats.IsPlayer)
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