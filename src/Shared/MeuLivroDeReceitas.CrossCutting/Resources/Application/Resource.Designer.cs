﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MeuLivroDeReceitas.CrossCutting.Resources.Application {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MeuLivroDeReceitas.CrossCutting.Resources.Application.Resource", typeof(Resource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Iniciando. Método: {0}. Título: {1}.
        /// </summary>
        public static string AddRecipe_Info_Starting {
            get {
                return ResourceManager.GetString("AddRecipe_Info_Starting", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Receita nao encontrada. Metodo {0}. Titulo {1}.
        /// </summary>
        public static string GetRecipeById_Info_RecipeNotFound {
            get {
                return ResourceManager.GetString("GetRecipeById_Info_RecipeNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Receitas nao encontradas. Metodo {0}..
        /// </summary>
        public static string GetRecipies_Info_RecipeNotFound {
            get {
                return ResourceManager.GetString("GetRecipies_Info_RecipeNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Receita nao encontrada. Metodo {0}. Titulo {1}.
        /// </summary>
        public static string GetRecipiesTitle_Info_RecipeNotFound {
            get {
                return ResourceManager.GetString("GetRecipiesTitle_Info_RecipeNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Caracteres do atributo título não pode ser {0}, deve ficar entre {1} e {2}. Método:{3}..
        /// </summary>
        public static string RecipeValidator_Error_CharactersTitle {
            get {
                return ResourceManager.GetString("RecipeValidator_Error_CharactersTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DataDraft não preenchido. Método:{0}..
        /// </summary>
        public static string RecipeValidator_Error_DataDraftIsNull {
            get {
                return ResourceManager.GetString("RecipeValidator_Error_DataDraftIsNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to FileExtension não preenchido. Método:{0}..
        /// </summary>
        public static string RecipeValidator_Error_FileExtensionIsNull {
            get {
                return ResourceManager.GetString("RecipeValidator_Error_FileExtensionIsNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Número do telefone celular fora do padrão. Método:{0}. Número:{1}.
        /// </summary>
        public static string RecipeValidator_Error_NonStandardCellNumber {
            get {
                return ResourceManager.GetString("RecipeValidator_Error_NonStandardCellNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Categoria não preenchida. Método:{0}..
        /// </summary>
        public static string RecipeValidator_Error_UnfilledCategory {
            get {
                return ResourceManager.GetString("RecipeValidator_Error_UnfilledCategory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Modo de preparo não preenchido. Método:{0}..
        /// </summary>
        public static string RecipeValidator_Error_UnfilledPreparationMode {
            get {
                return ResourceManager.GetString("RecipeValidator_Error_UnfilledPreparationMode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tempo de preparo não preenchido. Método:{0}..
        /// </summary>
        public static string RecipeValidator_Error_UnfilledPreparationTime {
            get {
                return ResourceManager.GetString("RecipeValidator_Error_UnfilledPreparationTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Titulo não preenchido. Método:{0}..
        /// </summary>
        public static string RecipeValidator_Error_UnfilledTitle {
            get {
                return ResourceManager.GetString("RecipeValidator_Error_UnfilledTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DataDraft não preenchido. Método:{0}..
        /// </summary>
        public static string UpdateRecipeDraftImage_Error_DataDraftIsNull {
            get {
                return ResourceManager.GetString("UpdateRecipeDraftImage_Error_DataDraftIsNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to FileExtension não preenchido. Método:{0}..
        /// </summary>
        public static string UpdateRecipeDraftImage_Error_FileExtensionIsNull {
            get {
                return ResourceManager.GetString("UpdateRecipeDraftImage_Error_FileExtensionIsNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Receita já cadastrada.
        /// </summary>
        public static string ValidarRecipeDTO_Info_RecipeAlreadyExists {
            get {
                return ResourceManager.GetString("ValidarRecipeDTO_Info_RecipeAlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Receita não encontrada..
        /// </summary>
        public static string ValidateRecipeModification_Info_RecipeNotExists {
            get {
                return ResourceManager.GetString("ValidateRecipeModification_Info_RecipeNotExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Receita nao encontrada. Metodo {0}. Titulo {1}.
        /// </summary>
        public static string ValidateRecipeModification_Info_RecipeNotFound {
            get {
                return ResourceManager.GetString("ValidateRecipeModification_Info_RecipeNotFound", resourceCulture);
            }
        }
    }
}
