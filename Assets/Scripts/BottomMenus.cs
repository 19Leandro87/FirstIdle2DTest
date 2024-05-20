using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomMenus : MonoBehaviour
{
    [SerializeField] private Canvas upgradesCanvas;
    [SerializeField] private Button upgradesButton;
    [SerializeField] private Canvas statsCanvas;
    [SerializeField] private Button statsButton;
    private Animator upgradesAnimator;
    private Animator statsAnimator;

    private void Awake() {
        upgradesAnimator = upgradesCanvas.GetComponent<Animator>();
        upgradesButton.onClick.AddListener(() => { 
            upgradesAnimator.SetBool("IsPressed", true);
        });

        statsAnimator = statsCanvas.GetComponent<Animator>();
        statsButton.onClick.AddListener(() => {
            statsAnimator.SetBool("IsPressed", true);
        });
    }
}
