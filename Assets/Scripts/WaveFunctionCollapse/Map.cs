using System;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
	Slot[] slots;
	Vector2Int mapSize;

	public Vector2Int MapSize => mapSize;

	int GetMapIndex(Vector2Int position) => position.y * mapSize.x + position.x;
	Vector2Int GetPositionFromIndex(int index) => new Vector2Int(index % mapSize.x, index / mapSize.x);


	public Map(Vector2Int mapSize, Module[] modules)
	{
		this.mapSize = mapSize;
		slots = new Slot[mapSize.x * mapSize.y];
		for (int i = 0; i < slots.Length; i++)
		{
			slots[i] = new Slot(modules);
		}
	}

	public Slot GetSlot(Vector2Int position)
	{
		int moduleIndex = GetMapIndex(position);
		if (moduleIndex >= 0 && moduleIndex < slots.Length)
		{
			return slots[moduleIndex];
		}
		return null;
	}

	public Vector2Int GetPosition(Slot slot)
	{
		int index = Array.FindIndex<Slot>(slots, s => s == slot);
		if (index == -1) Debug.LogError("Slot not in map jurisdiction");
		return GetPositionFromIndex(index);
	}
}
