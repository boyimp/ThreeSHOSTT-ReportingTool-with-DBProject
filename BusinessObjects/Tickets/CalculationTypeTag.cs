using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ThreeS.Domain.Models.Tickets
{
    [DataContract]
    public class CalculationTypeTag
    {
        public CalculationTypeTag()
        {
            CalculationTypeId = 0;
            CalculationName = "";
        }
        public CalculationTypeTag(int Id, string Name)
        {
            CalculationTypeId = Id;
            CalculationName = Name;
        }
        [DataMember(Name = "CI")]
        public int CalculationTypeId { get; set; }
        [DataMember(Name = "CN")]
        public string CalculationName { get; set; }
        public static CalculationTypeTag GetCalculationType(IList<CalculationType> AllCalculationTyps, int CalculationTypeId)
        {
            var calType = AllCalculationTyps.SingleOrDefault(x => x.Id == CalculationTypeId);
            return calType != null ? new Tickets.CalculationTypeTag(calType.Id, calType.Name) : new Tickets.CalculationTypeTag();
        }
        public static IEnumerable<CalculationType> GetCalculationTypes(IEnumerable<CalculationType> AllCalculationTyps, IList<CalculationTypeTag> CalculationTypeTags)
        {
            return AllCalculationTyps.Where(x => CalculationTypeTags.SingleOrDefault(y => y.CalculationTypeId == x.Id) != null);
            //var calType = AllCalculationTyps.SingleOrDefault(x => x.Id == CalculationTypeId);
            //return calType != null ? new Tickets.CalculationTypeTag(calType.Id, calType.Name) : new Tickets.CalculationTypeTag();
        }
    }
}
