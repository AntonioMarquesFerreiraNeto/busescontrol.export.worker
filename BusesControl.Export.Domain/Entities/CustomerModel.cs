using BusesControl.Export.Core.Enums;

namespace BusesControl.Export.Core.Entities
{
    public class CustomerModel
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string Cnpj { get; set; }
        public string Document 
        {
            get 
            { 
                return !string.IsNullOrEmpty(Cpf) 
                    ? Convert.ToUInt64(Cpf).ToString(@"000\.000\.000\-00") 
                    : Convert.ToUInt64(Cnpj).ToString(@"00\.000\.000\/0000\-00"); 
            }
        }
        public CustomerType Type { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public bool Active { get; set; }
    }
}
