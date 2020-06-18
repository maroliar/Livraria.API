namespace Livraria.Service
{
    /// <summary>
    /// Classe utilizada somente para armazenar informações que serão encriptadas no TOKEN
    /// </summary>
    public class TokenConfiguration
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int Seconds { get; set; }
    }
}
