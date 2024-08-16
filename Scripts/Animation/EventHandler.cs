public delegate void MovementDelegate(string playAnim);
public delegate void MovementInputXY(float inputX, float inputY, Direction direction, float speed);

public static class EventHandler
{
    //Movement Event
    public static event MovementDelegate PlayerMovementEvent;
    public static event MovementInputXY PlayerMovementInputEvent;

    public static event MovementDelegate SpearNpcMovementEvent;
    public static event MovementInputXY SpearNpcMovementInputEvent;

    public static event MovementDelegate BowNpcMovementEvent;
    public static event MovementInputXY BowNpcMovementInputEvent;

    public static event MovementDelegate WandNpcMovementEvent;
    public static event MovementInputXY WandNpcMovementInputEvent;

    //Movement Event Call for Publishers
    public static void PlayerCallMovementEvent(string playAnim)
    {
        if (PlayerMovementEvent != null)
            PlayerMovementEvent(playAnim);
    }

    public static void NpcCallMovementEvent(string playAnim, NPCType npcType)
    {
        switch(npcType)
        {
            case NPCType.Spear:
                if (SpearNpcMovementEvent != null)
                    SpearNpcMovementEvent(playAnim);
                break;
            case NPCType.Bow:
                if (BowNpcMovementEvent != null)
                    BowNpcMovementEvent(playAnim);
                break;
            case NPCType.Wand:
                if (WandNpcMovementEvent != null)
                    WandNpcMovementEvent(playAnim);
                break;
        }
    }

    public static void PlayerCallMovementInputEvent(float inputX, float inputY, Direction direction, float speed)
    {
        if (PlayerMovementInputEvent != null)
            PlayerMovementInputEvent(inputX, inputY, direction, speed);
    }

    public static void NpcCallMovementInputEvent(float inputX, float inputY, Direction direction, float speed, NPCType npcType)
    {
        switch (npcType)
        {
            case NPCType.Spear:
                if (SpearNpcMovementInputEvent != null)
                    SpearNpcMovementInputEvent(inputX, inputY, direction, speed);
                break;
            case NPCType.Bow:
                if (BowNpcMovementInputEvent != null)
                    BowNpcMovementInputEvent(inputX, inputY, direction, speed);
                break;
            case NPCType.Wand:
                if (WandNpcMovementInputEvent != null)
                    WandNpcMovementInputEvent(inputX, inputY, direction, speed);
                break;
        }
    }
}