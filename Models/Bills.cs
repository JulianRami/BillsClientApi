namespace BillsClientApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

    public class Bills
    {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }


    [JsonPropertyName("CodigoFactura")]
    [BsonElement("CodigoFactura")]
    public string InvoiceCode { get; set; } = null!;

    public string Cliente { get; set; } = null!;

    public string Ciudad { get; set; } = null!;

    public string NIT { get; set; } = null!;

    [BsonElement("TotalFactura")]
    public decimal TotalInvoice { get; set; }

    public decimal SubTotal { get; set; }

    public decimal Iva { get; set; }

    public decimal Retencion { get; set; }

    [BsonElement("Fecha de Creacion")]
    public DateTime CreationDate { get; set; }

    public string Estado { get; set; } = null!;

    public bool Pagada { get; set; }

    [BsonElement("Fecha de pago")]
    public string PaymentDate { get; set; }

    public string Correo { get; set; } = null!;
}

