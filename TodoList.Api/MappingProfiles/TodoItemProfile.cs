﻿using AutoMapper;
using TodoList.Api.Models;
using TodoList.Api.Data;
using TodoList.Api.DTOs;

namespace TodoList.Api.MappingProfiles
{
    public class TodoItemProfile : Profile
    {
        public TodoItemProfile()
        {
            CreateMap<TodoItemDto, TodoItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignore Id mapping (assuming it's generated by the system)
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => false)); // Set IsCompleted to false initially
        }
    }
}