using UnityEngine;

public enum InputLock
{
   // Player can tap anywhere to move boat
   None = 0,
   // Boat will ignore taps on the bottom of screen (where menu buttons are)
   LockOnlyBottom = 1,
   // Boat completly ignores input
   Locked = 2,
}

[CreateAssetMenu(fileName = "SOE_InputLockEvent", menuName = "ScriptableEvents/InputLock", order = 0)]
public class InputLockSOEvent : SOEvent<InputLock>
{
    
}