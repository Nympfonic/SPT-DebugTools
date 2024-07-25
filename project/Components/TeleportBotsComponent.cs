using Comfort.Common;
using EFT;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arys.SPTDebugTools.Components
{
	// If TeleportAllBots keybind is pressed and there are valid bots, teleport to where player is looking
	public class TeleportBotsComponent : IComponent
	{
		private readonly List<Player> _botTeleportList = [];
		private readonly BotSpawner _botSpawner;
		private readonly Player _mainPlayer;

		public TeleportBotsComponent()
		{
			_mainPlayer = Singleton<GameWorld>.Instance.MainPlayer;
			_botSpawner = Singleton<IBotGame>.Instance.BotsController.BotSpawner;

			_botSpawner.OnBotCreated += AddBotToTeleportList;
		}

		public void ManualUpdate()
		{
			if (DebugToolsPlugin.TeleportAllBotsKeybind.Value.IsDown() && _botTeleportList.Any())
			{
				Transform cameraTransform = _mainPlayer.CameraPosition;
				Physics.Raycast(
					cameraTransform.position + cameraTransform.forward,
					cameraTransform.forward,
					out RaycastHit hitInfo,
					50f,
					LayerMaskClass.HighPolyWithTerrainNoGrassMask
				);

				int botCount = _botTeleportList.Count;
				for (int i = botCount - 1; i >= 0; i--)
				{
					var bot = _botTeleportList[i];
					if (bot == null || !bot.HealthController.IsAlive)
					{
						continue;
					}

					bot.Transform.SetPositionAndRotation(hitInfo.point + Vector3.up, Quaternion.Euler(Vector3.zero));
					Physics.SyncTransforms();
					bot.CharacterController.attachedRigidbody.velocity = Vector3.zero;
				}

				_botTeleportList.RemoveAll(x => x == null || !x.HealthController.IsAlive);
			}
		}

		public void Destroy()
		{
			_botSpawner.OnBotCreated -= AddBotToTeleportList;
			int botCount = _botTeleportList.Count;
			for (int i = botCount - 1; i >= 0; i--)
			{
				if (_botTeleportList[i] == null) continue;
				_botTeleportList[i].OnIPlayerDeadOrUnspawn -= RemoveBotFromTeleportList;
			}
			_botTeleportList.Clear();
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
			bot.OnIPlayerDeadOrUnspawn -= RemoveBotFromTeleportList;
			_botTeleportList.Remove((Player)bot);
		}
	}
}
