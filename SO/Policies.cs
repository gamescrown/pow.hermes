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
                Save(Write);
            }
        }

        public int HasUserConsent
        {
            get => hasUserConsent;
            set
            {
                hasUserConsent = value;
                Save(Write);
                Debug.Log("HasUserConsent " + hasUserConsent);
            }
        }

        private void OnEnable()
        {
            string encryptedName = TextEncryption.Encrypt(name, Password);
            FilePath = Path.Combine(Application.persistentDataPath, encryptedName);
            TempFilePath = Path.Combine(Application.persistentDataPath, $"temp{encryptedName}");
            Load(reader =>
            {
                isAccepted = reader.ReadBoolean();
                hasUserConsent = reader.ReadInt32();
            });
        }

        private void Write(BinaryWriter writer)
        {
            writer.Write(isAccepted);
            writer.Write(hasUserConsent);
        }
    }
}