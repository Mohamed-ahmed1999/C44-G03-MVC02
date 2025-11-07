using AutoMapper;
using GymManagementBLL.ViewModels.MemberSessionViewModels;
using GymManagementBLL.ViewModels.MemberShipViewModels;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using GymManangementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
    
            MapSession();
            MapMember();
            MapPlan();
            MapTrainer();
            MapMemberShip();
            MapMemberSession();
        }

      
        private void MapSession()
        {
            CreateMap<Session, SessionViewModel>()
            .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.SessionTrainer.Name))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.SessionCategory.CategoryName))
            .ForMember(dest => dest.AvailableSlots, opt => opt.Ignore());

            CreateMap<CreateSessionViewModel, Session>();

            CreateMap<UpdateSessionViewModel, Session>().ReverseMap();

            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName));
        } 

        private void MapMember()
        {
            CreateMap<CreateMemberViewModel, Member>()
             .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src))
             .ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => src.HealthRecordViewModel));


            CreateMap<CreateMemberViewModel, Address>()
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.BuildingNumber))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City));



            CreateMap<HealthRecordViewModel, HealthRecord>().ReverseMap();

            CreateMap<Member, MemberViewModel>()
                        .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                        .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()))
                        .ForMember(dest => dest.Adress, opt => opt.MapFrom(src =>
                            $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"));

            CreateMap<Member, MemberToUpdateViewModel>()
                        .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
                        .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                        .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street)).ReverseMap();

            CreateMap<MemberToUpdateViewModel, Member>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Address.BuildingNumber = src.BuildingNumber;
                    dest.Address.City = src.City;
                    dest.Address.Street = src.Street;
                    dest.UpdatedAt = DateTime.Now;
                });

        }

       private void MapPlan()
        {
            CreateMap<Plan, PlanViewModel>()
               .ForMember(dest => dest.DurtaionDays, opt => opt.MapFrom(src => src.DurationDays)).ReverseMap();

            CreateMap<Plan, UpdatePlanViewModel>()
                        .ForMember(dest => dest.PlanName, o => o.MapFrom(x => x.Name))
                        .ForMember(dest => dest.DurtaionDays, o => o.MapFrom(x => x.DurationDays)).ReverseMap();

        }

        private void MapTrainer()
        {
            CreateMap<CreateTrainerViewModel, Trainer>()
                   .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                   {
                       BuildingNumber = src.BuildingNumber,
                       Street = src.Street,
                       City = src.City
                   }));
            CreateMap<Trainer, TrainerViewModel>()
                .ForMember(dist => dist.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.City} - {src.Address.Street}"));
            CreateMap<Trainer, TrainerToUpdateViewModel>()
                .ForMember(dist => dist.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dist => dist.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dist => dist.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber));

            CreateMap<TrainerToUpdateViewModel, Trainer>()
            .ForMember(dest => dest.Name, opt => opt.Ignore())
            .AfterMap((src, dest) =>
            {
                dest.Address.BuildingNumber = src.BuildingNumber;
                dest.Address.City = src.City;
                dest.Address.Street = src.Street;
                dest.UpdatedAt = DateTime.Now;
            });
        }

        private void MapMemberShip()
        {
              CreateMap<MemberShip, MemberShipViewModel>()
        .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member.Name))
        .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Plan.Name))
        .ForMember(dest => dest.MemberId, opt => opt.MapFrom(src => src.MemberId))
        .ForMember(dest => dest.PlanId, opt => opt.MapFrom(src => src.PlanId));
            CreateMap<CreateMemberShipViewModel, MemberShip>();
        }

        private void MapMemberSession()
        {
            CreateMap<MemberSession, MemberSessionViewModel>()
                .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member.Name))
                .ForMember(dest => dest.MemberId, opt => opt.MapFrom(src => src.MemberId))
                .ForMember(dest => dest.SessionDescription, opt => opt.MapFrom(src => src.Session.Description));
        }
    }
}
