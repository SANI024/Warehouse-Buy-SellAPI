namespace Warehouse_Buy_Sell.DTO
{
    public class InventoryResponceDto
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
