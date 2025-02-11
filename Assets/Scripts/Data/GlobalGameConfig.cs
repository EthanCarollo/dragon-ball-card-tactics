using UnityEngine;

[CreateAssetMenu(fileName = "GlobalGameConfig", menuName = "GlobalConfig/GlobalGameConfig")]
public class GlobalGameConfig : ScriptableObject
{
    private static GlobalGameConfig _instance;
    
    public bool debug;
    
    // If you want to update version of the game, update it here
    public static string version{ get => "0.0.7"; }
    public static string patchNotes{ get => 
@"<size=100%>v0.0.7 <size=50%>(actual)
<size=60%>- Add Gohan Mirai SSJ3, Broly Black (Berserk)
- Add Logo
- Add Pause Panel
- Fixed synergy content go out of the screen
- Fixed condition for passive card
- Add on pointer interaction with card
- Fix camera reset on end ultimate distance top attack

<size=100%>v0.0.6
<size=60%>- Fixed skip of patch scene 
- Add Gohan Mirai SSJ3
- Add Logo

<size=100%>v0.0.5  
<size=60%>- Fixed a bug causing incorrect damage calculation  
- Improved character movement fluidity 
- Add Yamcha, Metal Cooler, Toppo, Android 13, Recoom, Buucolo & Goku (Ikari)

<size=100%>v0.0.4  
<size=60%>- Added visual effects for special attacks  
- Optimized asset loading time  

<size=100%>v0.0.3  
<size=60%>- Added UI sound effects  
- Fixed character life refilling to default  

<size=100%>v0.0.2  
<size=60%>- Fixed issue where actualRound was not resetting on new game launch  
- Replaced Goku Grand Priest icon  
- Fixed issue preventing character placement if already on board  
- Updated slider when adding a star  

<size=100%>v0.0.1  
<size=60%>- First launch  
"; }

    public static GlobalGameConfig Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<GlobalGameConfig>("GlobalGameConfig");
                if (_instance == null)
                {
                    Debug.LogError("GlobalGameConfig instance not found in Resources folder!");
                }
            }
            return _instance;
        }
    }
}
