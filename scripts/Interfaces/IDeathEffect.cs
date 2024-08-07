using Godot;
using System;

public interface IDeathEffect
{
    void CreateEffect(string scene, Node parent, Vector2 position);
}
