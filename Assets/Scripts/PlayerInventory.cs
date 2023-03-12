using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    public int NumberOfPlants { get; private set; }

    public int PlantsCollected()
    {
        NumberOfPlants++;
        GameState gameState = FindObjectOfType<GameState>();
        gameState.Food = NumberOfPlants;
        return NumberOfPlants;
    }

    public int GetNumberOfPlants()
    {
        return NumberOfPlants;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
