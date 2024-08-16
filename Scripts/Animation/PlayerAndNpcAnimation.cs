using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAndNpcAnimation : MonoBehaviour
{
    private float xInput, yInput;
    private float attackSpeed = 1.5f;
    public Direction DirectionWay;

    private string playAnim;
    private string currentState;

    private string fileBasePath;
    private string filePath;
    Dictionary<string, string> textPaths;

    private bool isSetEmpty = false;

    public GameObject hat;
    public GameObject hair;
    public Texture2D EmptyTexture;

    [Header("BasePath of textures for runtime")]
    public string textureBasePath;

    [Header("Textures to use for showing the animation")]
    public Texture2D backgroundT;

    public Texture2D bodyT;
    public Texture2D outfitT;
    public Texture2D cloakT;
    public Texture2D faceitemsT;
    public Texture2D hairT;
    public Texture2D hatT;
    public Texture2D pritoolT;
    public Texture2D sectoolT;
    public Texture2D topT;

    [Header("Weapon Textures")]
    public Texture2D mainhandSwordAndShieldT;

    public Texture2D offhandSwordAndShieldT;
    public Texture2D spearT;
    public Texture2D mainhandBowT;
    public Texture2D offhandBowT;

    [Header("Location of the sprites used for the animation (inside 'Assets/Resouces')")]
    public string SpriteSetPath;

    WaitForSeconds delayTime = new WaitForSeconds(0.3f);

    private void Start()
    {
        DirectionWay = Direction.down;

        SetCharacterTextures();
    }

    public void PlayerAnimUpdate()
    {
        EventHandler.PlayerCallMovementInputEvent(xInput, yInput, DirectionWay, attackSpeed);
        if (playAnim != null)
        {
            EventHandler.PlayerCallMovementEvent(playAnim);
            playAnim = null;
        }
    }

    public void NpcAnimUpdate(NPCType npcType)
    {
        EventHandler.NpcCallMovementInputEvent(xInput, yInput, DirectionWay, attackSpeed, npcType);
        if (playAnim != null)
        {
            EventHandler.NpcCallMovementEvent(playAnim, npcType);
            playAnim = null;
        }
    }

    private void SetCharacterTextures()
    {
        if (backgroundT != null)
        {
            SetTexture(backgroundT, "background");
        }
        if (bodyT != null)
        {
            SetTexture(bodyT, "body");
        }
        if (outfitT != null)
        {
            SetTexture(outfitT, "outfit");
        }
        if (cloakT != null)
        {
            SetTexture(cloakT, "cloak");
        }
        if (faceitemsT != null)
        {
            SetTexture(faceitemsT, "faceitems");
        }
        if (hairT != null)
        {
            SetTexture(hairT, "hair");
        }
        if (hatT != null)
        {
            hair.SetActive(false);
            SetTexture(hatT, "hat");
        }
        if (pritoolT != null)
        {
            SetTexture(pritoolT, "pritool");
        }
        if (sectoolT != null)
        {
            SetTexture(sectoolT, "sectool");
        }
        if (topT != null)
        {
            SetTexture(topT, "top");
        }
        //Weapon Textures
        if (mainhandSwordAndShieldT != null)
        {
            SetTexture(mainhandSwordAndShieldT, "mainhand", true);
        }
        if (offhandSwordAndShieldT != null)
        {
            SetTexture(offhandSwordAndShieldT, "offhand", true);
        }
        if (spearT != null)
        {
            SetTexture(spearT, "mainhand", true);
        }
        if (mainhandBowT != null)
        {
            SetTexture(mainhandBowT, "mainhand", true);
        }
        if (offhandBowT != null)
        {
            SetTexture(offhandBowT, "offhand", true);
        }
    }

    private void SetTexture(Texture2D texture, string layer, bool combatAnimation = false)
    {
        //Base Location of all sprites
        if (!isSetEmpty)
        {
            fileBasePath = textureBasePath;
            fileBasePath = fileBasePath.Replace("Assets/Resources/", "");

            //specific part of the path
            filePath = "";
            string[] partedName = texture.name.Split('_');
            filePath += partedName[0] + "_" + partedName[1] + "_" + partedName[2] + "/";
            if (partedName[3] != "0bas") filePath += partedName[3] + "/";
            filePath += texture.name;

            textPaths = SetTextureFilePaths(filePath, partedName);
        }

        //Combat Textures
        StartCoroutine(combatTexture(layer, fileBasePath, filePath, textPaths, texture));

        //Base Textures
        if (!combatAnimation)
        {
            StartCoroutine(BaseTexture(layer, fileBasePath, filePath, textPaths, texture));
        }

        isSetEmpty = false;
    }

    IEnumerator combatTexture(string layer, string fileBasePath, string filePath, Dictionary<string, string> textPaths, Texture2D texture)
    {
        Texture2D pONE1Texture;
        Texture2D pONE2Texture;
        Texture2D pONE3Texture;
        Texture2D pPOL1Texture;
        Texture2D pPOL2Texture;
        Texture2D pPOL3Texture;
        Texture2D pBOW1Texture;
        Texture2D pBOW2Texture;
        Texture2D pBOW3Texture;

        if (layer == "mainhand") layer = "pritool";
        if (layer == "offhand") layer = "sectool";

        if (!isSetEmpty)
        {
            pONE1Texture = SetTexture(fileBasePath, textPaths, "pONE1", true);
            yield return null;
            pONE2Texture = SetTexture(fileBasePath, textPaths, "pONE2", true);
            yield return null;
            pONE3Texture = SetTexture(fileBasePath, textPaths, "pONE3", true);
            yield return null;
            pPOL1Texture = SetTexture(fileBasePath, textPaths, "pPOL1", true);
            yield return null;
            pPOL2Texture = SetTexture(fileBasePath, textPaths, "pPOL2", true);
            yield return null;
            pPOL3Texture = SetTexture(fileBasePath, textPaths, "pPOL3", true);
            yield return null;
            pBOW1Texture = SetTexture(fileBasePath, textPaths, "pBOW1", true);
            yield return null;
            pBOW2Texture = SetTexture(fileBasePath, textPaths, "pBOW2", true);
            yield return null;
            pBOW3Texture = SetTexture(fileBasePath, textPaths, "pBOW3", true);
            yield return null;
        }
        else
        {
            pONE1Texture = texture;
            pONE2Texture = texture;
            pONE3Texture = texture;
            pPOL1Texture = texture;
            pPOL2Texture = texture;
            pPOL3Texture = texture;
            pBOW1Texture = texture;
            pBOW2Texture = texture;
            pBOW3Texture = texture;
        }

        yield return null;

        if (pONE1Texture != null)
        {
            StartCoroutine(FillPlayerTextures(layer, pONE1Texture, "combat/pONE1"));
            yield return null;
            StartCoroutine(FillPlayerTextures(layer, pONE2Texture, "combat/pONE2"));
            yield return null;
        }
        if (pPOL1Texture != null)
        {
            StartCoroutine(FillPlayerTextures(layer, pPOL1Texture, "combat/pPOL1"));
            yield return null;
            StartCoroutine(FillPlayerTextures(layer, pPOL2Texture, "combat/pPOL2"));
            yield return null;
            StartCoroutine(FillPlayerTextures(layer, pPOL3Texture, "combat/pPOL3"));
            yield return null;
        }
        if (pBOW1Texture != null)
        {
            StartCoroutine(FillPlayerTextures(layer, pBOW1Texture, "combat/pBOW1"));
            yield return null;
            StartCoroutine(FillPlayerTextures(layer, pBOW2Texture, "combat/pBOW2"));
            yield return null;
            StartCoroutine(FillPlayerTextures(layer, pBOW3Texture, "combat/pBOW3"));
            yield return null;
        }
        if (pONE3Texture != null)
        {
            StartCoroutine(FillPlayerTextures(layer, pONE3Texture, "combat/pONE3"));
            yield return null;
        }
    }

    IEnumerator BaseTexture(string layer, string fileBasePath, string filePath, Dictionary<string, string> textPaths, Texture2D texture)
    {
        Texture2D p1Texture;
        Texture2D p1BTexture;
        Texture2D p1CTexture;
        Texture2D p2Texture;
        Texture2D p3Texture;
        Texture2D p4Texture;

        if (!isSetEmpty)
        {
            p1Texture = SetTexture(fileBasePath, textPaths, "p1", false);
            yield return null;
            p1BTexture = SetTexture(fileBasePath, textPaths, "p1B", false);
            yield return null;
            p1CTexture = SetTexture(fileBasePath, textPaths, "p1C", false);
            yield return null;
            p2Texture = SetTexture(fileBasePath, textPaths, "p2", false);
            yield return null;
            p3Texture = SetTexture(fileBasePath, textPaths, "p3", false);
            yield return null;
            if (layer == "pritool")
            {
                string fishing_test = filePath;
                if (fishing_test.Contains("6tla"))
                {
                    string[] p3pathParts = textPaths["p3"].Split('_');

                    string replacer = p3pathParts[6];
                    p3Texture = Resources.Load<Texture2D>(fileBasePath + textPaths["p3"].Replace(replacer, "roda").Replace(".png", ""));
                }
            }

            yield return null;

            p4Texture = SetTexture(fileBasePath, textPaths, "p4", false);

            yield return null;
        }
        else
        {
            p1Texture = texture;
            p1BTexture = texture;
            p1CTexture = texture;
            p2Texture = texture;
            p3Texture = texture;
            p4Texture = texture;
        }

        yield return null;

        StartCoroutine(FillPlayerTextures(layer, p1Texture, "p1"));
        yield return null;
        StartCoroutine(FillPlayerTextures(layer, p1BTexture, "p1B"));
        yield return null;
        StartCoroutine(FillPlayerTextures(layer, p1CTexture, "p1C"));
        yield return null;
        StartCoroutine(FillPlayerTextures(layer, p2Texture, "p2"));
        yield return null;
        StartCoroutine(FillPlayerTextures(layer, p3Texture, "p3"));
        yield return null;
        StartCoroutine(FillPlayerTextures(layer, p4Texture, "p4"));
        yield return null;
    }

    IEnumerator FillPlayerTextures(string layer, Texture2D pTexture, string key)
    {
        if (SpriteSetPath.EndsWith("/")) SpriteSetPath = SpriteSetPath.TrimEnd('/');
        Texture2D originp1 = Resources.Load<Texture2D>(SpriteSetPath + "/" + key + "/" + layer);

        yield return null;

        if (pTexture != null && originp1 != null)
        {
            Color[] newPixelsp1 = pTexture.GetPixels();
            originp1.SetPixels(newPixelsp1);
            originp1.Apply();
        }

        yield return null;
    }

    private static Texture2D SetTexture(string fileBasePath, Dictionary<string, string> textPaths, string textureKey, bool combatAnimation)
    {
        if (!fileBasePath.EndsWith("/")) fileBasePath += "/";
        Texture2D pTexture = null;
        if (textPaths[textureKey] != "")
        {
            pTexture = Resources.Load<Texture2D>(fileBasePath + textPaths[textureKey].Replace(".png", ""));
            if (combatAnimation)
                if (pTexture == null)
                    pTexture = Resources.Load<Texture2D>(fileBasePath + "combat/" + textPaths[textureKey].Replace(".png", ""));
        }

        return pTexture;
    }

    private static Dictionary<string, string> SetTextureFilePaths(string filePath, string[] partedName)
    {
        Dictionary<string, string> textPaths = new Dictionary<string, string>()
        {
            { "p1", "" },
            { "p1B", "" },
            { "p1C", "" },
            { "p2", "" },
            { "p3", "" },
            { "p4", "" },
            { "pONE1", "" },
            { "pONE2", "" },
            { "pONE3", "" },
            { "pPOL1", "" },
            { "pPOL2", "" },
            { "pPOL3", "" },
            { "pBOW1", "" },
            { "pBOW2", "" },
            { "pBOW3", "" },
        };
        Dictionary<string, string> newPaths = new Dictionary<string, string>();
        foreach (KeyValuePair<string, string> tp in textPaths)
        {
            if (filePath.Contains("char_a_" + partedName[2]))
                newPaths[tp.Key] = filePath.Replace("_" + partedName[2], "_" + tp.Key);
            else newPaths[tp.Key] = tp.Value;
        }

        return newPaths;
    }

    public void PlayAnimation(string anim)
    {
        if (currentState == anim) return;

        playAnim = anim;

        currentState = anim;
    }

    public void SwapHatButton()
    {
        if (hat.activeInHierarchy)
        {
            hat.SetActive(false);
            hair.SetActive(true);
        }
        else
        {
            hat.SetActive(true);
            hair.SetActive(false);
        }
    }

    private void MovementInput()
    {
        if (xInput != 0 || yInput != 0)
        {
            //capture movement
            if (xInput < 0) DirectionWay = Direction.left;
            else if (xInput > 0) DirectionWay = Direction.right;
            else if (yInput < 0) DirectionWay = Direction.down;
            else DirectionWay = Direction.up;
        }
    }

    public void SetInputXY(float x, float y)
    {
        xInput = x;
        yInput = y;
        MovementInput();
    }

    public void SetAttackSpeed(float speed)
    {
        attackSpeed = speed;
    }

    public void SwapTexture(Texture2D newTexture, string layer)
    {
        SetTexture(newTexture, layer);
    }

    public void SetEmptyTexture(string layer)
    {

        isSetEmpty = true;

        if (layer.CompareTo("outfit") != 0)
        {
            SetTexture(EmptyTexture, layer);
        }
        else
        {
            isSetEmpty = false;
            SetTexture(outfitT, layer);
        }
    }

    public void SetEmptyTextureAll()
    {
        string[] layer = {"outfit", "mainhand", "offhand" };

        for(int i=0;i<layer.Length; i++)
        {
            SetEmptyTexture(layer[i]);
        }
    }
}