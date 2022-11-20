using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interface for Clickable Objects, streamlines clicking
public interface IClickable
{
    void OnClick();
    void OnRightClick();
}
