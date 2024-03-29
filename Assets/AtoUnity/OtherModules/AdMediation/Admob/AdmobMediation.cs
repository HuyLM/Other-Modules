using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AtoGame.Mediation
{
#if ATO_ADMOB_MEDIATION_ENABLE
    public class AdmobMediation : SingletonFreeAlive<AdmobMediation>, IAdMediationHandler
    {
        [SerializeField] private bool enableTestDevices;
        [SerializeField] private List<string> deviceIds;

        [Header("Android")]
        [SerializeField] private string androidInterstitialAdId;
        [SerializeField] private string androidVideoAdId;
        [SerializeField] private string androidBannerAdId;
        [SerializeField] private string androidInterstitialAdTestId = "ca-app-pub-3940256099942544/1033173712";
        [SerializeField] private string androidVideoAdTestId = "ca-app-pub-3940256099942544/5224354917";
        [SerializeField] private string androidBannerAdTestId = "ca-app-pub-3940256099942544/6300978111";
        [Header("IOS")]
        [SerializeField] private string iosInterstitialAdId;
        [SerializeField] private string iosVideoAdId;
        [SerializeField] private string iosBannerAdId;
        [SerializeField] private string iosInterstitialAdTestId = "ca-app-pub-3940256099942544/4411468910";
        [SerializeField] private string iosVideoAdTestId = "ca-app-pub-3940256099942544/1712485313";
        [SerializeField] private string iosBannerAdTestId = "ca-app-pub-3940256099942544/2934735716";
        [Header("Privacy")]
        [SerializeField] private bool isAgeRestrictedUser = false;
        [SerializeField] private string maxAdContent = "MA";
        [Space(20)]
        [Header("Default Banner")]
        [SerializeField] private BannerSize defaultBannerSize;
        [SerializeField] private BannerPosition defaultBannerPosition = BannerPosition.BOTTOM_CENTER;

        private AdmobVideoRewardAd rewardedAd;
        private AdmobInterstitialAd interstitialAd;
        private AdmobBannerAd bannerAd;

        private bool isInitialized;

        private string InterstitialAdUnitId
        {
            get
            {
#if UNITY_ANDROID
#if TEST_AD
                return androidInterstitialAdTestId;
#else
                return androidInterstitialAdId;
#endif
#elif UNITY_IPHONE

#if TEST_AD
                return iosInterstitialAdTestId;
#else
              return iosInterstitialAdId;
#endif

#else
                return string.Empty;
#endif
            }
        }
        private string RewardedAdUnitId
        {
            get
            {
#if UNITY_ANDROID
#if TEST_AD
                return androidVideoAdTestId;
#else
                return androidVideoAdId;
#endif
#elif UNITY_IPHONE
#if TEST_AD
                return iosVideoAdTestId;
#else
                return iosVideoAdId;
#endif
#else
                return string.Empty;
#endif
            }
        }

        private string BannerAdUnitId
        {
            get
            {
#if UNITY_ANDROID
#if TEST_AD
                return androidBannerAdTestId;
#else
                return androidBannerAdId;
#endif
#elif UNITY_IPHONE
#if TEST_AD
                return iosBannerAdTestId;
#else
                return iosBannerAdId;
#endif
#else
                return string.Empty;
#endif
            }
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            AdMediation.SetHandler(this);
        }

        public void Init()
        {
#if UNITY_STANDALONE
            return;
#endif

            if (isInitialized == true)
            {
                return;
            }

            if (string.IsNullOrEmpty(RewardedAdUnitId) == false)
            {
                rewardedAd = new AdmobVideoRewardAd(RewardedAdUnitId);
            }
            if (string.IsNullOrEmpty(InterstitialAdUnitId) == false)
            {
                interstitialAd = new AdmobInterstitialAd(InterstitialAdUnitId);
            }


            // On Android, Unity is paused when displaying interstitial or rewarded video.
            // This setting makes iOS behave consistently with Android.
            GoogleMobileAds.Api.MobileAds.SetiOSAppPauseOnBackground(true);

            // When true all events raised by GoogleMobileAds will be raised
            // on the Unity main thread. The default value is false.
            // https://developers.google.com/admob/unity/quick-start#raise_ad_events_on_the_unity_main_thread
            GoogleMobileAds.Api.MobileAds.RaiseAdEventsOnUnityMainThread = true;

            if (enableTestDevices == false)
            {
                deviceIds.Clear();
            }

            deviceIds.Add(GoogleMobileAds.Api.AdRequest.TestDeviceSimulator);



            var debugGeography = GoogleMobileAds.Ump.Api.DebugGeography.EEA;
            var tagForUnderAgeOfConsent = isAgeRestrictedUser;
            // Confugre the ConsentDebugSettings.
            // The ConsentDebugSettings is serializable so you may expose this to your monobehavior.
            var consentDebugSettings = new GoogleMobileAds.Ump.Api.ConsentDebugSettings();
            consentDebugSettings.DebugGeography = debugGeography;
            consentDebugSettings.TestDeviceHashedIds = deviceIds;

            // Set tag for under age of consent. Here false means users are not under age.
            var consentRequestParameters = new GoogleMobileAds.Ump.Api.ConsentRequestParameters();
            consentRequestParameters.ConsentDebugSettings = consentDebugSettings;
            consentRequestParameters.TagForUnderAgeOfConsent = tagForUnderAgeOfConsent;

            GoogleMobileAds.Ump.Api.ConsentInformation.Update(consentRequestParameters,
                // OnConsentInformationUpdate
                (GoogleMobileAds.Ump.Api.FormError error) =>
                {
                    if (error == null)
                    {
                        // The consent information updated successfully.
                        Debug.Log(string.Format(
                            "Consent information updated to {0}. You may load the consent " +
                            "form.", GoogleMobileAds.Ump.Api.ConsentInformation.ConsentStatus));

                        GoogleMobileAds.Ump.Api.ConsentForm.LoadAndShowConsentFormIfRequired((GoogleMobileAds.Ump.Api.FormError formError) => {
                            if (formError != null)
                            {
                                // Consent gathering failed.
                                UnityEngine.Debug.LogError(formError);
                            }

                            if (ConsentInformation.CanRequestAds())
                            {
                                Debug.Log("ConsentInformation.CanRequestAds = true");
                                InitAdmob();
                            }
                        });
                    }
                    else
                    {
                        // The consent information failed to update.
                        Debug.LogError("Failed to update consent information with error: " +
                                error.Message);
                        InitAdmob();
                    }
                }
            );



            void InitAdmob()
            {
                AdsEventExecutor.ExecuteInUpdate(() => {
                    GoogleMobileAds.Api.RequestConfiguration requestConfiguration = new GoogleMobileAds.Api.RequestConfiguration
                    {
                        TestDeviceIds = deviceIds,
                        TagForChildDirectedTreatment = isAgeRestrictedUser ? GoogleMobileAds.Api.TagForChildDirectedTreatment.True : GoogleMobileAds.Api.TagForChildDirectedTreatment.False,
                        MaxAdContentRating = GoogleMobileAds.Api.MaxAdContentRating.ToMaxAdContentRating(maxAdContent)
                    };
                    GoogleMobileAds.Api.MobileAds.SetRequestConfiguration(requestConfiguration);
                    // Initialize the Google Mobile Ads SDK.
                    GoogleMobileAds.Api.MobileAds.Initialize(HandleInitCompleteAction);
                });
            }
        }

        private void HandleInitCompleteAction(GoogleMobileAds.Api.InitializationStatus initstatus)
        {
            if (initstatus == null)
            {
                Debug.LogError("Google Mobile Ads initialization failed.");
                return;
            }
            var adapterStatusMap = initstatus.getAdapterStatusMap();
            if (adapterStatusMap != null)
            {
                foreach (var item in adapterStatusMap)
                {
                    Debug.Log(string.Format("Adapter {0} is {1}",
                        item.Key,
                        item.Value.InitializationState));
                }
            }


            Debug.Log("Google Mobile Ads initialization complete.");
            isInitialized = true;

            //GoogleMobileAds.Api.MobileAds.SetApplicationVolume(0.5f);
            //GoogleMobileAds.Api.MobileAds.SetApplicationMuted(true);

            LoadInterstitial();
            LoadRewardVideo();
        }

        public void ShowTestSuite()
        {
            if (isInitialized)
            {
                Debug.Log("Opening ad Inspector.");
                GoogleMobileAds.Api.MobileAds.OpenAdInspector((GoogleMobileAds.Api.AdInspectorError error) =>
                {
                    // If the operation failed, an error is returned.
                    if (error != null)
                    {
                        Debug.Log("Ad Inspector failed to open with error: " + error);
                        return;
                    }
                    Debug.Log("Ad Inspector opened successfully.");
                });
            }
        }

    #region Video Reward

        public bool IsRewardVideoAvailable()
        {
            return rewardedAd != null && rewardedAd.IsAvailable;
        }

        public void ShowRewardVideo(Action<string, AdInfo> onCompleted = null, Action<string, AdInfo> onFailed = null)
        {
            if (IsRewardVideoAvailable())
            {
                rewardedAd.Show(onCompleted, onFailed);
            }
            else
            {
                onFailed?.Invoke("Reward Ad not available", null);
            }
        }

        public void LoadRewardVideo()
        {
            rewardedAd?.Request();
        }

    #endregion

    #region Interstitial
        public bool IsInterstitialAvailable()
        {
            return interstitialAd != null && interstitialAd.IsAvailable;
        }

        public void ShowInterstitial(Action<string, AdInfo> onCompleted = null, Action<string, AdInfo> onFailed = null)
        {
            if (IsInterstitialAvailable())
            {
                interstitialAd.Show(onCompleted, onFailed);
            }
            else
            {
                onFailed?.Invoke("Interstitial Ad not available", null);
            }
        }

        public void LoadInterstitial()
        {
            interstitialAd?.Request();
        }
    #endregion

    #region Banner

        public void ShowBanner(Action<string, AdInfo> onCompleted = null, Action<string, AdInfo> onFailed = null)
        {
            if(bannerAd != null)
            {
                bannerAd.DestroyBanner();
                bannerAd = null;
            }

            bannerAd = new AdmobBannerAd(BannerAdUnitId, AdmobHelper.GetBannerSize(defaultBannerSize), AdmobHelper.GetBannerPosition(defaultBannerPosition));
            bannerAd.Show(onCompleted, onFailed);
        }

        public void ShowBanner(BannerPosition position, bool isAdaptive, BannerSize size, int width, int height, Action<string, AdInfo> onCompleted = null, Action<string, AdInfo> onFailed = null)
        {
            Debug.Log("Coming Soon!!!");
        }

        public void DestroyBanner()
        {
            if (bannerAd != null)
            {
                bannerAd.DestroyBanner();
                bannerAd = null;
            }
        }

        public void HideBanner()
        {
            Debug.Log("Admob Mediation not support!!!");
        }

        public void DisplayBanner()
        {
            Debug.Log("Admob Mediation not support!!!");
        }
    #endregion
    }
#endif
}
