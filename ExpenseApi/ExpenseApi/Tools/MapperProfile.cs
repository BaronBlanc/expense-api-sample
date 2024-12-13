using AutoMapper;
using ExpenseApi.Commands;
using ExpenseApi.Model;

namespace ExpenseApi.Tools
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CreateExpenseCommand, Expense>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Currency, opt => opt.Ignore())
                .ForMember(dest => dest.CurrencyId, opt => opt.Ignore())
                .ForMember(dest => dest.Type, opt => opt.MapFrom<ExpenseTypeResolver>());

            CreateMap<Expense, ExpenseView>()
            .ForMember(dest => dest.User, dest => dest.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
            .ForMember(dest => dest.Currency, dest => dest.MapFrom(src => src.Currency.Symbol))
            .ForMember(dest => dest.Type, dest => dest.MapFrom(src => src.Type.ToString()));

            CreateMap<Expense, CreateExpenseResult>()
            .ForMember(dest => dest.Currency, dest => dest.MapFrom(src => src.Currency.Symbol))
            .ForMember(dest => dest.Type, dest => dest.MapFrom(src => src.Type.ToString()));

        }
    }
}
