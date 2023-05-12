namespace MeuLivroDeReceitas.CrossCutting.Dto.Login
{
    public class PasswordChangeDto
    {
        public string Email { get; set; }

        public string NewPassword { get; set; }

        public string RepeatNewPassword { get; set; }
    }
}
