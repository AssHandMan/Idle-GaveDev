using UnityEngine;

[System.Serializable]
public class WalletData
{
    public int money, diamonds, skins;

    public WalletData(PlayerWallet wallet) 
    {
        money = wallet.Money;
        diamonds = wallet.Diamonds;
        skins = wallet.Skins;
    }
}
