using System.Collections.Generic;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{
    public Dialogue[] Parse(string _CSVFileName)
    {
        List<Dialogue> dialogueList = new List<Dialogue>();
        TextAsset csvData = Resources.Load<TextAsset>(_CSVFileName);

        if (csvData == null)
        {
            return dialogueList.ToArray();
        }

        // 열을 줄 바꿈 단위로 쪼갬
        string[] data = csvData.text.Split('\n');

        for (int i = 1; i < data.Length;)
        {
            string[] row = data[i].Split(',');
            Dialogue dialogue = new Dialogue
            {
                ID = row.Length > 1 && int.TryParse(row[0].Trim(), out int ID) ? ID : -1,
                name = row.Length > 1 ? row[1].Trim() : string.Empty,
                eventNumber = row.Length > 3 && int.TryParse(row[3].Trim(), out int eventNum) ? eventNum : -1,
                newLineX = row.Length > 4 && int.TryParse(row[4].Trim(), out int newLineX) ? newLineX : -1,
                newLineY = row.Length > 5 && int.TryParse(row[5].Trim(), out int newLineY) ? newLineY : -1,
            };

            List<string> contextList = new List<string>();

            do
            {
                if (row.Length > 2 && !string.IsNullOrWhiteSpace(row[2]))
                {
                    contextList.Add(row[2].Trim());
                }

                // 대화가 계속될 때도 이벤트 번호와 좌표를 업데이트
                if (row.Length > 3 && int.TryParse(row[3].Trim(), out eventNum))
                {
                    dialogue.eventNumber = eventNum;
                }
                if (row.Length > 4 && int.TryParse(row[4].Trim(), out newLineX))
                {
                    dialogue.newLineX = newLineX;
                }
                if (row.Length > 5 && int.TryParse(row[5].Trim(), out newLineY))
                {
                    dialogue.newLineY = newLineY;
                }

                if (++i < data.Length)
                {
                    row = data[i].Split(',');

                    // 행이 비어 있거나 불완전한 경우 계속 진행
                    if (row.Length < 1 || string.IsNullOrWhiteSpace(row[0]))
                    {
                        continue;
                    }
                }
                else
                {
                    break;
                }
            } while (string.IsNullOrWhiteSpace(row[0]));


            dialogue.context = contextList.ToArray();
            dialogueList.Add(dialogue);

        }

        return dialogueList.ToArray();
    }

}