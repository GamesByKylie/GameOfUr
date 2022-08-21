using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public static class CSVLoader
{
	public static List<CrewMember> LoadMasterCrewRoster() {
		char[] lineDelimiter = new char[] { '@' };
		string filename = "crewmembers_database";
		string[] fileByLine = TryLoadListFromGameFolder(filename);

		var masterCrewList = new List<CrewMember>();

		//start at index 1 to skip the record headers
        //format is Name | ID | Home town | Description
		for (int row = 1; row < fileByLine.Length; row++) {
			string[] records = fileByLine[row].Split(lineDelimiter, StringSplitOptions.None);
            
			masterCrewList.Add(new CrewMember(records[1], int.Parse(records[0]), records[2], records[3]));
		}

		return masterCrewList;

	}

	public static void LoadTavernaGameBarks(out List<string> insults, out List<string> bragging) {

		insults = new List<string>();
		bragging = new List<string>();

		char[] lineDelimiter = new char[] { '@' };
		char newline = '%';
		string filename = "taverna_game_barks";

		string[] fileByLine = TryLoadListFromGameFolder(filename);

		for (int i = 0; i < fileByLine.Length; i++) {
			string[] texts = fileByLine[i].Split(lineDelimiter);
			string content = StripAndAddNewlines(texts[0], newline);
			if (texts[1] == "insult") {
				insults.Add(content);
			}
			else if (texts[1] == "bragging") {
				bragging.Add(content);
			}
			else {
				Debug.Log($"Taverna bark line {i} not marked insult or bragging");
			}
		}

	}

	public static void LoadPetteiaText(out List<string> flavor, out List<string> insults, out List<string> bragging, out List<string> win, out List<string> lose, out List<string> blocked) 
	{
		flavor = new List<string>();
		insults = new List<string>();
		bragging = new List<string>();
		win = new List<string>();
		lose = new List<string>();
		blocked = new List<string>();

		char[] lineDelimiter = new char[] { '@' };
		char newline = '%';
		string filename = "petteia_text";

		string[] fileByLine = TryLoadListFromGameFolder(filename);

		for (int i = 0; i < fileByLine.Length; i++) 
		{
			string[] texts = fileByLine[i].Split(lineDelimiter);
			string content = StripAndAddNewlines(texts[0], newline);
			switch (texts[1]) {
				case "flavor":
					flavor.Add(content);
					break;
				case "insult":
					insults.Add(content);
					break;
				case "brag":
					bragging.Add(content);
					break;
				case "blocked":
					blocked.Add(content);
					break;
				case "win":
					win.Add(content);
					break;
				case "lose":
					lose.Add(content);
					break;
				default:
					Debug.Log($"Petteia text line {i} not marked correctly: {texts[1]}!");
					break;
			}
		}
	}

	public static void LoadUrText() 
	{
		List<string> introFlavor = new List<string>();
		List<string> insult = new List<string>();
		List<string> win = new List<string>();
		List<string> lose = new List<string>();
		List<string> rosette = new List<string>();
		List<string> flip = new List<string>();
		List<string> capture = new List<string>();
		List<string> moveOn = new List<string>();
		List<string> moveOff = new List<string>();

		char[] lineDelimiter = new char[] { '@' };
		char newline = '%';
		string filename = "ur_text";

		string[] fileByLine = TryLoadListFromGameFolder(filename);

		for (int i = 0; i < fileByLine.Length; i++) {
			string[] texts = fileByLine[i].Split(lineDelimiter);
			string content = StripAndAddNewlines(texts[0], newline);
			switch (texts[1]) {
				case "rosette": rosette.Add(content); break;
				case "capture": capture.Add(content); break;
				case "flip": flip.Add(content); break;
				case "off": moveOff.Add(content); break;
				case "on": moveOn.Add(content); break;
				case "lose": lose.Add(content); break;
				case "win": win.Add(content); break;
				case "insult": insult.Add(content); break;
				case "intro": insult.Add(content); break;
				default: Debug.Log($"Ur text line {i} not marked correctly: {texts[1]}!"); break;
			}
		}

		GameManager.SetTextLists(introFlavor, insult, win, lose, rosette, flip, capture, moveOn, moveOff);
	}

	static string TryLoadFromGameFolder(string filename) {
		try {
			var localFile = "";
			var filePath = Application.dataPath + "/Resources/" + filename + ".txt";
			if (File.Exists(filePath)) {
				localFile = File.ReadAllText(filePath);
			}
			else
            {
				//Debug.Log(filename + " does not exist!");
			}

			//Debug.Log(Application.dataPath + "/" + filename + ".txt");
			//Debug.Log(localFile);
			if (localFile == "") {
				TextAsset file = (TextAsset)Resources.Load(filename, typeof(TextAsset));
				return file.text;
			}
			return localFile;

		}
		catch (Exception error) {
			Debug.Log("Sorry! No file: " + filename + " was found in the game directory '" + Application.dataPath + "' or the save file is corrupt!\nError Code: " + error);
			TextAsset file = (TextAsset)Resources.Load(filename, typeof(TextAsset));
			return file.text;
		}

	}

	static string[] TryLoadListFromGameFolder(string filename) {
		string[] splitFile = new string[] { "\r\n", "\r", "\n" };

		string filetext = TryLoadFromGameFolder(filename);
		string[] fileByLine = filetext.Split(splitFile, StringSplitOptions.None);

		// remove any trailing newlines since the parsers assume there's no newline at the end of the file, but VS auto-adds one
		return fileByLine
			.Where(line => !string.IsNullOrEmpty(line))
			.ToArray();
	}

	static string StripAndAddNewlines(string modify, char newline) {
		string s = modify.Replace(newline, '\n');
		if (s[0] == '\"') 
		{
			s = s.Substring(1, s.Length - 2);
		}
		
		return s;
	}
}
