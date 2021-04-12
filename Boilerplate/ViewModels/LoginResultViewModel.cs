using Entities.Authentication;

namespace Presentation.ViewModels
{
    public class LoginResultViewModel
    {
        public bool IsLogginApproved { get; set; }
        public string Reason { get; set; }
        public string Token { get; set; }
        
        public static LoginResultViewModel Parse(LoginResult loginResult)
        {
            var viewModel = new LoginResultViewModel
            {
                IsLogginApproved = loginResult.IsApproved
            };

            if (loginResult.IncorrectUsernameOrPassword)
                viewModel.Reason = "Usuário ou senha incorretos";

            else if (loginResult.IsUserBlocked)
                viewModel.Reason = "Limite máximo de tentativas excedidas, utilize o mecanismo de recuperação de senha";

            else if (loginResult.IsUserInactive)
                viewModel.Reason = "Usuário desabilitado! Verifique mais informações com o nosso time de suporte.";

            return viewModel;
        }
    }
}
