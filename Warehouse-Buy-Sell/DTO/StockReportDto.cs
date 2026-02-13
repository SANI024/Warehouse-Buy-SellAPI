namespace Warehouse_Buy_Sell.DTO
{
    public class StockReportDto
    {
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public List<StockItemDto> Items { get; set; }
    }
}
