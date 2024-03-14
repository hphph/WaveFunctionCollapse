using UnityEngine;


[CreateAssetMenu(fileName = "Module", menuName = "Wave Function Collapse/Module", order = 1)]
public class Module : ScriptableObject
{
    public Sprite Sprite;
    public int Weight = 1;
    public Module[] yPositive;
    public Module[] xPositive;
    public Module[] yNegative;
    public Module[] xNegative;
}
