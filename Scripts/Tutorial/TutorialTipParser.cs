using System.Collections.Generic;
using UnityEngine;

public class TutorialTipParser : MonoBehaviour
{
    public TutorialTips[] Parse(string filename)
    {
        List<TutorialTips> tutorialTips = new List<TutorialTips>();
        TextAsset csvData = Resources.Load<TextAsset>(filename);

        if (csvData == null)
        {
            return tutorialTips.ToArray();
        }


        string[] data = csvData.text.Split('\n');

        int currentID = -1;

        for (int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(',');

            if (row.Length < 2)
            {
                continue;
            }

            if (!int.TryParse(row[0], out currentID))
            {
                continue;
            }

            string currentContext = row[1].Trim().Replace("'", ",");

            TutorialTips tips = new TutorialTips
            {
                ID = currentID,
                Context = currentContext,
            };

            tutorialTips.Add(tips); // 리스트에 추가
        }

        return tutorialTips.ToArray();
    }
}
