﻿using MeuLivroDeReceitas.CrossCutting.EnumClass;

namespace MeuLivroDeReceitas.CrossCutting.Dto.Recipess
{
    public class AddRecipeDTO
    {
        public string Title { get; set; }

        public string? PreparationMode { get; set; }

        public int PreparationTimeMinute { get; set; }

        public ECategory Category { get; set; }
    }
}
