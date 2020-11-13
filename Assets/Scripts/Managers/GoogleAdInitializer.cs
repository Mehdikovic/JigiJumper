using GoogleMobileAds.Api;
using UnityEngine;


public class GoogleAdInitializer : MonoBehaviour
{
    private void Awake()
    {
        MobileAds.Initialize(initStatus => { });
    }
}
