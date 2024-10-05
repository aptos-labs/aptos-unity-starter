using Aptos;
using TMPro;
using UnityEngine;

public class CurrentBlockController : MonoBehaviour
{
    void Start()
    {
        GetCurrentBlock();
    }

    async void GetCurrentBlock()
    {
        var client = new AptosClient(Networks.Devnet);
        var ledgerInfo = await client.GetLedgerInfo();
        TMP_Text textComponent = transform.GetChild(0).GetComponent<TMP_Text>();
        textComponent.text = $"{ledgerInfo.BlockHeight}";
    }
}
