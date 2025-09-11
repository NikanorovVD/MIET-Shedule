namespace MietClient
{
    public class MietCookies
    {
        public MietCookies(string cookiesHeader, string cookiesString)
        {
            CookiesHeader = cookiesHeader;
            CookiesString = cookiesString;
        }

        public string CookiesHeader {  get; set; }
        public string CookiesString {  get; set; }
    }
}
