using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Domain.Model
{
    public class Contact
    {
        public readonly Person Person;
        public readonly Address Address;
        public readonly string Email;
        public readonly string HomePhone;

        public Contact(Person person, Address address, string email = "", string homePhone = "")
        {
            Person = person;
            Address = address;
            Email = email;
            HomePhone = homePhone;
        }
    }
}
