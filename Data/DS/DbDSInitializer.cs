using demoAPI.Model.DS;
using demoAPI.Model;

namespace demoAPI.Data.DS
{
    public static class DbDSInitializer
    {
        public static void Initialize(DSContext context)
        {
            context.Database.EnsureCreated();

            if (context.DSItems.Any())
            {
                return;   // DB has been seeded
            }

            //-----------------------------------

            var members = new Member[]
            {
                new Member{ Name="admin", Password="admin" },
                new Member{ Name="eddy", Password="eddy" }
            };
            foreach (var s in members)
            {
                context.Members.Add(s);
            }
            context.SaveChanges();

            //-----------------------------------

            var dsTransactions = new DSTransaction[]
            {
                new DSTransaction{ DSTypeID = 2, DSAccountID = 1, DSTransferOutID = 0, DSItemID = 0, DSItemSubID = 1, Description = "kfc", Amount = 15},
                new DSTransaction{ DSTypeID = 2, DSAccountID = 1, DSTransferOutID = 0, DSItemID = 2, DSItemSubID = 0, Description = "car", Amount = 50},
                new DSTransaction{ DSTypeID = 3, DSAccountID = 1, DSTransferOutID = 0, DSItemID = 0, DSItemSubID = 0, Description = "", Amount = 1000},
                new DSTransaction{ DSTypeID = 4, DSAccountID = 2, DSTransferOutID = 3, DSItemID = 0, DSItemSubID = 0, Description = "", Amount = 1000},
            };
            foreach (var s in dsTransactions)
            {
                context.DSTransactions.Add(s);
            }
            context.SaveChanges();

            //-----------------------------------

            var dsTypes = new DSType[]
            {
                new DSType{ ID = 1, Name="Credit" },
                new DSType{ ID = 2, Name="Debit" },
                new DSType{ ID = 3, Name="Transfer Out" },
                new DSType{ ID = 4, Name="Transfer In" },
            };
            foreach (var s in dsTypes)
            {
                context.DSTypes.Add(s);
            }
            context.SaveChanges();

            //-----------------------------------

            var dsAccounts = new TransProfile[]
            {
                new TransProfile{ Name="Maybank", IsActive=true },
                new TransProfile{ Name="Public", IsActive=true },
            };
            foreach (var s in dsAccounts)
            {
                context.DSAccounts.Add(s);
            }
            context.SaveChanges();

            //-----------------------------------

            var dsItems = new DSItem[]
            {
                new DSItem{ Name="foods", IsActive=true },
                new DSItem{ Name="petrol", IsActive=true },
            };
            foreach (var s in dsItems)
            {
                context.DSItems.Add(s);
            }
            context.SaveChanges();

            //-----------------------------------

            var dsItemSubs = new DSItemSub[]
            {
                new DSItemSub{  Name="dinner", IsActive=true, DSItemID = 1},
                new DSItemSub{  Name="lunch", IsActive=true, DSItemID = 1},
                new DSItemSub{  Name="car", IsActive=true, DSItemID = 2},
            };
            foreach (var e in dsItemSubs)
            {
                context.DSItemSubs.Add(e);
            }
            context.SaveChanges();
        }
    }
}
