using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;

namespace CISM_PJ.Models
{
    partial class CISM_Entities : DbContext
    {
        public CISM_Entities(string nameOrConnectionString)
          : base(GetEntityConnectionString("CISMDataEntities", nameOrConnectionString))
        {
        }

        private static string GetEntityConnectionString(string model, string providerConnectionString)
        {
            var efConnection = new EntityConnectionStringBuilder();
            // or the config file based connection without provider connection string
            // var efConnection = new EntityConnectionStringBuilder(@"metadata=res://*/model1.csdl|res://*/model1.ssdl|res://*/model1.msl;provider=System.Data.SqlClient;");
            efConnection.Provider = "System.Data.SqlClient";
            efConnection.ProviderConnectionString = providerConnectionString;
            // based on whether you choose to supply the app.config connection string to the constructor
            //efConnection.Metadata = string.Format("res://*/{0}.csdl|res://*/{0}.ssdl|res://*/{0}.msl", model);
            efConnection.Metadata = string.Format("res://*/", model);
            //connectionString = "metadata=res://*/;
            // Make sure the "res://*/..." matches what's already in your config file.
            return efConnection.ToString();
        }
    }
}