﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MeuLivroDeReceitas.CrossCutting.Resources.API {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MeuLivroDeReceitas.CrossCutting.Resources.API.Resource", typeof(Resource).Assembly);
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
        ///   Looks up a localized string similar to Inclusão da receita com sucesso..
        /// </summary>
        public static string IncludeRecipe_Return_Successfully {
            get {
                return ResourceManager.GetString("IncludeRecipe_Return_Successfully", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mensagem completa. Método:{0}, Erro: {1}.
        /// </summary>
        public static string OnException_Error_UnknownCompleteMessage {
            get {
                return ResourceManager.GetString("OnException_Error_UnknownCompleteMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mensagem curta. Método: {0}, Erro: {1}.
        /// </summary>
        public static string OnException_Error_UnknownShortMessage {
            get {
                return ResourceManager.GetString("OnException_Error_UnknownShortMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Erro não tratado. Método:{0}, Descrição:{1}.
        /// </summary>
        public static string ThrowUnknownError_Error_Throw {
            get {
                return ResourceManager.GetString("ThrowUnknownError_Error_Throw", resourceCulture);
            }
        }
    }
}
