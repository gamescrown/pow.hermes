namespace pow.hermes
{
    public static class CpmEventController
    {
        private static readonly int ecpm10 = 1000;
        private static readonly int ecpm100 = 10000;
        private static readonly int ecpm500 = 50000;
        private static readonly int ecpm1000 = 100000;
        private static readonly int ecpm2500 = 250000;

        public static void SendEcpmEvent(double ecpm)
        {
            if (ecpm < ecpm10)
            {
                BaseEventController.Instance.SendCpmEvent(CpmKey.newCPMlessthan10, ecpm);
            }

            if (ecpm >= ecpm10 && ecpm < ecpm2500)
            {
                BaseEventController.Instance.SendCpmEvent(CpmKey.newCPMgreaterthan10, ecpm);
            }

            if (ecpm >= ecpm100 && ecpm < ecpm2500)
            {
                BaseEventController.Instance.SendCpmEvent(CpmKey.newCPMgreaterthan100, ecpm);
            }

            if (ecpm >= ecpm500 && ecpm < ecpm2500)
            {
                BaseEventController.Instance.SendCpmEvent(CpmKey.newCPMgreaterthan500, ecpm);
            }

            if (ecpm >= ecpm1000 && ecpm < ecpm2500)
            {
                BaseEventController.Instance.SendCpmEvent(CpmKey.newCPMgreaterthan1000, ecpm);
            }
        }

        public static void SendEcpmEventExcludeBanner(double ecpm)
        {
            if (ecpm < ecpm2500)
            {
                BaseEventController.Instance.SendCpmEvent(CpmKey.AdRevenue_Total, ecpm);
            }
        }

        public static void SendEcpmEventOnlyRewarded(double ecpm)
        {
            if (ecpm > ecpm10 && ecpm < ecpm2500)
            {
                BaseEventController.Instance.SendCpmEvent(CpmKey.CPM10_rewardedad, ecpm);
            }
        }
    }
}