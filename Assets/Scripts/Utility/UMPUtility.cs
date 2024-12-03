using GoogleMobileAds.Api;
using GoogleMobileAds.Ump.Api;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[MonoSingletonPath("[UMP]/UMPManager")]
public class UMPManager : MonoSingleton<UMPManager>, ICanGetUtility, ICanSendEvent
{
    public void Init()
    {
        ConsentInformation.Reset();

        var debugSettings = new ConsentDebugSettings
        {
        // Geography appears as in EEA for debug devices.
             DebugGeography = DebugGeography.EEA,
            TestDeviceHashedIds = new List<string>
        {
            "2D1C614EFB2368631D823B0F057206FF"
        }
        };

        // Create a ConsentRequestParameters object.
        ConsentRequestParameters request = new ConsentRequestParameters();

        // Check the current consent information status.
        ConsentInformation.Update(request, OnConsentInfoUpdated);
    }

    void OnConsentInfoUpdated(FormError consentError)
    {
        if (consentError != null)
        {
            // Handle the error.
            UnityEngine.Debug.LogError(consentError);
            return;
        }

        ConsentForm.LoadAndShowConsentFormIfRequired((FormError formError) =>
        {
            if (formError != null)
            {
                // Consent gathering failed.
                UnityEngine.Debug.LogError(consentError);
                return;
            }

            // Consent has been gathered.
            if (ConsentInformation.CanRequestAds())
            {
                MobileAds.Initialize((InitializationStatus initstatus) =>
                {
                    // TODO: Request an ad.
                });
            }
        });
        // If the error is null, the consent information state was updated.
        // You are now ready to check if a form is available.
    }
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }
}
