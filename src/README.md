部署
--

- 配置爬虫程序
    - 爬虫是用 `python` 写的，因此需要 python 环境
        - 如果 IIS 没有找到 python 环境，则手动在数据表 `AbpSettings` 中修改 `acmMgr.PythonPath` 项的值
    - 需要安装 python 库
        - `pip install --upgrade git+https://github.com/QingQiz/python_modules`
        - `pip install argh`
        - **NOTE**: 如果要使用 IIS 部署的话，需要将包装在全局上
            - 如果已经装在了本地，需要先 `pip uninstall package`，然后用管理员权限执行 `pip install package`
    - 需要在数据库中设置当前爬虫的位置
        - 将 `crawler.py` 复制到部署目录下
- 配置数据库
    - 修改 `acmManager.Web.Mvc` 下的 `appsettings.json` 中的 `ConnectionStrings`
- 部署
    - `dotnet.ext`
        - 数据库: EF: 
            - `dotnet ef migrations add migration-name`
            - `dotnet ef database update`
    - `visual studio`
        - 数据库: EF: 
            - 打开 `Package manager console` 选择 `EntityFrameworkCore`
            - `Add-Migration migration-name`
            - `Update-Database`
        - 部署到 IIS
            - 在 IIS 中新建网站
            - 使用 Visual Studio 的 publish 功能发布到新建的网站的文件夹里
            - 将 `crawler.py` 复制到部署目录下
            - 重启 IIS
            - 可能需要修改 `appsettings.json` 中的数据库验证信息
        
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

- 当 Migration 和 数据库不匹配时（比如，在 A 机器上的所有 migration 都没有提交，而 B 机器上重新生产了 Migration，此时 A 机器和 B 机器的 Migration 记录不同步，当代码合并以后，无法进行数据库迁移）
    - 将 B 机器上的 Migration 记录全部删除（`src\acmManager.EntityFrameworkCore\Migrations`）
    - 清除 `__EFMigrationsHistory` 表的内容
    - 将 A 机器的 Migration 记录复制到 B 机器上 (当然也可以手动修改这个表，让 EF 认为你已经应用了迁移)
    - 将 A 机器上的 `__EFMigrationsHistory` 中的内容复制到 B 机器的数据库中
    - 重新生成 B 机器上的 migration
    - 应用迁移，更新数据库
    
- 从 ICollection 删除数据时，Manager 的 Delete 动作会顺便移除 ICollection 中的数据，因此不能直接用 foreach 删除
    - 错误的示范：
      ```c#
      foreach (var i in entity.icollection)
      {
          await _manager.Delete(i);
      }
      ```
    - 正确的示范：
      ```c#
      var collection = new List<T>(entity.icollection);
      foreach (var i in collection)
      {
          await _manager.Delete(i);
      }
      ```