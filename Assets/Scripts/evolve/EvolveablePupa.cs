using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EvolveablePupa : MonoBehaviour, IEvolveable
{
    private GameState _gameState;
    public GameObject nextEvolvePrefab;
    public UnityEvent onEvolve;

    private void Awake()
    {
        _gameState = FindObjectOfType<GameState>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameState.evolveState = GameState.EvolveState.Pupa;
        _gameState.player = gameObject;

        if (onEvolve != null)
        {
            onEvolve = new UnityEvent();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (CanEvolve())
            {
                Evolve();
            }
        }
    }

    public void Evolve()
    {
        print("Pupa: Evolving!");
        OnEvolve();

        Vector3 pos = transform.position;
        Destroy(gameObject);
        Instantiate(nextEvolvePrefab, pos, Quaternion.identity);
    }

    public bool CanEvolve()
    {
        return true;
    }

    public void OnEvolve()
    {
        onEvolve.Invoke();
    }
}