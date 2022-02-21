using System;
using System.Collections.Generic;
using System.Text;

namespace MOApiConsoleClient.Models
{
    public class Patient
    {
        public int ID { get; set; }
        public string FullName
        {
            get
            {
                return FirstName
                    + (string.IsNullOrEmpty(MiddleName) ? " " :
                        (" " + (char?)MiddleName[0] + ". ").ToUpper())
                    + LastName;
            }
        }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string OHIP { get; set; }
        public DateTime DOB { get; set; }
        public byte ExpYrVisits { get; set; }
        public Byte[] RowVersion { get; set; }
        public int DoctorID { get; set; }
        public Doctor Doctor { get; set; }
    }
}
