using System.ComponentModel;

namespace BusesControl.Export.Core.Enums
{
    public enum CustomerType
    {
        [Description("Pessoa física")]
        NaturalPerson = 1,
        [Description("Pessoa jurídica")]
        LegalEntity = 2
    }
}
