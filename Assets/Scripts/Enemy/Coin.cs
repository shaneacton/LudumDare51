using System;
using UnityEngine;

public class Coin : PickUp
{
    public override void pickUp()
    {
        // Debug.Log("got coin");
        GameManager.incrementScore();
    }
}
