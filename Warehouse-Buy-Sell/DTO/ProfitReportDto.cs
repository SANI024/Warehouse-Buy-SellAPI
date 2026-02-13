namespace Warehouse_Buy_Sell.DTO
{
    public class ProfitReportDto
    {
        public decimal TotalRevenue { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalProfit { get; set; }
        public List<ProductProfitDto> ProductProfits { get; set; }
    }
}
