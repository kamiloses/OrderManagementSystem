﻿namespace OrderManagementSystem.Dtos;

public class CreateUserDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public int? Age { get; set; }
    public string Role { get; set; } 
}