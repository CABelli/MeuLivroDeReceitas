namespace MeuLivroDeReceitas.CrossCutting.Dto.Login
{
    public class UserValidatorDto
    {
        public string? UserName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? RepeatNewPassword { get; set; }

        public List<string>? RolesName { get; set; }
    }
}
