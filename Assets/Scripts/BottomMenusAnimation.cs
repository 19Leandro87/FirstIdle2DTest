using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomMenus : MonoBehaviour
{
    private const string STOP_UP_DOWN = "StopUpDown";
    [SerializeField] private Canvas upgradesCanvas;
    [SerializeField] private Button upgradesButton;
    [SerializeField] private Canvas statsCanvas;
    [SerializeField] private Button statsButton;    
    [SerializeField] private Canvas unitsCanvas;
    [SerializeField] private Button unitsButton;
    private Animator upgradesAnimator;
    private Animator statsAnimator;
    private Animator unitsAnimator;

    private void Awake() {
        upgradesAnimator = upgradesCanvas.GetComponent<Animator>();
        upgradesButton.onClick.AddListener(() => { UpOrDownScroll(upgradesAnimator); });
        statsAnimator = statsCanvas.GetComponent<Animator>();
        statsButton.onClick.AddListener(() => { UpOrDownScroll(statsAnimator); });
        unitsAnimator = unitsCanvas.GetComponent<Animator>();
        unitsButton.onClick.AddListener(() => { UpOrDownScroll(unitsAnimator); });

    }

    private void UpOrDownScroll(Animator animator) {
        switch (animator.GetInteger(STOP_UP_DOWN)) {
            case 0: animator.SetInteger(STOP_UP_DOWN, 1); break;
            case 1: animator.SetInteger(STOP_UP_DOWN, 2); break;
            case 2: animator.SetInteger(STOP_UP_DOWN, 1); break;
        }
    }
}
