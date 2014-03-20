using CloudManager_for_ProfitBricks.Common;
using CloudManager_for_ProfitBricks.ProfitBricksApiService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudManager_for_ProfitBricks.Data
{
    class DatacenterDataSource
    {
        private static DatacenterDataSource _datacenterDataSource = new DatacenterDataSource();

        private ObservableCollection<dataCenterIdentifier> _dataCenterIdentifiers = new ObservableCollection<dataCenterIdentifier>();
        private ObservableCollection<dataCenter> _dataCenters = new ObservableCollection<dataCenter>();
        private ObservableCollection<snapshot> _snapshots = new ObservableCollection<snapshot>();
        private ObservableCollection<image> _images = new ObservableCollection<image>();
        private ObservableCollection<ipBlock> _ipBlocks = new ObservableCollection<ipBlock>();
        
        private CredentialItem credential;

        private ProfitBricksApi ProfitBricksApi;


        public ObservableCollection<dataCenterIdentifier> DataCenterIdentifiers
        {
            get { return this._dataCenterIdentifiers; }
        }
        public ObservableCollection<dataCenter> DataCenters
        {
            get { return this._dataCenters; }
        }
        public ObservableCollection<snapshot> Snapshots
        {
            get { return this._snapshots; }
        }
        public ObservableCollection<image> Images
        {
            get { return _images; }
        }
        public ObservableCollection<ipBlock> IpBlocks
        {
            get { return this._ipBlocks; }
        }

        //public DatacenterDataSource()
        //{
        //    credential = ((App)App.Current).CurrentCredential;
        //    ProfitBricksApi = new ProfitBricksApi(credential.User, credential.Password);
        //}

        public static async Task<IEnumerable<dataCenterIdentifier>> GetDataCenterIdentifiersAscnc()
        {
            await _datacenterDataSource.getDataCenterIdentifiersAscnc();
            return _datacenterDataSource.DataCenterIdentifiers;
        }
        private async Task getDataCenterIdentifiersAscnc()
        {
            credential = ((App)App.Current).CurrentCredential;
            ProfitBricksApi = new ProfitBricksApi(credential.User, credential.Password);
            var response = await this.ProfitBricksApi.Proxy.getAllDataCentersAsync();
            foreach (var item in response.@return)
            {
                this.DataCenterIdentifiers.Add(item);
            }
        }

        public static async Task<IEnumerable<dataCenter>> GetDataCentersAscnc()
        {
            await _datacenterDataSource.getDataCenterIdentifiersAscnc();
            return _datacenterDataSource.DataCenters;
        }
        private async Task getDataCentersAscnc()
        {
            credential = ((App)App.Current).CurrentCredential;
            ProfitBricksApi = new ProfitBricksApi(credential.User, credential.Password);
            foreach (var item in this.DataCenterIdentifiers)
            {
                var response = await this.ProfitBricksApi.Proxy.getDataCenterAsync(item.dataCenterId);
                this.DataCenters.Add(response.@return);
            }
        }

        public static async Task<IEnumerable<snapshot>> GetSnapshotsAscnc()
        {
            await _datacenterDataSource.getSnapshotsAscnc();
            return _datacenterDataSource.Snapshots;
        }
        private async Task getSnapshotsAscnc()
        {
            credential = ((App)App.Current).CurrentCredential;
            ProfitBricksApi = new ProfitBricksApi(credential.User, credential.Password);
            var response = await this.ProfitBricksApi.Proxy.getAllSnapshotsAsync();
            foreach (var item in response.@return)
            {
                this.Snapshots.Add(item);
            }
        }

        public static async Task<IEnumerable<ipBlock>> GetIpBlocksAscnc()
        {
            await _datacenterDataSource.getIpBlocksAscnc();
            return _datacenterDataSource.IpBlocks;
        }
        private async Task getIpBlocksAscnc()
        {
            credential = ((App)App.Current).CurrentCredential;
            ProfitBricksApi = new ProfitBricksApi(credential.User, credential.Password);
            var response = await this.ProfitBricksApi.Proxy.getAllPublicIpBlocksAsync();
            foreach (var item in response.@return)
            {
                this.IpBlocks.Add(item);
            }
        }

        public static async Task<IEnumerable<image>> GetImagesAscnc()
        {
            await _datacenterDataSource.getImagesAscnc();
            return _datacenterDataSource.Images;
        }
        private async Task getImagesAscnc()
        {
            credential = ((App)App.Current).CurrentCredential;
            ProfitBricksApi = new ProfitBricksApi(credential.User, credential.Password);
            var response = await this.ProfitBricksApi.Proxy.getAllImagesAsync();
            foreach (var item in response.@return)
            {
                this.Images.Add(item);
            }
        }

    }
}
