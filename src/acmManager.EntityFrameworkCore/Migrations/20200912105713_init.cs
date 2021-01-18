using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace acmManager.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "AbpAuditLogs",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: true),
                    ServiceName = table.Column<string>(maxLength: 256, nullable: true),
                    MethodName = table.Column<string>(maxLength: 256, nullable: true),
                    Parameters = table.Column<string>(maxLength: 1024, nullable: true),
                    ReturnValue = table.Column<string>(nullable: true),
                    ExecutionTime = table.Column<DateTime>(nullable: false),
                    ExecutionDuration = table.Column<int>(nullable: false),
                    ClientIpAddress = table.Column<string>(maxLength: 64, nullable: true),
                    ClientName = table.Column<string>(maxLength: 128, nullable: true),
                    BrowserInfo = table.Column<string>(maxLength: 512, nullable: true),
                    Exception = table.Column<string>(maxLength: 2000, nullable: true),
                    ImpersonatorUserId = table.Column<long>(nullable: true),
                    ImpersonatorTenantId = table.Column<int>(nullable: true),
                    CustomData = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_AbpAuditLogs", x => x.Id); });

            migrationBuilder.CreateTable(
                "AbpBackgroundJobs",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    JobType = table.Column<string>(maxLength: 512, nullable: false),
                    JobArgs = table.Column<string>(maxLength: 1048576, nullable: false),
                    TryCount = table.Column<short>(nullable: false),
                    NextTryTime = table.Column<DateTime>(nullable: false),
                    LastTryTime = table.Column<DateTime>(nullable: true),
                    IsAbandoned = table.Column<bool>(nullable: false),
                    Priority = table.Column<byte>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_AbpBackgroundJobs", x => x.Id); });

            migrationBuilder.CreateTable(
                "AbpDynamicParameters",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParameterName = table.Column<string>(nullable: true),
                    InputType = table.Column<string>(nullable: true),
                    Permission = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_AbpDynamicParameters", x => x.Id); });

            migrationBuilder.CreateTable(
                "AbpEditions",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 32, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_AbpEditions", x => x.Id); });

            migrationBuilder.CreateTable(
                "AbpEntityChangeSets",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrowserInfo = table.Column<string>(maxLength: 512, nullable: true),
                    ClientIpAddress = table.Column<string>(maxLength: 64, nullable: true),
                    ClientName = table.Column<string>(maxLength: 128, nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    ExtensionData = table.Column<string>(nullable: true),
                    ImpersonatorTenantId = table.Column<int>(nullable: true),
                    ImpersonatorUserId = table.Column<long>(nullable: true),
                    Reason = table.Column<string>(maxLength: 256, nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_AbpEntityChangeSets", x => x.Id); });

            migrationBuilder.CreateTable(
                "AbpLanguages",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 64, nullable: false),
                    Icon = table.Column<string>(maxLength: 128, nullable: true),
                    IsDisabled = table.Column<bool>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_AbpLanguages", x => x.Id); });

            migrationBuilder.CreateTable(
                "AbpLanguageTexts",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    LanguageName = table.Column<string>(maxLength: 128, nullable: false),
                    Source = table.Column<string>(maxLength: 128, nullable: false),
                    Key = table.Column<string>(maxLength: 256, nullable: false),
                    Value = table.Column<string>(maxLength: 67108864, nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_AbpLanguageTexts", x => x.Id); });

            migrationBuilder.CreateTable(
                "AbpNotifications",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    NotificationName = table.Column<string>(maxLength: 96, nullable: false),
                    Data = table.Column<string>(maxLength: 1048576, nullable: true),
                    DataTypeName = table.Column<string>(maxLength: 512, nullable: true),
                    EntityTypeName = table.Column<string>(maxLength: 250, nullable: true),
                    EntityTypeAssemblyQualifiedName = table.Column<string>(maxLength: 512, nullable: true),
                    EntityId = table.Column<string>(maxLength: 96, nullable: true),
                    Severity = table.Column<byte>(nullable: false),
                    UserIds = table.Column<string>(maxLength: 131072, nullable: true),
                    ExcludedUserIds = table.Column<string>(maxLength: 131072, nullable: true),
                    TenantIds = table.Column<string>(maxLength: 131072, nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_AbpNotifications", x => x.Id); });

            migrationBuilder.CreateTable(
                "AbpNotificationSubscriptions",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    NotificationName = table.Column<string>(maxLength: 96, nullable: true),
                    EntityTypeName = table.Column<string>(maxLength: 250, nullable: true),
                    EntityTypeAssemblyQualifiedName = table.Column<string>(maxLength: 512, nullable: true),
                    EntityId = table.Column<string>(maxLength: 96, nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_AbpNotificationSubscriptions", x => x.Id); });

            migrationBuilder.CreateTable(
                "AbpOrganizationUnitRoles",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    RoleId = table.Column<int>(nullable: false),
                    OrganizationUnitId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_AbpOrganizationUnitRoles", x => x.Id); });

            migrationBuilder.CreateTable(
                "AbpOrganizationUnits",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    ParentId = table.Column<long>(nullable: true),
                    Code = table.Column<string>(maxLength: 95, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpOrganizationUnits", x => x.Id);
                    table.ForeignKey(
                        "FK_AbpOrganizationUnits_AbpOrganizationUnits_ParentId",
                        x => x.ParentId,
                        "AbpOrganizationUnits",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "AbpTenantNotifications",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    NotificationName = table.Column<string>(maxLength: 96, nullable: false),
                    Data = table.Column<string>(maxLength: 1048576, nullable: true),
                    DataTypeName = table.Column<string>(maxLength: 512, nullable: true),
                    EntityTypeName = table.Column<string>(maxLength: 250, nullable: true),
                    EntityTypeAssemblyQualifiedName = table.Column<string>(maxLength: 512, nullable: true),
                    EntityId = table.Column<string>(maxLength: 96, nullable: true),
                    Severity = table.Column<byte>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_AbpTenantNotifications", x => x.Id); });

            migrationBuilder.CreateTable(
                "AbpUserAccounts",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    UserLinkId = table.Column<long>(nullable: true),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    EmailAddress = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_AbpUserAccounts", x => x.Id); });

            migrationBuilder.CreateTable(
                "AbpUserLoginAttempts",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    TenancyName = table.Column<string>(maxLength: 64, nullable: true),
                    UserId = table.Column<long>(nullable: true),
                    UserNameOrEmailAddress = table.Column<string>(maxLength: 256, nullable: true),
                    ClientIpAddress = table.Column<string>(maxLength: 64, nullable: true),
                    ClientName = table.Column<string>(maxLength: 128, nullable: true),
                    BrowserInfo = table.Column<string>(maxLength: 512, nullable: true),
                    Result = table.Column<byte>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_AbpUserLoginAttempts", x => x.Id); });

            migrationBuilder.CreateTable(
                "AbpUserNotifications",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    TenantNotificationId = table.Column<Guid>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_AbpUserNotifications", x => x.Id); });

            migrationBuilder.CreateTable(
                "AbpUserOrganizationUnits",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    OrganizationUnitId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_AbpUserOrganizationUnits", x => x.Id); });

            migrationBuilder.CreateTable(
                "AbpWebhookEvents",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WebhookName = table.Column<string>(nullable: false),
                    Data = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletionTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_AbpWebhookEvents", x => x.Id); });

            migrationBuilder.CreateTable(
                "AbpWebhookSubscriptions",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    WebhookUri = table.Column<string>(nullable: false),
                    Secret = table.Column<string>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Webhooks = table.Column<string>(nullable: true),
                    Headers = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_AbpWebhookSubscriptions", x => x.Id); });

            migrationBuilder.CreateTable(
                "acmMgr.Article",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    ViewTimes = table.Column<long>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_acmMgr.Article", x => x.Id); });

            migrationBuilder.CreateTable(
                "acmMgr.File",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    UploadName = table.Column<string>(nullable: false),
                    RealPath = table.Column<string>(nullable: false),
                    MimeType = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_acmMgr.File", x => x.Id); });

            migrationBuilder.CreateTable(
                "acmMgr.Problem",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Url = table.Column<string>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_acmMgr.Problem", x => x.Id); });

            migrationBuilder.CreateTable(
                "AbpDynamicParameterValues",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    DynamicParameterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpDynamicParameterValues", x => x.Id);
                    table.ForeignKey(
                        "FK_AbpDynamicParameterValues_AbpDynamicParameters_DynamicParameterId",
                        x => x.DynamicParameterId,
                        "AbpDynamicParameters",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AbpEntityDynamicParameters",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityFullName = table.Column<string>(nullable: true),
                    DynamicParameterId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpEntityDynamicParameters", x => x.Id);
                    table.ForeignKey(
                        "FK_AbpEntityDynamicParameters_AbpDynamicParameters_DynamicParameterId",
                        x => x.DynamicParameterId,
                        "AbpDynamicParameters",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AbpFeatures",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(maxLength: 2000, nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    EditionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpFeatures", x => x.Id);
                    table.ForeignKey(
                        "FK_AbpFeatures_AbpEditions_EditionId",
                        x => x.EditionId,
                        "AbpEditions",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AbpEntityChanges",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChangeTime = table.Column<DateTime>(nullable: false),
                    ChangeType = table.Column<byte>(nullable: false),
                    EntityChangeSetId = table.Column<long>(nullable: false),
                    EntityId = table.Column<string>(maxLength: 48, nullable: true),
                    EntityTypeFullName = table.Column<string>(maxLength: 192, nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpEntityChanges", x => x.Id);
                    table.ForeignKey(
                        "FK_AbpEntityChanges_AbpEntityChangeSets_EntityChangeSetId",
                        x => x.EntityChangeSetId,
                        "AbpEntityChangeSets",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AbpWebhookSendAttempts",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WebhookEventId = table.Column<Guid>(nullable: false),
                    WebhookSubscriptionId = table.Column<Guid>(nullable: false),
                    Response = table.Column<string>(nullable: true),
                    ResponseStatusCode = table.Column<int>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpWebhookSendAttempts", x => x.Id);
                    table.ForeignKey(
                        "FK_AbpWebhookSendAttempts_AbpWebhookEvents_WebhookEventId",
                        x => x.WebhookEventId,
                        "AbpWebhookEvents",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "acmMgr.Comment",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ArticleId = table.Column<long>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    ReplyToCommentId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_acmMgr.Comment", x => x.Id);
                    table.ForeignKey(
                        "FK_acmMgr.Comment_acmMgr.Article_ArticleId",
                        x => x.ArticleId,
                        "acmMgr.Article",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_acmMgr.Comment_acmMgr.Comment_ReplyToCommentId",
                        x => x.ReplyToCommentId,
                        "acmMgr.Comment",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "acmMgr.Contest",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    DescriptionId = table.Column<long>(nullable: true),
                    SignUpStartTime = table.Column<DateTime>(nullable: false),
                    SignUpEndTime = table.Column<DateTime>(nullable: false),
                    ResultId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_acmMgr.Contest", x => x.Id);
                    table.ForeignKey(
                        "FK_acmMgr.Contest_acmMgr.Article_DescriptionId",
                        x => x.DescriptionId,
                        "acmMgr.Article",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_acmMgr.Contest_acmMgr.Article_ResultId",
                        x => x.ResultId,
                        "acmMgr.Article",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "acmMgr.Certificate",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Level = table.Column<string>(nullable: true),
                    FileId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_acmMgr.Certificate", x => x.Id);
                    table.ForeignKey(
                        "FK_acmMgr.Certificate_acmMgr.File_FileId",
                        x => x.FileId,
                        "acmMgr.File",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "acmMgr.UserInfo",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentNumber = table.Column<string>(nullable: true),
                    Org = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    Gender = table.Column<int>(nullable: false),
                    Major = table.Column<string>(nullable: true),
                    ClassId = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    StudentType = table.Column<string>(nullable: true),
                    PhotoId = table.Column<long>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_acmMgr.UserInfo", x => x.Id);
                    table.ForeignKey(
                        "FK_acmMgr.UserInfo_acmMgr.File_PhotoId",
                        x => x.PhotoId,
                        "acmMgr.File",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "acmMgr.ProblemRecommend",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ProblemId = table.Column<long>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_acmMgr.ProblemRecommend", x => x.Id);
                    table.ForeignKey(
                        "FK_acmMgr.ProblemRecommend_acmMgr.Problem_ProblemId",
                        x => x.ProblemId,
                        "acmMgr.Problem",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "acmMgr.ProblemType",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ProblemId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_acmMgr.ProblemType", x => x.Id);
                    table.ForeignKey(
                        "FK_acmMgr.ProblemType_acmMgr.Problem_ProblemId",
                        x => x.ProblemId,
                        "acmMgr.Problem",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "AbpEntityDynamicParameterValues",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(nullable: false),
                    EntityId = table.Column<string>(nullable: true),
                    EntityDynamicParameterId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpEntityDynamicParameterValues", x => x.Id);
                    table.ForeignKey(
                        "FK_AbpEntityDynamicParameterValues_AbpEntityDynamicParameters_EntityDynamicParameterId",
                        x => x.EntityDynamicParameterId,
                        "AbpEntityDynamicParameters",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AbpEntityPropertyChanges",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityChangeId = table.Column<long>(nullable: false),
                    NewValue = table.Column<string>(maxLength: 512, nullable: true),
                    OriginalValue = table.Column<string>(maxLength: 512, nullable: true),
                    PropertyName = table.Column<string>(maxLength: 96, nullable: true),
                    PropertyTypeFullName = table.Column<string>(maxLength: 192, nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpEntityPropertyChanges", x => x.Id);
                    table.ForeignKey(
                        "FK_AbpEntityPropertyChanges_AbpEntityChanges_EntityChangeId",
                        x => x.EntityChangeId,
                        "AbpEntityChanges",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "acmMgr.ContestSignUp",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ContestId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_acmMgr.ContestSignUp", x => x.Id);
                    table.ForeignKey(
                        "FK_acmMgr.ContestSignUp_acmMgr.Contest_ContestId",
                        x => x.ContestId,
                        "acmMgr.Contest",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "AbpUsers",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    AuthenticationSource = table.Column<string>(maxLength: 64, nullable: true),
                    UserName = table.Column<string>(maxLength: 256, nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    EmailAddress = table.Column<string>(maxLength: 256, nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    Surname = table.Column<string>(maxLength: 64, nullable: false),
                    Password = table.Column<string>(maxLength: 128, nullable: false),
                    EmailConfirmationCode = table.Column<string>(maxLength: 328, nullable: true),
                    PasswordResetCode = table.Column<string>(maxLength: 328, nullable: true),
                    LockoutEndDateUtc = table.Column<DateTime>(nullable: true),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    IsLockoutEnabled = table.Column<bool>(nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 32, nullable: true),
                    IsPhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    SecurityStamp = table.Column<string>(maxLength: 128, nullable: true),
                    IsTwoFactorEnabled = table.Column<bool>(nullable: false),
                    IsEmailConfirmed = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: false),
                    NormalizedEmailAddress = table.Column<string>(maxLength: 256, nullable: false),
                    ConcurrencyStamp = table.Column<string>(maxLength: 128, nullable: true),
                    UserInfoId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUsers", x => x.Id);
                    table.ForeignKey(
                        "FK_AbpUsers_AbpUsers_CreatorUserId",
                        x => x.CreatorUserId,
                        "AbpUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_AbpUsers_AbpUsers_DeleterUserId",
                        x => x.DeleterUserId,
                        "AbpUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_AbpUsers_AbpUsers_LastModifierUserId",
                        x => x.LastModifierUserId,
                        "AbpUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_AbpUsers_acmMgr.UserInfo_UserInfoId",
                        x => x.UserInfoId,
                        "acmMgr.UserInfo",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "acmMgr.RecommendVote",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    RecommendId = table.Column<long>(nullable: true),
                    TypeId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_acmMgr.RecommendVote", x => x.Id);
                    table.ForeignKey(
                        "FK_acmMgr.RecommendVote_acmMgr.ProblemRecommend_RecommendId",
                        x => x.RecommendId,
                        "acmMgr.ProblemRecommend",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_acmMgr.RecommendVote_acmMgr.RecommendVote_TypeId",
                        x => x.TypeId,
                        "acmMgr.RecommendVote",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "AbpRoles",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 32, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 64, nullable: false),
                    IsStatic = table.Column<bool>(nullable: false),
                    IsDefault = table.Column<bool>(nullable: false),
                    NormalizedName = table.Column<string>(maxLength: 32, nullable: false),
                    ConcurrencyStamp = table.Column<string>(maxLength: 128, nullable: true),
                    Description = table.Column<string>(maxLength: 5000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpRoles", x => x.Id);
                    table.ForeignKey(
                        "FK_AbpRoles_AbpUsers_CreatorUserId",
                        x => x.CreatorUserId,
                        "AbpUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_AbpRoles_AbpUsers_DeleterUserId",
                        x => x.DeleterUserId,
                        "AbpUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_AbpRoles_AbpUsers_LastModifierUserId",
                        x => x.LastModifierUserId,
                        "AbpUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "AbpSettings",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpSettings", x => x.Id);
                    table.ForeignKey(
                        "FK_AbpSettings_AbpUsers_UserId",
                        x => x.UserId,
                        "AbpUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "AbpTenants",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenancyName = table.Column<string>(maxLength: 64, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    ConnectionString = table.Column<string>(maxLength: 1024, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    EditionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpTenants", x => x.Id);
                    table.ForeignKey(
                        "FK_AbpTenants_AbpUsers_CreatorUserId",
                        x => x.CreatorUserId,
                        "AbpUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_AbpTenants_AbpUsers_DeleterUserId",
                        x => x.DeleterUserId,
                        "AbpUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_AbpTenants_AbpEditions_EditionId",
                        x => x.EditionId,
                        "AbpEditions",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_AbpTenants_AbpUsers_LastModifierUserId",
                        x => x.LastModifierUserId,
                        "AbpUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "AbpUserClaims",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    ClaimType = table.Column<string>(maxLength: 256, nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserClaims", x => x.Id);
                    table.ForeignKey(
                        "FK_AbpUserClaims_AbpUsers_UserId",
                        x => x.UserId,
                        "AbpUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AbpUserLogins",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserLogins", x => x.Id);
                    table.ForeignKey(
                        "FK_AbpUserLogins_AbpUsers_UserId",
                        x => x.UserId,
                        "AbpUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AbpUserRoles",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserRoles", x => x.Id);
                    table.ForeignKey(
                        "FK_AbpUserRoles_AbpUsers_UserId",
                        x => x.UserId,
                        "AbpUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AbpUserTokens",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    Value = table.Column<string>(maxLength: 512, nullable: true),
                    ExpireDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserTokens", x => x.Id);
                    table.ForeignKey(
                        "FK_AbpUserTokens_AbpUsers_UserId",
                        x => x.UserId,
                        "AbpUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AbpPermissions",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    IsGranted = table.Column<bool>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    RoleId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpPermissions", x => x.Id);
                    table.ForeignKey(
                        "FK_AbpPermissions_AbpRoles_RoleId",
                        x => x.RoleId,
                        "AbpRoles",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_AbpPermissions_AbpUsers_UserId",
                        x => x.UserId,
                        "AbpUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AbpRoleClaims",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    RoleId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(maxLength: 256, nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpRoleClaims", x => x.Id);
                    table.ForeignKey(
                        "FK_AbpRoleClaims_AbpRoles_RoleId",
                        x => x.RoleId,
                        "AbpRoles",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_AbpAuditLogs_TenantId_ExecutionDuration",
                "AbpAuditLogs",
                new[] {"TenantId", "ExecutionDuration"});

            migrationBuilder.CreateIndex(
                "IX_AbpAuditLogs_TenantId_ExecutionTime",
                "AbpAuditLogs",
                new[] {"TenantId", "ExecutionTime"});

            migrationBuilder.CreateIndex(
                "IX_AbpAuditLogs_TenantId_UserId",
                "AbpAuditLogs",
                new[] {"TenantId", "UserId"});

            migrationBuilder.CreateIndex(
                "IX_AbpBackgroundJobs_IsAbandoned_NextTryTime",
                "AbpBackgroundJobs",
                new[] {"IsAbandoned", "NextTryTime"});

            migrationBuilder.CreateIndex(
                "IX_AbpDynamicParameters_ParameterName_TenantId",
                "AbpDynamicParameters",
                new[] {"ParameterName", "TenantId"},
                unique: true,
                filter: "[ParameterName] IS NOT NULL AND [TenantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                "IX_AbpDynamicParameterValues_DynamicParameterId",
                "AbpDynamicParameterValues",
                "DynamicParameterId");

            migrationBuilder.CreateIndex(
                "IX_AbpEntityChanges_EntityChangeSetId",
                "AbpEntityChanges",
                "EntityChangeSetId");

            migrationBuilder.CreateIndex(
                "IX_AbpEntityChanges_EntityTypeFullName_EntityId",
                "AbpEntityChanges",
                new[] {"EntityTypeFullName", "EntityId"});

            migrationBuilder.CreateIndex(
                "IX_AbpEntityChangeSets_TenantId_CreationTime",
                "AbpEntityChangeSets",
                new[] {"TenantId", "CreationTime"});

            migrationBuilder.CreateIndex(
                "IX_AbpEntityChangeSets_TenantId_Reason",
                "AbpEntityChangeSets",
                new[] {"TenantId", "Reason"});

            migrationBuilder.CreateIndex(
                "IX_AbpEntityChangeSets_TenantId_UserId",
                "AbpEntityChangeSets",
                new[] {"TenantId", "UserId"});

            migrationBuilder.CreateIndex(
                "IX_AbpEntityDynamicParameters_DynamicParameterId",
                "AbpEntityDynamicParameters",
                "DynamicParameterId");

            migrationBuilder.CreateIndex(
                "IX_AbpEntityDynamicParameters_EntityFullName_DynamicParameterId_TenantId",
                "AbpEntityDynamicParameters",
                new[] {"EntityFullName", "DynamicParameterId", "TenantId"},
                unique: true,
                filter: "[EntityFullName] IS NOT NULL AND [TenantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                "IX_AbpEntityDynamicParameterValues_EntityDynamicParameterId",
                "AbpEntityDynamicParameterValues",
                "EntityDynamicParameterId");

            migrationBuilder.CreateIndex(
                "IX_AbpEntityPropertyChanges_EntityChangeId",
                "AbpEntityPropertyChanges",
                "EntityChangeId");

            migrationBuilder.CreateIndex(
                "IX_AbpFeatures_EditionId_Name",
                "AbpFeatures",
                new[] {"EditionId", "Name"});

            migrationBuilder.CreateIndex(
                "IX_AbpFeatures_TenantId_Name",
                "AbpFeatures",
                new[] {"TenantId", "Name"});

            migrationBuilder.CreateIndex(
                "IX_AbpLanguages_TenantId_Name",
                "AbpLanguages",
                new[] {"TenantId", "Name"});

            migrationBuilder.CreateIndex(
                "IX_AbpLanguageTexts_TenantId_Source_LanguageName_Key",
                "AbpLanguageTexts",
                new[] {"TenantId", "Source", "LanguageName", "Key"});

            migrationBuilder.CreateIndex(
                "IX_AbpNotificationSubscriptions_NotificationName_EntityTypeName_EntityId_UserId",
                "AbpNotificationSubscriptions",
                new[] {"NotificationName", "EntityTypeName", "EntityId", "UserId"});

            migrationBuilder.CreateIndex(
                "IX_AbpNotificationSubscriptions_TenantId_NotificationName_EntityTypeName_EntityId_UserId",
                "AbpNotificationSubscriptions",
                new[] {"TenantId", "NotificationName", "EntityTypeName", "EntityId", "UserId"});

            migrationBuilder.CreateIndex(
                "IX_AbpOrganizationUnitRoles_TenantId_OrganizationUnitId",
                "AbpOrganizationUnitRoles",
                new[] {"TenantId", "OrganizationUnitId"});

            migrationBuilder.CreateIndex(
                "IX_AbpOrganizationUnitRoles_TenantId_RoleId",
                "AbpOrganizationUnitRoles",
                new[] {"TenantId", "RoleId"});

            migrationBuilder.CreateIndex(
                "IX_AbpOrganizationUnits_ParentId",
                "AbpOrganizationUnits",
                "ParentId");

            migrationBuilder.CreateIndex(
                "IX_AbpOrganizationUnits_TenantId_Code",
                "AbpOrganizationUnits",
                new[] {"TenantId", "Code"});

            migrationBuilder.CreateIndex(
                "IX_AbpPermissions_TenantId_Name",
                "AbpPermissions",
                new[] {"TenantId", "Name"});

            migrationBuilder.CreateIndex(
                "IX_AbpPermissions_RoleId",
                "AbpPermissions",
                "RoleId");

            migrationBuilder.CreateIndex(
                "IX_AbpPermissions_UserId",
                "AbpPermissions",
                "UserId");

            migrationBuilder.CreateIndex(
                "IX_AbpRoleClaims_RoleId",
                "AbpRoleClaims",
                "RoleId");

            migrationBuilder.CreateIndex(
                "IX_AbpRoleClaims_TenantId_ClaimType",
                "AbpRoleClaims",
                new[] {"TenantId", "ClaimType"});

            migrationBuilder.CreateIndex(
                "IX_AbpRoles_CreatorUserId",
                "AbpRoles",
                "CreatorUserId");

            migrationBuilder.CreateIndex(
                "IX_AbpRoles_DeleterUserId",
                "AbpRoles",
                "DeleterUserId");

            migrationBuilder.CreateIndex(
                "IX_AbpRoles_LastModifierUserId",
                "AbpRoles",
                "LastModifierUserId");

            migrationBuilder.CreateIndex(
                "IX_AbpRoles_TenantId_NormalizedName",
                "AbpRoles",
                new[] {"TenantId", "NormalizedName"});

            migrationBuilder.CreateIndex(
                "IX_AbpSettings_UserId",
                "AbpSettings",
                "UserId");

            migrationBuilder.CreateIndex(
                "IX_AbpSettings_TenantId_Name_UserId",
                "AbpSettings",
                new[] {"TenantId", "Name", "UserId"},
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_AbpTenantNotifications_TenantId",
                "AbpTenantNotifications",
                "TenantId");

            migrationBuilder.CreateIndex(
                "IX_AbpTenants_CreatorUserId",
                "AbpTenants",
                "CreatorUserId");

            migrationBuilder.CreateIndex(
                "IX_AbpTenants_DeleterUserId",
                "AbpTenants",
                "DeleterUserId");

            migrationBuilder.CreateIndex(
                "IX_AbpTenants_EditionId",
                "AbpTenants",
                "EditionId");

            migrationBuilder.CreateIndex(
                "IX_AbpTenants_LastModifierUserId",
                "AbpTenants",
                "LastModifierUserId");

            migrationBuilder.CreateIndex(
                "IX_AbpTenants_TenancyName",
                "AbpTenants",
                "TenancyName");

            migrationBuilder.CreateIndex(
                "IX_AbpUserAccounts_EmailAddress",
                "AbpUserAccounts",
                "EmailAddress");

            migrationBuilder.CreateIndex(
                "IX_AbpUserAccounts_UserName",
                "AbpUserAccounts",
                "UserName");

            migrationBuilder.CreateIndex(
                "IX_AbpUserAccounts_TenantId_EmailAddress",
                "AbpUserAccounts",
                new[] {"TenantId", "EmailAddress"});

            migrationBuilder.CreateIndex(
                "IX_AbpUserAccounts_TenantId_UserId",
                "AbpUserAccounts",
                new[] {"TenantId", "UserId"});

            migrationBuilder.CreateIndex(
                "IX_AbpUserAccounts_TenantId_UserName",
                "AbpUserAccounts",
                new[] {"TenantId", "UserName"});

            migrationBuilder.CreateIndex(
                "IX_AbpUserClaims_UserId",
                "AbpUserClaims",
                "UserId");

            migrationBuilder.CreateIndex(
                "IX_AbpUserClaims_TenantId_ClaimType",
                "AbpUserClaims",
                new[] {"TenantId", "ClaimType"});

            migrationBuilder.CreateIndex(
                "IX_AbpUserLoginAttempts_UserId_TenantId",
                "AbpUserLoginAttempts",
                new[] {"UserId", "TenantId"});

            migrationBuilder.CreateIndex(
                "IX_AbpUserLoginAttempts_TenancyName_UserNameOrEmailAddress_Result",
                "AbpUserLoginAttempts",
                new[] {"TenancyName", "UserNameOrEmailAddress", "Result"});

            migrationBuilder.CreateIndex(
                "IX_AbpUserLogins_UserId",
                "AbpUserLogins",
                "UserId");

            migrationBuilder.CreateIndex(
                "IX_AbpUserLogins_TenantId_UserId",
                "AbpUserLogins",
                new[] {"TenantId", "UserId"});

            migrationBuilder.CreateIndex(
                "IX_AbpUserLogins_TenantId_LoginProvider_ProviderKey",
                "AbpUserLogins",
                new[] {"TenantId", "LoginProvider", "ProviderKey"});

            migrationBuilder.CreateIndex(
                "IX_AbpUserNotifications_UserId_State_CreationTime",
                "AbpUserNotifications",
                new[] {"UserId", "State", "CreationTime"});

            migrationBuilder.CreateIndex(
                "IX_AbpUserOrganizationUnits_TenantId_OrganizationUnitId",
                "AbpUserOrganizationUnits",
                new[] {"TenantId", "OrganizationUnitId"});

            migrationBuilder.CreateIndex(
                "IX_AbpUserOrganizationUnits_TenantId_UserId",
                "AbpUserOrganizationUnits",
                new[] {"TenantId", "UserId"});

            migrationBuilder.CreateIndex(
                "IX_AbpUserRoles_UserId",
                "AbpUserRoles",
                "UserId");

            migrationBuilder.CreateIndex(
                "IX_AbpUserRoles_TenantId_RoleId",
                "AbpUserRoles",
                new[] {"TenantId", "RoleId"});

            migrationBuilder.CreateIndex(
                "IX_AbpUserRoles_TenantId_UserId",
                "AbpUserRoles",
                new[] {"TenantId", "UserId"});

            migrationBuilder.CreateIndex(
                "IX_AbpUsers_CreatorUserId",
                "AbpUsers",
                "CreatorUserId");

            migrationBuilder.CreateIndex(
                "IX_AbpUsers_DeleterUserId",
                "AbpUsers",
                "DeleterUserId");

            migrationBuilder.CreateIndex(
                "IX_AbpUsers_LastModifierUserId",
                "AbpUsers",
                "LastModifierUserId");

            migrationBuilder.CreateIndex(
                "IX_AbpUsers_UserInfoId",
                "AbpUsers",
                "UserInfoId");

            migrationBuilder.CreateIndex(
                "IX_AbpUsers_TenantId_NormalizedEmailAddress",
                "AbpUsers",
                new[] {"TenantId", "NormalizedEmailAddress"});

            migrationBuilder.CreateIndex(
                "IX_AbpUsers_TenantId_NormalizedUserName",
                "AbpUsers",
                new[] {"TenantId", "NormalizedUserName"});

            migrationBuilder.CreateIndex(
                "IX_AbpUserTokens_UserId",
                "AbpUserTokens",
                "UserId");

            migrationBuilder.CreateIndex(
                "IX_AbpUserTokens_TenantId_UserId",
                "AbpUserTokens",
                new[] {"TenantId", "UserId"});

            migrationBuilder.CreateIndex(
                "IX_AbpWebhookSendAttempts_WebhookEventId",
                "AbpWebhookSendAttempts",
                "WebhookEventId");

            migrationBuilder.CreateIndex(
                "IX_acmMgr.Certificate_FileId",
                "acmMgr.Certificate",
                "FileId");

            migrationBuilder.CreateIndex(
                "IX_acmMgr.Comment_ArticleId",
                "acmMgr.Comment",
                "ArticleId");

            migrationBuilder.CreateIndex(
                "IX_acmMgr.Comment_ReplyToCommentId",
                "acmMgr.Comment",
                "ReplyToCommentId");

            migrationBuilder.CreateIndex(
                "IX_acmMgr.Contest_DescriptionId",
                "acmMgr.Contest",
                "DescriptionId");

            migrationBuilder.CreateIndex(
                "IX_acmMgr.Contest_ResultId",
                "acmMgr.Contest",
                "ResultId");

            migrationBuilder.CreateIndex(
                "IX_acmMgr.ContestSignUp_ContestId",
                "acmMgr.ContestSignUp",
                "ContestId");

            migrationBuilder.CreateIndex(
                "IX_acmMgr.ProblemRecommend_ProblemId",
                "acmMgr.ProblemRecommend",
                "ProblemId");

            migrationBuilder.CreateIndex(
                "IX_acmMgr.ProblemType_ProblemId",
                "acmMgr.ProblemType",
                "ProblemId");

            migrationBuilder.CreateIndex(
                "IX_acmMgr.RecommendVote_RecommendId",
                "acmMgr.RecommendVote",
                "RecommendId");

            migrationBuilder.CreateIndex(
                "IX_acmMgr.RecommendVote_TypeId",
                "acmMgr.RecommendVote",
                "TypeId");

            migrationBuilder.CreateIndex(
                "IX_acmMgr.UserInfo_PhotoId",
                "acmMgr.UserInfo",
                "PhotoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "AbpAuditLogs");

            migrationBuilder.DropTable(
                "AbpBackgroundJobs");

            migrationBuilder.DropTable(
                "AbpDynamicParameterValues");

            migrationBuilder.DropTable(
                "AbpEntityDynamicParameterValues");

            migrationBuilder.DropTable(
                "AbpEntityPropertyChanges");

            migrationBuilder.DropTable(
                "AbpFeatures");

            migrationBuilder.DropTable(
                "AbpLanguages");

            migrationBuilder.DropTable(
                "AbpLanguageTexts");

            migrationBuilder.DropTable(
                "AbpNotifications");

            migrationBuilder.DropTable(
                "AbpNotificationSubscriptions");

            migrationBuilder.DropTable(
                "AbpOrganizationUnitRoles");

            migrationBuilder.DropTable(
                "AbpOrganizationUnits");

            migrationBuilder.DropTable(
                "AbpPermissions");

            migrationBuilder.DropTable(
                "AbpRoleClaims");

            migrationBuilder.DropTable(
                "AbpSettings");

            migrationBuilder.DropTable(
                "AbpTenantNotifications");

            migrationBuilder.DropTable(
                "AbpTenants");

            migrationBuilder.DropTable(
                "AbpUserAccounts");

            migrationBuilder.DropTable(
                "AbpUserClaims");

            migrationBuilder.DropTable(
                "AbpUserLoginAttempts");

            migrationBuilder.DropTable(
                "AbpUserLogins");

            migrationBuilder.DropTable(
                "AbpUserNotifications");

            migrationBuilder.DropTable(
                "AbpUserOrganizationUnits");

            migrationBuilder.DropTable(
                "AbpUserRoles");

            migrationBuilder.DropTable(
                "AbpUserTokens");

            migrationBuilder.DropTable(
                "AbpWebhookSendAttempts");

            migrationBuilder.DropTable(
                "AbpWebhookSubscriptions");

            migrationBuilder.DropTable(
                "acmMgr.Certificate");

            migrationBuilder.DropTable(
                "acmMgr.Comment");

            migrationBuilder.DropTable(
                "acmMgr.ContestSignUp");

            migrationBuilder.DropTable(
                "acmMgr.ProblemType");

            migrationBuilder.DropTable(
                "acmMgr.RecommendVote");

            migrationBuilder.DropTable(
                "AbpEntityDynamicParameters");

            migrationBuilder.DropTable(
                "AbpEntityChanges");

            migrationBuilder.DropTable(
                "AbpRoles");

            migrationBuilder.DropTable(
                "AbpEditions");

            migrationBuilder.DropTable(
                "AbpWebhookEvents");

            migrationBuilder.DropTable(
                "acmMgr.Contest");

            migrationBuilder.DropTable(
                "acmMgr.ProblemRecommend");

            migrationBuilder.DropTable(
                "AbpDynamicParameters");

            migrationBuilder.DropTable(
                "AbpEntityChangeSets");

            migrationBuilder.DropTable(
                "AbpUsers");

            migrationBuilder.DropTable(
                "acmMgr.Article");

            migrationBuilder.DropTable(
                "acmMgr.Problem");

            migrationBuilder.DropTable(
                "acmMgr.UserInfo");

            migrationBuilder.DropTable(
                "acmMgr.File");
        }
    }
}