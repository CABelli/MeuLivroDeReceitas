using System.ComponentModel;

namespace MeuLivroDeReceitas.CrossCutting.EnumClass
{
    public enum EMethodUserValidator
    {
        [Description("Authenticate")]
        Authenticate = 1,            
        [Description("AddUser")]
        AddUser = 2,
        [Description("UserChange")]
        UserChange = 3,
        [Description("PasswordChangeByForgot")]
        PasswordChangeByForgot = 4
    }
}
