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

        public static CredentialItem GetCredentialByName(string name)
        {
            _credentialDataSource.GetCredentialData();
            // Simple linear search is acceptable for small data sets
            var matches = _credentialDataSource.Credentials.Where((credential) => credential.Name.Equals(name));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static CredentialItem GetCredentialByUser(string user)
        {
            _credentialDataSource.GetCredentialData();
            // Simple linear search is acceptable for small data sets
            var matches = _credentialDataSource.Credentials.Where((credential) => credential.User.Equals(user));
            if (matches.Count() == 1) return matches.First();
            return null;
        }


        public static void AddCredential(string name, string user, string password)
        {
            PasswordVault vault = new PasswordVault();
            var matches = _credentialDataSource.Credentials.Where(credential => credential.Name.Equals(name));

            foreach (var match in matches.ToArray())
            {
                vault.Remove(vault.Retrieve(match.Name, match.User));
            }
            vault.Add(new PasswordCredential(name,user,password));
        }

        public static void RemoveCredential(string name, string user, string password)
        {
            PasswordVault vault = new PasswordVault();
            vault.Remove(new PasswordCredential(name, user, password));
        }

        private void GetCredentialData()
        {
            this.Credentials.Clear();
            PasswordVault vault = new PasswordVault();
            IReadOnlyCollection<PasswordCredential> _passwordcredentials = vault.RetrieveAll();
            foreach (PasswordCredential _passworcredential in _passwordcredentials )
            {
                PasswordCredential pwcredential = vault.Retrieve(_passworcredential.Resource, _passworcredential.UserName);
                CredentialItem credential = new CredentialItem(pwcredential.Resource, pwcredential.UserName, pwcredential.Password);
                this.Credentials.Add(credential);
            }
        }
    }
}