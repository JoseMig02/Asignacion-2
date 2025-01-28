using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.IO;

class PushProgram
{
    static void Main(string[] args)
    {
        string connectionString = "Server=userExample; Database=dbExample;Trusted_Connection=True;MultipleActiveResultSets=true; Integrated Security = True;TrustServerCertificate=True";
        
        string downloadsFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads";
        string outputFilePath = Path.Combine(downloadsFolder, "empleados.csv");

        try
        {
            string query = "SELECT Cedula, Nombre, Apellido, Salario FROM Nomina";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    using (StreamWriter writer = new StreamWriter(outputFilePath))
                    {
                        writer.WriteLine("Cedula,Nombre,Apellido,Salario");

                        while (reader.Read())
                        {
                            string cedula = reader["Cedula"].ToString();
                            string nombre = reader["Nombre"].ToString();
                            string apellido = reader["Apellido"].ToString();
                            string salario = reader["Salario"].ToString();

                            writer.WriteLine($"{cedula},{nombre},{apellido},{salario}");
                        }
                    }
                }
            }

            Console.WriteLine("Archivo CSV generado exitosamente en: " + outputFilePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}
