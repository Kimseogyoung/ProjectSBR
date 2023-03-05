using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using System.Text;

public class ProtoMenu : MonoBehaviour
{
    [MenuItem("Proto/Generate Proto Class")]
    static void GenerateProtoClass()
    {
        ProtoReader reader = new ProtoReader();
        string dirPath =  Path.Join(Application.dataPath, "Data/Proto/Csv");
        string scriptsPath = Path.Join(Application.dataPath, "Script/Application/ProtoClass");

        string[] filePaths = Directory.GetFiles(dirPath, "*.csv");
        

        for(int i=0; i<filePaths.Length; i++)
        {
            string fileName = Path.GetFileName(filePaths[i]).Replace(".csv", "");
            if(!reader.LoadCsvField(out string pkName, out List<string> names, out List<string> types, reader.ReadCsv(filePaths[i])))
            {
                continue;
            }
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"public class {fileName}Proto : ProtoItem");
            builder.AppendLine("{");

            for (int j = 0; j < names.Count; j++)
            {
                string fieldName = names[j];
                string fieldType = types[j];
                builder.AppendLine("    public " + fieldType + " " + fieldName + " { get; set; }" + (pkName == fieldName?"// pk ":""));
            }

            builder.AppendLine("}");

            using (StreamWriter writer = new StreamWriter(Path.Join(scriptsPath, $"{fileName}Proto.cs")))
            {
                writer.Write(builder.ToString());
            }

            GameLogger.Info($"Generate {fileName} from {filePaths[i]} done");

        }
       
        

    }
}

