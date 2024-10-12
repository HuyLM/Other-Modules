using AtoGame.OtherModules.DOTA;
using Sirenix.OdinInspector;
using UnityEngine;

public class Example : Example1 {

    // Tab B declared first, but displayed second
    [TabGroup("Tabs", "B"), PropertyOrder(1)]
    public string tabB1;

    // Tab A declared second, but displayed first
    [TabGroup("Tabs", "A")]
    public string tabA1;


    public BaseDoTweenAnimation dota;
    private void Start()
    {
        dota.Play(() => {
            Debug.Log("Ato");
        });
    }
}

public class Example1 : MonoBehaviour {

    // Tab B declared first, but displayed second
    [TabGroup("Tabs", "B"), PropertyOrder(1)]
    public string tabB;

}

// bo Values
// gan Tween
// OnCompleteEvent v
// bo UnityEvent ? v
// test play runtime
// them Paralle/Sequence 
// 
