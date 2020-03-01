namespace Vidly.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedUsers : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'3ff46a72-c786-437b-9091-da5adc6b59e5', N'admin@vidly.com', 0, N'AO/GBRJIjHqPAIyX97Yw/h8rr8tup9pEPEltNFwhqE4Q3BGWVSuUh9u/9hmB91LDdw==', N'70bdba3e-6f5c-4918-b169-69871163bd86', NULL, 0, 0, NULL, 1, 0, N'admin@vidly.com')
                INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'78f8c023-a753-4fea-9b13-2c52f71eb938', N'guest@vidly.com', 0, N'AMM09aTuDlse1yXmmsOLJKRq8tKtTCl717GhHQ2Ua4bHsFCQRKBAhyIWTal7uUHLMw==', N'3fadeb62-a73c-4c87-9c88-eb837cfa2b1b', NULL, 0, 0, NULL, 1, 0, N'guest@vidly.com')
                INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'baa0225a-a06b-41b9-b5c7-6a35546e58cc', N'CanManageMovies')
                INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'3ff46a72-c786-437b-9091-da5adc6b59e5', N'baa0225a-a06b-41b9-b5c7-6a35546e58cc')
            ");
        }
        
        public override void Down()
        {
        }
    }
}
