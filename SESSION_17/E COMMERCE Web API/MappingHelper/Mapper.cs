using AutoMapper;
using E_COMMERCE_Web_API.DTOs.CategoryDTOs;
using E_COMMERCE_Web_API.DTOs.ProductsDTOs;
using E_COMMERCE_Web_API.DTOs.CustomerDTOs;
using E_COMMERCE_Web_API.DTOs.OrderDTOs;
using E_COMMERCE_Web_API.DTOs.OrderDetailDTO;
using E_COMMERCE_Web_API.Entities;

namespace E_COMMERCE_Web_API.MappingHelper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ── Category ──────────────────────────────────────────────────────

            // Category → SlimCategoryDTO
            // Id and Name match by name — convention handles it.
            CreateMap<Category, SlimCategoryDTO>();

            // Category → CategoryDTO (inherits SlimCategoryDTO, adds Products)
            // Model has ICollection<Product>; DTO has ICollection<SlimProductDTO>.
            // AutoMapper resolves the element type using the Product → SlimProductDTO map below.
            CreateMap<Category, CategoryDTO>()
                .ForMember(dest => dest.Products,
                    opt => opt.MapFrom(src => src.Products));

            // CreateCategoryRequest → Category
            // Name matches by name — convention handles it.
            // Category.Id is DB-generated — ignore it so AutoMapper doesn't try to set it.
            CreateMap<CreateCategoryRequest, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Products, opt => opt.Ignore());

            // UpdateCategoryRequest → Category
            // Request uses NewName; model uses Name — explicit mapping required.
            // Id comes from the route — ignore it on the model.
            CreateMap<UpdateCategoryRequest, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Products, opt => opt.Ignore())
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.NewName));

            // ── Product ───────────────────────────────────────────────────────

            // Product → SlimProductDTO
            // Id, Name, Price all match by name — convention handles it.
            CreateMap<Product, SlimProductDTO>();

            // Product → ProductDTO (inherits SlimProductDTO, adds Category)
            // Nested object: AutoMapper resolves it using the Category → SlimCategoryDTO map above.
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.Category,
                    opt => opt.MapFrom(src => src.Category));

            // CreateProductRequestDTO → Product
            // Name, Price, CategoryId all match by name — convention handles it.
            // Id is DB-generated; navigation properties are loaded by EF — ignore them.
            CreateMap<CreateProductRequestDTO, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.OrderDetails, opt => opt.Ignore());

            // UpdateProductRequest → Product
            // Request uses NewProductName, ProductPrice, NewCategoryId;
            // model uses Name, Price, CategoryId — explicit mapping required for all three.
            // Id comes from the route — ignore it on the model.
            CreateMap<UpdateProductRequest, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.OrderDetails, opt => opt.Ignore())
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.NewProductName))
                .ForMember(dest => dest.Price,
                    opt => opt.MapFrom(src => src.ProductPrice))
                .ForMember(dest => dest.CategoryId,
                    opt => opt.MapFrom(src => src.NewCategoryId));

            // ── Customer ──────────────────────────────────────────────────────

            // Customer → CustomerDto
            // Id, Name, Email all match by name — convention handles it.
            CreateMap<Customer, CustomerDto>();

            // Customer → CustomerWithOrdersDto
            // Id, Name, Email match by name.
            // Model has ICollection<Order>; DTO has IEnumerable<OrderDto>.
            // AutoMapper resolves the element type using the Order → OrderDto map below.
            CreateMap<Customer, CustomerWithOrdersDto>()
                .ForMember(dest => dest.Orders,
                    opt => opt.MapFrom(src => src.Orders));

            // CreateCustomerDto → Customer
            // Name and Email match by name — convention handles it.
            // Id is DB-generated — ignore it.
            CreateMap<CreateCustomerDto, Customer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Orders, opt => opt.Ignore());

            // UpdateCustomerDto → Customer
            // Name and Email match by name (both nullable in the request — partial update).
            // Id comes from the route — ignore it on the model.
            CreateMap<UpdateCustomerDto, Customer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Orders, opt => opt.Ignore());

            // ── Order ─────────────────────────────────────────────────────────

            // Order → OrderDto
            // OrderDate and CustomerId match by name.
            // DTO uses OrderId; model uses Id — explicit mapping required.
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.OrderId,
                    opt => opt.MapFrom(src => src.Id));

            // Order → OrderWithDetailsDto
            // Id, OrderDate, CustomerId match by name.
            // CustomerName: pulled from the nested Customer navigation property — explicit mapping required.
            // OrderDetails: resolved using the OrderDetail → OrderDetailDto map below.
            CreateMap<Order, OrderWithDetailsDto>()
                .ForMember(dest => dest.CustomerName,
                    opt => opt.MapFrom(src => src.Customer != null ? src.Customer.Name : string.Empty))
                .ForMember(dest => dest.OrderDetails,
                    opt => opt.MapFrom(src => src.OrderDetails));

            // CreateOrderDto → Order
            // CustomerId matches by name.
            // OrderDate defaults to UtcNow if the request omits it.
            // Id is DB-generated; OrderDetails are handled separately by the service layer — ignore both.
            CreateMap<CreateOrderDto, Order>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.OrderDetails, opt => opt.Ignore())
                .ForMember(dest => dest.OrderDate,
                    opt => opt.MapFrom(src =>
                        src.OrderDate.HasValue
                            ? src.OrderDate.Value
                            : DateTime.UtcNow));

            // UpdateOrderDto → Order
            // Id and CustomerId come from the route / existing entity — ignore them.
            // OrderDate defaults to UtcNow if omitted.
            CreateMap<UpdateOrderDto, Order>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.OrderDetails, opt => opt.Ignore())
                .ForMember(dest => dest.OrderDate,
                    opt => opt.MapFrom(src =>
                        src.OrderDate.HasValue
                            ? src.OrderDate.Value
                            : DateTime.UtcNow));

            // ── OrderDetail ───────────────────────────────────────────────────

            // OrderDetail → OrderDetailDto
            // OrderId, ProductId, Quantity match by name.
            // ProductName: pulled from the nested Product navigation property — explicit mapping required.
            CreateMap<OrderDetail, OrderDetailDto>()
                .ForMember(dest => dest.ProductName,
                    opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : null));

            // CreateOrderDetailDto → OrderDetail
            // ProductId and Quantity match by name — convention handles it.
            // OrderId is injected by the service layer from the route — ignore it.
            // UnitPrice is resolved from the Product in the service layer — ignore it here.
            // Navigation properties are loaded by EF — ignore them.
            CreateMap<CreateOrderDetailDto, OrderDetail>()
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore());

            // UpdateOrderDetailDto → OrderDetail
            // Quantity is nullable in the request (partial update) — map only if provided.
            // Key fields and navigation properties come from the existing entity — ignore them.
            CreateMap<UpdateOrderDetailDto, OrderDetail>()
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                .ForMember(dest => dest.ProductId, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.Quantity,
                    opt => opt.MapFrom(src =>
                        src.Quantity.HasValue ? src.Quantity.Value : 0));
        }
    }
}