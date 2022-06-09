namespace pow.hermes
{
    public class PowAdjustEvent
    {
        internal string _key;
        internal string _token;

        public PowAdjustEvent(string key, string token)
        {
            _key = key;
            _token = token;
        }
        
    }
}