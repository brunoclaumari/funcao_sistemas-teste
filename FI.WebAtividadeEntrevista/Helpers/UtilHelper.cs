

using FI.AtividadeEntrevista.DML;
using System.Collections.Generic;
using System.Reflection;
using WebAtividadeEntrevista.Models;

namespace FI.WebAtividadeEntrevista.Helpers
{
    public class UtilHelper
    {
        public List<Beneficiario> PreencheBeneficiario(ClienteModel model)
        {
            List<Beneficiario> bene = new List<Beneficiario>();
            model.Beneficiarios.ForEach(b =>
            {
                bene.Add(new Beneficiario
                {
                    Id = b.Id,
                    Cpf = b.Cpf,
                    Nome = b.Nome,
                    IdCliente = model.Id,
                });
            });

            return bene;
        }

        public List<BeneficiarioModel> ConverteBeneficiarioParaBeneficiarioModel(Cliente cliente)
        {
            List<BeneficiarioModel> beneModel = new List<BeneficiarioModel>();
            cliente.Beneficiarios.ForEach(b =>
            {
                beneModel.Add(new BeneficiarioModel
                {
                    Id = b.Id,
                    Cpf = b.Cpf,
                    Nome = b.Nome,
                    IdCliente = cliente.Id,
                });
            });

            return beneModel;
        }

    }
}