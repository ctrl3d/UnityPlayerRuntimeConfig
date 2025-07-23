using UnityEngine;
using work.ctrl3d;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UnityPlayerRuntimeConfig.Apply();
    }
}