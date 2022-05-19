using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.user
{
    class TestUser : User
    {
        public TestUser()
        {

            Email = "test@protonmail.com";
            Password = "F123qwe$%^0000";            
            FullName = "Коновалов Алексей Сергеевич";
            FirstName = "Алексей";
            MiddleName = "Сергеевич";
            LastName = "Коновалов";
            BirthDate = "28.06.1986";
            PhoneNumber = "+7 (925) 618-69-36";
            Telegram = "@xeylov";
            Wallet = "$$$$$$";
            

        }
    }
}
