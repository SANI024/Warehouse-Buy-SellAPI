namespace Warehouse_Buy_Sell.DTO
{
    public class ProductProfitDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalCost { get; set; }
        public decimal Profit {  get; set; }
        
    }
}
