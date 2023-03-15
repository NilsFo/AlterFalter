using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveBar : MonoBehaviour
{
    public Color colorWorm, colorPupa, colorButterfly;
    public Sprite iconWorm, iconPupa, iconButterfly;
    public AnimationCurve butterflyDistanceMultCurve;
    public Slider slider;
    public TextMeshProUGUI textfield;
    public Image fillImage;
    public Image iconImage;

    private GameState _gameState;

    private void Awake()
    {
        _gameState = FindObjectOfType<GameState>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float p = 0;
        switch (_gameState.evolveState)
        {
            case GameState.EvolveState.Butterfly:
                textfield.text = "Finde Blume!";
                float maxDist = _gameState.butterflyFlowerMaxDistance;
                p = GetPercentage(_gameState.PlayerDistanceToFlower(), _gameState.butterflyFlowerMaxDistance);
                p = 1.0f - p;
                p = Math.Clamp(p, 0.0f, 1.0f);
                p = butterflyDistanceMultCurve.Evaluate(p);

                if (_gameState.playerState == GameState.PlayerState.Win)
                {
                    p = 1.0f;
                }

                slider.value = p;

                fillImage.color = colorButterfly;
                iconImage.sprite = iconButterfly;
                break;

            case GameState.EvolveState.Caterpillar:
                textfield.text = _gameState.foodCurrent + "/" + _gameState.foodTarget;
                p = GetPercentage(_gameState.foodCurrent, _gameState.foodTarget);
                slider.value = p;
                fillImage.color = colorWorm;
                iconImage.sprite = iconWorm;
                break;

            case GameState.EvolveState.Pupa:
                textfield.text = "Mash [E]!";
                slider.value = GetPercentage(_gameState.pupaEvolveCurrent, _gameState.pupaEvolveTarget);
                fillImage.color = colorPupa;
                iconImage.sprite = iconPupa;
                break;

            case GameState.EvolveState.Unknown:
                slider.value = 0;
                break;

            default:
                textfield.text = "UNKNOWN";
                slider.value = 0;
                Debug.LogError("Unknown state!");
                break;
        }

        if (_gameState.playerState == GameState.PlayerState.Lost)
        {
            slider.value = 0.0f;
        }
    }

    private float GetPercentage(int a, int b)
    {
        float af = a;
        float bf = b;
        return GetPercentage(af, bf);
    }

    private float GetPercentage(float a, float b)
    {
        return a / b;
    }
}