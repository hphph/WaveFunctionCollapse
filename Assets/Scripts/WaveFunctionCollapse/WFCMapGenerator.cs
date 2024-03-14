using UnityEngine;
using UnityEngine.Analytics;

public class WFCMapGenerator : MonoBehaviour
{
	[SerializeField]
	Vector2Int mapSize;
	[SerializeField]
	Module[] modules;

	GameObject mapRoot, moduleTemplate;
	PriorityQueue<Slot> collapseOrder;
	Map map;

	void Awake()
	{
		map = new Map(mapSize, modules);
		collapseOrder = new PriorityQueue<Slot>();
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

			Debug.Log(currentPosition);

			minSlot.Collapse();
			if(minSlot.Module == null) continue;

			GameObject goModule = Instantiate(moduleTemplate, mapRoot.transform);
			goModule.name = "Map Module (x:" + currentPosition.x + ", y:" + currentPosition.y + ")";
			goModule.GetComponent<SpriteRenderer>().sprite = minSlot.Module.Sprite;
			goModule.transform.position = new Vector3(currentPosition.x, currentPosition.y, 0);
			// GameObject goModule = new GameObject("Map Module (x:"+currentPosition)

			Slot yPositive = map.GetSlot(new Vector2Int(currentPosition.x, currentPosition.y + 1));
			Slot xPositive = map.GetSlot(new Vector2Int(currentPosition.x + 1, currentPosition.y));
			Slot yNegative = map.GetSlot(new Vector2Int(currentPosition.x, currentPosition.y - 1));
			Slot xNegative = map.GetSlot(new Vector2Int(currentPosition.x - 1, currentPosition.y));

			if(yPositive != null && !yPositive.IsCollapsed)
			{
				yPositive.Spread(minSlot.Module.yPositive);
				collapseOrder.Add(yPositive, yPositive.PossibilitiesCount);
			}
			if(xPositive != null && !xPositive.IsCollapsed)
			{
				xPositive.Spread(minSlot.Module.xPositive);
				collapseOrder.Add(xPositive, xPositive.PossibilitiesCount);
			}
			if(yNegative != null && !yNegative.IsCollapsed)
			{
				yNegative.Spread(minSlot.Module.yNegative);
				collapseOrder.Add(yNegative, yNegative.PossibilitiesCount);
			}
			if(xNegative != null && !xNegative.IsCollapsed)
			{
				xNegative.Spread(minSlot.Module.xNegative);
				collapseOrder.Add(xNegative, xNegative.PossibilitiesCount);
			}
		}
		Debug.Log("Generation Finished");
	}

}
