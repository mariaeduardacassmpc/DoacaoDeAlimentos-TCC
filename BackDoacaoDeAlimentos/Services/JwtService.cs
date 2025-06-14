using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }
        public string GerarToken(Usuario usuario, string cidade)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim("TpEntidade", usuario.TpEntidade),
                new Claim("cidade", cidade) 
            };

            var jwtKey = _config["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
                throw new InvalidOperationException("A chave JWT não está configurada. Verifique 'Jwt:Key' no arquivo de configuração.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds 
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GerarTokenRecuperacao(string email)
        {
            try
            {
                var dataExpiracao = DateTime.UtcNow.AddMinutes(30);
                var payload = $"{email}|{dataExpiracao:o}";

                var bytes = Encoding.UTF8.GetBytes(payload);
                var token = Convert.ToBase64String(bytes);

                return Uri.EscapeDataString(token);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar token de recuperação.");
            }
        }
    }
}
