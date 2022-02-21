using MOApiConsoleClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MOApiConsoleClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RunAsync().Wait();
            Console.ReadLine();
        }
        static async Task RunAsync()
        {
            using (var client = new HttpClient())
            {
                // Prepare Client
                client.BaseAddress = new Uri("http://localhost:15354/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Prepare Respopnse Object
                HttpResponseMessage response;
                // Get the Patients
                try
                {
                    response = await client.GetAsync("api/patients");
                    response.EnsureSuccessStatusCode(); // Throw exception if not success code

                    List<Patient> patients = await response.Content.ReadAsAsync<List<Patient>>();
                    foreach (Patient p in patients)
                    {
                        Console.WriteLine("ID:{0}\t{1}\tExpYrVisits:{2}\tDoctor:{3}", p.ID, p.FullName, p.ExpYrVisits, p.Doctor.FullName);
                    }
                }
                catch (HttpRequestException)
                {
                    Console.WriteLine("Could not get patient list.");
                    return;
                }
                //Add a patient, update it and then delete it.
                // HTTP POST
                var patient = new Patient() { FirstName = "Tyrion", LastName = "Lannister", OHIP = "4325436599", ExpYrVisits = 5, DoctorID = 2 };
                Console.WriteLine("\r\nADDING:\r\nID:{0}\t{1}\tExpYrVisits:{2}\tDoctorID:{3}", patient.ID, patient.FullName, patient.ExpYrVisits, patient.DoctorID);

                try
                {
                    response = await client.PostAsJsonAsync("api/patients", patient);
                    response.EnsureSuccessStatusCode(); // Throw exception if not success code
                    //Get the new ID
                    Uri patientUrl = response.Headers.Location;
                    int newID = Convert.ToInt32(patientUrl.ToString().Split('/').Last());
                    Console.WriteLine("SUCCESSFULLY Uploaded:\r\nID:{0}\t{1}\tExpYrVisits:{2}\tDoctorID:{3}", patient.ID, patient.FullName, patient.ExpYrVisits, patient.DoctorID);

                    // HTTP PUT
                    //We have to get a copy of the patient we added in order to have the correct row version
                    response = await client.GetAsync("api/patients" + "/" + newID);
                    response.EnsureSuccessStatusCode(); // Throw exception if not success code
                    Patient addedpatient = await response.Content.ReadAsAsync<Patient>();
                    Console.WriteLine("Record From Database:\r\nID:{0}\t{1}\tExpYrVisits:{2}\tDoctor:{3}", addedpatient.ID, addedpatient.FullName, addedpatient.ExpYrVisits, addedpatient.Doctor.FullName);

                    addedpatient.ExpYrVisits = 10;   // Update ExpYsVisits
                    response = await client.PutAsJsonAsync(patientUrl, addedpatient);
                    response.EnsureSuccessStatusCode(); // Throw exception if not success code
                    Console.WriteLine("Edited:\r\nID:{0}\t{1}\tExpYrVisits:{2}\t{3}", addedpatient.ID, addedpatient.FullName, addedpatient.ExpYrVisits, addedpatient.Doctor.FullName);

                    // HTTP DELETE
                    response = await client.DeleteAsync(patientUrl);
                    response.EnsureSuccessStatusCode(); // Throw exception if not success code
                    Console.WriteLine("DELETED!\r\n - GET THE LIST AGAIN TO SHOW IT IS GONE\r\n");

                    //GET LIST AND SHOW IT AGAIN
                    response = await client.GetAsync("api/patients");
                    if (response.IsSuccessStatusCode)
                    {
                        List<Patient> patients = await response.Content.ReadAsAsync<List<Patient>>();
                        foreach (Patient p in patients)
                        {
                            Console.WriteLine("ID:{0}\t{1}\tExpYrVisits:{2}\t{3}", p.ID, p.FullName, p.ExpYrVisits, p.Doctor.FullName);
                        }
                    }
                }
                catch (HttpRequestException)
                {
                    Console.WriteLine("Error during CRUD Test.");
                    return;
                }
            }
        }
    }
}
