//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::Xamarin.Forms.Xaml.XamlResourceIdAttribute("Tattel.MobileApp.Views.Master.Popup.notificationPopupPage.xaml", "Views/Master/Popup/notificationPopupPage.xaml", typeof(global::Tattel.MobileApp.Views.Master.Popup.notificationPopupPage))]

namespace Tattel.MobileApp.Views.Master.Popup {
    
    
    [global::Xamarin.Forms.Xaml.XamlFilePathAttribute("Views\\Master\\Popup\\notificationPopupPage.xaml")]
    public partial class notificationPopupPage : global::Rg.Plugins.Popup.Pages.PopupPage {
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.Label lblTitle;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.Label lblName;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Tattel.MobileApp.Extension.ExtButton Approve;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private void InitializeComponent() {
            global::Xamarin.Forms.Xaml.Extensions.LoadFromXaml(this, typeof(notificationPopupPage));
            lblTitle = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.Label>(this, "lblTitle");
            lblName = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.Label>(this, "lblName");
            Approve = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Tattel.MobileApp.Extension.ExtButton>(this, "Approve");
        }
    }
}