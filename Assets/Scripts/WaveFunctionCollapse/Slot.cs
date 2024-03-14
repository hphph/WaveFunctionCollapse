using System;
using System.Collections.Generic;
using UnityEngine;

public class Slot
{
	Module module;
	HashSet<Module> possibilities;
	bool isCollapsed = false;

	public Module Module => module;
	public bool IsCollapsed => isCollapsed;
	public int PossibilitiesCount => possibilities.Count;

	public Slot(Module[] posibilites)
	{
		this.possibilities = new HashSet<Module>(posibilites);
	}

	public void DebugPossibilities()
	{
		Debug.Log("SLOTS POSSIBILITIES");
		foreach(Module p in possibilities)
		{
			Debug.Log(p.name);
		}
	}

	public void Spread(Module[] limitingModules)
	{
		possibilities.IntersectWith(limitingModules);
		if (possibilities.Count == 1) this.Collapse();
		else if (possibilities.Count == 0) isCollapsed = true;
	}

	int ProbabilityWeightSum()
	{
		int sum = 0;
		foreach(var possibility in possibilities)
		{
			sum += possibility.Weight;
		}
		return sum;
	}

	public void Collapse()
	{
		System.Random random = new System.Random();
		int randomModuleIndex = random.Next(ProbabilityWeightSum());
		int i = 0;
		isCollapsed = true;
		foreach (Module possibility in possibilities)
		{
			for(int j = 0; j < possibility.Weight; j++)
			{
				if (i == randomModuleIndex)
				{
					module = possibility;
					return;
				}
				i++;
			}
		}
	}
}
