using Microsoft.Data.SqlClient;
using System;
using System.IO;

class PullProgram
{
    static void Main(string[] args)
    {
         string connectionString = "Server=userExample; Database=dbExample;Trusted_Connection=True;MultipleActiveResultSets=true; Integrated Security = True;TrustServerCertificate=True";

        Console.WriteLine("Por favor, ingrese la ruta completa del archivo CSV:");
        string inputFilePath = Console.ReadLine();

        if (!File.Exists(inputFilePath))
        {
            Console.WriteLine("Error: El archivo especificado no existe.");
            return;
        }

        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (StreamReader reader = new StreamReader(inputFilePath))
                {
                    string line;
                    int lineNumber = 0;

                    while ((line = reader.ReadLine()) != null)
                    {
                        lineNumber++;
                        if (lineNumber == 1) continue;

                        string[] values = line.Split(',');
                        if (values.Length != 4)
                        {
                            Console.WriteLine($"Error en la línea {lineNumber}: Número de columnas inválido");
                            continue;
                        }
                        string cedula = values[0];
                        string nombre = values[1];
                        string apellido = values[2];
                        decimal salario = decimal.Parse(values[3]);

                        string query = "INSERT INTO Nomina (Cedula, Nombre, Apellido, Salario) VALUES (@Cedula, @Nombre, @Apellido, @Salario)";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Cedula", cedula);
                            command.Parameters.AddWithValue("@Nombre", nombre);
                            command.Parameters.AddWithValue("@Apellido", apellido);
                            command.Parameters.AddWithValue("@Salario", salario);

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }

            Console.WriteLine("Datos importados correctamente desde el archivo CSV a la base de datos.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}
