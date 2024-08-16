using System;
using UnityEngine;

public class CabinTop : MonoBehaviour
{
    public static Action<bool> PlayerEnter;
    private bool isPlayerIn = false;
    private Animator Top;

    private void Awake()
    {
        Top = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerIn = true;
            PlayerEnter?.Invoke(isPlayerIn);
            Top.SetBool("Enter", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerIn = false;
            PlayerEnter?.Invoke(isPlayerIn);
            Top.SetBool("Enter", false);
        }
    }
}
