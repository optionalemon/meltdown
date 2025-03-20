using UnityEngine;
using UnityEngine.UI;

public class DisasterRoomManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Text disasterRoomHeader;
    public Text disasterRoomDescription;

    void Start()
    {
        string eventType = SceneNavigator.DISASTER_EVENT_TYPE;

        setDisasterEventText(eventType);
    }

    private void setDisasterEventText(string eventType)
    {
        switch (eventType)
        {
            case "WRONG_TOMATO":
                disasterRoomHeader.text = "WARNING: FOOD WASTE CRISIS";
                disasterRoomDescription.text = @"That tomato wasn’t bad—just different.

Every year, millions of food go to waste simply because they don’t look perfect. But food waste isn’t just about appearances—it’s about wasted water, labor, and energy. It’s about growing food that never gets eaten while people go hungry.
                
Rejecting" + " \"ugly\" " + @"food fuels overflowing landfills and environmental damage.

Next time, choose wisely. Every choice makes a difference.";
                break;
            case "WRONG_EGGS":
                disasterRoomHeader.text = "You are a menace to society lol";
                disasterRoomDescription.text = @"You are probably the reason why we are so doomed right now.

Look at you, being teleported to this special education room.

Have you no shame??

You need to start doing the right thing right now.

Or not. I don't care. You're the one who is gonna die anyway.

Cheers babe.";
                break;
            case "WRONG_MILK":
                disasterRoomHeader.text = "WARNING: PLASTIC WASTE CRISIS";
                disasterRoomDescription.text = @"That plastic bottle will outlive you.

Made from fossil fuels, it takes centuries to break down. Most plastic in Singapore isn’t recycled—it’s incinerated, releasing harmful emissions. The rest lingers in landfills and oceans, polluting ecosystems.

Microplastics leach into food, water, and the air we breathe.

Your choice matters. Next time, choose sustainability before it’s too late.";
                break;
            case "WRONG MEAT":
                disasterRoomHeader.text = "You are a menace to society lol";
                disasterRoomDescription.text = @"You are probably the reason why we are so doomed right now.

Look at you, being teleported to this special education room.

Have you no shame??

You need to start doing the right thing right now.

Or not. I don't care. You're the one who is gonna die anyway.

Cheers babe.";
                break;
            default:
                disasterRoomHeader.text = "You are a menace to society lol";
                disasterRoomDescription.text = @"You are probably the reason why we are so doomed right now.

Look at you, being teleported to this special education room.

Have you no shame??

You need to start doing the right thing right now.

Or not. I don't care. You're the one who is gonna die anyway.

Cheers babe.";
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
