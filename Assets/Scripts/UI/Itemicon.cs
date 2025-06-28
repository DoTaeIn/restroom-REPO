using UnityEngine;
using UnityEngine.UI;

public class Itemicon
{
    public string name;
    public Sprite icon;

    public Itemicon(string name, Sprite icon) // 생성자
    {
        this.name = name;
        this.icon = icon;
    }
}