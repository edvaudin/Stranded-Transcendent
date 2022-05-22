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

    private void Awake()
    {
        // If no Player ever existed, we are it.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            fireDelay.SetValueToBase();
            moveSpeed.SetValueToBase();
            projectileSpeed.SetValueToBase();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }


}
