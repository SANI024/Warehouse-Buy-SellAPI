namespace Warehouse_Buy_Sell.DTO
{
    public class SaleResponceDto
    {
        public int Id { get; set; }
        public DateTime SaleDate { get; set; }
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public List<SaleItemResponceDto> Items { get; set; }
    }
}
