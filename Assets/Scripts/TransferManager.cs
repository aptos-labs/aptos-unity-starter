using System;
using System.Collections.Generic;
using Aptos;
using UnityEngine;

public class TransferManager : MonoBehaviour
{
    AptosClient client = new AptosClient(Networks.Devnet);

    [SerializeField]
    public WalletManager walletManager;

    [SerializeField]
    public GameObject transactionExplorerButton;

    Hex transferTransactionHash;

    string transferRecipient;

    string transferAmount;

    void Start()
    {
        transactionExplorerButton.SetActive(false);
    }

    public void UpdateTransferRecipient(string recipient)
    {
        transferRecipient = recipient;
    }
    public void UpdateTransferAmount(string amount)
    {
        transferAmount = amount;
    }

    public async void SignAndSubmitTransferTransaction()
    {
        AccountAddress recipient = AccountAddress.From(transferRecipient);
        ulong parsedAmount = Convert.ToUInt64(decimal.Parse(transferAmount) * (decimal)Math.Pow(10, 8));

        var transaction = await client.Transaction.Build(
            sender: walletManager.account.Address,
            data: new GenerateEntryFunctionPayloadData(
                function: "0x1::aptos_account::transfer_coins",
                typeArguments: new List<object>() { "0x1::aptos_coin::AptosCoin" },
                functionArguments: new List<object>() { recipient, parsedAmount }
            )
        );

        var pendingTransaction = await client.Transaction.SignAndSubmitTransaction(walletManager.account, transaction);

        transactionExplorerButton.SetActive(true);
        transferTransactionHash = pendingTransaction.Hash;

        await client.Transaction.WaitForTransaction(pendingTransaction.Hash);

        walletManager.UpdateBalance();
    }

    public void OpenInExplorer()
    {
        Application.OpenURL($"https://explorer.aptoslabs.com/txn/{transferTransactionHash}?network=devnet");
    }
}
