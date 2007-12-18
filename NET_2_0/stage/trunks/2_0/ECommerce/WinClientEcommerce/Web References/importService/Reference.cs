using MasterCSharp.WebServices;
//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.1.4322.2032
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

// 
// Il codice sorgente è stato generato automaticamente da Microsoft.VSDesigner, versione 1.1.4322.2032.
// 
namespace WinClientEcommerce.importService {
    using System.Diagnostics;
    using System.Xml.Serialization;
    using System;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Web.Services;
    
    
    /// <remarks/>
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ImportServiceSoap", Namespace="http://rainbowportal.net/ecommerce/service/")]
    public class ImportService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        public AuthHeader AuthHeaderValue;
        
        /// <remarks/>
        public ImportService() {
            string urlSetting = System.Configuration.ConfigurationSettings.AppSettings["WinClientEcommerce.importService.ImportService"];
            if ((urlSetting != null)) {
                this.Url = string.Concat(urlSetting, "");
            }
            else {
                this.Url = "http://localhost/rainbow/ecommerce/service/importservice.asmx";
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("AuthHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://rainbowportal.net/ecommerce/service/UpdateProducts", RequestNamespace="http://rainbowportal.net/ecommerce/service/", ResponseNamespace="http://rainbowportal.net/ecommerce/service/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [CompressionExtension]
        public void UpdateProducts(System.Data.DataSet products) {
            this.Invoke("UpdateProducts", new object[] {
                        products});
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginUpdateProducts(System.Data.DataSet products, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("UpdateProducts", new object[] {
                        products}, callback, asyncState);
        }
        
        /// <remarks/>
        public void EndUpdateProducts(System.IAsyncResult asyncResult) {
            this.EndInvoke(asyncResult);
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://rainbowportal.net/ecommerce/service/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://rainbowportal.net/ecommerce/service/", IsNullable=false)]
    public class AuthHeader : System.Web.Services.Protocols.SoapHeader {
        
        /// <remarks/>
        public string UserName;
        
        /// <remarks/>
        public string Password;
    }
}
