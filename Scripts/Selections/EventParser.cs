using System.Collections.Generic;
using UnityEngine;

public class EventParser : MonoBehaviour
{
    public Selections[] Parse(string filename)
    {
        List<Selections> selectList = new List<Selections>();
        TextAsset csvData = Resources.Load<TextAsset>(filename);

        if (csvData == null)
        {
            return selectList.ToArray();
        }

        string[] data = csvData.text.Split('\n');

        // 현재 이벤트 아이디
        int currentID = -1;

        for (int i = 1; i < data.Length; i++) // 첫 행은 목록?이므로 i를 1로 시작
        {
            string[] row = data[i].Split(',');
            if (row.Length < 4) continue;

            // row[0]번째가 비어있지 않다면, 해당 정보를 currentID에 대입
            if (!string.IsNullOrEmpty(row[0]))
            {
                if (!int.TryParse(row[0], out currentID))
                {
                    continue;
                }
            }

            // 현재 ID가 설정되지 않은 경우 스킵
            if (currentID == -1)
            {
                continue;
            }

            // 각 열의 값을 파싱할 때 예외 처리 추가
            int  nextX, nextY;
            string MethodName = "";

            if (!string.IsNullOrEmpty(row[2]))
            {
                MethodName = row[2].TrimEnd('\r');
            }

            if (!int.TryParse(row[3], out nextX))
            {
                continue;
            }
            if (!int.TryParse(row[4], out nextY))
            {
                continue;
            }

            Selections selectOptions = new Selections
            {
                ID = currentID,
                Option = row[1],
                MethodName = MethodName,
                NextLineX = nextX,
                NextLineY = nextY
            };
            selectList.Add(selectOptions);
        }

        return selectList.ToArray();
    }
}

