namespace Ideastrike.Nancy.Helpers
{
    public static class GravatarExtensions
    {
        public static string ToGravatarUrl(this string emailAddress)
        {
            var x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            var bs = System.Text.Encoding.UTF8.GetBytes(emailAddress);
            bs = x.ComputeHash(bs);
            var s = new System.Text.StringBuilder();
            foreach (var b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            return string.Format("http://www.gravatar.com/avatar/{0}", s);
        }
    }
}