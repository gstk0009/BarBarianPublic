using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ClockSystem : MonoBehaviour
{
    public static Action OnTimeChanged;
    public static Action OnCheckTaxPayment;
    public static Action TaxDialogueEvent; // 회관 대화 이벤트
    public static int Minute { get; private set; }
    public static int Hour { get; private set; }
    public static int Dday { get; private set; }

    [SerializeField] private float minuteToRealTime = 0.1f; 
    [SerializeField] private float timer;



    void Start()
    {
        timer = minuteToRealTime;
    }

    void Update()
    {
        if (Player.Instance.isPlayerInteracting)
            return;
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Minute++;

            if (Minute >= 60)
            {
                Minute = 0;
                Hour++;
                if (Hour >= 24)
                {
                    Hour = 0;
                    Dday++;
                    if (Dday % GameManager.Instance.TaxManager.TaxDue == 1 && SceneManager.GetActiveScene().buildIndex == 2)
                        OnCheckTaxPayment?.Invoke();

                    TaxDialogueEvent?.Invoke();
                }
               

            }

            OnTimeChanged?.Invoke();

            timer = minuteToRealTime;
        }
    }

   

    public static void NewLife()
    {
        Minute = 0;
        Hour = 7;
        Dday = 1;

        OnTimeChanged?.Invoke();
    }

    public static void Init(int[] LoadTime)
    {
        Hour = LoadTime[0];
        Minute = LoadTime[1];
        Dday = LoadTime[2];

        OnTimeChanged?.Invoke();
    }

    public static bool IsDayOrNight()
    {
        if (Hour >= 18 || Hour <= 6)
            return false;
        else
            return true;
    }
}