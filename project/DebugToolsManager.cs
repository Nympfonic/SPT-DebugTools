using Arys.SPTDebugTools.Components;
using System.Collections.Generic;
using UnityEngine;

namespace Arys.SPTDebugTools
{
	[DisallowMultipleComponent]
	public class DebugToolsManager : MonoBehaviour
	{
		private readonly List<IComponent> _components = [];

		internal void AddComponent<T>()
			where T : IComponent, new()
		{
			_components.Add(new T());
		}

		internal void RemoveComponent<T>(T component)
			where T : IComponent
		{
			_components.Remove(component);
		}

		private void Start()
		{
			AddComponent<TeleportBotsComponent>();
		}

		private void Update()
		{
			int componentCount = _components.Count;
			for (int i = componentCount - 1; i >= 0; i--)
			{
				if (_components[i] == null) continue;
				_components[i].ManualUpdate();
			}
		}

		private void OnDestroy()
		{
			int componentCount = _components.Count;
			for (int i = componentCount - 1; i >= 0; i--)
			{
				if (_components[i] == null) continue;
				_components[i].Destroy();
			}
		}
	}
}