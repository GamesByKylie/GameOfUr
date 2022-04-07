using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TextLoader : MonoBehaviour
{
    private const char DELIM = '@';
    private const string FOLDER = "Assets\\Resources";

    public static List<CrewMember> LoadCrewList()
    {
        List<CrewMember> crewList = new List<CrewMember>();

        string fileName = "crewmembers_database.txt";

        StreamReader reader = new StreamReader(FOLDER + "/" + fileName);
        //Title line
        reader.ReadLine();

        while (!reader.EndOfStream)
        {
            string[] content = reader.ReadLine().Split(DELIM);
            CrewMember crew = new CrewMember(content[1], int.Parse(content[0]), content[2], content[3]);
            crewList.Add(crew);
        }

        reader.Close();

        return crewList;
    }

    
}
