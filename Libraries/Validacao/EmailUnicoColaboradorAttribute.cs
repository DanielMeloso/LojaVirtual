using LojaVirtual.Models;
using LojaVirtual.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Libraries.Validacao
{
    public class EmailUnicoColaboradorAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // pegar o valor to campo E-mail
            string Email = (value as string).Trim();

            // Obter o repository do Colaborador
            IColaboradorRepository _colaboradorRepository = (IColaboradorRepository)validationContext.GetService(typeof(IColaboradorRepository));

            // Fazer verificação
            List<Colaborador> colaboradores = _colaboradorRepository.ObterColaboradorPorEmail(Email);
            Colaborador objColaborador = (Colaborador)validationContext.ObjectInstance;
            
            if (colaboradores.Count > 1)
            {
                return new ValidationResult("E-mail já cadastrado");
            }

            if (colaboradores.Count == 1 && objColaborador.Id != colaboradores[0].Id)
            {
                return new ValidationResult("E-mail já cadastrado");
            }

            return ValidationResult.Success;
        }

    }
}
