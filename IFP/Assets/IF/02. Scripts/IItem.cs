using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem {
    string name { get; }
    Sprite icon { get; }
    Transform transform { get; }
}
