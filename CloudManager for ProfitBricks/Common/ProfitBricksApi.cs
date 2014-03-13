using CloudManager_for_ProfitBricks.Data;
using CloudManager_for_ProfitBricks.ProfitBricksApiService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CloudManager_for_ProfitBricks.Common
{
    public class ProfitBricksApi
    {
        private const string _apiUri = "https://api.profitbricks.com/1.2";
        private CredentialItem credential;

        public ProfitbricksApiServicePortTypeClient Proxy;
        private BasicHttpBinding binding;
        private EndpointAddress endpointAddress;
        private string user;
        private string password;

        //public ProfitBricksApi(string user, string password)
        //public ProfitBricksApi()
        //{
        //    // create a new Basic HTTP Binding
        //    this._binding = new BasicHttpBinding();
        //    this._binding.Security.Mode = BasicHttpSecurityMode.Transport;
        //    this._binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
            
        //    // set SOAP Interface URL
        //    this._endpointAddress = new EndpointAddress(_apiUri);
        //    // create API Client
        //    this.ApiClient = new ProfitBricksApiService.ProfitbricksApiServicePortTypeClient(this._binding, this._endpointAddress);
        //    // Set Credidentials
        //    //this.ApiClient.ClientCredentials.UserName.UserName = (user);
        //    //this.ApiClient.ClientCredentials.UserName.Password = (password);
        //    // ready to consume SOAP
        //}

        public ProfitBricksApi(string user, string password)
        {
            // create a new Basic HTTP Binding
            this.binding = new BasicHttpBinding();
            this.binding.Security.Mode = BasicHttpSecurityMode.Transport;
            this.binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

            // set SOAP Interface URL
            this.endpointAddress = new EndpointAddress(_apiUri);
            // create API Client
            this.Proxy = new ProfitBricksApiService.ProfitbricksApiServicePortTypeClient(this.binding, this.endpointAddress);
            // Set Credidentials
            this.Proxy.ClientCredentials.UserName.UserName = user;
            this.Proxy.ClientCredentials.UserName.Password = password;
            // ready to consume SOAP
        }


        //public CredentialItem Credential
        //{
        //    get
        //    {
        //        return new CredentialItem(this.credential.Name, this.Proxy.ClientCredentials.UserName.UserName, this.Proxy.ClientCredentials.UserName.Password);
        //    }
        //    set
        //    {
        //        this.credential = value;
        //    }

        //}

    }
}
