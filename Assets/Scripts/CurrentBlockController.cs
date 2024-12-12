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
        AptosUnityClient client = new AptosUnityClient(Networks.Devnet);
        var ledgerInfo = await client.GetLedgerInfo();
        TMP_Text textComponent = transform.GetChild(0).GetComponent<TMP_Text>();
        textComponent.text = $"{ledgerInfo.BlockHeight}";
    }
}
