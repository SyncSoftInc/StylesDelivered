using SyncSoft.App.Securities;

namespace SyncSoft.StylesDelivered.DTO.Common
{
    public class AddressDTO
    {
        public string ID { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        public string ToSha1()
        {
            var hash = $"{ Utils.FormatAddress(Address1).ToLower() }|{ Utils.FormatAddress(Address2).ToLower() }|{ Utils.FormatAddress(City).ToLower() }|{ Utils.FormatAddress(State).ToLower() }|{ Utils.FormatAddress(ZipCode).ToLower() }";
            return hash.ToSha1String();
        }

        public static bool operator ==(AddressDTO obj1, AddressDTO obj2)
        {
            return obj1?.ToSha1() == obj2?.ToSha1();
        }

        public static bool operator !=(AddressDTO obj1, AddressDTO obj2)
        {
            return obj1?.ToSha1() != obj2?.ToSha1();
        }

        public override bool Equals(object obj)
        {
            return this?.ToSha1() == ((AddressDTO)obj)?.ToSha1();
        }

        public override int GetHashCode()
        {
            return this.ToSha1().GetHashCode();
        }

        //public static IEqualityComparer<AddressDTO> Comparer { get; } = new AddressComparer();

        //class AddressComparer : IEqualityComparer<AddressDTO>
        //{
        //    public bool Equals(AddressDTO x, AddressDTO y)
        //    {
        //        return x.ToSha1() == y.ToSha1();
        //    }

        //    public int GetHashCode(AddressDTO obj)
        //    {
        //        return obj.ToSha1().GetHashCode();
        //    }
        //}
    }
}
