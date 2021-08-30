using System;

#nullable disable

namespace DataTable.Models
{
    /// <summary>
    /// Entity framework által generált kód.
    /// </summary>

    public partial class Address
    {
        public int AddressId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public int StateProvinceId { get; set; }
        public string PostalCode { get; set; }
        public Guid Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
        public byte Active { get; set; }
    }
}