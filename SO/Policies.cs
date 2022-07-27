using System.IO;
using pow.aidkit;
using UnityEngine;

namespace pow.hermes
{
    [CreateAssetMenu(fileName = "Policies", menuName = "POW_SDK/Hermes/Policies", order = 0)]
    public class Policies : StoredScriptableObject
    {
        [SerializeField] private bool isAccepted;
        [SerializeField] private string privacyLink;
        [SerializeField] private string termsLink;
        [SerializeField] private int hasUserConsent = -1;

        public string PrivacyLink => privacyLink;

        public string TermsLink => termsLink;

        public bool IsAccepted
        {
            get => isAccepted;
            set
            {
                isAccepted = value;
                Save();
            }
        }

        public int HasUserConsent
        {
            get => hasUserConsent;
            set
            {
                hasUserConsent = value;
                Save();
                Debug.Log("HasUserConsent " + hasUserConsent);
            }
        }

        protected override void Write(BinaryWriter writer)
        {
            writer.Write(isAccepted);
            writer.Write(hasUserConsent);
        }

        protected override void Read(BinaryReader reader)
        {
            isAccepted = reader.ReadBoolean();
            hasUserConsent = reader.ReadInt32();
        }

        public override void Reset()
        {
            base.Reset();
            isAccepted = false;
        }
    }
}