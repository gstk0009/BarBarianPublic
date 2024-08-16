using System;
using UnityEngine;
[Serializable]
public class Dialogue
{
    public string name;
    public string[] context;
    public int eventNumber;
    public int newLineX;
    public int newLineY;
    public int ID;
}

[Serializable]
public class DialogueEvent
{
    // 얘를 딕셔너리로 식별하는 key로 사용
    public string eventName;
    public Dialogue[] dialogues;

    // x부터 y까지의 대사를 추출
    // ex. 3번째 줄부터 7번째 줄까지의 대사를 추출
    public Vector2 line;
    
    // 파싱된 x, y값으로 line 초기화
    public void SetNewLine(int x, int y)
    {
        line = new Vector2(x, y);
    }

}