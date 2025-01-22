using UnityEngine.Events;
using UnityEngine.Events;

public static class GlobalEventSystem {
    public static readonly UnityEvent<bool> OnTransitionToEndScreen = new UnityEvent<bool>();
}
