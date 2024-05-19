using System.Collections.Generic;
using UnityEngine;

public class WFCMapGenerator : MonoBehaviour
{
	[SerializeField]
	Vector2Int mapSize;
	[SerializeField]
	Module[] modules;

	GameObject mapRoot, moduleTemplate;
	PriorityQueueSet<Slot> collapseOrder;
	Map map;

	void Awake()
	{
		map = new Map(mapSize, modules);
		collapseOrder = new PriorityQueueSet<Slot>();
		mapRoot = new GameObject("Map Root");
		moduleTemplate = new GameObject("Module Template");
		moduleTemplate.AddComponent<SpriteRenderer>();

		WFCMapGenerate();
	}
	
	void WFCMapGenerate()
	{
		// Add midPoint to collapseOrder
		// Loop until collapseOrder is empty
		// 		Collapse lowest cost
		// 		Add to collapseOrder adjacent slots (if not collapsed)
		//		Remove collapsed slot

		Vector2Int currentPosition = new Vector2Int(mapSize.x / 2, mapSize.y / 2);
		Slot firstSlot = map.GetSlot(currentPosition);
		collapseOrder.Add(firstSlot, modules.Length);
		while(collapseOrder.Count > 0)
		{
			Slot minSlot = collapseOrder.ExtractMin();
			if(minSlot == null) Debug.LogError("Min slot null");
			currentPosition = map.GetPosition(minSlot);

			minSlot.Collapse();
			if(minSlot.Module == null) continue;

			GameObject goModule = Instantiate(moduleTemplate, mapRoot.transform);
			goModule.name = "Map Module (x:" + currentPosition.x + ", y:" + currentPosition.y + ")";
			goModule.GetComponent<SpriteRenderer>().sprite = minSlot.Module.Sprite;
			goModule.transform.position = new Vector3(currentPosition.x, currentPosition.y, 0);
			// GameObject goModule = new GameObject("Map Module (x:"+currentPosition)

			Spread(currentPosition, minSlot);
		}
		Debug.Log("Generation Finished");
	}

	void Spread(Vector2Int spreadingSlotPos, Slot spreadingSlot)
	{
		Slot yPositive = map.GetSlot(new Vector2Int(spreadingSlotPos.x, spreadingSlotPos.y + 1));
		Slot xPositive = map.GetSlot(new Vector2Int(spreadingSlotPos.x + 1, spreadingSlotPos.y));
		Slot yNegative = map.GetSlot(new Vector2Int(spreadingSlotPos.x, spreadingSlotPos.y - 1));
		Slot xNegative = map.GetSlot(new Vector2Int(spreadingSlotPos.x - 1, spreadingSlotPos.y));

		if(yPositive != null && !yPositive.IsCollapsed)
		{
			int lastPossibilitiesCount = yPositive.PossibilitiesCount;
			HashSet<Module> sum = new HashSet<Module>();
			foreach(Module m in spreadingSlot.Possibilities)
			{
				foreach(Module possibility in m.yPositive)
				{
					sum.Add(possibility);
				}
			}
			yPositive.Spread(sum);
			if(yPositive.PossibilitiesCount < lastPossibilitiesCount) 
			{
				collapseOrder.Add(yPositive, yPositive.PossibilitiesCount);
				Spread(new Vector2Int(spreadingSlotPos.x, spreadingSlotPos.y + 1), yPositive); 
			}
		}
		if(xPositive != null && !xPositive.IsCollapsed)
		{
			int lastPossibilitiesCount = xPositive.PossibilitiesCount;
			HashSet<Module> sum = new HashSet<Module>();
			foreach(Module m in spreadingSlot.Possibilities)
			{
				foreach(Module possibility in m.xPositive)
				{
					sum.Add(possibility);
				}
			}
			xPositive.Spread(sum);
			if(xPositive.PossibilitiesCount < lastPossibilitiesCount) 
			{
				collapseOrder.Add(xPositive, xPositive.PossibilitiesCount);
				Spread(new Vector2Int(spreadingSlotPos.x + 1, spreadingSlotPos.y), xPositive); 
			}
		}
		if(yNegative != null && !yNegative.IsCollapsed)
		{
			int lastPossibilitiesCount = yNegative.PossibilitiesCount;
			HashSet<Module> sum = new HashSet<Module>();
			foreach(Module m in spreadingSlot.Possibilities)
			{
				foreach(Module possibility in m.yNegative)
				{
					sum.Add(possibility);
				}
			}
			yNegative.Spread(sum);
			if(yNegative.PossibilitiesCount < lastPossibilitiesCount) 
			{
				collapseOrder.Add(yNegative, yNegative.PossibilitiesCount);
				Spread(new Vector2Int(spreadingSlotPos.x, spreadingSlotPos.y - 1), yNegative); 
			}
		}
		if(xNegative != null && !xNegative.IsCollapsed)
		{
			int lastPossibilitiesCount = xNegative.PossibilitiesCount;
			HashSet<Module> sum = new HashSet<Module>();
			foreach(Module m in spreadingSlot.Possibilities)
			{
				foreach(Module possibility in m.xNegative)
				{
					sum.Add(possibility);
				}
			}
			xNegative.Spread(sum);
			if(xNegative.PossibilitiesCount < lastPossibilitiesCount) 
			{
				collapseOrder.Add(xNegative, xNegative.PossibilitiesCount);
				Spread(new Vector2Int(spreadingSlotPos.x - 1, spreadingSlotPos.y), xNegative); 
			}
		}
	}

}

