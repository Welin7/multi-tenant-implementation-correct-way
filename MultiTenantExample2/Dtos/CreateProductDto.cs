﻿namespace MultiTenantExample2.Dtos
{
    public class CreateProductDto
    {   
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Rate { get; set; }
    }
}
