using KRA.Domain.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KRA.Models;
using KRA.Entities;
using KRA.Data.Repository;

namespace KRA.Domain.Services
{
    public class AccountTeamSizeService : IAccountTeamSizeService
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IAccountTeamSizeDal TeamSizeDal;
        public AccountTeamSizeService(IAccountTeamSizeDal TeamSizeDal)
        {
            this.TeamSizeDal = TeamSizeDal;
        }
        private AccountTeamSizeModel EntityToModelMapper(AccountTeamSize Teamsize)
        {
            AccountTeamSizeModel Size = new AccountTeamSizeModel()
            {
                AccountID = Teamsize.AccountId,
                AddedBy = Teamsize.AddedBy,
                AddedOn = Teamsize.AddedOn,
                Quarter = Teamsize.Quarter,
                TeamSize = Teamsize.TeamSize,
                TeamSizeID = Teamsize.TeamSizeID,
                Year = Teamsize.Year
            };
            return Size;
        }
        private AccountTeamSize ModelToEntityMapper(AccountTeamSizeModel Teamsize)
        {
            AccountTeamSize Size = new AccountTeamSize()
            {
                AccountId = Teamsize.AccountID,
                AddedBy = Teamsize.AddedBy,
                AddedOn = Teamsize.AddedOn,
                Quarter = Teamsize.Quarter,
                TeamSize = Teamsize.TeamSize,
                TeamSizeID = Teamsize.TeamSizeID,
                Year = Teamsize.Year
            };
            return Size;
        }

        public AccountTeamSizeModel GetTeamSize(int AccountId, string Quarter, int Year)
        {
            AccountTeamSizeModel size;
            try
            {
                AccountTeamSize Teamsize = TeamSizeDal.GetTeamSize(AccountId, Quarter, Year);
                size = EntityToModelMapper(Teamsize);
            }
            catch (Exception ex)
            {
                logger.Error(ex.InnerException);
                logger.Error(ex.Message);
                logger.Error(ex.Source);
                size = null;
            }
            return size;
        }

        public bool UpdateTeamSize(AccountTeamSizeModel TeamSize)
        {
            var result = true;
            try
            {
                TeamSizeDal.AddTeamSize(ModelToEntityMapper(TeamSize));
            }
            catch (Exception ex)
            {
                logger.Error(ex.InnerException);
                logger.Error(ex.Message);
                logger.Error(ex.Source);
                result = false;
            }
            return result;
        }
    }
}
