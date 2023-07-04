using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.Mediation
{
#if ATO_ADMOB_MEDIATION_ENABLE
    public class AdmobBannerAd : BaseAd
    {
        private string adUnitId;
        private GoogleMobileAds.Api.AdSize size;
        private GoogleMobileAds.Api.AdPosition position;

        GoogleMobileAds.Api.BannerView _bannerView;

        public override bool IsAvailable
        {
            get
            {
                if(_bannerView != null)
                {
                    return true; ;
                }
                return false;
            }
        }

        public AdmobBannerAd(string adUnitId, GoogleMobileAds.Api.AdSize size, GoogleMobileAds.Api.AdPosition position)
        {
            this.adUnitId = adUnitId;
            this.size = size;
            this.position = position;

            if (_bannerView != null)
            {
                _bannerView.Destroy();
            }

            _bannerView = new GoogleMobileAds.Api.BannerView(adUnitId, size, position);
            CallAddEvent();

        }

        protected override void CallAddEvent()
        {
            if(_bannerView != null)
            {
                _bannerView.OnAdPaid += OnAdPaid;
                _bannerView.OnAdFullScreenContentClosed += OnAdFullScreenContentClosed;
                _bannerView.OnAdFullScreenContentOpened += OnAdFullScreenContentOpened;
                _bannerView.OnBannerAdLoaded += OnAdLoadedEvent;
                _bannerView.OnBannerAdLoadFailed += OnAdLoadFailedEvent;
            }
        }

        protected override void CallRequest()
        {
        }

        protected override void CallShow()
        {
            if (_bannerView != null)
            {
                var adRequest = new GoogleMobileAds.Api.AdRequest();
                _bannerView.LoadAd(adRequest);
            }
        }

        public void DestroyBanner()
        {
            if (_bannerView != null)
            {
                _bannerView.OnAdPaid -= OnAdPaid;
                _bannerView.OnAdFullScreenContentClosed -= OnAdFullScreenContentClosed;
                _bannerView.OnAdFullScreenContentOpened -= OnAdFullScreenContentOpened;
                _bannerView.OnBannerAdLoaded -= OnAdLoadedEvent;
                _bannerView.OnBannerAdLoadFailed -= OnAdLoadFailedEvent;
                Debug.Log("Destroying banner ad.");
                _bannerView.Destroy();
                _bannerView = null;
            }
        }

#region Listeners

        private void OnAdFullScreenContentOpened()
        {
            Debug.Log($"[AdMediation-AdmobBannerAd]: {adUnitId} got OnAdFullScreenContentOpened");
            OnCompleted(true, string.Empty, new AdInfo());
            AdMediation.onBannerCompletedEvent?.Invoke(adUnitId, new AdInfo());
        }

        private void OnAdLoadedEvent()
        {
            Debug.Log($"[AdMediation-AdmobBannerAd]: {adUnitId} got OnAdLoadedEvent");
            OnAdLoadSuccess(new AdInfo());
            if (_bannerView != null)
            {
                _bannerView.Show();
            }
        }

        private void OnAdLoadFailedEvent(GoogleMobileAds.Api.LoadAdError error)
        {
            Debug.Log($"[AdMediation-AdmobBannerAd]: {adUnitId} got OnAdLoadFailedEvent");
            OnAdLoadFailed(error.GetCause().ToString());
            AdMediation.onBannerFailedEvent?.Invoke(error.GetCause().ToString());
        }

        private void OnAdFullScreenContentClosed()
        {
            Debug.Log($"[AdMediation-AdmobBannerAd]: {adUnitId} got OnAdFullScreenContentClosed");
        }

        private void OnAdPaid(GoogleMobileAds.Api.AdValue obj)
        {
            Debug.Log($"[AdMediation-AdmobBannerAd]: {adUnitId} got OnAdPaid With AdInfo " + obj.ToString());
            OnAdOpening(obj.ConvertToImpression());
            AdMediation.onAdRevenuePaidEvent.Invoke(obj.ConvertToImpression());
        }


#endregion

    }
#endif
}
