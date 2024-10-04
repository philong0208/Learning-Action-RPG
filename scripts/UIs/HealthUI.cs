using Godot;
using System;

public class HealthUI : Control
{
    private int hearts = 4;
    private int maxHearts = 4;

    PlayerStats stats;
    private TextureRect heartUIEmpty;
    private TextureRect heartUIFull;

    public int Hearts
    {
        get => hearts;
        private set => SetHearts(value);
    }

    public int MaxHearts
    {
        get => maxHearts;
        private set => SetMaxHearts(value);
    }

    public override void _Ready()
    {
        stats = PlayerStats.Instance;
        heartUIEmpty = (TextureRect)GetNode("HeartUIEmpty");
        heartUIFull = (TextureRect)GetNode("HeartUIFull");

        this.MaxHearts = stats.MaxHealth;
        this.Hearts = stats.Health;

        stats.Connect("healthChanged", this, "SetHearts");
        stats.Connect("maxHealthChanged", this, "SetMaxHearts");
    }

    private void SetHearts(int value)
    {
        this.hearts = Mathf.Clamp(value, 0, MaxHearts);
        if (heartUIFull != null)
        {
            heartUIFull.RectSize = new Vector2(Hearts * 15, heartUIFull.RectSize.y);
        }
    }

    private void SetMaxHearts(int value)
    {
        this.maxHearts = Mathf.Max(value, 1);
        this.Hearts = Mathf.Min(Hearts, MaxHearts);
        if (heartUIEmpty != null)
        {
            heartUIEmpty.RectSize = new Vector2(MaxHearts * 15, heartUIEmpty.RectSize.y);
        }
    }
}
