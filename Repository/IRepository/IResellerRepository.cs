using MechantInventory.Model;
using MechantInventory.Model.Dto;

namespace MechantInventory.Repository.IRepository
{
    public interface IResellerRepository : IRepository<Reseller>
    {
        Task<Reseller> GetResellerByIdAsync(int resellerId);
        Task<ResellerTransaction> AddTransactionAsync(ResellerTransactionDto dto);
        Task<ResellerPayment> AddPaymentAsync(ResellerPaymentDto dto);
        Task<ResellerReturn> AddReturnAsync(ResellerReturnDto dto);


        //Task<ResellerTransaction> CreateTransactionWithItemsAsync(ResellerTransactionDto dto);

        //Task<ResellerTransaction> GetTransactionByIdAsync(int id);
        //Task<ResellerPayment> CreatePaymentAsync(ResellerPayment payment);
        //Task<ResellerReturn> ProcessReturnAsync(ResellerReturn resellerReturn);
        //Task<IEnumerable<ResellerTransaction>> GetOverdueTransactionsAsync();
        //Task UpdateTransactionStatusAsync(int resellerTransactionId);
    }
}
