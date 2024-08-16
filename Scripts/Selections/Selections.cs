using System;

[Serializable]
public class Selections 
{
    public int ID;
    public string Option;
    public string MethodName; // 선택지에 따라 실행할 함수의 이름
    public int NextLineX, NextLineY;
}

[Serializable]
public class SelectEvent
{
    public Selections[] Selecter { get; set; }
}
