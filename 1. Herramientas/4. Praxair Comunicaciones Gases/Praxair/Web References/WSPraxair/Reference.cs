﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.8922
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.CompactFramework.Design.Data, Version 2.0.50727.8922.
// 
namespace PraxairComunicaciones.WSPraxair {
    using System.Diagnostics;
    using System.Web.Services;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="DataAccessSoap", Namespace="http://tempuri.org/")]
    public partial class DataAccess : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        /// <remarks/>
        public DataAccess() {
            this.Url = "http://localhost/WebServicesOBC/DataAccess.asmx";
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetFechaSistema", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetFechaSistema() {
            object[] results = this.Invoke("GetFechaSistema", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetFechaSistema(System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetFechaSistema", new object[0], callback, asyncState);
        }
        
        /// <remarks/>
        public string EndGetFechaSistema(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetDatosCarga", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetDatosCarga(string sTerminal, int iCodigoProceso, int iCodigoPrograma) {
            object[] results = this.Invoke("GetDatosCarga", new object[] {
                        sTerminal,
                        iCodigoProceso,
                        iCodigoPrograma});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetDatosCarga(string sTerminal, int iCodigoProceso, int iCodigoPrograma, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetDatosCarga", new object[] {
                        sTerminal,
                        iCodigoProceso,
                        iCodigoPrograma}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EndGetDatosCarga(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ConfirmarCarga", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void ConfirmarCarga(string sTerminal, int iCodigoProceso, int iCodigoPrograma) {
            this.Invoke("ConfirmarCarga", new object[] {
                        sTerminal,
                        iCodigoProceso,
                        iCodigoPrograma});
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginConfirmarCarga(string sTerminal, int iCodigoProceso, int iCodigoPrograma, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("ConfirmarCarga", new object[] {
                        sTerminal,
                        iCodigoProceso,
                        iCodigoPrograma}, callback, asyncState);
        }
        
        /// <remarks/>
        public void EndConfirmarCarga(System.IAsyncResult asyncResult) {
            this.EndInvoke(asyncResult);
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/SendDatos", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void SendDatos(string sTerminal, int iCodigoPrograma, int iCodigoProceso, string sDatosEnviados) {
            this.Invoke("SendDatos", new object[] {
                        sTerminal,
                        iCodigoPrograma,
                        iCodigoProceso,
                        sDatosEnviados});
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginSendDatos(string sTerminal, int iCodigoPrograma, int iCodigoProceso, string sDatosEnviados, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("SendDatos", new object[] {
                        sTerminal,
                        iCodigoPrograma,
                        iCodigoProceso,
                        sDatosEnviados}, callback, asyncState);
        }
        
        /// <remarks/>
        public void EndSendDatos(System.IAsyncResult asyncResult) {
            this.EndInvoke(asyncResult);
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/UploadArchivoPDF", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool UploadArchivoPDF(string nombreArchivo, [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] byte[] bytes) {
            object[] results = this.Invoke("UploadArchivoPDF", new object[] {
                        nombreArchivo,
                        bytes});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginUploadArchivoPDF(string nombreArchivo, byte[] bytes, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("UploadArchivoPDF", new object[] {
                        nombreArchivo,
                        bytes}, callback, asyncState);
        }
        
        /// <remarks/>
        public bool EndUploadArchivoPDF(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((bool)(results[0]));
        }
    }
}
