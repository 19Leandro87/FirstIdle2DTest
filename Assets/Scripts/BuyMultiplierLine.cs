using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyMultiplierLine : MonoBehaviour
{
    [SerializeField] private Button buy1Button;
    [SerializeField] private Button buy10Button;
    [SerializeField] private Button buy100Button;

    private void Start() {
        buy1Button.onClick.AddListener(() => { UnitsManager.Instance.SetBuyMultiplier(1); });
        buy10Button.onClick.AddListener(() => { UnitsManager.Instance.SetBuyMultiplier(10); });
        buy100Button.onClick.AddListener(() => { UnitsManager.Instance.SetBuyMultiplier(100); });
    }
}
