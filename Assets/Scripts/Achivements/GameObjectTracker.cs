﻿using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public static class GameObjectTracker
{
    public static List<NaturalObject> gameObjects = new();
    // ADD to this list when player collect it
    public static Dictionary<int, string> collected = new();
    // add to this list after the achivement window show it
    public static List<int> updatedcollected = new();
    public static int DeerCounnt;
    public static int TreeCount;
}