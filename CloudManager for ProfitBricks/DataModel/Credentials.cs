using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace CloudManager_for_ProfitBricks.Data
{
    public class CredentialItem
    {
        public CredentialItem(String Name, String User, String Password)
        {
            this.Name = Name;
            this.User = User;
            this.Password = Password;
        }

        public string Name { get; private set; }
        public string User { get; private set; }
        public string Password { get; private set; }

        public override string ToString()
        {
            return this.Name;
        }
    }


    public sealed class CredentialDataSource
    {
        private static CredentialDataSource _credentialDataSource = new CredentialDataSource();

        private ObservableCollection<CredentialItem> _credentials = new ObservableCollection<CredentialItem>();
        public ObservableCollection<CredentialItem> Credentials
        {
            get { return this._credentials; }
        }

        public static IEnumerable<CredentialItem> GetCredentials()
        {
            _credentialDataSource.GetCredentialData();
            return _credentialDataSource.Credentials;
        }

        public static CredentialItem GetCredential(string name)
        {
            _credentialDataSource.GetCredentialData();
            // Simple linear search is acceptable for small data sets
            var matches = _credentialDataSource.Credentials.Where((credential) => credential.Name.Equals(name));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        private void GetCredentialData()
        {
            if (this.Credentials.Count != 0)
            {
                return;
            }
            PasswordVault vault = new PasswordVault();

            IReadOnlyCollection<PasswordCredential> _passwordcredentials = vault.RetrieveAll();

            foreach (PasswordCredential _passworcredential in _passwordcredentials )
            {
                CredentialItem credential = new CredentialItem(_passworcredential.Resource,_passworcredential.UserName,_passworcredential.Password);
                this.Credentials.Add(credential);
            }
            this.Credentials.Add(new CredentialItem("DesingItem01", "DesignUser01", "DesignPassword01"));
            this.Credentials.Add(new CredentialItem("DesingItem02", "DesignUser02", "DesignPassword02"));
            this.Credentials.Add(new CredentialItem("DesingItem03", "DesignUser03", "DesignPassword03"));


        }
    }
}