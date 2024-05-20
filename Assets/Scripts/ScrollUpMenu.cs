using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScrollUpMenu : MonoBehaviour
{
    private const string IS_PRESSED = "IsPressed";
    private Animator animator;
    private Touch touch;

    private void Start() {
        animator = GetComponent<Animator>();
        touch = Input.touches[0];
    }

    private void Update() {
        if (touch.phase == TouchPhase.Began) OnDown();
    }

    private void OnDown() {
        Debug.Log("touch");
        animator.SetBool(IS_PRESSED, true);
    }
}
