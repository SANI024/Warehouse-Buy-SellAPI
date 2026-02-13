namespace Warehouse_Buy_Sell.DTO
{
    public class InternalMovementResponceDto
    {
        public int Id { get; set; }
        public DateTime MovementDate { get; set; }
        public int FromWarehouseId { get; set; }
        public string FromWarehouseName { get; set; }
        public int ToWarehouseId { get; set; }
        public string ToWarehouseName { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<InternalMovementItemResponceDto> Items { get; set; }
    }
}
