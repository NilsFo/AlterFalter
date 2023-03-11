using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FoodProgressTracker : MonoBehaviour
{

    private GameState _gameState;
    public TextMeshProUGUI text;

    private void Awake()
    {
        _gameState=FindObjectOfType<GameState>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Food: " + _gameState.Food + "/" + _gameState.foodTarget;
    }
}
