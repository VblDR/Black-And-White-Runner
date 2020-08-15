using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class GoogleInterstitialScript : MonoBehaviour
{
    private string appId = "ca-app-pub-3940256099942544~3347511713";
    private InterstitialAd interstitial;

    private void Start()
    {
        MobileAds.Initialize(appId);
        this.RequestInterstitial();
    }


    private void RequestInterstitial()
    {
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";

        this.interstitial = new InterstitialAd(adUnitId);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().AddTestDevice("2B9F4C867444DDAAF6745C4D4387401E").AddTestDevice("D71202487FA94E485235A500A800C160").AddTestDevice("0F25E200ED613A40BA265F9950B19B0F").Build(); //AdRequest request = new AdRequest.Builder().AddTestDevice(%device_id%).Build(); for testing Google Ads
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);

        // Called when an ad request has successfully loaded.
        this.interstitial.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.interstitial.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this.interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;
    }

    public void ShowInterstitial()
    {
        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
        }
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.Message);
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }

    private void OnDestroy()
    {
        this.interstitial.OnAdLoaded -= HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitial.OnAdFailedToLoad -= HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.interstitial.OnAdOpening -= HandleOnAdOpened;
        // Called when the ad is closed.
        this.interstitial.OnAdClosed -= HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this.interstitial.OnAdLeavingApplication -= HandleOnAdLeavingApplication;
    }
}
