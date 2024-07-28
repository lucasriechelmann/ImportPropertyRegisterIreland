using Microsoft.VisualBasic.FileIO;
using System.Data.SqlClient;

string csvFilePath = "C:\\Git\\PPR-ALL.csv";

try
{
    using (TextFieldParser parser = new TextFieldParser(csvFilePath))
    {
        parser.TextFieldType = FieldType.Delimited;
        parser.SetDelimiters(",");
        int count = 0;
        while (!parser.EndOfData)
        {
            count++;
            string[] fields = parser.ReadFields();
            if (count > 1)
            {                
                DateTime dateOfSale;
                decimal price = 0;

                var property = new Property()
                {
                    DateOfSale = DateTime.TryParse(fields[0], out dateOfSale) ? dateOfSale : new DateTime?(),
                    Address = fields[1],
                    County = fields[2],
                    Eircode = fields[3],
                    Price = decimal.TryParse(fields[4].Replace("�", ""), out price) ? price : new decimal?(),
                    NotFullMarketPrice = fields[5],
                    VATExclusive = fields[6],
                    DescriptionOfProperty = fields[7],
                    PropertySizeDescription = fields[8]
                };
                InsertProperty(property);
                Console.WriteLine(property.ToString());
            }
        }
    }
}
catch(Exception ex)
{
    Console.WriteLine(ex.Message);
    Console.WriteLine(ex.StackTrace);
}

Console.ReadLine();

static void InsertProperty(Property property)
{
    string connectionString = "Server=.;Database=PropertyRegisterIE;User Id=sa;Password=123456;";

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        string query = @"INSERT INTO Property (DateOfSale, Address, County, Eircode, Price, NotFullMarketPrice, VATExclusive, DescriptionOfProperty, PropertySizeDescription)
                             VALUES (@DateOfSale, @Address, @County, @Eircode, @Price, @NotFullMarketPrice, @VATExclusive, @DescriptionOfProperty, @PropertySizeDescription)";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@DateOfSale", property.DateOfSale);
            command.Parameters.AddWithValue("@Address", property.Address);
            command.Parameters.AddWithValue("@County", property.County);
            command.Parameters.AddWithValue("@Eircode", property.Eircode);
            command.Parameters.AddWithValue("@Price", property.Price);
            command.Parameters.AddWithValue("@NotFullMarketPrice", property.NotFullMarketPrice);
            command.Parameters.AddWithValue("@VATExclusive", property.VATExclusive);
            command.Parameters.AddWithValue("@DescriptionOfProperty", property.DescriptionOfProperty);
            command.Parameters.AddWithValue("@PropertySizeDescription", property.PropertySizeDescription);

            command.ExecuteNonQuery();
        }

        connection.Close();
    }
}
public class Property
{
    public DateTime? DateOfSale { get; set; }
    public string Address { get; set; }
    public string County { get; set; }
    public string Eircode { get; set; }
    public decimal? Price { get; set; }
    public string NotFullMarketPrice { get; set; }
    public string VATExclusive { get; set; }
    public string DescriptionOfProperty { get; set; }
    public string PropertySizeDescription { get; set; }
    public override string ToString()
    {
        return $"DateOfSale: {DateOfSale} | Address: {Address} | County: {County} | Eircode: {Eircode} | Price: {Price} | NotFullMarketPrice: {NotFullMarketPrice} | VATExclusive: {VATExclusive} | DescriptionOfProperty: {DescriptionOfProperty} | PropertySizeDescription: {PropertySizeDescription}";
    }
}