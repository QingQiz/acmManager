部署
--

- 配置爬虫程序
    - 爬虫是用 `python` 写的，因此需要 python 环境
    - 需要安装 python 库
        - `pip install --upgrade git+https://github.com/QingQiz/python_modules`
        - `pip install argh`
    - 需要在数据库中设置当前爬虫的位置
        - 如果没有改 `crawler.py` 的位置，就不用动数据库
        
备注
--

- AbpSetting
    - 设置项应在 `Core/Configuration/AppSettingProvider.cs` 中注册，并设置默认值
    - 数据库 `AbpSettings` 中的值会覆盖默认值
- 数据库默认值
    - `EF/Seed/**/*.cs` 中
- 加表
    - 在 `Core/` 中加表
    - 在 `EF/acmManagerDbContext` 中注册
- 添加 Role
    - 在 `Core/Authorization/Roles/StaticRoleNames.cs` 中添加 RoleName
    - 在 `EF/Seed/Tenants/TenantRoleAndUserBuilder` 中为 Role 添加权限
- 添加权限
    - 在 `Core/Authorization/PermissionNames.cs` 中添加权限名
    - 在 `Core/Authorization/acmManagerAuthorizationProvider` 中注册权限
    - 在 `EF/Seed/Tenants/TenantRoleAndUserBuilder` 中为静态 role 添加默认权限
    
- 更改学生信息表
    - 在 `Core/Authorizaiotn/UserInfo.cs` 中修改字段
    - 执行 `dotnet.exe ef migrations add migration_name`
    - 执行 `dotnet.exe ef database update`
    - 修改 `Application/Users/Dto/UserInfoDto`
    - 修改 `crawler.py`
    - 修改 `Application/Users/UserAppService.cs` 中的 `CreateAsync`
    - 修改 `Application/Authorization/Accounts/AccountAppService.cs` 中的 `Register`
 - MVC 依赖注入(`inject`) 时使用的方法必须是 `[UnitOfWord]` 和 `virtual` 的
 
 感慨
 --
 
 - Abp 太刁了，由于 `Unit Of Work` 每个事务结束后自动提交数据更改，根本不需要显示更新数据库，数据变动就和变量赋值一样方便，
   而且由于是事务结束后提交，因此事务异常退出的时候根本不会有数据库变动，回滚就是一个很自然的事了