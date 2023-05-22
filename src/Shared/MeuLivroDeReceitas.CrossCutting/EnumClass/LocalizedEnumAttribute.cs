using System.ComponentModel;
using System.Reflection;
namespace MeuLivroDeReceitas.CrossCutting.EnumClass
{
    public class LocalizedEnumAttribute : DescriptionAttribute
    {
        private PropertyInfo _nameProperty;
        private Type _resourceType;

        public LocalizedEnumAttribute(string displayNameKey)
            : base(displayNameKey) {   }

        public Type ResourceType
        {
            get { return _resourceType; }
            set 
            {
                _resourceType = value;
                _nameProperty = _resourceType.GetProperty(this.Description, BindingFlags.Static | BindingFlags.Public);
            }
        }

        public override string Description
        {
            get
            {
                if (_nameProperty == null) return base.Description;
                return (string)_nameProperty.GetValue(_nameProperty.DeclaringType, null);
            }
        }
    }
}
