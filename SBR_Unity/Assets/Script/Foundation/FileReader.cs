using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class FileReader
{
    public string ReadFile(string path)
    {
        path = Application.dataPath + path;
        string result = File.ReadAllText(path);

        if(result == null || result == string.Empty)
        {
            Debug.Log($"{path} is Null");
            return string.Empty;
        }
        return result;
    }
}
