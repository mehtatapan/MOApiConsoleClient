using System;
using System.Collections.Generic;
using System.Text;

namespace MOApiConsoleClient.Models
{
    public class Doctor
    {
        public int ID { get; set; }
        public string FullName
        {
            get
            {
                return "Dr. " + FirstName
                    + (string.IsNullOrEmpty(MiddleName) ? " " :
                        (" " + (char?)MiddleName[0] + ". ").ToUpper())
                    + LastName;
            }
        }
        public string FormalName
        {
            get
            {
                return LastName + ", " + FirstName
                    + (string.IsNullOrEmpty(MiddleName) ? "" :
                        (" " + (char?)MiddleName[0] + ".").ToUpper());
            }
        }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Byte[] RowVersion { get; set; }
        public ICollection<Patient> Patients { get; set; }
    }
}
