using KRA.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KRA.Models;
using KRA.Data.Repository;

namespace KRA.Domain.Service
{
    public class AccountsService : IAccountsService

    {
        private readonly IAccountsDal AccountRepository;
        private readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(AccountsService));

        public AccountsService(IAccountsDal AccountRepository)
        {
            this.AccountRepository = AccountRepository;
        }
        private Entities.Accounts ModelToEntityMapper(Models.AccountsModel Account)
        {
            Entities.Accounts AccountEntity = new Entities.Accounts
            {

                AccountID = Account.AccountID,
                Name = Account.Name,
                ManagerID = Account.ManagerID
            };

            return AccountEntity;
        }
        private AccountsModel EntityToModelMapper(Entities.Accounts Account)
        {
            AccountsModel account = new AccountsModel()
            {

                AccountID = Account.AccountID,
                Name = Account.Name,
                ManagerID = Account.ManagerID
            };

            return account;
        }
        public bool AddAccount(AccountsModel Account)
        {
            try
            {
                Entities.Accounts AccountEntity = this.ModelToEntityMapper(Account);
                if (AccountRepository.AddAccount(AccountEntity))
                {

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in AddAccount");
                logger.Error(ex.InnerException);
                logger.Error(ex.Message);
                logger.Error(ex.Source);
                return false;
            }
        }

        public List<Models.AccountsModel> GetAllAccounts()
        {
            List<AccountsModel> AccountList;
            AccountList = (from account in AccountRepository.GetAllAccounts()
                           select new Models.AccountsModel
                           {
                               AccountID = account.AccountID,
                               Name = account.Name,
                               ManagerID = account.ManagerID
                           }).ToList();
            return AccountList;
        }
        public AccountsModel GetAccount(int AccountId)
        {
            AccountsModel account = EntityToModelMapper(AccountRepository.GetAccount(AccountId));
            return account;
        }

        public List<AccountsModel> GetAllAccounts(int AdmId)
        {
            List<AccountsModel> AccountList;
            AccountList = (from account in AccountRepository.GetAllAccounts(AdmId)
                           select new Models.AccountsModel
                           {
                               AccountID = account.AccountID,
                               Name = account.Name,
                               ManagerID = account.ManagerID
                           }).ToList();
            return AccountList;
        }
    }
}
