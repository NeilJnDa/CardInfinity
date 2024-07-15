using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }
    private static GameManager _instance;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    [SerializeField]
    private Transform cardHoverPlaneTransform;
    public Plane CardHoverPlane
    {
        get
        {
            return new Plane(cardHoverPlaneTransform.up, cardHoverPlaneTransform.position);
        }
    }

    public void EndRound()
    {

    }
}
