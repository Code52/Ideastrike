using System.Data.Entity;
using System.Data.Entity.Design;
using System.Data.Entity.Infrastructure;
using System.Data.Metadata.Edm;
using System.Data.Objects;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Ideastrike.Nancy.App_Start
{
    /// <summary>
    /// Adapted Initializer from http://nuget.org/packages/EFCodeFirst.CreateTablesOnly
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AppHarborDatabaseInitializer<T> : IDatabaseInitializer<T> where T : DbContext
    {
        private EdmMetadata _edmMetaData;

        public void InitializeDatabase(T context)
        {
            var objectContext = ((IObjectContextAdapter)context).ObjectContext;
            var modelHash = GetModelHash(objectContext);

            if (CompatibleWithModel(modelHash, context, objectContext)) return;

            DeleteExistingTables(objectContext);
            CreateTables(objectContext);

            SaveModelHashToDatabase(context, modelHash, objectContext);
            SeedData(context);
        }

        public virtual void SeedData(T context)
        {
            
        }

        private void SaveModelHashToDatabase(T context, string modelHash, ObjectContext objectContext)
        {
            if (_edmMetaData != null) objectContext.Detach(_edmMetaData);

            _edmMetaData = new EdmMetadata();
            context.Set<EdmMetadata>().Add(_edmMetaData);

            _edmMetaData.ModelHash = modelHash;
            context.SaveChanges();
        }

        private void CreateTables(ObjectContext objectContext)
        {
            string dataBaseCreateScript = objectContext.CreateDatabaseScript();
            objectContext.ExecuteStoreCommand(dataBaseCreateScript);
        }

        private void DeleteExistingTables(ObjectContext objectContext)
        {
            objectContext.ExecuteStoreCommand(Dropallconstraintsscript);
            objectContext.ExecuteStoreCommand(Deletealltablesscript);
        }

        private string GetModelHash(ObjectContext context)
        {
            var csdlXmlString = GetCsdlXmlString(context).ToString();
            return ComputeSha256Hash(csdlXmlString);
        }

        private bool CompatibleWithModel(string modelHash, DbContext context, ObjectContext objectContext)
        {
            var isEdmMetaDataInStore = objectContext.ExecuteStoreQuery<int>(LookupEdmMetaDataTable).FirstOrDefault();
            if (isEdmMetaDataInStore == 1)
            {
                _edmMetaData = context.Set<EdmMetadata>().FirstOrDefault();
                if (_edmMetaData != null)
                {
                    return modelHash == _edmMetaData.ModelHash;
                }
            }
            return false;
        }

        private string GetCsdlXmlString(ObjectContext context)
        {
            if (context != null)
            {
                var entityContainerList = context.MetadataWorkspace.GetItems<EntityContainer>(DataSpace.SSpace);
                if (entityContainerList != null)
                {
                    EntityContainer entityContainer = entityContainerList.FirstOrDefault();
                    var generator = new EntityModelSchemaGenerator(entityContainer);
                    var stringBuilder = new StringBuilder();
                    var xmlWRiter = XmlWriter.Create(stringBuilder);
                    generator.GenerateMetadata();
                    generator.WriteModelSchema(xmlWRiter);
                    xmlWRiter.Flush();
                    return stringBuilder.ToString();
                }
            }
            return string.Empty;
        }

        private static string ComputeSha256Hash(string input)
        {
            byte[] buffer = new SHA256Managed().ComputeHash(Encoding.ASCII.GetBytes(input));
            var builder = new StringBuilder(buffer.Length * 2);
            foreach (byte num in buffer)
            {
                builder.Append(num.ToString("X2", CultureInfo.InvariantCulture));
            }
            return builder.ToString();
        }

        private const string Dropallconstraintsscript =
            @"select  
                'ALTER TABLE ' + so.table_name + ' DROP CONSTRAINT ' + so.constraint_name  
                from INFORMATION_SCHEMA.TABLE_CONSTRAINTS so";

        private const string Deletealltablesscript =
            @"declare @cmd varchar(4000)
                declare cmds cursor for 
                Select
                    'drop table [' + Table_Name + ']'
                From
                    INFORMATION_SCHEMA.TABLES

                open cmds
                while 1=1
                begin
                    fetch cmds into @cmd
                    if @@fetch_status != 0 break
                    print @cmd
                    exec(@cmd)
                end
                close cmds
                deallocate cmds";

        private const string LookupEdmMetaDataTable =
            @"Select COUNT(*) 
              FROM INFORMATION_SCHEMA.TABLES T 
              Where T.TABLE_NAME = 'EdmMetaData'";
    }
}

