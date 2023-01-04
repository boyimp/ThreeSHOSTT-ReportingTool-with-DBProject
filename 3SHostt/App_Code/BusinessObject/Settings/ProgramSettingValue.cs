using System.ComponentModel.DataAnnotations;
using ThreeS.Infrastructure.Data;

namespace ThreeS.Domain.Models.Settings
{
    public class ProgramSettingValue : EntityClass
    {
        [StringLength(250)]
        public string Value { get; set; }
    }
}
