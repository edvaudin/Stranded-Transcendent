using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    private static PlayerStatManager instance;
    public static PlayerStatManager Instance { get { return instance; } }

    public Stat fireDelay;
    public Stat moveSpeed;
    public Stat projectileSpeed;
    public Stat projectileRange;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            fireDelay.SetValueToBase();
            moveSpeed.SetValueToBase();
            projectileSpeed.SetValueToBase();
            projectileRange.SetValueToBase();
        }
        else if (instance != this)
        {
            Debug.Log($"Destroying duplicate psm. Player Move Speed {moveSpeed.Value}");
            
            Destroy(gameObject);
            return;
        }
        else
        {
            Debug.Log($"Using the psm from last scene. Player Move Speed {moveSpeed.Value}");
        }
    }


}
