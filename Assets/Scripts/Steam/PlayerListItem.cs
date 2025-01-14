using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;

public class PlayerListItem : MonoBehaviour
{
    public string playerName;
    public int connectionID;
    public ulong PlayerSteamID;
    private bool AvatarReceived;

    public Text playerNameText;
    public RawImage playerIcon;
    public Text playerReadyText;
    public bool ready;

    protected Callback<AvatarImageLoaded_t> imageLoaded;

    public void ChangeReadyStatus()
    {
        if (ready)
        {
            playerReadyText.text = "Ready";
            playerReadyText.color = Color.green;
        }
        else
        {
            playerReadyText.text = "Unready";
            playerReadyText.color = Color.red;
        }
    }

    private void Start()
    {
        imageLoaded = Callback<AvatarImageLoaded_t>.Create(OnImageLoaded);
    }

    public void SetPlayerValues()
    {
        playerNameText.text = playerName;
        ChangeReadyStatus();
        if (!AvatarReceived) { GetPlayerIcon(); }
    }

    void GetPlayerIcon()
    {
        int imageID = SteamFriends.GetLargeFriendAvatar((CSteamID)PlayerSteamID);
        if (imageID == -1) { return; }
        playerIcon.texture = GetSteamImageAsTexture(imageID);
    }

    private void OnImageLoaded(AvatarImageLoaded_t callback)
    {
        if (callback.m_steamID.m_SteamID == PlayerSteamID)
        {
            playerIcon.texture = GetSteamImageAsTexture(callback.m_iImage);
        }
        else
        {
            return;
        }
    }

    private Texture2D GetSteamImageAsTexture(int iImage)
    {
        Texture2D texture = null;

        bool isValid = SteamUtils.GetImageSize(iImage, out uint width, out uint height);
        if (isValid)
        {
            byte[] image = new byte[width * height * 4];

            isValid = SteamUtils.GetImageRGBA(iImage, image, (int)(width * height * 4));

            if (isValid)
            {
                texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false, true);
                texture.LoadRawTextureData(image);
                texture.Apply();
            }
        }
        AvatarReceived = true;
        return texture;
    }
}
