using System.Collections.Generic;

namespace _40KListBot
{
    public class ArmyList : ArmyListItem
    {
        public string Name {get; set;}
        public List<Detachment> Detachments {get; set;} 
    }
    public class Detachment : ArmyListItem
    {
        public string Name {get; set;}
        public List<ForceOrg> ForceOrgs {get; set;}
    }
    public class ForceOrg : ArmyListItem
    {
        public string Name {get; set;}
        public List<Unit> Units {get; set;}
    }
    public class Unit : ArmyListItem
    {
        public string Name {get; set;}
        public List<UnitModel> Models {get; set;}
    }
    public class UnitModel : ArmyListItem
    {
        public string Name {get; set;}
        public List<WarGear> WarGears {get; set;}
    }
    //How Detailed do we need to be? Honestly this is supposed to be 'simpler' to read than BScribe, so  simple names is how e go
    public class WarGear : ArmyListItem
    {
        public string Name {get; set;}
    }
    public interface ArmyListItem
    {
        public string Name {get; set;}
    }
}