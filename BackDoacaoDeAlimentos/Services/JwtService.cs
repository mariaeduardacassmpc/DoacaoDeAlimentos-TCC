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
        public string GerarToken(Usuario usuario)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim("TpEntidade", usuario.TpEntidade) 
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
    }
}
