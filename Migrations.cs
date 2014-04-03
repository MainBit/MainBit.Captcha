using MainBit.Captcha.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace MainBit.Captcha
{
    public class Migrations : DataMigrationImpl
    {
        public int Create()
        {
            ContentDefinitionManager.AlterPartDefinition(
                typeof(CaptchaPart).Name, 
                builder => builder.Attachable()
            );

            return 1;
        }
    }
}