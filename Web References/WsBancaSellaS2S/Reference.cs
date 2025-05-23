﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace RentalInRome.WsBancaSellaS2S {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1087.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="WSs2sSoap", Namespace="https://ecomms2s.sella.it/")]
    public partial class WSs2s : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback callRefundS2SOperationCompleted;
        
        private System.Threading.SendOrPostCallback callPagamS2SOperationCompleted;
        
        private System.Threading.SendOrPostCallback callDeleteS2SOperationCompleted;
        
        private System.Threading.SendOrPostCallback callSettleS2SOperationCompleted;
        
        private System.Threading.SendOrPostCallback callVerifycardS2SOperationCompleted;
        
        private System.Threading.SendOrPostCallback callReadTrxS2SOperationCompleted;
        
        private System.Threading.SendOrPostCallback callCheckCartaS2SOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public WSs2s() {
            this.Url = global::RentalInRome.Properties.Settings.Default.RentalInRome_WsBancaSellaS2S_WSs2s;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event callRefundS2SCompletedEventHandler callRefundS2SCompleted;
        
        /// <remarks/>
        public event callPagamS2SCompletedEventHandler callPagamS2SCompleted;
        
        /// <remarks/>
        public event callDeleteS2SCompletedEventHandler callDeleteS2SCompleted;
        
        /// <remarks/>
        public event callSettleS2SCompletedEventHandler callSettleS2SCompleted;
        
        /// <remarks/>
        public event callVerifycardS2SCompletedEventHandler callVerifycardS2SCompleted;
        
        /// <remarks/>
        public event callReadTrxS2SCompletedEventHandler callReadTrxS2SCompleted;
        
        /// <remarks/>
        public event callCheckCartaS2SCompletedEventHandler callCheckCartaS2SCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://ecomms2s.sella.it/callRefundS2S", RequestNamespace="https://ecomms2s.sella.it/", ResponseNamespace="https://ecomms2s.sella.it/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Xml.XmlNode callRefundS2S(string shopLogin, string uicCode, string amount, string shopTransactionId, string bankTransactionId) {
            object[] results = this.Invoke("callRefundS2S", new object[] {
                        shopLogin,
                        uicCode,
                        amount,
                        shopTransactionId,
                        bankTransactionId});
            return ((System.Xml.XmlNode)(results[0]));
        }
        
        /// <remarks/>
        public void callRefundS2SAsync(string shopLogin, string uicCode, string amount, string shopTransactionId, string bankTransactionId) {
            this.callRefundS2SAsync(shopLogin, uicCode, amount, shopTransactionId, bankTransactionId, null);
        }
        
        /// <remarks/>
        public void callRefundS2SAsync(string shopLogin, string uicCode, string amount, string shopTransactionId, string bankTransactionId, object userState) {
            if ((this.callRefundS2SOperationCompleted == null)) {
                this.callRefundS2SOperationCompleted = new System.Threading.SendOrPostCallback(this.OncallRefundS2SOperationCompleted);
            }
            this.InvokeAsync("callRefundS2S", new object[] {
                        shopLogin,
                        uicCode,
                        amount,
                        shopTransactionId,
                        bankTransactionId}, this.callRefundS2SOperationCompleted, userState);
        }
        
        private void OncallRefundS2SOperationCompleted(object arg) {
            if ((this.callRefundS2SCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.callRefundS2SCompleted(this, new callRefundS2SCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://ecomms2s.sella.it/callPagamS2S", RequestNamespace="https://ecomms2s.sella.it/", ResponseNamespace="https://ecomms2s.sella.it/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Xml.XmlNode callPagamS2S(
                    string shopLogin, 
                    string uicCode, 
                    string amount, 
                    string shopTransactionId, 
                    string cardNumber, 
                    string expiryMonth, 
                    string expiryYear, 
                    string buyerName, 
                    string buyerEmail, 
                    string languageId, 
                    string cvv, 
                    string min, 
                    string transKey, 
                    string PARes, 
                    string customInfo, 
                    string IDEA) {
            object[] results = this.Invoke("callPagamS2S", new object[] {
                        shopLogin,
                        uicCode,
                        amount,
                        shopTransactionId,
                        cardNumber,
                        expiryMonth,
                        expiryYear,
                        buyerName,
                        buyerEmail,
                        languageId,
                        cvv,
                        min,
                        transKey,
                        PARes,
                        customInfo,
                        IDEA});
            return ((System.Xml.XmlNode)(results[0]));
        }
        
        /// <remarks/>
        public void callPagamS2SAsync(
                    string shopLogin, 
                    string uicCode, 
                    string amount, 
                    string shopTransactionId, 
                    string cardNumber, 
                    string expiryMonth, 
                    string expiryYear, 
                    string buyerName, 
                    string buyerEmail, 
                    string languageId, 
                    string cvv, 
                    string min, 
                    string transKey, 
                    string PARes, 
                    string customInfo, 
                    string IDEA) {
            this.callPagamS2SAsync(shopLogin, uicCode, amount, shopTransactionId, cardNumber, expiryMonth, expiryYear, buyerName, buyerEmail, languageId, cvv, min, transKey, PARes, customInfo, IDEA, null);
        }
        
        /// <remarks/>
        public void callPagamS2SAsync(
                    string shopLogin, 
                    string uicCode, 
                    string amount, 
                    string shopTransactionId, 
                    string cardNumber, 
                    string expiryMonth, 
                    string expiryYear, 
                    string buyerName, 
                    string buyerEmail, 
                    string languageId, 
                    string cvv, 
                    string min, 
                    string transKey, 
                    string PARes, 
                    string customInfo, 
                    string IDEA, 
                    object userState) {
            if ((this.callPagamS2SOperationCompleted == null)) {
                this.callPagamS2SOperationCompleted = new System.Threading.SendOrPostCallback(this.OncallPagamS2SOperationCompleted);
            }
            this.InvokeAsync("callPagamS2S", new object[] {
                        shopLogin,
                        uicCode,
                        amount,
                        shopTransactionId,
                        cardNumber,
                        expiryMonth,
                        expiryYear,
                        buyerName,
                        buyerEmail,
                        languageId,
                        cvv,
                        min,
                        transKey,
                        PARes,
                        customInfo,
                        IDEA}, this.callPagamS2SOperationCompleted, userState);
        }
        
        private void OncallPagamS2SOperationCompleted(object arg) {
            if ((this.callPagamS2SCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.callPagamS2SCompleted(this, new callPagamS2SCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://ecomms2s.sella.it/callDeleteS2S", RequestNamespace="https://ecomms2s.sella.it/", ResponseNamespace="https://ecomms2s.sella.it/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Xml.XmlNode callDeleteS2S(string shopLogin, string shopTransactionId, string bankTransactionId) {
            object[] results = this.Invoke("callDeleteS2S", new object[] {
                        shopLogin,
                        shopTransactionId,
                        bankTransactionId});
            return ((System.Xml.XmlNode)(results[0]));
        }
        
        /// <remarks/>
        public void callDeleteS2SAsync(string shopLogin, string shopTransactionId, string bankTransactionId) {
            this.callDeleteS2SAsync(shopLogin, shopTransactionId, bankTransactionId, null);
        }
        
        /// <remarks/>
        public void callDeleteS2SAsync(string shopLogin, string shopTransactionId, string bankTransactionId, object userState) {
            if ((this.callDeleteS2SOperationCompleted == null)) {
                this.callDeleteS2SOperationCompleted = new System.Threading.SendOrPostCallback(this.OncallDeleteS2SOperationCompleted);
            }
            this.InvokeAsync("callDeleteS2S", new object[] {
                        shopLogin,
                        shopTransactionId,
                        bankTransactionId}, this.callDeleteS2SOperationCompleted, userState);
        }
        
        private void OncallDeleteS2SOperationCompleted(object arg) {
            if ((this.callDeleteS2SCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.callDeleteS2SCompleted(this, new callDeleteS2SCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://ecomms2s.sella.it/callSettleS2S", RequestNamespace="https://ecomms2s.sella.it/", ResponseNamespace="https://ecomms2s.sella.it/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Xml.XmlNode callSettleS2S(string shopLogin, string uicCode, string amount, string shopTransID, string bankTransID) {
            object[] results = this.Invoke("callSettleS2S", new object[] {
                        shopLogin,
                        uicCode,
                        amount,
                        shopTransID,
                        bankTransID});
            return ((System.Xml.XmlNode)(results[0]));
        }
        
        /// <remarks/>
        public void callSettleS2SAsync(string shopLogin, string uicCode, string amount, string shopTransID, string bankTransID) {
            this.callSettleS2SAsync(shopLogin, uicCode, amount, shopTransID, bankTransID, null);
        }
        
        /// <remarks/>
        public void callSettleS2SAsync(string shopLogin, string uicCode, string amount, string shopTransID, string bankTransID, object userState) {
            if ((this.callSettleS2SOperationCompleted == null)) {
                this.callSettleS2SOperationCompleted = new System.Threading.SendOrPostCallback(this.OncallSettleS2SOperationCompleted);
            }
            this.InvokeAsync("callSettleS2S", new object[] {
                        shopLogin,
                        uicCode,
                        amount,
                        shopTransID,
                        bankTransID}, this.callSettleS2SOperationCompleted, userState);
        }
        
        private void OncallSettleS2SOperationCompleted(object arg) {
            if ((this.callSettleS2SCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.callSettleS2SCompleted(this, new callSettleS2SCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://ecomms2s.sella.it/callVerifycardS2S", RequestNamespace="https://ecomms2s.sella.it/", ResponseNamespace="https://ecomms2s.sella.it/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Xml.XmlNode callVerifycardS2S(string shopLogin, string shopTransactionId, string cardNumber, string expMonth, string expYear, string CVV2) {
            object[] results = this.Invoke("callVerifycardS2S", new object[] {
                        shopLogin,
                        shopTransactionId,
                        cardNumber,
                        expMonth,
                        expYear,
                        CVV2});
            return ((System.Xml.XmlNode)(results[0]));
        }
        
        /// <remarks/>
        public void callVerifycardS2SAsync(string shopLogin, string shopTransactionId, string cardNumber, string expMonth, string expYear, string CVV2) {
            this.callVerifycardS2SAsync(shopLogin, shopTransactionId, cardNumber, expMonth, expYear, CVV2, null);
        }
        
        /// <remarks/>
        public void callVerifycardS2SAsync(string shopLogin, string shopTransactionId, string cardNumber, string expMonth, string expYear, string CVV2, object userState) {
            if ((this.callVerifycardS2SOperationCompleted == null)) {
                this.callVerifycardS2SOperationCompleted = new System.Threading.SendOrPostCallback(this.OncallVerifycardS2SOperationCompleted);
            }
            this.InvokeAsync("callVerifycardS2S", new object[] {
                        shopLogin,
                        shopTransactionId,
                        cardNumber,
                        expMonth,
                        expYear,
                        CVV2}, this.callVerifycardS2SOperationCompleted, userState);
        }
        
        private void OncallVerifycardS2SOperationCompleted(object arg) {
            if ((this.callVerifycardS2SCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.callVerifycardS2SCompleted(this, new callVerifycardS2SCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://ecomms2s.sella.it/callReadTrxS2S", RequestNamespace="https://ecomms2s.sella.it/", ResponseNamespace="https://ecomms2s.sella.it/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Xml.XmlNode callReadTrxS2S(string shopLogin, string shopTransactionId, string bankTransactionId) {
            object[] results = this.Invoke("callReadTrxS2S", new object[] {
                        shopLogin,
                        shopTransactionId,
                        bankTransactionId});
            return ((System.Xml.XmlNode)(results[0]));
        }
        
        /// <remarks/>
        public void callReadTrxS2SAsync(string shopLogin, string shopTransactionId, string bankTransactionId) {
            this.callReadTrxS2SAsync(shopLogin, shopTransactionId, bankTransactionId, null);
        }
        
        /// <remarks/>
        public void callReadTrxS2SAsync(string shopLogin, string shopTransactionId, string bankTransactionId, object userState) {
            if ((this.callReadTrxS2SOperationCompleted == null)) {
                this.callReadTrxS2SOperationCompleted = new System.Threading.SendOrPostCallback(this.OncallReadTrxS2SOperationCompleted);
            }
            this.InvokeAsync("callReadTrxS2S", new object[] {
                        shopLogin,
                        shopTransactionId,
                        bankTransactionId}, this.callReadTrxS2SOperationCompleted, userState);
        }
        
        private void OncallReadTrxS2SOperationCompleted(object arg) {
            if ((this.callReadTrxS2SCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.callReadTrxS2SCompleted(this, new callReadTrxS2SCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://ecomms2s.sella.it/callCheckCartaS2S", RequestNamespace="https://ecomms2s.sella.it/", ResponseNamespace="https://ecomms2s.sella.it/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Xml.XmlNode callCheckCartaS2S(string shopLogin, string shopTransactionId, string cardNumber, string expMonth, string expYear, string CVV2, string withAuth) {
            object[] results = this.Invoke("callCheckCartaS2S", new object[] {
                        shopLogin,
                        shopTransactionId,
                        cardNumber,
                        expMonth,
                        expYear,
                        CVV2,
                        withAuth});
            return ((System.Xml.XmlNode)(results[0]));
        }
        
        /// <remarks/>
        public void callCheckCartaS2SAsync(string shopLogin, string shopTransactionId, string cardNumber, string expMonth, string expYear, string CVV2, string withAuth) {
            this.callCheckCartaS2SAsync(shopLogin, shopTransactionId, cardNumber, expMonth, expYear, CVV2, withAuth, null);
        }
        
        /// <remarks/>
        public void callCheckCartaS2SAsync(string shopLogin, string shopTransactionId, string cardNumber, string expMonth, string expYear, string CVV2, string withAuth, object userState) {
            if ((this.callCheckCartaS2SOperationCompleted == null)) {
                this.callCheckCartaS2SOperationCompleted = new System.Threading.SendOrPostCallback(this.OncallCheckCartaS2SOperationCompleted);
            }
            this.InvokeAsync("callCheckCartaS2S", new object[] {
                        shopLogin,
                        shopTransactionId,
                        cardNumber,
                        expMonth,
                        expYear,
                        CVV2,
                        withAuth}, this.callCheckCartaS2SOperationCompleted, userState);
        }
        
        private void OncallCheckCartaS2SOperationCompleted(object arg) {
            if ((this.callCheckCartaS2SCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.callCheckCartaS2SCompleted(this, new callCheckCartaS2SCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1087.0")]
    public delegate void callRefundS2SCompletedEventHandler(object sender, callRefundS2SCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1087.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class callRefundS2SCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal callRefundS2SCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Xml.XmlNode Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Xml.XmlNode)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1087.0")]
    public delegate void callPagamS2SCompletedEventHandler(object sender, callPagamS2SCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1087.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class callPagamS2SCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal callPagamS2SCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Xml.XmlNode Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Xml.XmlNode)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1087.0")]
    public delegate void callDeleteS2SCompletedEventHandler(object sender, callDeleteS2SCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1087.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class callDeleteS2SCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal callDeleteS2SCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Xml.XmlNode Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Xml.XmlNode)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1087.0")]
    public delegate void callSettleS2SCompletedEventHandler(object sender, callSettleS2SCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1087.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class callSettleS2SCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal callSettleS2SCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Xml.XmlNode Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Xml.XmlNode)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1087.0")]
    public delegate void callVerifycardS2SCompletedEventHandler(object sender, callVerifycardS2SCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1087.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class callVerifycardS2SCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal callVerifycardS2SCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Xml.XmlNode Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Xml.XmlNode)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1087.0")]
    public delegate void callReadTrxS2SCompletedEventHandler(object sender, callReadTrxS2SCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1087.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class callReadTrxS2SCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal callReadTrxS2SCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Xml.XmlNode Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Xml.XmlNode)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1087.0")]
    public delegate void callCheckCartaS2SCompletedEventHandler(object sender, callCheckCartaS2SCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1087.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class callCheckCartaS2SCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal callCheckCartaS2SCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Xml.XmlNode Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Xml.XmlNode)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591