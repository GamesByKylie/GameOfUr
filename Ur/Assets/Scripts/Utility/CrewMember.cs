using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewMember
{
    private string crewName;
    private string homeTown;
    private string crewDescription;
    private Sprite crewPortrait;

    public CrewMember(string name, int id, string home, string descrip)
    {
        crewName = name;
        homeTown = home;
        crewDescription = descrip;
        crewPortrait = Resources.Load<Sprite>("crew_portraits/" + id);
    }

    public override string ToString()
    {
        return $"{crewName} from {homeTown}: {crewDescription}";
    }

    public string CrewName { get { return crewName; }  }

    public string HomeTown { get { return homeTown; } }

    public string CrewDescription { get { return crewDescription; } }

    public Sprite CrewPortrait { get { return crewPortrait; } }
}
