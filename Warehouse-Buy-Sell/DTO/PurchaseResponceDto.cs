namespace Warehouse_Buy_Sell.DTO
{
    public class PurchaseResponceDto
    {
        public int Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<PurchaseItemResponceDto> Items { get; set; }
    }
}
