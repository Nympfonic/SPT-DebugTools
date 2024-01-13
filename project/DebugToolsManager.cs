using EFT;
using Comfort.Common;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Arys.DebugTools
{
    public class DebugToolsManager : MonoBehaviour
    {
        private readonly GameWorld _gameWorld;
        private readonly Player _player;
        private readonly BotSpawner _botSpawner;
        private readonly List<Player> _botTeleportList = [];

        public DebugToolsManager()
        {
            _gameWorld = Singleton<GameWorld>.Instance;
            _botSpawner = Singleton<IBotGame>.Instance.BotsController.BotSpawner;
            _player = _gameWorld.MainPlayer;

            _botSpawner.OnBotCreated += AddBotToTeleportList;
        }

        private void Update()
        {
            TeleportAllBotsToTarget();
        }

        private void TeleportAllBotsToTarget()
        {
            Transform cameraTransform = _player.CameraPosition;
            Physics.Raycast(cameraTransform.position + cameraTransform.forward, cameraTransform.forward, out RaycastHit hitInfo);

            // If TeleportAllBots keybind is pressed and there are valid bots, teleport to where player is looking
            if (Plugin.TeleportAllBotsKeybind.Value.IsDown() && _botTeleportList.Any())
            {
                foreach (var player in _botTeleportList)
                {
                    player.Transform.SetPositionAndRotation(hitInfo.point + Vector3.up, Quaternion.Euler(0f, 0f, 0f));
                }
            }
        }

        private void AddBotToTeleportList(BotOwner bot)
        {
            var botPlayer = bot.GetPlayer;
            if (!_botTeleportList.Contains(botPlayer) && bot.Profile.Info.Settings.Role != WildSpawnType.shooterBTR)
            {
                botPlayer.OnIPlayerDeadOrUnspawn += RemoveBotFromTeleportList;
                _botTeleportList.Add(botPlayer);
            }
        }

        private void RemoveBotFromTeleportList(IPlayer bot)
        {
            _botTeleportList.Remove((Player)bot);
        }
    }
}