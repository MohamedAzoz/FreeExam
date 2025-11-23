using AutoMapper;
using FreeExam.Application.Contracts.DTOs.Answer;
using FreeExam.Application.Contracts.DTOs.Exam;
using FreeExam.Application.Contracts.DTOs.Option;
using FreeExam.Application.Contracts.DTOs.Quetion;
using FreeExam.Application.Contracts.DTOs.Subject;
using FreeExam.Domain.Entities;

namespace FreeExam.Application.AutoMapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            #region Question Mappers

            CreateMap<CreateQuestionDto, Question>()
                .ForMember((src)=>src.Type,dis=>dis.MapFrom(d=>d.Type))
                .ForMember((src)=>src.Content,dis=>dis.MapFrom(d=>d.Content))
                .ForMember((src)=>src.ExamId,dis=>dis.MapFrom(d=>d.ExamId))
                .ReverseMap();


            CreateMap<Question, QuestionDto>()
                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.OptionAswerId, opt => opt.MapFrom(src => src.Answer.OptionId))
                .ForMember(dest => dest.Options, opt => opt.MapFrom(
                    src => src.Options.OrderBy(o => o.Id)
                ));


            CreateMap<Answer , AnswerDto>()
                .ForMember((src)=>src.OptionId,(dis=>dis.MapFrom(d=>d.OptionId)))
                .ForMember((src)=>src.QuestionId,(dis=>dis.MapFrom(d=>d.QuestionId)))
                .ReverseMap();

            CreateMap<Option, OptionDto>()
                .ForMember(dest => dest.OptionId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text));


            CreateMap<CreateOptionDto, Option>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text));


            #endregion

            #region Eaxm Mappers

            CreateMap<Exam , CreateExamDto>()
                .ForMember((src)=>src.Name,(dis=>dis.MapFrom(d=>d.Name)))
                .ForMember((src)=>src.SubjectId,(dis=>dis.MapFrom(d=>d.SubjectId)))
                .ReverseMap();
            
            CreateMap<Exam, ExamDto>()
                .ForMember((src)=>src.Id,(dis=>dis.MapFrom(d=>d.Id)))
                .ForMember((src)=>src.Name,(dis=>dis.MapFrom(d=>d.Name)))
                .ForMember((src)=>src.SubjectId,(dis=>dis.MapFrom(d=>d.SubjectId)))
                .ReverseMap();

            #endregion

            #region Subject Mappers

            CreateMap<Subject, CreateSubjectDto>()
                .ForMember((src)=>src.SubjectName,(dis=>dis.MapFrom(d=>d.Name)))
                .ReverseMap();
            
            CreateMap<Subject, SubjectDto>()
                .ForMember((src)=>src.SubjectName,(dis=>dis.MapFrom(d=>d.Name)))
                .ForMember((src)=>src.SubjectId,(dis=>dis.MapFrom(d=>d.Id)))
                .ReverseMap();
            
            CreateMap<Subject, CreateSubjectDto>()
                .ForMember((src)=>src.SubjectName,(dis=>dis.MapFrom(d=>d.Name)))
                .ReverseMap();

            #endregion

            #region

            #endregion
        
        
        }
    }
}
