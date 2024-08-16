using System;

[Serializable]
public class TutorialTips // 로딩할 때 보여줄 한 줄 Tip
{
    public int ID;
    public string Context;
}

[Serializable]
public class LoadingTips
{
    public TutorialTips[] Tips { get; set; }
}
