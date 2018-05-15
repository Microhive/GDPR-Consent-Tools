namespace GDPR.Core
{
    public class Policy
    {
        public string Purpose { get; set; }

        public string PII { get; set; }

        public Policy(string purpose, string pii)
        {
            Purpose = purpose;
            PII = pii;
        }
    }
}
