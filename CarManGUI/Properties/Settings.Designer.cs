﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CarManGUI.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2, 2, 2, 2")]
        public global::System.Windows.Forms.Padding StandartMargin {
            get {
                return ((global::System.Windows.Forms.Padding)(this["StandartMargin"]));
            }
            set {
                this["StandartMargin"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("ActiveBorder")]
        public global::System.Drawing.Color StandartColor {
            get {
                return ((global::System.Drawing.Color)(this["StandartColor"]));
            }
            set {
                this["StandartColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2, 2, 2, 2")]
        public global::System.Windows.Forms.Padding DefaultMargin {
            get {
                return ((global::System.Windows.Forms.Padding)(this["DefaultMargin"]));
            }
            set {
                this["DefaultMargin"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("DarkSlateGray")]
        public global::System.Drawing.Color DefaultColor {
            get {
                return ((global::System.Drawing.Color)(this["DefaultColor"]));
            }
            set {
                this["DefaultColor"] = value;
            }
        }
    }
}