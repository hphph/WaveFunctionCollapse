using UnityEngine;

public class Tests : MonoBehaviour
{
	[SerializeField]
	Module[] modules;
	void Start()
	{
		Slot a = new Slot(modules);
		a.DebugPossibilities();
		a.Spread(modules[0].xNegative);
		a.DebugPossibilities();
		a.Spread(modules[3].xNegative);
		a.DebugPossibilities();
		a.Collapse();
		Debug.Log(a.Module.name + " " + a.IsCollapsed);
		// Slot b = new Slot(modules[0].xNegative);
		// Slot c = new Slot(modules[1].yNegative);
	}
}
