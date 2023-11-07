using InventoryDTO.DTO.Customer_Dto;
using InventoryDTO.DTO.SalesOrder_Dto;
using InventoryEntities.Models;
using InventoryRepositories.Repositories.Interfaces;
using InventoryServices.Services.ServiceInterface;

namespace InventoryServices.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<CustomerDTO>> GetAll()
        {
            var customer = await _unitOfWork.Customer.GetAll(includeProperties: "SalesOrders");

            var customerDTO = new List<CustomerDTO>();
            foreach (var item in customer)
            {
                customerDTO.Add(new CustomerDTO()
                {
                    CustomerId = item.CustomerId,
                    CustomerName = item.CustomerName,
                    EmailId = item.EmailId,
                    PhoneNumber = item.PhoneNumber,
                    StreetAddress = item.StreetAddress,
                    City = item.City,
                    State = item.State,
                    Pincode = item.Pincode,
                    salesOrders = item.SalesOrders.Select(x => new SalesOrderDTO
                    {
                        SalesId = x.SalesId,
                        SaleDate = x.SaleDate,
                        QuantityForSale = x.QuantityForSale,
                        TotalCostOfSalesOrder = x.TotalCostOfSalesOrder,
                        SalesDiscount = x.SalesDiscount,
                        CustomerId = x.CustomerId
                    }).ToList()
                });
            }
            return customerDTO;
        }
        public async Task<CustomerDTO> GetById(int id)
        {
            var customer = await _unitOfWork.Customer.GetById(x => x.CustomerId == id, includeProperties: "SalesOrders");

            if (customer == null)
            {
                return null;
            }
            var customerDTO = new CustomerDTO
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.CustomerName,
                EmailId = customer.EmailId,
                PhoneNumber = customer.PhoneNumber,
                StreetAddress = customer.StreetAddress,
                City = customer.City,
                State = customer.State,
                Pincode = customer.Pincode,
                salesOrders = customer.SalesOrders.Select(x => new SalesOrderDTO
                {
                    SalesId = x.SalesId,
                }).ToList()
            };
            return customerDTO;
        }
        public async Task<CustomerDTO> Create(AddCustomerRequestDTO addCustomerRequestDTO)
        {
            //Map or Convert DTO to Model
            var customer = new Customer()
            {
                CustomerName = addCustomerRequestDTO.CustomerName,
                EmailId = addCustomerRequestDTO.EmailId,
                PhoneNumber = addCustomerRequestDTO.PhoneNumber,
                StreetAddress = addCustomerRequestDTO.StreetAddress,
                City = addCustomerRequestDTO.City,
                State = addCustomerRequestDTO.State,
                Pincode = addCustomerRequestDTO.Pincode
            };

            //Use Model to Create Region
            customer = await _unitOfWork.Customer.Add(customer);
            _unitOfWork.save();

            //Map Model Back to DTO
            var customerDTO = new CustomerDTO
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.CustomerName,
                EmailId = customer.EmailId,
                PhoneNumber = customer.PhoneNumber,
                StreetAddress = customer.StreetAddress,
                City = customer.City,
                State = customer.State,
                Pincode = customer.Pincode
            };
            return customerDTO;
        }

        public async Task<Customer> DeleteById(int id)
        {
            Customer deleteCustomerById = await _unitOfWork.Customer.GetById(x => x.CustomerId == id, includeProperties: "SalesOrders");

            if (deleteCustomerById == null)
            {
                return null;
            }
            await _unitOfWork.Customer.Delete(deleteCustomerById);
            _unitOfWork.save();
            return deleteCustomerById;
        }

        public async Task<UpdateCustomerRequestDTO> Update(int id, UpdateCustomerRequestDTO updateCustomerRequestDTO)
        {
            var customer = _unitOfWork.Customer.GetById(x => x.CustomerId == id, includeProperties: "SalesOrders");
            if (customer == null)
            {
                return null;
            }
            _unitOfWork.DetachAllEntities();

            //Map DTO to Model
            var customerModel = new Customer
            {
                CustomerId = id,
                CustomerName = updateCustomerRequestDTO.CustomerName,
                EmailId = updateCustomerRequestDTO.EmailId,
                PhoneNumber = updateCustomerRequestDTO.PhoneNumber,
                StreetAddress = updateCustomerRequestDTO.StreetAddress,
                City = updateCustomerRequestDTO.City,
                State = updateCustomerRequestDTO.State,
                Pincode = updateCustomerRequestDTO.Pincode
            };

            //check if region exists
            customerModel = await _unitOfWork.Customer.Update(customerModel);
            _unitOfWork.save();

            if (customerModel == null)
            {
                return null;
            }

            //Convert Model to DTO
            var categoryDTO = new UpdateCustomerRequestDTO
            {
                CustomerName = customerModel.CustomerName,
                EmailId = customerModel.EmailId,
                PhoneNumber = customerModel.PhoneNumber,
                StreetAddress = customerModel.StreetAddress,
                City = customerModel.City,
                State = customerModel.State,
                Pincode = customerModel.Pincode
            };
            return categoryDTO;
        }
    }
}
