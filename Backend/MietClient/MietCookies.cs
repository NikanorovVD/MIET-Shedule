namespace MietClient
{
    public class MietCookies
    {
        public MietCookies(string cookiesString)
        {
            CookiesString = cookiesString;
        }

        public string CookiesString {  get; set; }
    }
}
