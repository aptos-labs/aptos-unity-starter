using System;
using Aptos;
using TMPro;
using UnityEngine;

public class WalletManager : MonoBehaviour
{

    AptosClient client = new AptosClient(Networks.Devnet);

    public Ed25519Account account;

    [SerializeField]
    public TMP_Text addressComponent;

    [SerializeField]
    public TMP_Text balanceComponent;

    void Start()
    {
        var serializedPrivateKey = PlayerPrefs.GetString("serialized-ed25519-private-key");
        if (serializedPrivateKey != "")
        {
            account = new Ed25519Account(Ed25519PrivateKey.Deserialize(new(serializedPrivateKey)));
        }
        else
        {
            account = Ed25519Account.Generate();
            PlayerPrefs.SetString(
                "serialized-ed25519-private-key",
                account.PrivateKey.BcsToHex().ToString()
            );
        }
        addressComponent.text = account.Address.ToString();
        UpdateBalance();
    }

    public async void UpdateBalance()
    {
        var balance = await client.Account.GetCoinBalance(account.Address);
        balanceComponent.text = $"{balance.Amount / (decimal)Math.Pow(10, 8)} APT";
    }

    public async void FundAccount()
    {
        await client.FundAccount(account.Address, 100_000_000);
        UpdateBalance();
    }

    public void OpenInExplorer()
    {
        Application.OpenURL($"https://explorer.aptoslabs.com/account/{account.Address}?network=devnet");
    }

}
