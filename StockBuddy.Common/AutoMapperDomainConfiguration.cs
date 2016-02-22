using AutoMapper;
using StockBuddy.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBuddy.Common
{
    public static class AutoMapperDomainConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new HistoryProfile());
            });
        }
    }

    public class HistoryProfile : Profile
    {
        protected override void Configure()
        {
            AutoMapper.Mapper.CreateMap<Historic, History>()
                    .ForMember(dest => dest.AdustedClosePrice, opt => opt.MapFrom(src => src.AdjustedClose))
                    .ForMember(dest => dest.ClosePrice, opt => opt.MapFrom(src => src.Close))
                    .ForMember(dest => dest.HighPrice, opt => opt.MapFrom(src => src.High))
                    .ForMember(dest => dest.LowPrice, opt => opt.MapFrom(src => src.Low))
                    .ForMember(dest => dest.OpenPrice, opt => opt.MapFrom(src => src.Open))
                    .ForMember(dest => dest.PreviousClosePrice, opt => opt.MapFrom(src => src.PreviousClose));
        }
    }
}
